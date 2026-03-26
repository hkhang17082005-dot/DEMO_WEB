using Microsoft.EntityFrameworkCore;
using SRB_ViewModel.Entities;

namespace SRB_ViewModel.Models;

public static class RoleModels
{
   public static void InitModelRole(this ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<TypeRole>(entity =>
      {
         entity.Property(e => e.TypeName)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnType("varchar(255)");

         entity.HasIndex(e => e.TypeName)
            .IsUnique();

         entity.Property(e => e.TypeSlug)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

         entity.HasIndex(e => e.TypeSlug)
            .IsUnique();

         entity.Property(e => e.TypeDesc)
            .HasMaxLength(500)
            .HasColumnType("nvarchar(500)");

         entity.Property(e => e.CreatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("GETUTCDATE()");

         entity.Property(e => e.UpdatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("GETUTCDATE()");
      });

      modelBuilder.Entity<Role>(entity =>
      {
         entity.Property(e => e.RoleName)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnType("varchar(255)");

         entity.HasIndex(e => e.RoleName)
            .IsUnique();

         entity.Property(e => e.RoleSlug)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

         entity.HasIndex(e => e.RoleSlug)
            .IsUnique();

         entity.Property(e => e.RoleDesc)
            .HasMaxLength(500)
            .HasColumnType("nvarchar(500)");

         entity.HasOne(e => e.TypeRole)
            .WithMany(r => r.Roles)
            .HasForeignKey(u => u.TypeRoleID)
            .OnDelete(DeleteBehavior.Restrict);

         entity.Property(e => e.CreatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("GETUTCDATE()");

         entity.Property(e => e.UpdatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("GETUTCDATE()");
      });
   }
}
