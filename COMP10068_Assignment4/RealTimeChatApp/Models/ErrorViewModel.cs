namespace RealTimeChatApp.Models
{
    /// <summary>
    /// Represents a model for displaying error information in views.
    /// </summary>
    /// <remarks>
    /// This view model is typically used to pass error information from controllers
    /// to error views, particularly for displaying request-specific error details.
    /// </remarks>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the unique request identifier associated with the error.
        /// </summary>
        /// <value>
        /// The request ID as a string, or null if no request ID is available.
        /// </value>
        /// <remarks>
        /// This property is typically populated with the current activity ID or
        /// trace identifier when an error occurs during request processing.
        /// </remarks>
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether the request ID should be displayed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the RequestId is not null or empty; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This computed property is useful for conditionally showing the request ID
        /// in error views only when it's available.
        /// </remarks>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}