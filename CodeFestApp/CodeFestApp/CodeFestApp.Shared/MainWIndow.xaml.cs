using System;

using ReactiveUI;

using Splat;

using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace CodeFestApp
{
    public sealed partial class MainWindow : Page
    {
        public MainWindow()
        {
            InitializeComponent();
            HideStatusBar();

            DataContext = Locator.Current.GetService(typeof(IScreen));
        }

        private static async void HideStatusBar()
        {
            var statusBar = StatusBar.GetForCurrentView();
            await statusBar.HideAsync();
        }
    }
}
