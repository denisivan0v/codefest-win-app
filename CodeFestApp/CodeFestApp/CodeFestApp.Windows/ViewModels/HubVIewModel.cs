using ReactiveUI;

using Splat;

namespace CodeFestApp.ViewModels
{
    public class HubViewModel : ReactiveObject, IScreen
    {
        public HubViewModel(IMutableDependencyResolver dependencyResolver, RoutingState routingState)
        {
            Router = routingState ?? new RoutingState();
            RegisterParts(dependencyResolver ?? Locator.CurrentMutable);
        }

        public HubViewModel() : this(null, null)
        {
        }

        public RoutingState Router { get; private set; }

        private void RegisterParts(IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterConstant(this, typeof(IScreen));
            // dependencyResolver.Register(() => new WelcomeView(), typeof(IViewFor<WelcomeViewModel>));
        }
    }
}