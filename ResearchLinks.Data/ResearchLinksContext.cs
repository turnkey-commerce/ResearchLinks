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
        public DbSet<Project> Projects { get; set; }
        public DbSet<ResearchItem> ResearchItems { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Remove the cascade delete for Users on ResearchItem.
            modelBuilder.Entity<ResearchItem>()
                .HasRequired(r => r.User).WithMany().HasForeignKey(r => r.UserId).WillCascadeOnDelete(false);
        }
        
    }
}
