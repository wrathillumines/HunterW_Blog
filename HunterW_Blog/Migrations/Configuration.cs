namespace HunterW_Blog.Migrations
{
    using HunterW_Blog.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HunterW_Blog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(HunterW_Blog.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            #region roleManager
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }
            #endregion

            //Create users that occupy roles of Admin and/or Moderator
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "hwphotog@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "hwphotog@gmail.com",
                    Email = "hwphotog@gmail.com",
                    FirstName = "Hunter",
                    LastName = "Williams",
                    DisplayName = "dhunterw"
                }, "dog2ape");
            }

            if (!context.Users.Any(u => u.Email == "coderfoundry@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "coderfoundry@mailinator.com",
                    Email = "coderfoundry@mailinator.com",
                    FirstName = "Coder",
                    LastName = "Foundry",
                    DisplayName = "cfmoderator"
                }, "letMein2");
            }

            var userId = userManager.FindByEmail("hwphotog@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            userId = userManager.FindByEmail("coderfoundry@mailinator.com").Id;
            userManager.AddToRole(userId, "Moderator");

        }
    }
}
