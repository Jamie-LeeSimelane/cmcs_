using System.ComponentModel.DataAnnotations;

namespace cmcs_.Models
{
    public class Rate
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lecturer name is required.")]
        [StringLength(100, ErrorMessage = "Lecturer name cannot be longer than 100 characters.")]
        public string LecturerName { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Hourly rate must be a positive value.")]
        public double HourlyRate { get; set; }

        // Optional: Method to get formatted hourly rate
        public string GetFormattedHourlyRate()
        {
            return $"{HourlyRate:C}"; // Formats as currency based on current culture
        }
    }
}
