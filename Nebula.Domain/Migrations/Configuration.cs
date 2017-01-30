using Nebula.Domain.Entities;

namespace Nebula.Domain.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Nebula.Domain.Concrete.NebulaDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Nebula.Domain.Concrete.NebulaDbContext context)
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

            context.ExamTypes.AddOrUpdate(
                m => m.Id,
                new ExamType { Id =  1, Name = "ზოგადი უნარები" }
                );

           context.CSQuestionCategories.AddOrUpdate(
               m => m.Id,
               new CSQuestionCategory { Id = 1, Name = "ვერბალური" },
               new CSQuestionCategory { Id = 2, Name = "მათემატიკური" }
               );

  
          //  InitializeIdentityForEF(context);

        }

        private void InitializeIdentityForEF(Nebula.Domain.Concrete.NebulaDbContext context)
        {
            var UserManager = new UserManager<User>(new UserStore<User>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var dbUser = UserManager.FindByEmail("Admin@Gmail.com");
            if(dbUser == null)
            {
                var user = new User() { FirstName = "Admin", LastName = "Admin", Email = "Admin@gmail.com", UserName = "Admin@gmail.com", RegisterDate = DateTime.Now, LastLogin = DateTime.Now ,EmailConfirmed = true, HasUploadPdf = true};

                string password = "12abCD!@";
                string roleName = "Admin";

                //Create Role Admin if it does not exist
                if (!RoleManager.RoleExists("Admin"))
                {
                    var roleresult = RoleManager.Create(new IdentityRole(roleName));
                }

                //Create User=Admin with password=123456
                
                var adminresult = UserManager.Create(user, password);

                //Add User Admin to Role Admin
                if (adminresult.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, roleName);
                }
            }
      
        }
    
}
}
