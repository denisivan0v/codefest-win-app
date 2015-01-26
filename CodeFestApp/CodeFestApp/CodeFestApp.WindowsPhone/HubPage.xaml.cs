using System;

using CodeFestApp.Data;
using CodeFestApp.ViewModels;

using ReactiveUI;

using Windows.Graphics.Display;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CodeFestApp
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : IViewFor<HubViewModel>
    {
        public HubPage()
        {
            InitializeComponent();

            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            NavigationCacheMode = NavigationCacheMode.Required;
            
            this.WhenAnyValue(x => x.ViewModel)
                .Subscribe(x => DataContext = x);

            this.WhenAnyObservable(x => x.ViewModel.NavigateToSectionCommand)
                .Subscribe(x =>
                    {
                        var args = (ItemClickEventArgs)x;
                        var sampleDataGroup = (SampleDataGroup)args.ClickedItem;
                        ViewModel.HostScreen.Router.Navigate.Execute(new SectionViewModel(ViewModel.HostScreen, sampleDataGroup));
                    });

            this.WhenAnyObservable(x => x.ViewModel.NavigateToItemCommand)
                .Subscribe(x =>
                {
                    var args = (ItemClickEventArgs)x;
                    var sampleDataGroup = (SampleDataItem)args.ClickedItem;
                    ViewModel.HostScreen.Router.Navigate.Execute(new ItemViewModel(ViewModel.HostScreen, sampleDataGroup));
                });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (HubViewModel)value; }
        }

        public HubViewModel ViewModel { get; set; }
    }
}