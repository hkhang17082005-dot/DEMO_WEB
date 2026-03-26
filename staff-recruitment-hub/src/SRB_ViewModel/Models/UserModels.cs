using Microsoft.EntityFrameworkCore;
using SRB_ViewModel.Entities;

namespace SRB_ViewModel.Models;

public static class UserModels
{
   public static void InitModelUser(this ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<User>(entity =>
      {
         entity.Property(e => e.UserID)
            .HasColumnType("char(36)");

         entity.Property(e => e.Username)
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

         entity.Property(e => e.HashPassword)
            .HasMaxLength(255)
            .HasColumnType("varchar(255)");

         entity.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsUnicode(false);

         entity.Property(e => e.CreatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("GETUTCDATE()");

         entity.Property(e => e.UpdatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("GETUTCDATE()");

         entity.HasIndex(e => e.Username)
            .IsUnique();
      });

      modelBuilder.Entity<UserRoles>(entity =>
      {
         entity.HasKey(e => new { e.UserID, e.RoleID });

         entity.Property(e => e.UserID)
            .HasColumnType("char(36)");

         entity.Property(e => e.CreatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("GETUTCDATE()");

         entity.Property(e => e.UpdatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("GETUTCDATE()");

         entity.HasOne(d => d.User)
            .WithMany(p => p.UserRoles)
            .HasForeignKey(d => d.UserID)
            .OnDelete(DeleteBehavior.Restrict);

         entity.HasOne(d => d.Role)
            .WithMany(p => p.UserRoles)
            .HasForeignKey(d => d.RoleID)
            .OnDelete(DeleteBehavior.Restrict);
      });
   }
}
