using System;

using CodeFestApp.DataModel;
using CodeFestApp.DI;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class LectureViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly IViewModelFactory<SpeakerViewModel> _speakerViewModelFactory;
        private readonly IViewModelFactory<DayViewModel> _dayViewModelFactory;
        private readonly IViewModelFactory<TrackViewModel> _trackViewModelFactory;
        private readonly Lecture _lecture;

        public LectureViewModel(IScreen hostScreen,
                                Lecture lecture,
                                IViewModelFactory<SpeakerViewModel> speakerViewModelFactory,
                                IViewModelFactory<DayViewModel> dayViewModelFactory,
                                IViewModelFactory<TrackViewModel> trackViewModelFactory)
        {
            HostScreen = hostScreen;
            _lecture = lecture;
            _speakerViewModelFactory = speakerViewModelFactory;
            _dayViewModelFactory = dayViewModelFactory;
            _trackViewModelFactory = trackViewModelFactory;

            NavigateToSpeaker = ReactiveCommand.Create();

            this.WhenAnyObservable(x => x.NavigateToSpeaker)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));
        }

        public TrackViewModel Track
        {
            get { return _trackViewModelFactory.Create(_lecture.Track); }
        }

        public string ConferenceTitle
        {
            get { return "codefest 2015"; }
        }

        public string Title
        {
            get { return _lecture.Title; }
        }

        public SpeakerViewModel Speaker
        {
            get { return _speakerViewModelFactory.Create(_lecture.Speaker); }
        }
        
        public string Description
        {
            get { return _lecture.Description; }
        }

        public DayViewModel Day
        {
            get { return _dayViewModelFactory.Create(_lecture.Day); }
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