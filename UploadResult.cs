namespace cmcs_.Models  // Adjust the namespace based on your project structure
{
    public class UploadResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the upload was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the message associated with the upload result.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Optional: Constructor for easy creation of UploadResult instances.
        /// </summary>
        /// <param name="success">Indicates if the upload was successful.</param>
        /// <param name="message">The message associated with the upload result.</param>
        public UploadResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
