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
        private readonly IUnityContainer _container = new UnityContainer();

        public AppBootstrapper()
        {
            PerformRegister(_container);

            LogHost.Default.Level = LogLevel.Debug;
            Locator.Current = new UnityDependencyResolver(_container);
            Router = new RoutingState();

            ReadSchedule();
            NavigateToHub();
        }

        public RoutingState Router { get; private set; }
        public bool IsLoading { get; set; }

        private void PerformRegister(IUnityContainer container)
        {
            container.RegisterInstance(typeof(IScreen), this, Lifetime.External)

                     .RegisterType<IScheduleSource, ScheduleSource>(Lifetime.Singleton)
                     .RegisterType<IScheduleReader, ScheduleReader>(Lifetime.Singleton)

                     .RegisterType<IViewModelFactory, UnityViewModelFactory>(Lifetime.Singleton)

                     .RegisterType<IViewFor<HubViewModel>, HubView>()
                     .RegisterType<IViewFor<TrackViewModel>, SectionPage>()
                     .RegisterType<IViewFor<ItemViewModel>, ItemPage>()
                     .RegisterType<IViewFor<DayViewModel>, DayView>()
                     .RegisterType<IViewFor<LectureViewModel>, LectureView>()
                     .RegisterType<IViewFor<SpeakerViewModel>, SpeakerView>();
        }

        private void ReadSchedule()
        {
            IsLoading = true;

            var scheduleReader = _container.Resolve<IScheduleReader>();
            scheduleReader.ReadSchedule().Wait();

            IsLoading = false;
        }

        private void NavigateToHub()
        {
            var hubViewModel = _container.Resolve<HubViewModel>();
            Router.Navigate.Execute(hubViewModel);
        }
    }
}
