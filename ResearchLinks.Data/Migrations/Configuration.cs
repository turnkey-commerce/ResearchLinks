namespace ResearchLinks.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ResearchLinks.Data.Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<ResearchLinks.Data.ResearchLinksContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ResearchLinks.Data.ResearchLinksContext context)
        {
            //  This method will be called after migrating to the latest version.

            var roles = new List<Role>{
                new Role{RoleName = RoleConstants.RoleAdmin},
            };
            roles.ForEach(r => context.Roles.AddOrUpdate(r));

            // Create a user.
            Utility.CreateNewUser(context, "james", "james2013", "info@turnkey-commerce.com", RoleConstants.RoleAdmin, "James", "Culbertson");
        }
    }
}
