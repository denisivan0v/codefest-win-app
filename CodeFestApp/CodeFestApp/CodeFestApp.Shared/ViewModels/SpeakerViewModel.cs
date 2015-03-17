using System;
using System.Collections.Generic;
using System.Linq;

using CodeFestApp.Analytics;
using CodeFestApp.DataModel;
using CodeFestApp.DI;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class SpeakerViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly Speaker _speaker;
        private readonly IViewModelFactory _viewModelFactory;

        public SpeakerViewModel(IScreen hostScreen,
                                Speaker speaker,
                                IViewModelFactory viewModelFactory,
                                IAnalyticsLogger logger)
        {
            HostScreen = hostScreen;
            _speaker = speaker;
            _viewModelFactory = viewModelFactory;

            NavigateToLectureCommand = ReactiveCommand.Create();

            this.WhenAnyObservable(x => x.NavigateToLectureCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));

            this.WhenAnyObservable(x => x.NavigateToLectureCommand.ThrownExceptions)
                .Subscribe(logger.LogException);

            this.WhenNavigatedTo(() => logger.LogViewModelRouted(this));
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
                               .Select(x => _viewModelFactory.Create<LectureViewModel, Lecture>(x))
                               .OrderBy(x => x.Start)
                               .GroupBy(x => x.Start.ToString("f"));
            }
        }

        public ReactiveCommand<object> NavigateToLectureCommand { get; private set; }  

        public string UrlPathSegment
        {
            get { return "speaker"; }
        }

        public IScreen HostScreen { get; private set; }
    }
} 