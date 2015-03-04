using System;
using System.Linq;

using CodeFestApp.DataModel;
using CodeFestApp.DI;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class HubViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly IViewModelFactory<DayViewModel> _dayViewModelFactory;

        public HubViewModel(IScreen screen, IScheduleReader scheduleReader, IViewModelFactory<DayViewModel> dayViewModelFactory)
        {
            _dayViewModelFactory = dayViewModelFactory;
            HostScreen = screen;
            NavigateToDayCommand = ReactiveCommand.Create();

            this.WhenAnyObservable(x => x.NavigateToDayCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));

            var days = scheduleReader.GetDaysAsync();
            days.Wait();

            Days = new ReactiveList<DayViewModel>(days.Result.Select(x => _dayViewModelFactory.Create(x)));
        }

        public ReactiveCommand<object> NavigateToDayCommand { get; private set; }

        public int ActiveSection { get; set; }

        public string CurrentLecturesSectionTitle
        {
            get { return "ИДЕТ СЕЙЧАС"; }
        }

        public string DaysSectionTitle
        {
            get { return "ДНИ КОНФЕРЕНЦИИ"; }
        }

        public ReactiveList<DayViewModel> Days { get; private set; }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment
        {
            get { return "hubpage"; }
        }
    }
}