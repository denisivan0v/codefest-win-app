using System;
using System.Linq;
using System.Reactive.Linq;

using CodeFestApp.Data;
using CodeFestApp.ViewModels;

using ReactiveUI;

using Windows.Graphics.Display;
using Windows.UI.Xaml.Controls;

namespace CodeFestApp
{
    public sealed partial class HubPage : IViewFor<HubViewModel>
    {
        public HubPage()
        {
            InitializeComponent();

            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            this.WhenAnyValue(x => x.ViewModel)
                .Subscribe(x => DataContext = x);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (HubViewModel)value; }
        }

        public HubViewModel ViewModel { get; set; }

        private void DaysGrid_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.NavigateToDayCommand.Execute(e.ClickedItem);
        }

        private void Hub_OnSectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {
            var hub = (Hub)sender;
            var activeSection = hub.SectionsInView[0];
            ViewModel.ActiveSection = hub.Sections.IndexOf(activeSection);
        }
    }
}