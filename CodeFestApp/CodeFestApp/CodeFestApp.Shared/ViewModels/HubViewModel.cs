using System;
using System.Linq;
using System.Reactive.Disposables;

using CodeFestApp.DataModel;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class HubViewModel : ReactiveObject, IRoutableViewModel
    {
        public HubViewModel(IScreen screen, IScheduleReader scheduleReader)
        {
            HostScreen = screen;
            NavigateToDayCommand = ReactiveCommand.Create();

            this.WhenAnyObservable(x => x.NavigateToDayCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));

            var days = scheduleReader.GetDaysAsync();
            days.Wait();

            Days = new ReactiveList<DayViewModel>(days.Result.Select(x => new DayViewModel(HostScreen, x)));
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