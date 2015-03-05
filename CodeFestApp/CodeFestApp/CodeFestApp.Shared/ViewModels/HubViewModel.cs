using System;
using System.Linq;

using CodeFestApp.DataModel;
using CodeFestApp.DI;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class HubViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;

        public HubViewModel(IScreen screen, IScheduleReader scheduleReader, IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            HostScreen = screen;
            NavigateToDayCommand = ReactiveCommand.Create();

            this.WhenAnyObservable(x => x.NavigateToDayCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));

            var days = scheduleReader.GetDays();
            Days = new ReactiveList<DayViewModel>(days.Select(x => _viewModelFactory.Create<DayViewModel, Day>(x)));
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