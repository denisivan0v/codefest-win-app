using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using CodeFestApp.DataModel;
using CodeFestApp.DI;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class HubViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly ObservableAsPropertyHelper<IEnumerable<DayViewModel>> _days;

        public HubViewModel(IScreen screen, IScheduleReader scheduleReader, IViewModelFactory viewModelFactory)
        {
            HostScreen = screen;

            NavigateToDayCommand = ReactiveCommand.Create();
            NavigateToTwitterFeedCommand = ReactiveCommand.Create();

            LoadDaysCommand = ReactiveCommand.CreateAsyncTask(_ => Task.Run(
                () =>
                    {
                        var days = scheduleReader.GetDays();
                        return days.Select(viewModelFactory.Create<DayViewModel, Day>);
                    }));

            this.WhenAnyObservable(x => x.NavigateToDayCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));

            this.WhenAnyObservable(x => x.NavigateToTwitterFeedCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(new TweetsViewModel(HostScreen)));

            this.WhenAnyObservable(x => x.LoadDaysCommand)
                .ToProperty(this, x => x.Days, out _days);
        }

        public ReactiveCommand<object> NavigateToDayCommand { get; private set; }
        public ReactiveCommand<object> NavigateToTwitterFeedCommand { get; private set; }
        public ReactiveCommand<IEnumerable<DayViewModel>> LoadDaysCommand { get; private set; }

        public int ActiveSection { get; set; }

        public string CurrentLecturesSectionTitle
        {
            get { return "ИДЕТ СЕЙЧАС"; }
        }

        public string DaysSectionTitle
        {
            get { return "ДНИ КОНФЕРЕНЦИИ"; }
        }

        public Uri TwitterIcon
        {
            get { return new Uri("ms-appx:///Assets/TwitterIcon.png"); }
        }

        public IEnumerable<DayViewModel> Days
        {
            get { return _days.Value; }
        }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment
        {
            get { return "hubpage"; }
        }
    }
}