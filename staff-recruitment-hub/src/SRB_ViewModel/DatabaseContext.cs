using Microsoft.EntityFrameworkCore;

using SRB_ViewModel.Models;
using SRB_ViewModel.Entities;

namespace SRB_ViewModel;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.InitModelRole();
      modelBuilder.InitModelPermission();
      modelBuilder.InitModelUser();
      modelBuilder.InitModelBusiness();
      modelBuilder.InitModelPost();

      /* Load Data */
      modelBuilder.InitCoreData();
   }

   public DbSet<Role> Roles { get; set; }

   public DbSet<User> Users { get; set; }

   public DbSet<UserRoles> UserRoles { get; set; }

   public DbSet<UserProfile> UserProfiles { get; set; }

   public DbSet<Business> Businesses { get; set; }

   public DbSet<Job> Jobs { get; set; }

   public DbSet<Location> Locations { get; set; }

   public DbSet<SavedJob> SavedJobs { get; set; }

   public DbSet<JobPost> JobPosts { get; set; }
}
