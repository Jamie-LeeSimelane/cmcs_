using Microsoft.AspNetCore.Mvc;
using cmcs_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http; // Include for IFormFile
using System.IO; // Include for Path

namespace cmcs_.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly IClaimRepository _claimRepository; // Use the interface for dependency injection

        // Constructor to inject the repository
        public ClaimsController(IClaimRepository claimRepository)
        {
            _claimRepository = claimRepository; // Assign the injected repository
        }

        // GET: Submit a claim form
        public IActionResult SubmitClaim()
        {
            return View(); // This will look for SubmitClaim.cshtml in Views/Claims
        }

        // POST: Submit the claim (handles the form submission)
        [HttpPost]
        public IActionResult SubmitClaim(Claim model)
        {
            if (ModelState.IsValid)
            {
                model.Status = "Pending"; // Set default status to Pending
                _claimRepository.AddClaim(model); // Add the claim to the in-memory list
                return RedirectToAction("Success"); // Redirect to the Success page
            }
            return View(model); // Return the view with the model if validation fails
        }

        // GET: Track Claim (shows the list of submitted claims with status)
        public IActionResult TrackClaim()
        {
            var claims = _claimRepository.GetAllClaims(); // Get all claims from the repository
            return View(claims); // Pass the claims to the view
        }

        // GET: Upload supporting documents (returns the upload form)
        public IActionResult UploadSupportingDocuments()
        {
            return View(new ClaimDocument()); // Pass a new ClaimDocument model to the view
        }

        // POST: Handle document upload
        [HttpPost]
        public IActionResult UploadSupportingDocuments(ClaimDocument model, IFormFile document)
        {
            if (document != null && document.Length > 0)
            {
                try
                {
                    // Ensure the directory exists
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "YourUploadPath"); // Change to your desired path
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    // Save the file
                    var filePath = Path.Combine(uploadDir, document.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        document.CopyTo(stream);
                    }

                    // Set the FilePath in the model
                    model.FilePath = filePath; // Store the file path
                    _claimRepository.AddClaimDocument(model); // Optionally, save the model to your repository

                    ViewBag.Message = "File uploaded successfully!";
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "An error occurred while uploading the file: " + ex.Message;
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Please select a valid file.";
            }

            return View(model); // Return the model to the view
        }

        // GET: Verify Claims (for program coordinators and academic managers)
        [Authorize(Roles = "Manager,Coordinator")] // Ensure only specific roles can access this
        public IActionResult VerifyClaims()
        {
            var claims = _claimRepository.GetAllClaims(); // Get all claims from the repository
            return View(claims); // Pass the list of claims for verification
        }

        // POST: Approve a claim
        [HttpPost]
        public IActionResult ApproveClaim(int id)
        {
            _claimRepository.ApproveClaim(id); // Use repository to approve the claim
            return RedirectToAction("VerifyClaims");
        }

        // POST: Reject a claim
        [HttpPost]
        public IActionResult RejectClaim(int id)
        {
            _claimRepository.RejectClaim(id); // Use repository to reject the claim
            return RedirectToAction("VerifyClaims");
        }

        // GET: Success page
        public IActionResult Success()
        {
            return View(); // This will look for Success.cshtml in Views/Claims
        }
    }
}
