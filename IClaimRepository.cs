using System.Collections.Generic;

namespace cmcs_.Models
{
    public interface IClaimRepository
    {
        void AddClaim(Claim claim);
        IEnumerable<Claim> GetAllClaims();
        void ApproveClaim(int claimId);
        void RejectClaim(int claimId);
        void AddClaimDocument(ClaimDocument document); 
    }
}
