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
            var container = new UnityContainer();
            var dependencyResolver = new UnityDependencyResolver(PerformRegister(container));
            Locator.Current = dependencyResolver;

            LogHost.Default.Level = LogLevel.Debug;

            var hubViewModel = container.Resolve<HubViewModel>();

            Router = new RoutingState();
            Router.Navigate.Execute(hubViewModel);
        }

        public RoutingState Router { get; private set; }

        private IUnityContainer PerformRegister(IUnityContainer container)
        {
            return container.RegisterInstance(typeof(IScreen), this, Lifetime.External)

                            .RegisterType<IScheduleSource, ScheduleSource>(Lifetime.Singleton)
                            .RegisterType<IScheduleReader, ScheduleReader>(Lifetime.Singleton)

                            .RegisterType(typeof(IViewModelFactory<>), typeof(UnityViewModelFactory<>), Lifetime.Singleton)

                            .RegisterType<IViewFor<HubViewModel>, HubPage>()
                            .RegisterType<IViewFor<TrackViewModel>, SectionPage>()
                            .RegisterType<IViewFor<ItemViewModel>, ItemPage>()
                            .RegisterType<IViewFor<DayViewModel>, DayView>()
                            .RegisterType<IViewFor<LectureViewModel>, LectureView>()
                            .RegisterType<IViewFor<SpeakerViewModel>, SpeakerView>();
        }
    }
}
