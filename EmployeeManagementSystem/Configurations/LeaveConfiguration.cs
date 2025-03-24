using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagementSystem.Configurations
{
    public class LeaveConfiguration : IEntityTypeConfiguration<Leave>
    {
        public void Configure (EntityTypeBuilder<Leave> builder) 
        {
            builder.HasKey(l => l.LeaveId);

            builder.Property(l => l.StartDate)
               .IsRequired();

            builder.Property(l => l.EndDate)
                   .IsRequired();

            builder.Property(l => l.LeaveType)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(l => l.Reason)
                .IsRequired(false)
                .HasColumnType("TEXT");

            builder.Property(l => l.Status)
                   .HasDefaultValue("Pending")
                   .HasMaxLength(20);

            builder.Property(l => l.AppliedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(l => l.Employee)
                   .WithMany(e => e.Leaves)
                   .HasForeignKey(l => l.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
