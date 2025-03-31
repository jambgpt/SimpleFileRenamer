using Microsoft.Win32;
using SimpleFileRenamer.Core;
using SimpleFileRenamer.Models;
using SimpleFileRenamer.ViewModels;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace SimpleFileRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        private readonly RenamerLogic _renamerLogic;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
            _renamerLogic = new RenamerLogic();

            FileList.SetViewModel(_viewModel);
            PatternInput.SetViewModel(_viewModel);
        }

        public MainWindow(List<string> filePaths) : this()
        {
            // Load files from the provided paths (used when launched from Explorer)
            LoadFilesFromPaths(filePaths);
        }

        private void AddFilesButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = "Select files to rename"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                LoadFilesFromPaths(openFileDialog.FileNames.ToList());
            }
        }

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            using var folderDialog = new FolderBrowserDialog
            {
                Description = "Select a folder with files to rename",
                UseDescriptionForTitle = true,
                ShowNewFolderButton = false
            };

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var filePaths = Directory.GetFiles(folderDialog.SelectedPath).ToList();
                LoadFilesFromPaths(filePaths);
            }
        }

        private void ClearListButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ClearFiles();
            UpdateRenameButtonState();
            StatusText.Text = "File list cleared";
        }

        private void PatternInput_PatternChanged(object sender, EventArgs e)
        {
            UpdateFilePreview();
        }

        private void RenameFilesButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteRenaming();
        }

        private void LoadFilesFromPaths(List<string> filePaths)
        {
            if (filePaths == null || filePaths.Count == 0)
                return;

            var fileItems = new List<FileItem>();
            foreach (var path in filePaths)
            {
                if (File.Exists(path))
                {
                    var fileInfo = new FileInfo(path);
                    fileItems.Add(new FileItem(fileInfo));
                }
            }

            _viewModel.AddFiles(fileItems);
            UpdateFilePreview();
            UpdateRenameButtonState();
            StatusText.Text = $"Added {fileItems.Count} files";
        }

        private void UpdateFilePreview()
        {
            if (_viewModel.Files.Count == 0 || _viewModel.RenamePattern == null)
                return;

            try
            {
                // Get the rename pattern from the UI
                var pattern = _viewModel.RenamePattern;

                // Update previews for all files
                foreach (var file in _viewModel.Files)
                {
                    var result = _renamerLogic.GenerateNewFileName(file, pattern);
                    file.NewFileName = result.NewFileName;
                    file.HasConflict = result.HasError;
                    file.ErrorMessage = result.ErrorMessage;
                }

                // Check for duplicate new filenames
                var newNames = _viewModel.Files.GroupBy(f => Path.Combine(Path.GetDirectoryName(f.FullPath) ?? string.Empty, f.NewFileName))
                                              .Where(g => g.Count() > 1);

                foreach (var group in newNames)
                {
                    foreach (var file in group)
                    {
                        file.HasConflict = true;
                        file.ErrorMessage = "Duplicate filename would be created";
                    }
                }

                FileList.RefreshList();
                StatusText.Text = "Preview updated";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Error generating preview: {ex.Message}";
            }
        }

        private void UpdateRenameButtonState()
        {
            bool hasValidFiles = _viewModel.Files.Count > 0 && 
                                _viewModel.Files.Any(f => !f.HasConflict && f.OriginalFileName != f.NewFileName);
            
            RenameFilesButton.IsEnabled = hasValidFiles;
        }

        private void ExecuteRenaming()
        {
            try
            {
                // Filter only files that have a new name and no conflicts
                var filesToRename = _viewModel.Files
                    .Where(f => !f.HasConflict && f.OriginalFileName != f.NewFileName)
                    .ToList();

                if (filesToRename.Count == 0)
                {
                    StatusText.Text = "No valid files to rename";
                    return;
                }

                // Create undo file if the option is checked
                if (CreateUndoFileCheckbox.IsChecked == true)
                {
                    var saveFileDialog = new SaveFileDialog
                    {
                        Title = "Save undo file",
                        Filter = "Text files (*.txt)|*.txt",
                        FileName = $"UndoRename_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        using var writer = new StreamWriter(saveFileDialog.FileName);
                        writer.WriteLine("# Rename Undo File - Generated on: " + DateTime.Now.ToString());
                        writer.WriteLine("# To use this file for manual undo, rename files back using the pairs listed below");
                        writer.WriteLine();
                        
                        foreach (var file in filesToRename)
                        {
                            var directory = Path.GetDirectoryName(file.FullPath) ?? string.Empty;
                            var newFullPath = Path.Combine(directory, file.NewFileName);
                            writer.WriteLine($"{newFullPath}|{file.FullPath}");
                        }
                    }
                }

                // Perform the actual renaming
                var results = _renamerLogic.RenameFiles(filesToRename);
                
                // Update the UI with results
                int successCount = results.Count(r => r.Success);
                int errorCount = results.Count - successCount;
                
                // Update the file items with the results
                foreach (var result in results)
                {
                    var file = _viewModel.Files.FirstOrDefault(f => f.FullPath == result.OriginalPath);
                    if (file != null)
                    {
                        if (result.Success)
                        {
                            // Update the file item with its new path
                            file.UpdatePathAfterRename(result.NewPath);
                        }
                        else
                        {
                            file.HasConflict = true;
                            file.ErrorMessage = result.ErrorMessage;
                        }
                    }
                }
                
                // Refresh the list to reflect changes
                FileList.RefreshList();
                
                // Update status text
                StatusText.Text = $"Renamed {successCount} files successfully. {errorCount} errors.";
                
                // Show detailed results in a message box
                if (errorCount > 0)
                {
                    var errorDetails = string.Join("\n", 
                        results.Where(r => !r.Success)
                              .Select(r => $"Error: {Path.GetFileName(r.OriginalPath)} - {r.ErrorMessage}"));
                    
                    System.Windows.MessageBox.Show(
                        $"Renamed {successCount} files.\n{errorCount} files could not be renamed:\n\n{errorDetails}",
                        "Rename Results",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
                else
                {
                    System.Windows.MessageBox.Show(
                        $"Successfully renamed {successCount} files.",
                        "Rename Results",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Error during rename operation: {ex.Message}";
                System.Windows.MessageBox.Show(
                    $"An error occurred during the rename operation:\n\n{ex.Message}",
                    "Rename Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
