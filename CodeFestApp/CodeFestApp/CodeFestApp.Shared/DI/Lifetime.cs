using Microsoft.Practices.Unity;

namespace CodeFestApp.DI
{
    public class Lifetime
    {
        public static LifetimeManager Singleton
        {
            get { return new ContainerControlledLifetimeManager(); }
        }

        public static LifetimeManager External
        {
            get { return new ExternallyControlledLifetimeManager(); }
        }
    }
}
