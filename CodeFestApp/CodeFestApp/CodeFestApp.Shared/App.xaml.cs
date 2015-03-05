using System;

using ReactiveUI;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

#if WINDOWS_PHONE_APP
using System.Reactive.Linq;
using Splat;
using Windows.Phone.UI.Input;
#endif

namespace CodeFestApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton instance of the <see cref="App"/> class. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();

            RxApp.SuspensionHost.CreateNewAppState = () => new AppBootstrapper();
            RxApp.SuspensionHost.SetupDefaultSuspendResume();

#if WINDOWS_PHONE_APP
            Observable.FromEventPattern<BackPressedEventArgs>(x => HardwareButtons.BackPressed += x,
                                                              x => HardwareButtons.BackPressed -= x)
                      .Subscribe(x =>
                      {
                          var hostScreen = (IScreen)Locator.Current.GetService(typeof(IScreen));
                          if (hostScreen.Router.NavigationStack.Count > 1)
                          {
                              hostScreen.Router.NavigateBack.Execute(null);
                              x.EventArgs.Handled = true;
                          }
                      });
#endif
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            new AutoSuspendHelper(this).OnLaunched(e);
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            var rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame { CacheSize = 3 };

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainWindow), e.Arguments);
            }
            
            // Ensure the current window is active
            Window.Current.Activate();
        }
    }
}