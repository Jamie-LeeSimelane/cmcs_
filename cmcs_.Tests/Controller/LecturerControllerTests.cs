using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using cmcs_.Models;
using cmcs_.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UploadResult = cmcs_.Services.UploadResult; // Include the UploadResult class
using System.Collections.Generic;
using cmcs_Views.Lecturer.Controllers; // Ensure this is included for List<T>

namespace cmcs_Views.Lecturer.Tests.Controllers // Updated the namespace accordingly
{
    public class LecturerControllerTests
    {
        private readonly LecturerController _controller;
        private readonly Mock<IClaimService> _mockClaimService;
        private readonly Mock<IDocumentService> _mockDocumentService;
        private readonly Mock<ILogger<LecturerController>> _mockLogger;

        public LecturerControllerTests()
        {
            _mockClaimService = new Mock<IClaimService>();
            _mockDocumentService = new Mock<IDocumentService>();
            _mockLogger = new Mock<ILogger<LecturerController>>();

            _controller = new Lecturer.Controllers.LecturerController(
                _mockClaimService.Object,
                _mockDocumentService.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task SubmitClaim_ShouldReturnRedirectToAction_WhenClaimIsValid()
        {
            // Arrange
            var claim = new Claim
            {
                LecturerName = "John Doe",
                HoursWorked = 5,
                HourlyRate = 50,
                Description = "Lecture on AI",
                Notes = "Good session",
                Documents = new List<ClaimDocument>() // Ensure Documents list is initialized
            };

            // Mock the document upload result as successful
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(1); // Pretend the file is non-empty
            mockFile.Setup(f => f.FileName).Returns("testfile.pdf"); // Mock file name

            _mockDocumentService
                .Setup(d => d.UploadDocumentAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync(new UploadResult { Success = true });

            // Mock the claim submission as successful
            _mockClaimService
                .Setup(c => c.SubmitClaimAsync(It.IsAny<Claim>()))
                .ReturnsAsync((true, "Claim submitted successfully"));

            // Ensure ModelState is valid
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.SubmitClaim(claim, mockFile.Object) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ClaimStatus", result.ActionName);
        }

        [Fact]
        public async Task SubmitClaim_ShouldReturnView_WhenDocumentUploadFails()
        {
            // Arrange
            var claim = new Claim
            {
                LecturerName = "John Doe",
                HoursWorked = 5,
                HourlyRate = 50,
                Description = "Lecture on AI",
                Notes = "Good session",
                Documents = new List<ClaimDocument>() // Ensure Documents list is initialized
            };

            // Mock the document upload result as unsuccessful
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(1); // Pretend the file is non-empty
            _mockDocumentService
                .Setup(d => d.UploadDocumentAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync(new UploadResult { Success = false, Message = "Upload failed." });

            // Act
            var result = await _controller.SubmitClaim(claim, mockFile.Object) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Upload failed.", result.ViewData["ErrorMessage"]); // Access ViewData instead of ViewBag
            Assert.Equal(claim, result.Model);
        }

        [Fact]
        public async Task SubmitClaim_ShouldReturnView_WhenModelStateIsInvalid()
        {
            // Arrange
            var claim = new Claim(); // Invalid claim without necessary properties
            _controller.ModelState.AddModelError("LecturerName", "Lecturer name is required."); // Simulating model validation error

            // Act
            var result = await _controller.SubmitClaim(claim, null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(claim, result.Model); // Should return the model with validation errors
        }
    }
}
