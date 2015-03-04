using Microsoft.Practices.Unity;

namespace CodeFestApp.DI
{
    public class Lifetime
    {
        public static LifetimeManager Singleton
        {
            get { return new ContainerControlledLifetimeManager(); }
        }
    }
}
