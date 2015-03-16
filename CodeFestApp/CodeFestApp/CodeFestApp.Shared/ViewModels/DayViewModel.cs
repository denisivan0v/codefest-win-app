using System;
using System.Collections.Generic;
using System.Linq;

using CodeFestApp.DataModel;
using CodeFestApp.DI;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class DayViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly Day _day;

        private readonly IEnumerable<IGrouping<string, LectureViewModel>> _lectures;
        private readonly ReactiveList<Uri> _speakerAvatars;

        public DayViewModel(IScreen hostScreen, Day day, IViewModelFactory lectureViewModelFactory)
        {
            HostScreen = hostScreen;
            _day = day;

            _lectures = _day.Lectures
                            .Select(lectureViewModelFactory.Create<LectureViewModel, Lecture>)
                            .OrderBy(x => x.Start)
                            .GroupBy(x => x.Start.ToString("t"));

            _speakerAvatars = new ReactiveList<Uri>(_day.Lectures.SelectMany(x => x.Speakers).Select(x => x.Avatar));
            
            NavigateToLectureCommand = ReactiveCommand.Create();

            this.WhenAnyObservable(x => x.NavigateToLectureCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));
        }

        public string ConferenceTitle
        {
            get { return "codefest 2015"; }
        }

        public string Title
        {
            get { return _day.Title; }
        }

        public DateTime Date
        {
            get { return _day.Date; }
        }

        public ReactiveList<Uri> SpeakerAvatars
        {
            get
            {
                _speakerAvatars.Sort((x, y) => (new Random()).Next(0, 10));
                return _speakerAvatars;
            }
        }

        public IEnumerable<IGrouping<string, LectureViewModel>> Lectures
        {
            get { return _lectures; }
        }

        public ReactiveCommand<object> NavigateToLectureCommand { get; private set; }  

        public string UrlPathSegment
        {
            get { return "day"; }
        }

        public IScreen HostScreen { get; private set; }
    }
} 