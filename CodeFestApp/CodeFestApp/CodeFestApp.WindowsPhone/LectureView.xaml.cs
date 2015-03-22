using System;

using CodeFestApp.ViewModels;

using ReactiveUI;

using Windows.UI.Xaml.Controls;

namespace CodeFestApp
{
    public sealed partial class LectureView : IViewFor<LectureViewModel>
    {
        public LectureView()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .Subscribe(x => DataContext = x);

            this.WhenAnyValue(x => x.ViewModel.LoadLectureAttitude)
                .Subscribe(x => x.ExecuteAsyncTask());
        } 

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (LectureViewModel)value; }
        }

        public LectureViewModel ViewModel { get; set; }

        private void SpeakersListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.NavigateToSpeaker.Execute(e.ClickedItem);
        }
    }
}