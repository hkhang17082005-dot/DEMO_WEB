using Microsoft.EntityFrameworkCore;
using SRB_ViewModel.Entities;
using SRB_ViewModel.Models;
namespace SRB_ViewModel;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.InitModelRole();
      modelBuilder.InitModelPermission();
      modelBuilder.InitModelUser();

      /* Load Data */
      modelBuilder.InitCoreData();
   }

   public DbSet<Role> Roles { get; set; }
   public DbSet<User> Users { get; set; }
   public DbSet<Job> Jobs { get; set; }
   public DbSet<Location> Locations { get; set; }

}
