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
            Like = ReactiveCommand.CreateAsyncTask(
                _ =>
                    {
                        var httpClient = mobileServiceClientFactory.Create();
                        return httpClient.PostAsync(string.Format("/like/{0}/{1}",
                                                                  DeviceInfo.GetDeviceIdentity(),
                                                                  _lecture.Id),
                                                    null);
                    });

            this.WhenAnyObservable(x => x.NavigateToSpeaker)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));

            this.WhenAnyObservable(x => x.Like)
                .Select(x => x)
                .Subscribe();

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

        public ReactiveCommand<object> NavigateToSpeaker { get; private set; } 
        public ReactiveCommand<HttpResponseMessage> Like { get; private set; } 
        public ReactiveCommand<object> Dislike { get; private set; } 

        public string UrlPathSegment
        {
            get { return "lecture"; }
        }

        public IScreen HostScreen { get; private set; }
    }
} 