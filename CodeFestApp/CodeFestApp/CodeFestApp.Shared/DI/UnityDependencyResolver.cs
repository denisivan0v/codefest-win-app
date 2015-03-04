using System;
using System.Collections.Generic;

using Microsoft.Practices.Unity;

using Splat;

namespace CodeFestApp.DI
{
    public class UnityDependencyResolver : IMutableDependencyResolver
    {
        private readonly IUnityContainer _container;

        public UnityDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        void IDisposable.Dispose()
        {
            _container.Dispose();
        }

        object IDependencyResolver.GetService(Type serviceType, string contract)
        {
            return _container.Resolve(serviceType);
        }

        IEnumerable<object> IDependencyResolver.GetServices(Type serviceType, string contract)
        {
            return _container.ResolveAll(serviceType);
        }

        void IMutableDependencyResolver.Register(Func<object> factory, Type serviceType, string contract)
        {
            var instance = factory();
            _container.RegisterInstance(serviceType, instance);
            _container.RegisterInstance(serviceType, instance.GetType().Name, instance);
        }

        IDisposable IMutableDependencyResolver.ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            throw new NotImplementedException();
        }
    }
}
