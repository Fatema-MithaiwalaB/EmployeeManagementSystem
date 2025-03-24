using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagementSystem.Configurations
{
    public class TimesheetConfiguration : IEntityTypeConfiguration<Timesheet>
    {
        public void Configure(EntityTypeBuilder<Timesheet> builder)
        {
            builder.HasKey(t => t.TimesheetId);

            builder.Property(t => t.Date)
                .IsRequired();

            builder.Property(t => t.StartTime)
               .IsRequired()
               .HasColumnType("TIME");

            builder.Property(t => t.EndTime)
                   .IsRequired()
                   .HasColumnType("TIME");

            builder.Property(t => t.TotalHours)
                .IsRequired()
                .HasColumnType("DECIMAL(5,2)");

            builder.Property(t => t.Description)
               .HasColumnType("TEXT")
               .IsRequired(false);

            builder.Property(t => t.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(t => t.Employee)
               .WithMany(e => e.Timesheets)
               .HasForeignKey(t => t.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
