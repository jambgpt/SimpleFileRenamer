using SimpleFileRenamer.Models;
using System.Text.RegularExpressions;

namespace SimpleFileRenamer.Core
{
    /// <summary>
    /// Handles parsing and validation of file rename patterns
    /// </summary>
    public class PatternParser
    {
        /// <summary>
        /// Validates a rename pattern to ensure it's well-formed
        /// </summary>
        /// <param name="pattern">The pattern to validate</param>
        /// <returns>A validation result with any error information</returns>
        public PatternValidationResult ValidatePattern(RenamePattern? pattern)
        {
            if (pattern == null)
            {
                return new PatternValidationResult 
                { 
                    IsValid = false, 
                    ErrorMessage = "Pattern is null" 
                };
            }

            // Check if at least one operation is applied
            bool hasOperation = !string.IsNullOrEmpty(pattern.Prefix) ||
                               !string.IsNullOrEmpty(pattern.Suffix) ||
                               pattern.UseSequence ||
                               (!string.IsNullOrEmpty(pattern.FindText) && 
                                pattern.FindText != pattern.ReplaceText);

            if (!hasOperation)
            {
                return new PatternValidationResult 
                { 
                    IsValid = false, 
                    ErrorMessage = "Pattern does not make any changes" 
                };
            }

            // Validate regex pattern if regex is enabled
            if (pattern.UseRegex && !string.IsNullOrEmpty(pattern.FindText))
            {
                try
                {
                    // Test the regex pattern by creating a Regex object
                    new Regex(pattern.FindText);
                }
                catch (ArgumentException ex)
                {
                    return new PatternValidationResult 
                    { 
                        IsValid = false, 
                        ErrorMessage = $"Invalid regular expression: {ex.Message}" 
                    };
                }
            }

            // Validate sequence format if sequence is enabled
            if (pattern.UseSequence)
            {
                try
                {
                    // Test the sequence format by formatting a sample number
                    int testNumber = pattern.SequenceStart;
                    string formattedNumber = testNumber.ToString(pattern.SequenceFormat);
                }
                catch (FormatException ex)
                {
                    return new PatternValidationResult 
                    { 
                        IsValid = false, 
                        ErrorMessage = $"Invalid sequence format: {ex.Message}" 
                    };
                }
            }

            return new PatternValidationResult { IsValid = true };
        }
    }

    /// <summary>
    /// Contains the result of pattern validation
    /// </summary>
    public class PatternValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
