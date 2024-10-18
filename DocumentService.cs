using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace cmcs_.Services
{
    public interface IDocumentService
    {
        Task<UploadResult> UploadDocumentAsync(IFormFile file);
    }

    public class DocumentService : IDocumentService
    {
        private readonly string _uploadPath;
        private readonly ILogger<DocumentService> _logger;

        public DocumentService(ILogger<DocumentService> logger)
        {
            _logger = logger;
            _uploadPath = Path.Combine("wwwroot", "Uploads"); // Ensure uploads are in the wwwroot folder
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath); // Create the directory if it doesn't exist
            }
        }

        public async Task<UploadResult> UploadDocumentAsync(IFormFile file)
        {
            // Validate file
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file was uploaded.");
                return new UploadResult { Success = false, Message = "No file uploaded." };
            }

            // Check file size limit (e.g., 5 MB)
            const long maxFileSize = 5 * 1024 * 1024; // 5 MB
            if (file.Length > maxFileSize)
            {
                _logger.LogWarning("File size exceeds the maximum limit.");
                return new UploadResult { Success = false, Message = "File size exceeds the maximum limit of 5 MB." };
            }

            // Validate file extension
            var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" }; // Add allowed file types
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                _logger.LogWarning($"File type '{fileExtension}' is not allowed.");
                return new UploadResult { Success = false, Message = $"File type '{fileExtension}' is not allowed." };
            }

            // Generate a unique filename to avoid overwriting
            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_uploadPath, uniqueFileName); // Ensure safe file name

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream); // Use asynchronous file copy
                }
                _logger.LogInformation($"File uploaded successfully: {uniqueFileName}");
                return new UploadResult { Success = true, FilePath = filePath }; // Return file path for further use
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading file: {ex.Message}");
                return new UploadResult { Success = false, Message = "Error uploading file. Please try again." };
            }
        }
    }

    public class UploadResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string FilePath { get; set; } // Optional: Return the file path if needed
    }
}
