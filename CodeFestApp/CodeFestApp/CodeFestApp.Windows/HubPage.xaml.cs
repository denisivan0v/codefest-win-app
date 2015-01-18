using System;
using System.Reactive;

using CodeFestApp.Data;
using CodeFestApp.ViewModels;

using ReactiveUI;

using Windows.UI.Xaml.Controls;

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

            this.Bind(ViewModel, x => x.Section3Items, x => x.Section3.DataContext);
            this.BindCommand(ViewModel, x => x.NavigateToSectionCommand, x => x.Hub, "SectionHeaderClick");
            
            // this.BindCommand(ViewModel, x => x.NavigateToItemCommand, x => x.Section3.ContentTemplate.LoadContent(), "ItemClick");

            this.WhenAnyObservable(x => x.ViewModel.NavigateToSectionCommand)
                .Subscribe(x =>
                    {
                        var eventPattern = (EventPattern<HubSectionHeaderClickEventArgs>)x;

                        var sampleDataGroup = (SampleDataGroup)eventPattern.EventArgs.Section.DataContext;
                        ViewModel.HostScreen.Router.Navigate.Execute(new SectionViewModel(ViewModel.HostScreen, sampleDataGroup));
                    });

            this.WhenAnyObservable(x => x.ViewModel.NavigateToItemCommand)
                .Subscribe(x =>
                {
                    var eventPattern = (EventPattern<ItemClickEventArgs>)x;

                    var sampleDataItem = (SampleDataItem)eventPattern.EventArgs.ClickedItem;
                    ViewModel.HostScreen.Router.Navigate.Execute(new ItemViewModel(ViewModel.HostScreen, sampleDataItem));
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
