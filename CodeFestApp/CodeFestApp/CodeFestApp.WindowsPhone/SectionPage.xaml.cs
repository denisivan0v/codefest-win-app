using System;

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

            this.WhenAnyValue(x => x.ViewModel)
                .Subscribe(x => DataContext = x);

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
            set { ViewModel = (SectionViewModel)value; }
        }

        public SectionViewModel ViewModel { get; set; }
    }
}