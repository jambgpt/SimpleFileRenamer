namespace SimpleFileRenamer.Models
{
    /// <summary>
    /// Defines the position where a sequence number should be applied
    /// </summary>
    public enum SequencePosition
    {
        Prefix,
        Suffix,
        Replace
    }

    /// <summary>
    /// Contains the pattern settings for a file renaming operation
    /// </summary>
    public class RenamePattern
    {
        /// <summary>
        /// Text to add at the beginning of the filename
        /// </summary>
        public string Prefix { get; set; } = string.Empty;

        /// <summary>
        /// Text to add at the end of the filename
        /// </summary>
        public string Suffix { get; set; } = string.Empty;

        /// <summary>
        /// Text to find in the original filename
        /// </summary>
        public string FindText { get; set; } = string.Empty;

        /// <summary>
        /// Text to replace the found text with
        /// </summary>
        public string ReplaceText { get; set; } = string.Empty;

        /// <summary>
        /// Whether to use regular expressions for find and replace
        /// </summary>
        public bool UseRegex { get; set; } = false;

        /// <summary>
        /// Whether to include sequence numbers in the filenames
        /// </summary>
        public bool UseSequence { get; set; } = false;

        /// <summary>
        /// The starting number for the sequence
        /// </summary>
        public int SequenceStart { get; set; } = 1;

        /// <summary>
        /// The amount to increment the sequence for each file
        /// </summary>
        public int SequenceIncrement { get; set; } = 1;

        /// <summary>
        /// The format string for the sequence number (e.g., "000" for 001, 002, etc.)
        /// </summary>
        public string SequenceFormat { get; set; } = "000";

        /// <summary>
        /// Where the sequence number should be placed in the filename
        /// </summary>
        public SequencePosition SequencePosition { get; set; } = SequencePosition.Prefix;
    }
}
