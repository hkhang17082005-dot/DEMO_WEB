using Microsoft.EntityFrameworkCore;

using SRB_ViewModel.Entities;

namespace SRB_ViewModel.Models;

public static class BusinessModels
{
   public static void InitModelBusiness(this ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<Business>(entity =>
      {
         entity.Property(e => e.BusinessID)
            .HasColumnType("char(36)")
            .IsUnicode(false);

         entity.Property(e => e.BusinessName)
            .IsRequired()
            .HasMaxLength(255);

         entity.Property(e => e.TaxCode)
            .HasMaxLength(50)
            .IsUnicode(false);

         entity.Property(e => e.Website)
            .HasMaxLength(255)
            .IsUnicode(false);

         entity.Property(e => e.LogoUrl)
            .HasMaxLength(500)
            .IsUnicode(false);

         entity.Property(e => e.ContactEmail)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode(false);

         entity.Property(e => e.PhoneNumber)
            .HasMaxLength(20)
            .IsUnicode(false);

         entity.Property(e => e.Address)
            .HasMaxLength(500);

         entity.Property(e => e.Description)
         .HasColumnType("nvarchar(max)");

         entity.Property(e => e.Industry)
            .HasMaxLength(100);

         entity.Property(e => e.CompanySize)
            .HasMaxLength(50);

         entity.Property(e => e.CreatedByID)
            .IsRequired()
            .HasColumnType("char(36)");

         entity.Property(e => e.IsVerified)
            .HasDefaultValue(false);

         entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

         entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

         entity.HasOne(b => b.CreatedBy)
            .WithMany() // Một User có thể tạo nhiều Business
            .HasForeignKey(b => b.CreatedByID)
            .OnDelete(DeleteBehavior.Restrict);

         entity.HasMany(e => e.Employees)
            .WithOne(u => u.Business)
            .HasForeignKey(u => u.BusinessID)
            .OnDelete(DeleteBehavior.Restrict);
      });
   }

}
