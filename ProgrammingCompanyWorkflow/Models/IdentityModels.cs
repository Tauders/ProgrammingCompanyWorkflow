using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ProgrammingCompanyWorkflow.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Project> Projects { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<Access> Accesses { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(): base("DefaultConnection")
        {

        }
        public DbSet<Component> Components { get; set; }
        public DbSet<ProjectComponent> ProjectComponents { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMovement> ProjectMovements { get; set; }
        public DbSet<Access> Accesses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable("Users").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<ApplicationUser>().ToTable("Users").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        }
    }
}