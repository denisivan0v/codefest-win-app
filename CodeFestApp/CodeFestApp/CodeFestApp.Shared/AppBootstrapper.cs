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

            LogHost.Default.Level = LogLevel.Debug;
            Locator.Current = new UnityDependencyResolver(_container);
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

                     .RegisterType<IAnalyticsLogger, FlurryAnalyticsWrapper>(Lifetime.Singleton)

                     .RegisterType<IScheduleSource, ScheduleSource>(Lifetime.Singleton)
                     .RegisterType<IScheduleReader, ScheduleReader>(Lifetime.Singleton)

                     .RegisterType<IViewModelFactory, UnityViewModelFactory>(Lifetime.Singleton)

                     .RegisterType<IViewFor<HubViewModel>, HubView>()
                     .RegisterType<IViewFor<DayViewModel>, DayView>()
                     .RegisterType<IViewFor<TrackViewModel>, TrackView>()
                     .RegisterType<IViewFor<LectureViewModel>, LectureView>()
                     .RegisterType<IViewFor<SpeakerViewModel>, SpeakerView>()
                     .RegisterType<IViewFor<TweetsViewModel>, TweetsView>();
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
