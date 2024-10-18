using System.ComponentModel.DataAnnotations;

namespace cmcs_.Models
{
    public class ClaimDocument
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Claim ID is required.")]
        public int ClaimId { get; set; }

        [Required(ErrorMessage = "File name is required.")]
        [StringLength(255, ErrorMessage = "File name cannot be longer than 255 characters.")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "File path is required.")]
        public string FilePath { get; set; }

        // Navigation property for the related Claim
        public virtual Claim Claim { get; set; }
    }
}
