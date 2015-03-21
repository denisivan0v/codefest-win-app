using System;

using CodeFestApp.Analytics;

using Microsoft.WindowsAzure.MobileServices;

using ReactiveUI;

using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
#if WINDOWS_PHONE_APP
using System.Reactive.Linq;
using Splat;
using Windows.Phone.UI.Input;
#endif

namespace CodeFestApp
{
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        // http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x409
        public static MobileServiceClient CodefestappClient =
            new MobileServiceClient(MobileServicesSettings.Url, MobileServicesSettings.Key);
#endif

        private readonly AppBootstrapper _appBootstrapper = new AppBootstrapper();

        public App()
        {
            InitializeComponent();

            var logger = (IAnalyticsLogger)Locator.Current.GetService(typeof(IAnalyticsLogger));
            RxApp.SuspensionHost.CreateNewAppState = () => _appBootstrapper;
            RxApp.SuspensionHost.IsResuming.Subscribe(_ => logger.StartSession());
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

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            new AutoSuspendHelper(this).OnLaunched(e);
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            var hostScreen = (IScreen)Locator.Current.GetService(typeof(IScreen));
            Window.Current.Content = new RoutedViewHost
                {
                    DataContext = hostScreen,
                    Router = hostScreen.Router
                };
            
            // Ensure the current window is active
            Window.Current.Activate();
        }
    }
}