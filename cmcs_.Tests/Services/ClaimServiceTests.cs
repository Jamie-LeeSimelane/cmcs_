using System.Linq;
using System.Threading.Tasks;
using Xunit;
using cmcs_.Models;
using cmcs_.Services;
using Microsoft.EntityFrameworkCore;

namespace cmcs_.Tests.Services
{
    public class ClaimServiceTests
    {
        private readonly IClaimService _claimService;
        private readonly ApplicationDbContext _context;

        public ClaimServiceTests()
        {
            // Initialize the ApplicationDbContext with an in-memory database for testing purposes
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _claimService = new ClaimService(_context);
        }

        [Fact]
        public async Task AddClaimAsync_ShouldAddClaim()
        {
            // Arrange: Create a new valid claim with all required fields
            var claim = new Claim
            {
                LecturerName = "John Doe",
                HoursWorked = 5,
                HourlyRate = 50,
                Description = "Lecture on AI",
                Notes = "Well done session"
            };

            // Act: Call AddClaimAsync
            await _claimService.AddClaimAsync(claim);

            // Assert: Ensure the claim is added successfully
            var result = await _context.Claims.FindAsync(claim.Id);
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.LecturerName);
        }

        [Fact]
        public async Task ApproveClaim_ShouldUpdateClaimStatusToApproved()
        {
            // Arrange: Add a pending claim and update status to "Approved"
            var claim = new Claim
            {
                LecturerName = "John Doe",
                HoursWorked = 5,
                HourlyRate = 50,
                Description = "Lecture on AI",
                Notes = "Well done session",
                Status = "Pending"
            };

            await _context.Claims.AddAsync(claim);
            await _context.SaveChangesAsync();

            // Act: Update status to "Approved"
            claim.Status = "Approved";
            await _claimService.UpdateClaimAsync(claim);

            // Assert: Ensure the status is updated
            var result = await _claimService.GetClaimByIdAsync(claim.Id);
            Assert.Equal("Approved", result.Status);
        }

        [Fact]
        public async Task RejectClaim_ShouldUpdateClaimStatusToRejected()
        {
            // Arrange: Add a pending claim
            var claim = new Claim
            {
                LecturerName = "John Doe",
                HoursWorked = 5,
                HourlyRate = 50,
                Description = "Lecture on AI",
                Notes = "Well done session",
                Status = "Pending"
            };

            await _context.Claims.AddAsync(claim);
            await _context.SaveChangesAsync();

            // Act: Update status to "Rejected"
            claim.Status = "Rejected";
            await _claimService.UpdateClaimAsync(claim);

            // Assert: Ensure the status is updated
            var result = await _claimService.GetClaimByIdAsync(claim.Id);
            Assert.Equal("Rejected", result.Status);
        }

        [Fact]
        public async Task GetAllPendingClaimsAsync_ShouldReturnPendingClaims()
        {
            // Arrange: Add two claims, one pending and one approved
            var claim1 = new Claim { LecturerName = "John Doe", HoursWorked = 5, Status = "Pending", Description = "Lecture on AI", Notes = "Pending session" };
            var claim2 = new Claim { LecturerName = "Jane Smith", HoursWorked = 8, Status = "Approved", Description = "Lecture on ML", Notes = "Approved session" };

            await _context.Claims.AddRangeAsync(claim1, claim2);
            await _context.SaveChangesAsync();

            // Act: Retrieve all pending claims
            var pendingClaims = await _claimService.GetAllPendingClaimsAsync();

            // Assert: Ensure only pending claims are returned
            Assert.Single(pendingClaims);
            Assert.Equal("Pending", pendingClaims.First().Status);
        }

        [Fact]
        public async Task GetClaimsByLecturerAsync_ShouldReturnClaimsForSpecificLecturer()
        {
            // Arrange: Add claims for different lecturers
            var claim1 = new Claim { LecturerName = "John", HoursWorked = 10, Status = "Pending", Description = "Lecture 1", Notes = "Pending" };
            var claim2 = new Claim { LecturerName = "Jane", HoursWorked = 8, Status = "Approved", Description = "Lecture 2", Notes = "Approved" };

            await _context.Claims.AddRangeAsync(claim1, claim2);
            await _context.SaveChangesAsync();

            // Act: Retrieve claims for a specific lecturer
            var lecturerClaims = await _claimService.GetClaimsByLecturerAsync("John");

            // Assert: Ensure only claims for the specific lecturer are returned
            Assert.Single(lecturerClaims);
            Assert.Equal(10, lecturerClaims.First().HoursWorked);
        }
    }
}
