using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cmcs_.Models
{
    public class Claim
    {
        private string name;
        private object userName;

        public int Id { get; set; }

        [Required(ErrorMessage = "Lecturer Name is required.")]
        public string LecturerName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public decimal Amount { get; set; }

        public string Status { get; set; } = "Pending"; // Default status to "Pending"

        [DataType(DataType.Date)]
        public DateTime DateSubmitted { get; set; } = DateTime.Now;

        [Range(1, int.MaxValue, ErrorMessage = "Hours worked must be greater than 0.")]
        public int HoursWorked { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Hourly rate must be a positive value.")]
        public decimal HourlyRate { get; set; }

        [StringLength(1000, ErrorMessage = "Notes cannot be longer than 1000 characters.")]
        public string Notes { get; set; }

        // Change from List<string> to List<ClaimDocument> for document management
        public List<ClaimDocument> Documents { get; set; }

        public Claim()
        {
            Documents = new List<ClaimDocument>();
        }

        public Claim(string name, object userName)
        {
            this.name = name;
            this.userName = userName;
        }

        // Calculated property for total amount
        public decimal TotalAmount => HoursWorked * HourlyRate;
    }
}
