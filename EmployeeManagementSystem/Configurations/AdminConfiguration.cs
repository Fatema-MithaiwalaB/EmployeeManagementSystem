using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {

            // Primary Key
            builder.HasKey(a => a.AdminId);

            // Constraints
            builder.Property(a => a.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Email)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.HasIndex(a => a.Email)
                   .IsUnique();

            builder.Property(a => a.PasswordHash)
               .IsRequired()
               .HasMaxLength(255);

            builder.Property(a => a.Phone)
                   .IsRequired()
                   .HasMaxLength(15);

            builder.Property(a => a.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(a => a.UpdatedAt)
                   .IsRequired(false); // Nullable

            builder.HasOne(a => a.Role)
                .WithMany(r => r.Admins)
                .HasForeignKey(a => a.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
