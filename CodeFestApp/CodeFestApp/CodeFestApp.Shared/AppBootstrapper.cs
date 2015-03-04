using CodeFestApp.DataModel;
using CodeFestApp.DI;
using CodeFestApp.ViewModels;

using Microsoft.Practices.Unity;

using ReactiveUI;

using Splat;

namespace CodeFestApp
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public AppBootstrapper()
        {
            var container = PerformRegister();
            var dependencyResolver = new UnityDependencyResolver(container);
            Locator.Current = dependencyResolver;

            LogHost.Default.Level = LogLevel.Debug;

            var hubViewModel = container.Resolve<HubViewModel>();

            Router = new RoutingState();
            Router.Navigate.Execute(hubViewModel);
        }

        public RoutingState Router { get; private set; }

        private IUnityContainer PerformRegister()
        {
            var container = new UnityContainer();

            return container.RegisterInstance(typeof(IScreen), this, new ExternallyControlledLifetimeManager())

                            .RegisterType<IViewFor<HubViewModel>, HubPage>()
                            .RegisterType<IViewFor<TrackViewModel>, SectionPage>()
                            .RegisterType<IViewFor<ItemViewModel>, ItemPage>()
                            .RegisterType<IViewFor<DayViewModel>, DayView>()
                            .RegisterType<IViewFor<LectureViewModel>, LectureView>()

                            .RegisterType<IScheduleSource, ScheduleSource>(new ContainerControlledLifetimeManager())
                            .RegisterType<IScheduleReader, ScheduleReader>(new ContainerControlledLifetimeManager());
        }
    }
}
