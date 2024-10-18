using cmcs_.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace cmcs_.Services
{
    public class ClaimService : IClaimService
    {
        private readonly ApplicationDbContext _context;

        public ClaimService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddClaimAsync(Claim claim)
        {
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim), "Claim cannot be null.");
            }

            // Optional: Check for validation criteria
            if (claim.HoursWorked <= 0)
            {
                throw new ArgumentException("Hours worked must be greater than zero.");
            }

            await _context.Claims.AddAsync(claim);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            return await _context.Claims.ToListAsync();
        }

        public async Task<IEnumerable<Claim>> GetAllPendingClaimsAsync()
        {
            return await _context.Claims
                .Where(c => c.Status == "Pending")
                .ToListAsync();
        }

        public async Task<Claim> GetClaimByIdAsync(int id)
        {
            return await _context.Claims.FindAsync(id);
        }

        public async Task UpdateClaimAsync(Claim claim)
        {
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim), "Claim cannot be null.");
            }

            _context.Claims.Update(claim);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Claim>> GetClaimsByLecturerAsync(string lecturerUserName)
        {
            return await _context.Claims
                .Where(c => c.LecturerName == lecturerUserName)
                .ToListAsync();
        }

        public async Task<(bool success, string message)> SubmitClaimAsync(Claim claim)
        {
            if (claim == null)
            {
                return (false, "Claim cannot be null.");
            }

            // Optional: Validate other properties if necessary
            if (claim.HoursWorked <= 0)
            {
                return (false, "Hours worked must be greater than zero.");
            }

            await AddClaimAsync(claim);
            return (true, "Claim submitted successfully.");
        }
    }
}
