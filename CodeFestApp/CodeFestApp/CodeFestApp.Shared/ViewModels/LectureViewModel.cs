using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public class LectureViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly Lecture _lecture;
        private readonly ObservableAsPropertyHelper<bool> _isInFavorites;
        
        private bool _isLiked; 
        private bool _isDisliked;

        public LectureViewModel(IScreen hostScreen,
                                Lecture lecture,
                                IViewModelFactory viewModelFactory,
                                IAnalyticsLogger logger,
                                MobileServicesClientFactory mobileServiceClientFactory)
        {
            HostScreen = hostScreen;
            _lecture = lecture;
            _viewModelFactory = viewModelFactory;

            NavigateToSpeaker = ReactiveCommand.Create();

            var deviceIdentity = DeviceInfo.GetDeviceIdentity();
            var httpClient = mobileServiceClientFactory.Create();

            LoadLectureAttitude = ReactiveCommand.CreateAsyncTask(
                async _ =>
                    {
                        var response = await httpClient.GetAsync(string.Format("attitude/{0}/{1}",
                                                                               deviceIdentity,
                                                                               _lecture.Id));
                        return await response.Content.ReadAsStringAsync();
                    });

            CheckIsInFavorites = ReactiveCommand.CreateAsyncTask(
                async _ =>
                    {
                        var response = await httpClient.GetAsync(string.Format("favorite/lectures/check/{0}/{1}",
                                                                               deviceIdentity,
                                                                               _lecture.Id));
                        var responseContent = await response.Content.ReadAsStringAsync();
                        return bool.Parse(responseContent);
                    });

            Like = ReactiveCommand.CreateAsyncTask(
                this.WhenAny(x => x.Start, x => x.IsLiked, (s, l) => s.Value <= DateTime.Now && !l.Value),
                _ =>
                    {
                        logger.LogLectureLikeEvent(_lecture);
                        return httpClient.PostAsync(string.Format("like/{0}/{1}",
                                                                  deviceIdentity,
                                                                  _lecture.Id),
                                                    null);
                    });

            Dislike = ReactiveCommand.CreateAsyncTask(
                this.WhenAny(x => x.Start, x => x.IsDisliked, (s, l) => s.Value <= DateTime.Now && !l.Value),
                _ =>
                    {
                        logger.LogLectureDislikeEvent(_lecture);
                        return httpClient.PostAsync(string.Format("dislike/{0}/{1}",
                                                                  deviceIdentity,
                                                                  _lecture.Id),
                                                    null);
                    });

            ManageFavorites = ReactiveCommand.CreateAsyncTask(
                _ =>
                    {
                        if (IsInFavorites)
                        {
                            return httpClient.DeleteAsync(string.Format("favorite/lectures/remove/{0}/{1}",
                                                                        deviceIdentity,
                                                                        _lecture.Id));
                        }

                        return httpClient.PostAsync(string.Format("favorite/lectures/add/{0}/{1}",
                                                                  deviceIdentity,
                                                                  _lecture.Id),
                                                    null);
                    });

            this.WhenAnyObservable(x => x.NavigateToSpeaker)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));

            this.WhenAnyObservable(x => x.LoadLectureAttitude)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Subscribe(x =>
                    {
                        if (!x.Contains(deviceIdentity))
                        {
                            return;
                        }

                        if (x.Contains("dislike"))
                        {
                            IsDisliked = true;
                        }
                        else
                        {
                            IsLiked = true;
                        }
                    });

            this.WhenAnyObservable(x => x.CheckIsInFavorites)
                .ToProperty(this, x => x.IsInFavorites, out _isInFavorites);

            this.WhenAnyObservable(x => x.Like)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Subscribe(async x =>
                    {
                        if (x.IsSuccessStatusCode)
                        {
                            var content = await x.Content.ReadAsStringAsync();
                            if (content.Contains("like"))
                            {
                                IsLiked = true;
                                IsDisliked = false;
                            }
                        }
                    });

            this.WhenAnyObservable(x => x.Dislike)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Subscribe(async x =>
                    {
                        if (x.IsSuccessStatusCode)
                        {
                            var content = await x.Content.ReadAsStringAsync();
                            if (content.Contains("dislike"))
                            {
                                IsDisliked = true;
                                IsLiked = false;
                            }
                        }
                    });

            this.WhenAnyObservable(x => x.ManageFavorites)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Subscribe(x => CheckIsInFavorites.ExecuteAsyncTask());

            this.WhenAnyObservable(x => x.ThrownExceptions,
                                   x => x.NavigateToSpeaker.ThrownExceptions,
                                   x => x.Like.ThrownExceptions,
                                   x => x.Dislike.ThrownExceptions)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Subscribe(logger.LogException);
         
            this.WhenNavigatedTo(() =>
                {
                    Task.Run(() => logger.LogViewModelRouted(this));
                    return Disposable.Empty;
                });
        }

        public TrackViewModel Track
        {
            get { return _viewModelFactory.Create<TrackViewModel, Track>(_lecture.Track); }
        }

        public string ConferenceTitle
        {
            get { return "codefest 2015"; }
        }

        public string Title
        {
            get { return _lecture.Title; }
        }

        public IEnumerable<SpeakerViewModel> Speakers
        {
            get { return _lecture.Speakers.Select(x => _viewModelFactory.Create<SpeakerViewModel, Speaker>(x)); }
        }
        
        public string Description
        {
            get { return _lecture.Description; }
        }

        public DayViewModel Day
        {
            get { return _viewModelFactory.Create<DayViewModel, Day>(_lecture.Day); }
        }

        public DateTime Start
        {
            get { return _lecture.Start; }
        }
        
        public DateTime End
        {
            get { return _lecture.End; }
        }

        public bool IsLiked
        {
            get { return _isLiked; }
            set { this.RaiseAndSetIfChanged(ref _isLiked, value); }
        }

        public bool IsDisliked
        {
            get { return _isDisliked; }
            set { this.RaiseAndSetIfChanged(ref _isDisliked, value); }
        }

        public bool IsInFavorites
        {
            get { return _isInFavorites.Value; }
        }

        public ReactiveCommand<object> NavigateToSpeaker { get; private set; } 
        public ReactiveCommand<string> LoadLectureAttitude { get; private set; } 
        public ReactiveCommand<bool> CheckIsInFavorites { get; private set; } 
        public ReactiveCommand<HttpResponseMessage> Like { get; private set; }
        public ReactiveCommand<HttpResponseMessage> Dislike { get; private set; } 
        public ReactiveCommand<HttpResponseMessage> ManageFavorites { get; private set; } 

        public string UrlPathSegment
        {
            get { return "lecture"; }
        }

        public IScreen HostScreen { get; private set; }
    }
} 