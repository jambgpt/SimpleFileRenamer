namespace SimpleFileRenamer.Models
{
    /// <summary>
    /// Contains the result of a filename generation operation
    /// </summary>
    public class RenameResult
    {
        /// <summary>
        /// The new filename that was generated
        /// </summary>
        public string NewFileName { get; set; } = string.Empty;

        /// <summary>
        /// Whether there was an error generating the new filename
        /// </summary>
        public bool HasError { get; set; } = false;

        /// <summary>
        /// Error message, if an error occurred
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
