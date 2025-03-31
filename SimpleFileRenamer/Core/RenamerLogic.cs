using SimpleFileRenamer.Models;
using System.IO;
using System.Text.RegularExpressions;
using SequencePosition = SimpleFileRenamer.Models.SequencePosition;

namespace SimpleFileRenamer.Core
{
    /// <summary>
    /// Core logic for file renaming operations
    /// </summary>
    public class RenamerLogic
    {
        private readonly PatternParser _patternParser;

        public RenamerLogic()
        {
            _patternParser = new PatternParser();
        }

        /// <summary>
        /// Generates a new filename based on the original file and the rename pattern
        /// </summary>
        /// <param name="file">The original file</param>
        /// <param name="pattern">The rename pattern to apply</param>
        /// <returns>The result containing the new filename and any error information</returns>
        public RenameResult GenerateNewFileName(FileItem? file, RenamePattern? pattern)
        {
            if (file == null)
                return new RenameResult { HasError = true, ErrorMessage = "File is null" };

            if (pattern == null)
                return new RenameResult { NewFileName = file.OriginalFileName };

            try
            {
                string fileName = Path.GetFileNameWithoutExtension(file.OriginalFileName);
                string extension = Path.GetExtension(file.OriginalFileName);
                
                // Apply prefix if specified
                if (!string.IsNullOrEmpty(pattern.Prefix))
                {
                    fileName = pattern.Prefix + fileName;
                }

                // Apply find and replace if specified
                if (!string.IsNullOrEmpty(pattern.FindText) && pattern.FindText != pattern.ReplaceText)
                {
                    if (pattern.UseRegex)
                    {
                        try
                        {
                            fileName = Regex.Replace(fileName, pattern.FindText, pattern.ReplaceText ?? string.Empty);
                        }
                        catch (ArgumentException ex)
                        {
                            return new RenameResult
                            {
                                NewFileName = file.OriginalFileName,
                                HasError = true,
                                ErrorMessage = $"Invalid regex pattern: {ex.Message}"
                            };
                        }
                    }
                    else
                    {
                        fileName = fileName.Replace(pattern.FindText, pattern.ReplaceText ?? string.Empty);
                    }
                }

                // Apply sequence number if specified
                if (pattern.UseSequence)
                {
                    string sequenceNumber = (pattern.SequenceStart + file.Index * pattern.SequenceIncrement).ToString(pattern.SequenceFormat);
                    
                    // Apply sequence at the specified position
                    switch (pattern.SequencePosition)
                    {
                        case SequencePosition.Prefix:
                            fileName = sequenceNumber + fileName;
                            break;
                        case SequencePosition.Suffix:
                            fileName = fileName + sequenceNumber;
                            break;
                        case SequencePosition.Replace:
                            fileName = sequenceNumber;
                            break;
                    }
                }

                // Apply suffix if specified
                if (!string.IsNullOrEmpty(pattern.Suffix))
                {
                    fileName = fileName + pattern.Suffix;
                }

                // Combine with extension
                string newFileName = fileName + extension;

                // Validate the new filename
                if (!IsValidFileName(newFileName))
                {
                    return new RenameResult
                    {
                        NewFileName = newFileName,
                        HasError = true,
                        ErrorMessage = "Invalid characters in filename"
                    };
                }

                return new RenameResult { NewFileName = newFileName };
            }
            catch (Exception ex)
            {
                return new RenameResult
                {
                    NewFileName = file.OriginalFileName,
                    HasError = true,
                    ErrorMessage = $"Error generating filename: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Renames the specified files according to their new filenames
        /// </summary>
        /// <param name="files">The files to rename</param>
        /// <returns>List of results for each file rename operation</returns>
        public List<FileRenameOperationResult> RenameFiles(List<FileItem> files)
        {
            var results = new List<FileRenameOperationResult>();

            foreach (var file in files)
            {
                var result = new FileRenameOperationResult
                {
                    OriginalPath = file.FullPath,
                    NewPath = Path.Combine(Path.GetDirectoryName(file.FullPath) ?? string.Empty, file.NewFileName)
                };

                try
                {
                    // Skip if the file doesn't need to be renamed
                    if (file.OriginalFileName == file.NewFileName)
                    {
                        result.Success = true;
                        results.Add(result);
                        continue;
                    }

                    // Skip if there's a conflict
                    if (file.HasConflict)
                    {
                        result.Success = false;
                        result.ErrorMessage = file.ErrorMessage;
                        results.Add(result);
                        continue;
                    }

                    // Check if the destination file already exists
                    if (File.Exists(result.NewPath))
                    {
                        result.Success = false;
                        result.ErrorMessage = "Destination file already exists";
                        results.Add(result);
                        continue;
                    }

                    // Attempt to rename the file
                    File.Move(file.FullPath, result.NewPath);
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.ErrorMessage = ex.Message;
                }

                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// Checks if a filename is valid for the current OS
        /// </summary>
        /// <param name="fileName">The filename to check</param>
        /// <returns>True if the filename is valid, false otherwise</returns>
        private bool IsValidFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            // Check for invalid characters
            char[] invalidChars = Path.GetInvalidFileNameChars();
            if (fileName.Any(c => invalidChars.Contains(c)))
                return false;

            // Check for reserved Windows filenames
            string[] reservedNames = { "CON", "PRN", "AUX", "NUL", 
                                      "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
                                      "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };
            
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName).ToUpperInvariant();
            if (reservedNames.Contains(nameWithoutExtension))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Result of a file rename operation
    /// </summary>
    public class FileRenameOperationResult
    {
        public string OriginalPath { get; set; } = string.Empty;
        public string NewPath { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
