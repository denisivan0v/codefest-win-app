using System.Data.Entity.Migrations;

namespace CodeFestApp.MobileService.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Models.MobileServiceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "CodeFestApp.MobileService.Models.MobileServiceContext";
        }
    }
}
