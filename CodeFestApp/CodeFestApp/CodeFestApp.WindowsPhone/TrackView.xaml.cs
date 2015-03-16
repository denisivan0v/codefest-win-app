using CodeFestApp.ViewModels;

using ReactiveUI;

using Windows.UI.Xaml.Controls;

namespace CodeFestApp
{
    public sealed partial class TrackView : IViewFor<TrackViewModel>
    {
        public TrackView()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .BindTo(this, x => x.DataContext);
        }
        
        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (TrackViewModel)value; }
        }

        public TrackViewModel ViewModel { get; set; }

        private void LecturesListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.NavigateToLectureCommand.Execute(e.ClickedItem);
        }
    }
}