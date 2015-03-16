using System;

using CodeFestApp.ViewModels;

using ReactiveUI;

using Windows.UI.Xaml.Controls;

namespace CodeFestApp
{
    public sealed partial class SpeakerView : IViewFor<SpeakerViewModel>
    {
        public SpeakerView()
        {
            InitializeComponent();
            
            this.WhenAnyValue(x => x.ViewModel)
                .Subscribe(x => DataContext = x);
        } 

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (SpeakerViewModel)value; }
        }

        public SpeakerViewModel ViewModel { get; set; }

        private void LecturesListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.NavigateToLectureCommand.Execute(e.ClickedItem);
        }
    }
}