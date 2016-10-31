using System.Data.Entity.Migrations;

namespace Jeuci.WeChatApp.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<WeChatApp.EntityFramework.WeChatAppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "WeChatApp";
        }

        protected override void Seed(WeChatApp.EntityFramework.WeChatAppDbContext context)
        {
            // This method will be called every time after migrating to the latest version.
            // You can add any seed data here...
        }
    }
}
