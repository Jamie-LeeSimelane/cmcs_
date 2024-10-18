using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using cmcs_.Models; 
using cmcs_.Services; 
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace cmcs_Views.Lecturer.Controllers 
{
    public class LecturerController : Controller
    {
        private readonly IClaimService _claimService;
        private readonly IDocumentService _documentService;
        private readonly ILogger<LecturerController> _logger;

        public LecturerController(IClaimService claimService, IDocumentService documentService, ILogger<LecturerController> logger)
        {
            _claimService = claimService;
            _documentService = documentService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult SubmitClaim()
        {
            return View(); // Ensure you have SubmitClaim.cshtml in Views/Lecturer
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim(Claim claim, IFormFile document)
        {
            if (ModelState.IsValid)
            {
                if (document != null && document.Length > 0)
                {
                    var uploadResult = await _documentService.UploadDocumentAsync(document);

                    // Check if the upload was successful
                    if (!uploadResult.Success)
                    {
                        ViewBag.ErrorMessage = uploadResult.Message;
                        return View(claim); // Return view with the error message
                    }

                    // Initialize the Documents collection if null
                    claim.Documents ??= new List<ClaimDocument>();

                    // Add the uploaded document to the claim's documents collection
                    var claimDocument = new ClaimDocument
                    {
                        FileName = Path.GetFileName(document.FileName),
                        FilePath = Path.Combine("Uploads", Path.GetFileName(document.FileName)) // Update path accordingly
                    };
                    claim.Documents.Add(claimDocument);
                }

                // Submit the claim and handle the result
                var result = await _claimService.SubmitClaimAsync(claim);
                if (result.success)
                {
                    ViewBag.Message = result.message; // Success message
                    return RedirectToAction("ClaimStatus");
                }
                else
                {
                    ViewBag.ErrorMessage = result.message; // Error message
                }
            }
            else
            {
                // Log validation errors
                _logger.LogWarning("Model state is invalid for claim submission.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning(error.ErrorMessage); // Log individual validation errors
                }
            }

            return View(claim); // Return view with validation errors
        }

        public async Task<IActionResult> ClaimStatus()
        {
            var claims = await _claimService.GetClaimsByLecturerAsync(User.Identity.Name);
            return View(claims); // Ensure you have ClaimStatus.cshtml in Views/Lecturer
        }

        public async Task<IActionResult> VerifyClaims()
        {
            var claims = await _claimService.GetAllPendingClaimsAsync();
            return View(claims); // Ensure you have VerifyClaims.cshtml in Views/Lecturer
        }

        [HttpPost]
        public async Task<IActionResult> ApproveClaim(int claimId)
        {
            var claim = await _claimService.GetClaimByIdAsync(claimId);
            if (claim != null)
            {
                claim.Status = "Approved";
                await _claimService.UpdateClaimAsync(claim);
                return RedirectToAction("VerifyClaims");
            }
            _logger.LogWarning($"Claim with ID {claimId} not found for approval.");
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> RejectClaim(int claimId)
        {
            var claim = await _claimService.GetClaimByIdAsync(claimId);
            if (claim != null)
            {
                claim.Status = "Rejected";
                await _claimService.UpdateClaimAsync(claim);
                return RedirectToAction("VerifyClaims");
            }
            _logger.LogWarning($"Claim with ID {claimId} not found for rejection.");
            return NotFound();
        }
    }
}
