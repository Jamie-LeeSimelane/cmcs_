namespace cmcs_.Models // Ensure this namespace matches your existing project's structure
{
    public class ClaimModel
    {
        public int Id { get; set; } // Unique identifier for each claim
        public string LecturerName { get; set; } // Name of the lecturer submitting the claim
        public int HoursWorked { get; set; } // Number of hours worked
        public decimal HourlyRate { get; set; } // Hourly rate
        public string Notes { get; set; } // Additional notes for the claim
        public string Status { get; set; } // Status of the claim (Pending, Approved, Rejected)
    }
}
