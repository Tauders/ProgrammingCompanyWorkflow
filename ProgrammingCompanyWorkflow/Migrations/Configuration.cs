using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProgrammingCompanyWorkflow.Models;

namespace ProgrammingCompanyWorkflow.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProgrammingCompanyWorkflow.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProgrammingCompanyWorkflow.Models.ApplicationDbContext context)
        {
            var adminrole = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "admin" };
            var userrole = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "user" };
            var managerrole = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "manager" };
            var programmerrole = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "programmer" };

            context.Roles.AddOrUpdate(x => x.Name, adminrole);
            context.Roles.AddOrUpdate(x => x.Name, userrole);
            context.Roles.AddOrUpdate(x => x.Name, managerrole);
            context.Roles.AddOrUpdate(x => x.Name, programmerrole);
            var admin = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin",
                PasswordHash = "ANJaRuwwAcOTkys07uS9olDtCF04Gj66FOk+74f6XSMn/qMyR2Pe5avqh/dQwZNpbg==",
                SecurityStamp = "3b56aaa6-3009-48ec-b130-b810f45306eb"
            };
            context.Users.AddOrUpdate(x=>x.UserName,admin);
            context.SaveChanges();
            //context.Users.AddOrUpdate(x => x.UserName, admin);
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
           // um.Create(admin);
            var asd = um.FindByName("admin");
            um.AddToRole(asd.Id, "admin");
        }
    }
}
