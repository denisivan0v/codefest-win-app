using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using CodeFestApp.DataModel;
using CodeFestApp.DI;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class TrackViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly Track _track;
        private readonly IViewModelFactory _viewModelFactory;

        public TrackViewModel(IScreen hostScreen, Track track, IViewModelFactory viewModelFactory)
        {
            _track = track;
            _viewModelFactory = viewModelFactory;
            HostScreen = hostScreen;

            NavigateToLectureCommand = ReactiveCommand.Create();
        }

        public string Title
        {
            get { return _track.Title; }
        }

        public Color Color
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
                             .GroupBy(x => x.Start.ToString("t"));
            }
        }

        public string UrlPathSegment
        {
            get { return "section"; }
        }

        public IScreen HostScreen { get; private set; }

        public ReactiveCommand<object> NavigateToLectureCommand { get; private set; }
    }
}