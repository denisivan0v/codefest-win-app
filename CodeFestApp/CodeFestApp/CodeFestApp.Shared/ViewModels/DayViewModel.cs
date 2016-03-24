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
    public class DayViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly Day _day;
        private readonly ObservableAsPropertyHelper<IEnumerable<IGrouping<string, LectureViewModel>>> _lectures;
        
        public DayViewModel(IScreen hostScreen,
                            Day day,
                            IViewModelFactory lectureViewModelFactory,
                            IAnalyticsLogger logger)
        {
            HostScreen = hostScreen;
            _day = day;

            NavigateToLectureCommand = ReactiveCommand.Create();
            
            LoadLectures = ReactiveCommand.CreateAsyncTask(_ => Task.Run(
                () => _day.Lectures
                          .Select(lectureViewModelFactory.Create<LectureViewModel, Lecture>)
                          .OrderBy(x => x.Start)
                          .GroupBy(x => x.Start.ToString("t"))
                          .AsEnumerable()));

            this.WhenAnyObservable(x => x.NavigateToLectureCommand)
                .Subscribe(x => HostScreen.Router.Navigate.ExecuteAsyncTask(x));

            this.WhenAnyObservable(x => x.LoadLectures)
                .ToProperty(this, x => x.Lectures, out _lectures);

            this.WhenAnyObservable(x => x.ThrownExceptions,
                                   x => x.LoadLectures.ThrownExceptions,
                                   x => x.NavigateToLectureCommand.ThrownExceptions)
                .SubscribeOn(RxApp.TaskpoolScheduler)
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
            get { return _day.Title; }
        }

        public DateTime Date
        {
            get { return _day.Date; }
        }

        public IEnumerable<Uri> SpeakerAvatars
        {
            get { return _day.Lectures.SelectMany(x => x.Speakers).Select(x => x.Avatar); }
        }

        public IEnumerable<IGrouping<string, LectureViewModel>> Lectures
        {
            get { return _lectures.Value; }
        }

        public ReactiveCommand<object> NavigateToLectureCommand { get; private set; }
        public ReactiveCommand<IEnumerable<IGrouping<string, LectureViewModel>>> LoadLectures { get; private set; }

        public string UrlPathSegment
        {
            get { return "day"; }
        }

        public IScreen HostScreen { get; private set; }
    }
} 