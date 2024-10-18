using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // For logging
using cmcs_.Services;
using System.Threading.Tasks;

public class CoordinatorController : Controller
{
    private readonly IClaimService _claimService;
    private readonly ILogger<CoordinatorController> _logger; // Add logging

    public CoordinatorController(IClaimService claimService, ILogger<CoordinatorController> logger)
    {
        _claimService = claimService;
        _logger = logger;
    }

    public async Task<IActionResult> Claims()
    {
        var claims = await _claimService.GetAllPendingClaimsAsync();
        return View(claims);
    }

    [HttpPost]
    public async Task<IActionResult> ApproveClaim(int id)
    {
        var claim = await _claimService.GetClaimByIdAsync(id);
        if (claim == null)
        {
            _logger.LogWarning($"Claim with ID {id} not found for approval.");
            TempData["ErrorMessage"] = "Claim not found."; // Feedback to user
            return RedirectToAction("Claims");
        }

        claim.Status = "Approved";
        await _claimService.UpdateClaimAsync(claim);
        _logger.LogInformation($"Claim with ID {id} approved."); // Log the action

        TempData["SuccessMessage"] = $"Claim {id} has been approved."; // Optional user feedback
        return RedirectToAction("Claims");
    }

    [HttpPost]
    public async Task<IActionResult> RejectClaim(int id)
    {
        var claim = await _claimService.GetClaimByIdAsync(id);
        if (claim == null)
        {
            _logger.LogWarning($"Claim with ID {id} not found for rejection.");
            TempData["ErrorMessage"] = "Claim not found."; // Feedback to user
            return RedirectToAction("Claims");
        }

        claim.Status = "Rejected";
        await _claimService.UpdateClaimAsync(claim);
        _logger.LogInformation($"Claim with ID {id} rejected."); // Log the action

        TempData["SuccessMessage"] = $"Claim {id} has been rejected."; // Optional user feedback
        return RedirectToAction("Claims");
    }
}
