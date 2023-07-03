using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsWebsite.API.Models;
using System.Reflection.Emit;

namespace NewsWebsite.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers", schema: "Auth");
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRole", schema: "Auth");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaim", schema: "Auth");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogin", schema: "Auth");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserToken", schema: "Auth");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaim", schema: "Auth");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRole", schema: "Auth");

            modelBuilder.Entity<Author>().HasMany<News>().WithOne(p=>p.Author).HasForeignKey(p => p.AuthorId).HasPrincipalKey(p=>p.Id);

            modelBuilder.Entity<News>().ToTable("News", schema: "News");
            modelBuilder.Entity<Author>().ToTable("Authors", schema: "News").HasIndex(p=>p.Email).IsUnique();
            modelBuilder.Entity<Author>().ToTable("Authors", schema: "News").HasIndex(p=>p.Name).IsUnique();
            modelBuilder.Entity<News>(p =>
            {
                p.Property(n => n.NewsDetails).HasColumnName("News");
                p.Property(n => n.CreationDate).HasDefaultValueSql("GETDATE()");
            });
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<News> News { get; set; }
    }
}
