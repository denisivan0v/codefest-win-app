using System;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<LectureViewModel> Lectures
        {
            get { return _day.Lectures.Select(x => new LectureViewModel(HostScreen, x)); }
        }

        public string UrlPathSegment
        {
            get { return "item"; }
        }

        public IScreen HostScreen { get; private set; }
    }
} 