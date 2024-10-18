using cmcs_.Models; // Ensure this namespace includes your Claim model
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cmcs_.Services
{
    public interface IClaimService
    {
        // Adds a new claim to the system
        Task AddClaimAsync(Claim claim);

        // Fetches all claims
        Task<IEnumerable<Claim>> GetClaimsAsync();

        // Fetches all pending claims
        Task<IEnumerable<Claim>> GetAllPendingClaimsAsync(); // This is the method to get all pending claims

        // Fetches a claim by its ID
        Task<Claim> GetClaimByIdAsync(int id);

        // Updates an existing claim
        Task UpdateClaimAsync(Claim claim);

        // Fetches claims submitted by a specific lecturer
        Task<IEnumerable<Claim>> GetClaimsByLecturerAsync(string lecturerUserName);

        // Submits a claim with validation
        Task<(bool success, string message)> SubmitClaimAsync(Claim claim);
    }
}
