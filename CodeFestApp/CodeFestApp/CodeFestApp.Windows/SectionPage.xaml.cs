using System;

using CodeFestApp.ViewModels;

using ReactiveUI;

namespace CodeFestApp
{
    public sealed partial class SectionPage : IViewFor<TrackViewModel>
    {
        public SectionPage()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .Subscribe(x => DataContext = x);

            this.BindCommand(ViewModel, x => x.HostScreen.Router.NavigateBack, x => x.GoBackButton);
        }
        
        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (TrackViewModel)value; }
        }

        public TrackViewModel ViewModel { get; set; }
    }
}