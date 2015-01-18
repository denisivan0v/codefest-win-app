using System;
using System.Reactive.Linq;

using CodeFestApp.Data;
using CodeFestApp.ViewModels;

using ReactiveUI;

using Windows.UI.Xaml.Controls;

namespace CodeFestApp
{
    public sealed partial class SectionListView : IViewFor<SectionListViewModel>
    {
        public SectionListView()
        {
            InitializeComponent();
            Sections.Events().ItemClick
                    .Select(x => (SampleDataGroup)x.ClickedItem)
                    .Subscribe(x => ViewModel.NavigateToSectionCommand.Execute(new SectionViewModel(ViewModel.HostScreen, x)));
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (SectionListViewModel)value; }
        }

        public SectionListViewModel ViewModel { get; set; }
    }
}
