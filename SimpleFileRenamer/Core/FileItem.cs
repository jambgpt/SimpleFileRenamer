using System.IO;

namespace SimpleFileRenamer.Core
{
    /// <summary>
    /// Represents a file that can be renamed, with original and new filename information
    /// </summary>
    public class FileItem
    {
        /// <summary>
        /// The full path to the file
        /// </summary>
        public string FullPath { get; private set; }

        /// <summary>
        /// The original filename including extension
        /// </summary>
        public string OriginalFileName { get; private set; }

        /// <summary>
        /// The new filename including extension
        /// </summary>
        public string NewFileName { get; set; }

        /// <summary>
        /// Indicates whether there's a conflict or error with the new filename
        /// </summary>
        public bool HasConflict { get; set; }

        /// <summary>
        /// Error message explaining the conflict, if any
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The file's size in bytes
        /// </summary>
        public long FileSize { get; private set; }

        /// <summary>
        /// The file's last modified date
        /// </summary>
        public DateTime LastModified { get; private set; }

        /// <summary>
        /// The file's index in the list (used for sequence numbering)
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Creates a new FileItem from a FileInfo object
        /// </summary>
        /// <param name="fileInfo">FileInfo object for the file</param>
        public FileItem(FileInfo fileInfo)
        {
            FullPath = fileInfo.FullName;
            OriginalFileName = fileInfo.Name;
            NewFileName = fileInfo.Name;
            FileSize = fileInfo.Length;
            LastModified = fileInfo.LastWriteTime;
            HasConflict = false;
            ErrorMessage = string.Empty;
        }

        /// <summary>
        /// Creates a new FileItem from a file path
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        public FileItem(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            FullPath = fileInfo.FullName;
            OriginalFileName = fileInfo.Name;
            NewFileName = fileInfo.Name;
            FileSize = fileInfo.Length;
            LastModified = fileInfo.LastWriteTime;
            HasConflict = false;
            ErrorMessage = string.Empty;
        }

        /// <summary>
        /// Updates the file path after a successful rename operation
        /// </summary>
        /// <param name="newPath">The new path of the file</param>
        public void UpdatePathAfterRename(string newPath)
        {
            FullPath = newPath;
            OriginalFileName = Path.GetFileName(newPath);
            NewFileName = OriginalFileName;
            HasConflict = false;
            ErrorMessage = string.Empty;
        }
    }
}
