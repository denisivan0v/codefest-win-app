using System;

using CodeFestApp.DataModel;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class DayViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly Day _day;

        public DayViewModel(IScreen hostScreen, Day day)
        {
            _day = day;
            HostScreen = hostScreen;
        }

        public string Title
        {
            get { return _day.Title; }
        }

        public DateTime Date
        {
            get { return _day.Date; }
        }

        public string UrlPathSegment
        {
            get { return "item"; }
        }

        public IScreen HostScreen { get; private set; }
    }
} 