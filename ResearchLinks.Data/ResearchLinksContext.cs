using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using ResearchLinks.Data.Models;

namespace ResearchLinks.Data
{
    public class ResearchLinksContext : DbContext, IDisposable
    {
        public DbSet<User> Users {get; set;}
        public DbSet<Role> Roles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ResearchItem> ResearchItems { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Remove the cascade delete for Users on ResearchItem.
            modelBuilder.Entity<ResearchItem>()
                .HasRequired(r => r.User).WithMany().HasForeignKey(r => r.UserName).WillCascadeOnDelete(false);

            // Maps to the expected many-to-many join table name for roles to users.
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .Map(m =>
                {
                    m.ToTable("RoleMemberships");
                    m.MapLeftKey("UserName");
                    m.MapRightKey("RoleName");
                });
        }
        
    }
}
