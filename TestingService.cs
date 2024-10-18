using cmcs_.Models;
using System;
using System.Collections.Generic;

namespace cmcs_.Services
{
    public class TestingService
    {
        /// <summary>
        /// Validates the submission of a claim.
        /// </summary>
        /// <param name="claim">The claim to validate.</param>
        /// <returns>A tuple indicating whether the claim is valid and a list of validation messages.</returns>
        public (bool isValid, List<string> messages) ValidateClaimSubmission(Claim claim)
        {
            var messages = new List<string>();

            // Check if the claim is null
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim), "Claim cannot be null.");
            }

            // Validate LecturerName
            if (string.IsNullOrWhiteSpace(claim.LecturerName))
            {
                messages.Add("Lecturer name is required.");
            }

            // Validate HoursWorked
            if (claim.HoursWorked <= 0)
            {
                messages.Add("Hours worked must be greater than 0.");
            }

            // Validate HourlyRate
            if (claim.HourlyRate <= 0)
            {
                messages.Add("Hourly rate must be greater than 0.");
            }

            // Validate Notes (optional)
            if (claim.Notes != null && claim.Notes.Length > 500)
            {
                messages.Add("Notes cannot exceed 500 characters.");
            }

            // If there are no validation messages, return valid
            return (messages.Count == 0, messages);
        }
    }
}
