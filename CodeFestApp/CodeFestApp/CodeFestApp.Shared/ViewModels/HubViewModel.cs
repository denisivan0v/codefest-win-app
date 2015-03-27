using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

using CodeFestApp.Analytics;
using CodeFestApp.DataModel;
using CodeFestApp.DI;
using CodeFestApp.Utils;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class HubViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly ObservableAsPropertyHelper<IEnumerable<DayViewModel>> _days;
        private readonly ObservableAsPropertyHelper<IEnumerable<TrackViewModel>> _tracks;
        private readonly ObservableAsPropertyHelper<IEnumerable<AlphaKeyGroup<SpeakerViewModel>>> _speakers;
        private IEnumerable<IGrouping<string, LectureViewModel>> _favoriteLectures;

        public HubViewModel(IScreen screen,
                            IScheduleReader scheduleReader,
                            IViewModelFactory viewModelFactory,
                            MobileServicesClientFactory mobileServiceClientFactory,
                            IAnalyticsLogger logger)
        {
            HostScreen = screen;

            var deviceIdentity = DeviceInfo.GetDeviceIdentity();
            var mobileServiceClient = mobileServiceClientFactory.Create();

            LoadDaysCommand = ReactiveCommand.CreateAsyncTask(_ => Task.Run(
                () =>
                    {
                        var days = scheduleReader.GetDays();
                        return days.Select(viewModelFactory.Create<DayViewModel, Day>);
                    }));
            LoadTracksCommand = ReactiveCommand.CreateAsyncTask(_ => Task.Run(
                () =>
                    {
                        var tracks = scheduleReader.GetTracks();
                        return tracks.Select(viewModelFactory.Create<TrackViewModel, Track>);
                    }));
            LoadSpeakersCommand = ReactiveCommand.CreateAsyncTask(_ => Task.Run(
                () =>
                    {
                        var speakers = scheduleReader.GetSpeakers();
                        var viewModels = speakers.Select(viewModelFactory.Create<SpeakerViewModel, Speaker>);
                        return AlphaKeyGroup<SpeakerViewModel>.CreateGroups(viewModels, x => x.Title, true);
                    }));
            LoadFavorites = ReactiveCommand.CreateAsyncTask(
                _ =>
                    {
                        var favs = mobileServiceClient.GetTable<FavoriteLecture>();
                        return favs.Where(x => x.DeviceIdentity == deviceIdentity).ToEnumerableAsync();
                    });

            NavigateToDayCommand = ReactiveCommand.Create();
            NavigateToTrackCommand = ReactiveCommand.Create();
            NavigateToSpeakerCommand = ReactiveCommand.Create();
            NavigateToTwitterFeedCommand = ReactiveCommand.Create();
            NavigateToAboutCommand = ReactiveCommand.Create();

            this.WhenAnyObservable(x => x.LoadDaysCommand)
                .ToProperty(this, x => x.Days, out _days);

            this.WhenAnyObservable(x => x.LoadTracksCommand)
                .ToProperty(this, x => x.Tracks, out _tracks);

            this.WhenAnyObservable(x => x.LoadSpeakersCommand)
                .ToProperty(this, x => x.Speakers, out _speakers);

            this.WhenAnyObservable(x => x.NavigateToDayCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));

            this.WhenAnyObservable(x => x.NavigateToTrackCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));

            this.WhenAnyObservable(x => x.NavigateToSpeakerCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));

            this.WhenAnyObservable(x => x.NavigateToTwitterFeedCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(viewModelFactory.Create<TweetsViewModel>()));

            this.WhenAnyObservable(x => x.NavigateToAboutCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(viewModelFactory.Create<AboutViewModel>()));

            this.WhenAnyObservable(x => x.LoadFavorites)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Subscribe(fav =>
                    {
                        var lectureIds = fav.Select(x => x.LectureId).ToArray();
                        FavoriteLectures = scheduleReader.GetLectures()
                                                         .Where(x => lectureIds.Contains(x.Id))
                                                         .Select(viewModelFactory.Create<LectureViewModel, Lecture>)
                                                         .OrderBy(x => x.Start)
                                                         .GroupBy(x => x.Start.ToString("f"));
                    });

            this.WhenAnyObservable(x => x.ThrownExceptions,
                                   x => x.LoadDaysCommand.ThrownExceptions,
                                   x => x.LoadTracksCommand.ThrownExceptions,
                                   x => x.LoadSpeakersCommand.ThrownExceptions,
                                   x => x.NavigateToDayCommand.ThrownExceptions,
                                   x => x.NavigateToTrackCommand.ThrownExceptions,
                                   x => x.NavigateToSpeakerCommand.ThrownExceptions,
                                   x => x.NavigateToTwitterFeedCommand.ThrownExceptions)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Subscribe(logger.LogException);

            this.WhenNavigatedTo(() =>
                {
                    Task.Run(() => logger.LogViewModelRouted(this));
                    return Disposable.Empty;
                });
        }

        public ReactiveCommand<IEnumerable<DayViewModel>> LoadDaysCommand { get; private set; }
        public ReactiveCommand<IEnumerable<TrackViewModel>> LoadTracksCommand { get; private set; }
        public ReactiveCommand<IEnumerable<AlphaKeyGroup<SpeakerViewModel>>> LoadSpeakersCommand { get; private set; }
        public ReactiveCommand<IEnumerable<FavoriteLecture>> LoadFavorites { get; private set; }
        public ReactiveCommand<object> NavigateToDayCommand { get; private set; }
        public ReactiveCommand<object> NavigateToTrackCommand { get; private set; }
        public ReactiveCommand<object> NavigateToSpeakerCommand { get; private set; }
        public ReactiveCommand<object> NavigateToTwitterFeedCommand { get; private set; }
        public ReactiveCommand<object> NavigateToAboutCommand { get; private set; }

        public int ActiveSection { get; set; }

        public string CurrentLecturesSectionTitle
        {
            get { return "ИДЕТ СЕЙЧАС"; }
        }

        public string DaysSectionTitle
        {
            get { return "ДНИ КОНФЕРЕНЦИИ"; }
        }

        public string TracksSectionTitle
        {
            get { return "СЕКЦИИ"; }
        }

        public string SpeakersSectionTitle
        {
            get { return "СПИКЕРЫ"; }
        }

        public string FavoriteLecturesSectionTitle
        {
            get { return "ИЗБРАННОЕ"; }
        }

        public Uri TwitterIcon
        {
            get { return new Uri("ms-appx:///Assets/TwitterIcon.png"); }
        }

        public IEnumerable<DayViewModel> Days
        {
            get { return _days.Value; }
        }

        public IEnumerable<TrackViewModel> Tracks
        {
            get { return _tracks.Value; }
        }

        public IEnumerable<AlphaKeyGroup<SpeakerViewModel>> Speakers
        {
            get { return _speakers.Value; }
        }

        public IEnumerable<IGrouping<string, LectureViewModel>> FavoriteLectures
        {
            get { return _favoriteLectures; }
            set { this.RaiseAndSetIfChanged(ref _favoriteLectures, value); }
        }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment
        {
            get { return "main"; }
        }
    }
}