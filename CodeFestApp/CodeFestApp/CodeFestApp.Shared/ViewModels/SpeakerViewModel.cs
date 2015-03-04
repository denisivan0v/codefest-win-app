using System;
using System.Collections.Generic;
using System.Linq;

using CodeFestApp.DataModel;
using CodeFestApp.DI;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class SpeakerViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly Speaker _speaker;
        private readonly IViewModelFactory<LectureViewModel> _lectureViewModelFactory;

        public SpeakerViewModel(IScreen hostScreen,
                                Speaker speaker,
                                IViewModelFactory<LectureViewModel> lectureViewModelFactory,
                                IScheduleReader scheduleReader)
        {
            HostScreen = hostScreen;
            _speaker = speaker;
            _lectureViewModelFactory = lectureViewModelFactory;

            _speaker.Lectures = scheduleReader.GetSpeakerLections(_speaker.Id);

            NavigateToLectureCommand = ReactiveCommand.Create();

            this.WhenAnyObservable(x => x.NavigateToLectureCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));
        }

        public string Title
        {
            get { return _speaker.Title; }
        }

        public string JobTitle
        {
            get { return _speaker.JobTitle; }
        }

        public string Company
        {
            get { return _speaker.Company.Title; }
        }
        
        public string Description
        {
            get { return _speaker.Description; }
        }
        
        public Uri Avatar
        {
            get { return _speaker.Avatar; }
        }

        public IEnumerable<IGrouping<string, LectureViewModel>> Lectures
        {
            get
            {
                return _speaker.Lectures
                               .Select(x => _lectureViewModelFactory.Create(x))
                               .OrderBy(x => x.Start)
                               .GroupBy(x => x.Start.ToString("f"));
            }
        }

        public ReactiveCommand<object> NavigateToLectureCommand { get; private set; }  

        public string UrlPathSegment
        {
            get { return "item"; }
        }

        public IScreen HostScreen { get; private set; }
    }
} 