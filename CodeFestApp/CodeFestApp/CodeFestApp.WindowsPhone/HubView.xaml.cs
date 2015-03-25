using System;
using System.Reactive.Linq;

using CodeFestApp.ViewModels;

using ReactiveUI;

using Windows.Graphics.Display;
using Windows.UI.Xaml.Controls;

namespace CodeFestApp
{
    public sealed partial class HubView : IViewFor<HubViewModel>, IDisposable
    {
        private readonly IDisposable _dataContextSub;
        private readonly IDisposable _loadDaysSub;
        private readonly IDisposable _loadTracksSub;
        private readonly IDisposable _loadSpeakersSub;

        public HubView()
        {
            InitializeComponent();

            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            _dataContextSub = this.WhenAnyValue(x => x.ViewModel)
                                  .BindTo(this, x => x.DataContext);

            _loadDaysSub = this.WhenAnyValue(x => x.ViewModel.LoadDaysCommand)
                               .ObserveOn(RxApp.TaskpoolScheduler)
                               .Subscribe(x => x.ExecuteAsyncTask());

            _loadTracksSub = this.WhenAnyValue(x => x.ViewModel.LoadTracksCommand)
                                 .ObserveOn(RxApp.TaskpoolScheduler)
                                 .Subscribe(x => x.ExecuteAsyncTask());

            _loadSpeakersSub = this.WhenAnyValue(x => x.ViewModel.LoadSpeakersCommand)
                                   .ObserveOn(RxApp.TaskpoolScheduler)
                                   .Subscribe(x => x.ExecuteAsyncTask());
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (HubViewModel)value; }
        }

        public HubViewModel ViewModel { get; set; }

        public void Dispose()
        {
            _dataContextSub.Dispose();
            _loadDaysSub.Dispose();
            _loadTracksSub.Dispose();
            _loadSpeakersSub.Dispose();
        }

        private void DaysGridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.NavigateToDayCommand.Execute(e.ClickedItem);
        }

        private void TracksListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.NavigateToTrackCommand.Execute(e.ClickedItem);
        }
        
        private void SpeakersGridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.NavigateToSpeakerCommand.Execute(e.ClickedItem);
        }

        private void Hub_OnSectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {
            var hub = (Hub)sender;
            var activeSection = hub.SectionsInView[0];
            ViewModel.ActiveSection = hub.Sections.IndexOf(activeSection);
        }
    }
}