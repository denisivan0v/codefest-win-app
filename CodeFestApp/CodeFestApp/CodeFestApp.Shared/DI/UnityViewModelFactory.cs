using Microsoft.Practices.Unity;

using ReactiveUI;

namespace CodeFestApp.DI
{
    public class UnityViewModelFactory : IViewModelFactory
    {
        private readonly IUnityContainer _container;

        public UnityViewModelFactory(IUnityContainer container)
        {
            _container = container;
        }

        public TViewModel Create<TViewModel, TModel>(TModel param) where TViewModel : IRoutableViewModel
        {
            return _container.Resolve<TViewModel>(new DependencyOverride(typeof(TModel), param));
        }
    }
}
