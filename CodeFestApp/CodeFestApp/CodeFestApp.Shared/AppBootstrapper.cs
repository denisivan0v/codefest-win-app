using System;

using CodeFestApp.Analytics;
using CodeFestApp.DataModel;
using CodeFestApp.DI;
using CodeFestApp.ViewModels;

using Microsoft.Practices.Unity;

using ReactiveUI;

using Splat;

namespace CodeFestApp
{
    public class AppBootstrapper : ReactiveObject, IScreen, IDisposable
    {
        private readonly IUnityContainer _container = new UnityContainer();

        public AppBootstrapper()
        {
            PerformRegister(_container);

            Locator.Current = new UnityDependencyResolver(_container);
            LogHost.Default.Level = LogLevel.Debug;
            
            Router = new RoutingState();

            StartAnalyticsSession();
            ReadSchedule();
            NavigateToHub();
        }

        public RoutingState Router { get; private set; }

        public void Dispose()
        {
            _container.Dispose();
        }

        private void PerformRegister(IUnityContainer container)
        {
            container.RegisterInstance(typeof(IScreen), this, Lifetime.External)
                     .RegisterInstance(new TwitterKeys(), Lifetime.Singleton)
                     .RegisterInstance(new FlurryKey(), Lifetime.Singleton)
                     .RegisterInstance(new MobileServicesClientFactory(), Lifetime.Singleton)

                     .RegisterType<IAnalyticsLogger, FlurryAnalyticsWrapper>(Lifetime.Singleton)

                     .RegisterType<IScheduleSource, ScheduleSource>(Lifetime.Singleton)
                     .RegisterType<IScheduleReader, ScheduleReader>(Lifetime.Singleton)

                     .RegisterType<IViewModelFactory, UnityViewModelFactory>(Lifetime.Singleton)

                     .RegisterType<IViewFor<HubViewModel>, HubView>(Lifetime.Singleton)
                     .RegisterType<IViewFor<DayViewModel>, DayView>(Lifetime.Transient)
                     .RegisterType<IViewFor<TrackViewModel>, TrackView>(Lifetime.Transient)
                     .RegisterType<IViewFor<LectureViewModel>, LectureView>(Lifetime.Transient)
                     .RegisterType<IViewFor<SpeakerViewModel>, SpeakerView>(Lifetime.Transient)
                     .RegisterType<IViewFor<TweetsViewModel>, TweetsView>(Lifetime.Transient)
                     .RegisterType<IViewFor<AboutViewModel>, AboutView>(Lifetime.Transient);
        }

        private void StartAnalyticsSession()
        {
            var analytics = _container.Resolve<IAnalyticsLogger>();
            analytics.StartSession();
        }

        private void ReadSchedule()
        {
            var scheduleReader = _container.Resolve<IScheduleReader>();
            scheduleReader.ReadSchedule().Wait();
        }

        private void NavigateToHub()
        {
            var hubViewModel = _container.Resolve<HubViewModel>();
            Router.Navigate.Execute(hubViewModel);
        }
    }
}
