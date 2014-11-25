using System;
using System.Reactive;

using CodeFestApp.Data;
using CodeFestApp.ViewModels;

using ReactiveUI;

using Windows.UI.Xaml.Controls;

namespace CodeFestApp
{
    public sealed partial class SectionPage : IViewFor<SectionViewModel>
    {
        public SectionPage()
        {
            InitializeComponent();

            this.Bind(ViewModel, x => x.Group, x => x.Container.DataContext);
            this.BindCommand(ViewModel, x => x.NavigateToItemCommand, x => x.ItemGridView, "ItemClick");

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
            set { ViewModel = (SectionViewModel)value; }
        }

        public SectionViewModel ViewModel { get; set; }
    }
}