using System.Collections.Generic;
using System.Linq;

namespace cmcs_.Models
{
    public class ClaimRepository : IClaimRepository
    {
        private static List<Claim> _claims = new List<Claim>();
        private static List<ClaimDocument> _claimDocuments = new List<ClaimDocument>(); // New list to hold claim documents

        // Method to add a claim
        public void AddClaim(Claim claim)
        {
            claim.Id = _claims.Count > 0 ? _claims.Max(c => c.Id) + 1 : 1; // Auto-increment Id
            _claims.Add(claim);
        }

        // Method to get all claims
        public IEnumerable<Claim> GetAllClaims()
        {
            return _claims;
        }

        // Method to approve a claim
        public void ApproveClaim(int claimId)
        {
            var claim = _claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                claim.Status = "Approved";
            }
        }

        // Method to reject a claim
        public void RejectClaim(int claimId)
        {
            var claim = _claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                claim.Status = "Rejected";
            }
        }

        // New method to add a claim document
        public void AddClaimDocument(ClaimDocument document)
        {
            document.Id = _claimDocuments.Count > 0 ? _claimDocuments.Max(d => d.Id) + 1 : 1; // Auto-increment Id
            _claimDocuments.Add(document);
        }
    }
}
