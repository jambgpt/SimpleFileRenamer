using Microsoft.Win32;
using SimpleFileRenamer.Core;
using SimpleFileRenamer.Models;
using SimpleFileRenamer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
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
        private AppSettings _settings;

        public MainWindow()
        {
            // Load settings first
            _settings = AppSettings.Load();
            
            // Apply theme
            ThemeManager.ApplyTheme(_settings.Theme);
            
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
            _renamerLogic = new RenamerLogic();

            FileList.SetViewModel(_viewModel);
            PatternInput.SetViewModel(_viewModel);
            
            // Apply settings
            CreateUndoFileCheckbox.IsChecked = _settings.CreateUndoFileByDefault;
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
        
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // Pass the current pattern to the settings window
            var currentPattern = _viewModel.RenamePattern?.Clone();
            
            var settingsWindow = new SettingsWindow(_settings, currentPattern);
            settingsWindow.Owner = this;
            if (settingsWindow.ShowDialog() == true)
            {
                // Settings were updated
                _settings = settingsWindow.UpdatedSettings;
                _settings.Save();
                
                // Apply new theme if changed
                ThemeManager.ApplyTheme(_settings.Theme);
                
                // Apply other settings
                CreateUndoFileCheckbox.IsChecked = _settings.CreateUndoFileByDefault;
                
                // Apply saved patterns to pattern control if needed
                if (_settings.DefaultPatterns?.Count > 0 && settingsWindow.ShouldLoadDefaultPattern)
                {
                    // Load the first default pattern
                    PatternInput.LoadPattern(_settings.DefaultPatterns[0]);
                    UpdateFilePreview();
                }
                
                StatusText.Text = "Settings updated";
            }
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

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if there's a last undo file
            if (string.IsNullOrEmpty(_viewModel.LastUndoFilePath))
            {
                MessageBox.Show(
                    "No recent rename operation to undo.",
                    "Undo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }
            
            // Confirm with the user
            var result = MessageBox.Show(
                "Are you sure you want to undo the last rename operation?",
                "Confirm Undo",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
                
            if (result != MessageBoxResult.Yes) return;
            
            // Process the undo file
            try
            {
                var undoResult = _viewModel.ProcessUndoFile(_viewModel.LastUndoFilePath);
                
                // Display the result
                if (undoResult.Success)
                {
                    if (undoResult.FailedItems.Count > 0)
                    {
                        var detailsWindow = new UndoResultWindow(undoResult);
                        detailsWindow.Owner = this;
                        detailsWindow.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show(
                            "All files were successfully restored to their original names.",
                            "Undo Successful",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    
                    // Clear the last undo file path
                    _viewModel.LastUndoFilePath = string.Empty;
                    
                    // Update status
                    StatusText.Text = undoResult.Message;
                }
                else
                {
                    MessageBox.Show(
                        $"Failed to undo the rename operation: {undoResult.Message}",
                        "Undo Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error during undo operation: {ex.Message}",
                    "Undo Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        
        private void UndoHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            var undoWindow = new UndoWindow();
            undoWindow.Owner = this;
            undoWindow.ShowDialog();
            
            // Refresh the file list after undo operations
            FileList.RefreshList();
            UpdateRenameButtonState();
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

                // Create lists for undo file
                var originalPaths = new List<string>();
                var newPaths = new List<string>();
                
                foreach (var file in filesToRename)
                {
                    originalPaths.Add(file.FullPath);
                    var directory = Path.GetDirectoryName(file.FullPath) ?? string.Empty;
                    var newFullPath = Path.Combine(directory, file.NewFileName);
                    newPaths.Add(newFullPath);
                }
                
                // Create undo file
                if (CreateUndoFileCheckbox.IsChecked == true)
                {
                    try
                    {
                        string undoFilePath = _viewModel.CreateUndoFile(originalPaths, newPaths);
                        StatusText.Text = $"Undo file created: {undoFilePath}";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Could not create undo file: {ex.Message}",
                            "Undo File Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
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
