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

            Like = ReactiveCommand.CreateAsyncTask(
                this.WhenAny(x => x.IsLiked, x => !x.Value),
                _ => httpClient.PostAsync(string.Format("like/{0}/{1}",
                                                        deviceIdentity,
                                                        _lecture.Id),
                                          null));

            Dislike = ReactiveCommand.CreateAsyncTask(
                this.WhenAny(x => x.IsDisiked, x => !x.Value),
                _ => httpClient.PostAsync(string.Format("dislike/{0}/{1}",
                                                        deviceIdentity,
                                                        _lecture.Id),
                                          null));

            this.WhenAnyObservable(x => x.NavigateToSpeaker)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));

            this.WhenAnyObservable(x => x.LoadLectureAttitude)
                .Subscribe(x =>
                    {
                        if (!x.Contains(deviceIdentity))
                        {
                            return;
                        }
                        
                        if (x.Contains("dislike"))
                        {
                            IsDisiked = true;
                        }
                        else
                        {
                            IsLiked = true;
                        }
                    });

            this.WhenAnyObservable(x => x.Like)
                .Subscribe(x =>
                    {
                        if (x.IsSuccessStatusCode)
                        {
                            IsLiked = true;
                            IsDisiked = false;
                        }
                    });

            this.WhenAnyObservable(x => x.Dislike)
                .Subscribe(x =>
                    {
                        if (x.IsSuccessStatusCode)
                        {
                            IsDisiked = true;
                            IsLiked = false;
                        }
                    });
            
            this.WhenAnyObservable(x => x.ThrownExceptions,
                                   x => x.NavigateToSpeaker.ThrownExceptions,
                                   x => x.Like.ThrownExceptions,
                                   x => x.Dislike.ThrownExceptions)
                .ObserveOn(RxApp.TaskpoolScheduler)
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

        public bool IsDisiked
        {
            get { return _isDisliked; }
            set { this.RaiseAndSetIfChanged(ref _isDisliked, value); }
        }

        public ReactiveCommand<object> NavigateToSpeaker { get; private set; } 
        public ReactiveCommand<string> LoadLectureAttitude { get; private set; } 
        public ReactiveCommand<HttpResponseMessage> Like { get; private set; }
        public ReactiveCommand<HttpResponseMessage> Dislike { get; private set; } 

        public string UrlPathSegment
        {
            get { return "lecture"; }
        }

        public IScreen HostScreen { get; private set; }
    }
} 