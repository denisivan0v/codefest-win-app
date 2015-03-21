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
    public class TrackViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly Track _track;
        private readonly IViewModelFactory _viewModelFactory;

        public TrackViewModel(IScreen hostScreen, Track track, IViewModelFactory viewModelFactory, IAnalyticsLogger logger)
        {
            _track = track;
            _viewModelFactory = viewModelFactory;
            HostScreen = hostScreen;

            NavigateToLectureCommand = ReactiveCommand.Create();

            this.WhenAnyObservable(x => x.NavigateToLectureCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));

            this.WhenAnyObservable(x => x.ThrownExceptions,
                                   x => x.NavigateToLectureCommand.ThrownExceptions)
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Subscribe(logger.LogException);

            this.WhenNavigatedTo(() =>
                {
                    Task.Run(() => logger.LogViewModelRouted(this));
                    return Disposable.Empty;
                });
        }

        public string ConferenceTitle 
        {
            get { return "codefest 2015"; }
        }

        public string Title
        {
            get { return string.Format("{0}, {1}", _track.Title, _track.Room.Title); }
        }

        public string Color
        {
            get { return _track.Color; }
        }

        public string RoomTitle
        {
            get { return _track.Room.Title; }
        }
 
        public IEnumerable<IGrouping<string, LectureViewModel>> Lectures
        {
            get
            {
                return _track.Lectures
                             .Select(x => _viewModelFactory.Create<LectureViewModel, Lecture>(x))
                             .OrderBy(x => x.Start)
                             .GroupBy(x => x.Start.ToString("f"));
            }
        }

        public string UrlPathSegment
        {
            get { return "track"; }
        }

        public IScreen HostScreen { get; private set; }

        public ReactiveCommand<object> NavigateToLectureCommand { get; private set; }
    }
}