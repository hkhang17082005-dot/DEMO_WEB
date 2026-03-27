using Microsoft.EntityFrameworkCore;
using SRB_ViewModel.Entities;

namespace SRB_ViewModel.Models;

public static class PostModels
{
   public static void InitModelPost(this ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<JobPost>(entity =>
      {
         entity.Property(e => e.JobPostID)
            .IsRequired()
            .HasColumnType("char(36)");

         entity.Property(e => e.CreatedByID)
            .IsRequired()
            .HasColumnType("char(36)");

         entity.Property(e => e.UpdatedByID)
            .HasColumnType("char(36)");

         entity.Property(e => e.BusinessID)
            .IsRequired()
            .HasColumnType("char(36)");

         entity.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnType("nvarchar(255)");

         entity.Property(e => e.Description)
            .HasColumnType("nvarchar(max)");

         entity.Property(e => e.CreatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("GETUTCDATE()");

         entity.Property(e => e.UpdatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("GETUTCDATE()");

         entity.HasOne(d => d.Business)
            .WithMany()
            .HasForeignKey(d => d.BusinessID)
            .OnDelete(DeleteBehavior.Restrict);
      });
   }
}
