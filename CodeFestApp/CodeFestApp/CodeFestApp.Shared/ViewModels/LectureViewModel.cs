using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

using CodeFestApp.Analytics;
using CodeFestApp.DataModel;
using CodeFestApp.DI;

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
                                IAnalyticsLogger logger)
        {
            HostScreen = hostScreen;
            _lecture = lecture;
            _viewModelFactory = viewModelFactory;

            NavigateToSpeaker = ReactiveCommand.Create();

            this.WhenAnyObservable(x => x.NavigateToSpeaker)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));

            this.WhenAnyObservable(x => x.ThrownExceptions,
                                   x => x.NavigateToSpeaker.ThrownExceptions)
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

        public string UrlPathSegment
        {
            get { return "lecture"; }
        }

        public IScreen HostScreen { get; private set; }
    }
} 