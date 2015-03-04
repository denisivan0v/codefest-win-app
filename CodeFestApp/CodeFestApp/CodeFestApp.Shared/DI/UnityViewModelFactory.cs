using Microsoft.Practices.Unity;

namespace CodeFestApp.DI
{
    public class UnityViewModelFactory<TViewModel> : IViewModelFactory<TViewModel>
    {
        private readonly IUnityContainer _container;

        public UnityViewModelFactory(IUnityContainer container)
        {
            _container = container;
        }

        public TViewModel Create<TParam>(TParam param)
        {
            return _container.Resolve<TViewModel>(new DependencyOverride(typeof(TParam), param));
        }
    }
}
