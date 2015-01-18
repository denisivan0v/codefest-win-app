using CodeFestApp.ViewModels;

using ReactiveUI;

using Splat;

namespace CodeFestApp
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public AppBootstrapper(IMutableDependencyResolver dependencyResolver = null, RoutingState router = null)
        {
            Router = router ?? new RoutingState();
            dependencyResolver = dependencyResolver ?? Locator.CurrentMutable;

            RegisterParts(dependencyResolver);

            LogHost.Default.Level = LogLevel.Debug;

            Router.Navigate.Execute(new HubViewModel(this));
        }

        public RoutingState Router { get; private set; }

        private void RegisterParts(IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterConstant(this, typeof(IScreen));

            dependencyResolver.Register(() => new HubPage(), typeof(IViewFor<HubViewModel>));
#if WINDOWS_PHONE_APP
            dependencyResolver.Register(() => new SectionListView(), typeof(IViewFor<SectionListViewModel>));
#endif
            dependencyResolver.Register(() => new SectionPage(), typeof(IViewFor<SectionViewModel>));
            dependencyResolver.Register(() => new ItemPage(), typeof(IViewFor<ItemViewModel>));
        }
    }
}
