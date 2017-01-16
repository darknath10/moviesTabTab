namespace MoviesTabTab.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MoviesTabTab.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MoviesTabTab.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //context.Halls.AddOrUpdate(
            //    p => p.HallName,
            //    new Hall { HallName = "A", SeatsCapacity = 100 },
            //    new Hall { HallName = "B", SeatsCapacity = 80 },
            //    new Hall { HallName = "C", SeatsCapacity = 60 },
            //    new Hall { HallName = "D", SeatsCapacity = 40 },
            //    new Hall { HallName = "E", SeatsCapacity = 30}
            //    );

            //var roleStore = new RoleStore<IdentityRole>(context);
            //var roleManager = new RoleManager<IdentityRole>(roleStore);

            //string[] roles = { "admin", "customer" };

            //foreach (var role in roles)
            //{
            //    if (!roleManager.RoleExists(role))
            //    {
            //        roleManager.Create(new IdentityRole(role));
            //    }
            //}

            //var userStore = new UserStore<ApplicationUser>(context);
            //var userManager = new UserManager<ApplicationUser>(userStore);

            //var adminUser = new ApplicationUser() { UserName = "admin", Email = "moviestabtab@gmail.com" };
            //userManager.Create(adminUser, "P@ssw0rd");
            //userManager.AddToRole(adminUser.Id, "admin");
        }
    }
}
