using Microsoft.EntityFrameworkCore;
using SRB_ViewModel.Entities;

namespace SRB_ViewModel.Models;

public static class PermissionModels
{
   public static void InitModelPermission(this ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<Permission>(entity =>
      {
         entity.Property(e => e.PerName)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnType("varchar(255)");

         entity.HasIndex(e => e.PerName)
            .IsUnique();

         entity.Property(e => e.PerSlug)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

         entity.HasIndex(e => e.PerSlug)
            .IsUnique();

         entity.Property(e => e.PerDesc)
            .HasMaxLength(500)
            .HasColumnType("nvarchar(500)");

         entity.Property(e => e.CreatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("GETUTCDATE()");

         entity.Property(e => e.UpdatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("GETUTCDATE()");
      });

      modelBuilder.Entity<RolePermission>(entity =>
      {
         entity.HasKey(e => new { e.RoleID, e.PerID });

         entity.Property(e => e.CreatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("GETUTCDATE()");

         entity.Property(e => e.UpdatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("GETUTCDATE()");

         entity.HasOne(d => d.Role)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(d => d.RoleID)
            .OnDelete(DeleteBehavior.Restrict);

         entity.HasOne(d => d.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(d => d.PerID)
            .OnDelete(DeleteBehavior.Restrict);
      });
   }
}

