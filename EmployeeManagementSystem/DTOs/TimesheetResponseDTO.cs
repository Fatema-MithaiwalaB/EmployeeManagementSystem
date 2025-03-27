namespace EmployeeManagementSystem.DTOs
{
    public class TimesheetResponseDTO
    {
        public int TimesheetId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal TotalHours { get; set; }
        public string Description { get; set; }
    }
}
