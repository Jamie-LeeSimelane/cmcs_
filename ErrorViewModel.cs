namespace cmcs_.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        // Additional properties to capture error details
        public string ErrorMessage { get; set; } // To hold a specific error message
        public int StatusCode { get; set; } // To hold HTTP status codes if necessary
    }
}
