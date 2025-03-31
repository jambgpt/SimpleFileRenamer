using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace SimpleFileRenamer.Core
{
    /// <summary>
    /// Manages the creation and processing of undo files for rename operations
    /// </summary>
    public class UndoManager
    {
        private const string UNDO_FILE_EXTENSION = ".undorename";
        
        /// <summary>
        /// Creates an undo file for a batch of renames
        /// </summary>
        /// <param name="originalPaths">The original file paths</param>
        /// <param name="newPaths">The new file paths</param>
        /// <param name="operationDateTime">The date and time of the operation (optional)</param>
        /// <returns>The path to the created undo file</returns>
        public static string CreateUndoFile(IList<string> originalPaths, IList<string> newPaths, DateTime? operationDateTime = null)
        {
            if (originalPaths == null || newPaths == null || originalPaths.Count != newPaths.Count)
            {
                throw new ArgumentException("Original paths and new paths must be valid and have the same count");
            }
            
            // Build the undo data
            var undoData = new UndoData
            {
                OperationTime = operationDateTime ?? DateTime.Now,
                Items = new List<UndoItem>()
            };
            
            for (int i = 0; i < originalPaths.Count; i++)
            {
                undoData.Items.Add(new UndoItem
                {
                    OriginalPath = originalPaths[i],
                    NewPath = newPaths[i]
                });
            }
            
            // Generate a unique filename for the undo file
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string undoFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "SimpleFileRenamer",
                "UndoFiles",
                $"Undo_{timestamp}{UNDO_FILE_EXTENSION}");
            
            // Ensure the directory exists
            string undoDirectory = Path.GetDirectoryName(undoFilePath);
            if (!string.IsNullOrEmpty(undoDirectory) && !Directory.Exists(undoDirectory))
            {
                Directory.CreateDirectory(undoDirectory);
            }
            
            // Serialize and save the undo data
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonData = JsonSerializer.Serialize(undoData, options);
            File.WriteAllText(undoFilePath, jsonData, Encoding.UTF8);
            
            return undoFilePath;
        }
        
        /// <summary>
        /// Process an undo file to revert file renames
        /// </summary>
        /// <param name="undoFilePath">The path to the undo file</param>
        /// <returns>A result object containing success/failure information and details</returns>
        public static UndoResult ProcessUndoFile(string undoFilePath)
        {
            if (string.IsNullOrEmpty(undoFilePath) || !File.Exists(undoFilePath))
            {
                return new UndoResult
                {
                    Success = false,
                    Message = "Undo file not found",
                    FailedItems = new List<UndoFailure>()
                };
            }
            
            try
            {
                // Read and deserialize the undo file
                string jsonData = File.ReadAllText(undoFilePath, Encoding.UTF8);
                var undoData = JsonSerializer.Deserialize<UndoData>(jsonData);
                
                if (undoData == null || undoData.Items == null || !undoData.Items.Any())
                {
                    return new UndoResult
                    {
                        Success = false,
                        Message = "Invalid or empty undo file",
                        FailedItems = new List<UndoFailure>()
                    };
                }
                
                // Process each undo item (rename from new path back to original path)
                var failedItems = new List<UndoFailure>();
                int successCount = 0;
                
                foreach (var item in undoData.Items)
                {
                    try
                    {
                        // Check if the new path exists
                        if (!File.Exists(item.NewPath) && !Directory.Exists(item.NewPath))
                        {
                            failedItems.Add(new UndoFailure
                            {
                                Item = item,
                                Reason = "The renamed file/folder no longer exists"
                            });
                            continue;
                        }
                        
                        // Check if the original path is already taken
                        if (File.Exists(item.OriginalPath) || Directory.Exists(item.OriginalPath))
                        {
                            failedItems.Add(new UndoFailure
                            {
                                Item = item,
                                Reason = "The original path is already occupied by another file/folder"
                            });
                            continue;
                        }
                        
                        // Perform the rename operation
                        if (File.Exists(item.NewPath))
                        {
                            File.Move(item.NewPath, item.OriginalPath);
                        }
                        else if (Directory.Exists(item.NewPath))
                        {
                            Directory.Move(item.NewPath, item.OriginalPath);
                        }
                        
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        failedItems.Add(new UndoFailure
                        {
                            Item = item,
                            Reason = $"Error: {ex.Message}"
                        });
                    }
                }
                
                // After processing, delete the undo file if all items were successful
                if (failedItems.Count == 0)
                {
                    try
                    {
                        File.Delete(undoFilePath);
                    }
                    catch
                    {
                        // Ignore errors when deleting the undo file
                    }
                }
                
                // Return the result
                return new UndoResult
                {
                    Success = successCount > 0,
                    Message = $"Undone {successCount} of {undoData.Items.Count} items",
                    FailedItems = failedItems
                };
            }
            catch (Exception ex)
            {
                return new UndoResult
                {
                    Success = false,
                    Message = $"Error processing undo file: {ex.Message}",
                    FailedItems = new List<UndoFailure>()
                };
            }
        }
        
        /// <summary>
        /// Gets a list of available undo files
        /// </summary>
        /// <returns>A list of undo file information</returns>
        public static List<UndoFileInfo> GetAvailableUndoFiles()
        {
            var result = new List<UndoFileInfo>();
            
            string undoDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "SimpleFileRenamer",
                "UndoFiles");
                
            if (!Directory.Exists(undoDirectory))
            {
                return result;
            }
            
            // Find all undo files
            string[] undoFiles = Directory.GetFiles(undoDirectory, $"*{UNDO_FILE_EXTENSION}");
            
            foreach (string filePath in undoFiles)
            {
                try
                {
                    // Read basic metadata from file
                    string jsonData = File.ReadAllText(filePath, Encoding.UTF8);
                    var undoData = JsonSerializer.Deserialize<UndoData>(jsonData);
                    
                    if (undoData != null)
                    {
                        result.Add(new UndoFileInfo
                        {
                            FilePath = filePath,
                            OperationTime = undoData.OperationTime,
                            ItemCount = undoData.Items?.Count ?? 0,
                            DisplayName = $"Rename operation from {undoData.OperationTime:g} ({undoData.Items?.Count ?? 0} files)"
                        });
                    }
                }
                catch
                {
                    // If we can't read the file, just use file information
                    var fileInfo = new FileInfo(filePath);
                    result.Add(new UndoFileInfo
                    {
                        FilePath = filePath,
                        OperationTime = fileInfo.CreationTime,
                        ItemCount = 0,
                        DisplayName = $"Rename operation from {fileInfo.CreationTime:g}"
                    });
                }
            }
            
            // Sort by most recent first
            return result.OrderByDescending(f => f.OperationTime).ToList();
        }
    }
    
    /// <summary>
    /// Represents undo data for a rename operation
    /// </summary>
    public class UndoData
    {
        /// <summary>
        /// Gets or sets the operation time
        /// </summary>
        public DateTime OperationTime { get; set; }
        
        /// <summary>
        /// Gets or sets the list of undo items
        /// </summary>
        public List<UndoItem> Items { get; set; } = new List<UndoItem>();
    }
    
    /// <summary>
    /// Represents a single file/folder rename operation
    /// </summary>
    public class UndoItem
    {
        /// <summary>
        /// Gets or sets the original path
        /// </summary>
        public string OriginalPath { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the new path
        /// </summary>
        public string NewPath { get; set; } = string.Empty;
    }
    
    /// <summary>
    /// Represents the result of an undo operation
    /// </summary>
    public class UndoResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Gets or sets a message describing the result
        /// </summary>
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the list of failed items
        /// </summary>
        public List<UndoFailure> FailedItems { get; set; } = new List<UndoFailure>();
    }
    
    /// <summary>
    /// Represents a failed undo operation for a specific item
    /// </summary>
    public class UndoFailure
    {
        /// <summary>
        /// Gets or sets the undo item that failed
        /// </summary>
        public UndoItem Item { get; set; } = new UndoItem();
        
        /// <summary>
        /// Gets or sets the reason for the failure
        /// </summary>
        public string Reason { get; set; } = string.Empty;
    }
    
    /// <summary>
    /// Represents information about an undo file
    /// </summary>
    public class UndoFileInfo
    {
        /// <summary>
        /// Gets or sets the file path
        /// </summary>
        public string FilePath { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the operation time
        /// </summary>
        public DateTime OperationTime { get; set; }
        
        /// <summary>
        /// Gets or sets the number of items in the undo file
        /// </summary>
        public int ItemCount { get; set; }
        
        /// <summary>
        /// Gets or sets the display name for the undo file
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;
    }
}