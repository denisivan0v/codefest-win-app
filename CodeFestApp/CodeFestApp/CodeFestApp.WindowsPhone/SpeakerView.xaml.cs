using System;
using System.Threading.Tasks;

using CodeFestApp.ViewModels;

using ReactiveUI;

using Windows.UI.ViewManagement;

namespace CodeFestApp
{
    public sealed partial class SpeakerView : IViewFor<SpeakerViewModel>
    {
        public SpeakerView()
        {
            InitializeComponent();
            HideStatusBar();
            
            this.WhenAnyValue(x => x.ViewModel)
                .Subscribe(x => DataContext = x);
        } 

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (SpeakerViewModel)value; }
        }

        public SpeakerViewModel ViewModel { get; set; }

        private static async void HideStatusBar()
        {
            var statusBar = StatusBar.GetForCurrentView();
            await statusBar.HideAsync();
        }
    }
}