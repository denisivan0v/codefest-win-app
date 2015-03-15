using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CodeFestApp.DataModel;
using CodeFestApp.DI;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class HubViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly ObservableAsPropertyHelper<IEnumerable<DayViewModel>> _days;
        private readonly ObservableAsPropertyHelper<IEnumerable<TrackViewModel>> _tracks;

        public HubViewModel(IScreen screen, IScheduleReader scheduleReader, IViewModelFactory viewModelFactory)
        {
            HostScreen = screen;

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
            NavigateToDayCommand = ReactiveCommand.Create();
            NavigateToTrackCommand = ReactiveCommand.Create();
            NavigateToTwitterFeedCommand = ReactiveCommand.Create();

            this.WhenAnyObservable(x => x.LoadDaysCommand)
                .ToProperty(this, x => x.Days, out _days);

            this.WhenAnyObservable(x => x.LoadTracksCommand)
                .ToProperty(this, x => x.Tracks, out _tracks);

            this.WhenAnyObservable(x => x.NavigateToDayCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));

            this.WhenAnyObservable(x => x.NavigateToTrackCommand)
               .Subscribe(x => HostScreen.Router.Navigate.Execute(x));

            this.WhenAnyObservable(x => x.NavigateToTwitterFeedCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(new TweetsViewModel(HostScreen)));
        }

        public ReactiveCommand<IEnumerable<DayViewModel>> LoadDaysCommand { get; private set; }
        public ReactiveCommand<IEnumerable<TrackViewModel>> LoadTracksCommand { get; private set; }
        public ReactiveCommand<object> NavigateToDayCommand { get; private set; }
        public ReactiveCommand<object> NavigateToTrackCommand { get; private set; }
        public ReactiveCommand<object> NavigateToTwitterFeedCommand { get; private set; }


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

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment
        {
            get { return "hubpage"; }
        }
    }
}