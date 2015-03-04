using System;

using CodeFestApp.DataModel;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class LectureViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly Lecture _lecture;

        public LectureViewModel(IScreen hostScreen, Lecture lecture)
        {
            _lecture = lecture;
            HostScreen = hostScreen;
        }

        public TrackViewModel Track
        {
            get { return new TrackViewModel(HostScreen, _lecture.Track); }
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
            get { return new SpeakerViewModel(HostScreen, _lecture.Speaker); }
        }
        
        public string Description
        {
            get { return _lecture.Description; }
        }

        public DayViewModel Day
        {
            get { return new DayViewModel(HostScreen, _lecture.Day); }
        }

        public DateTime Start
        {
            get { return _lecture.Start; }
        }
        
        public DateTime End
        {
            get { return _lecture.End; }
        }

        public string UrlPathSegment
        {
            get { return "lecture"; }
        }

        public IScreen HostScreen { get; private set; }
    }
} 