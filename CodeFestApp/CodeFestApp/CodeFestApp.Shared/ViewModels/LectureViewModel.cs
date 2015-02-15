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

        public string Title
        {
            get { return _lecture.Title; }
        }

        public string Description
        {
            get { return _lecture.Description; }
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
            get { return "item"; }
        }

        public IScreen HostScreen { get; private set; }
    }
} 