using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {  

            builder.HasKey(e => e.EmployeeId);

            // Constraints
            builder.Property(e => e.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Email)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.HasIndex(e => e.Email)
                   .IsUnique();

            builder.Property(e => e.Address)
                .IsRequired(false);

            builder.Property(e => e.PasswordHash)
              .IsRequired()
              .HasMaxLength(255);

            builder.Property(e => e.Phone)
                   .IsRequired()
                   .HasMaxLength(15);

            builder.Property(e => e.TechStack)
                   .HasMaxLength(255);

            builder.Property(e => e.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.UpdatedAt)
                   .IsRequired(false);

            // Relationships
            builder.HasOne(e => e.Department)
                   .WithMany(d => d.Employees)
                   .HasForeignKey(e => e.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Timesheets)
                   .WithOne(t => t.Employee)
                   .HasForeignKey(t => t.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Leaves)
                   .WithOne(l => l.Employee)
                   .HasForeignKey(l => l.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
