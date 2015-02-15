using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using CodeFestApp.Data;
using CodeFestApp.DataModel;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class HubViewModel : ReactiveObject, IRoutableViewModel
    {
        private SampleDataGroup _groupToNavigate;

        public HubViewModel(IScreen screen, IScheduleSource scheduleSource)
        {
            HostScreen = screen;
            NavigateToSectionCommand = ReactiveCommand.Create();
            NavigateToDayCommand = ReactiveCommand.Create();

            this.WhenAnyValue(x => x.GroupToNavigate)
                .Where(x => x != null)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(new SectionViewModel(HostScreen, x)));

            this.WhenAnyObservable(x => x.NavigateToDayCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));

            this.WhenNavigatedTo(() =>
                {
                    SetDaysViewModel(scheduleSource);
                    return Disposable.Empty;
                });
        }

        public ReactiveCommand<object> NavigateToSectionCommand { get; private set; }
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

        public SampleDataGroup GroupToNavigate 
        {
            get { return _groupToNavigate; }
            set { this.RaiseAndSetIfChanged(ref _groupToNavigate, value); }
        }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment
        {
            get { return "hubpage"; }
        }

        private async void SetDaysViewModel(IScheduleSource scheduleSource)
        {
            var scheduleJson = await scheduleSource.ReadScheduleAsync();
            var scheduleReader = new ScheduleReader(scheduleJson);
            var days = await scheduleReader.GetDaysAsync();
            Days = new ReactiveList<DayViewModel>(days.Select(x => new DayViewModel(HostScreen, x)));
        }
    }
}