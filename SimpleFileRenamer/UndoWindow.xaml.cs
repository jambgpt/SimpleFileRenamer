using System.Collections.Generic;
using System.Windows;
using SimpleFileRenamer.Core;
using System.Linq;
using System.Collections.ObjectModel;
using MessageBox = System.Windows.MessageBox;

namespace SimpleFileRenamer
{
    /// <summary>
    /// Interaction logic for UndoWindow.xaml
    /// </summary>
    public partial class UndoWindow : Window
    {
        private ObservableCollection<UndoFileInfo> _undoFiles = new ObservableCollection<UndoFileInfo>();
        
        /// <summary>
        /// Creates a new instance of the UndoWindow
        /// </summary>
        public UndoWindow()
        {
            InitializeComponent();
            
            // Set the list view's data source
            UndoFilesListView.ItemsSource = _undoFiles;
            
            // Load the undo files
            LoadUndoFiles();
        }
        
        /// <summary>
        /// Loads the available undo files
        /// </summary>
        private void LoadUndoFiles()
        {
            // Clear the current list
            _undoFiles.Clear();
            
            // Get the available undo files
            List<UndoFileInfo> undoFiles = UndoManager.GetAvailableUndoFiles();
            
            // Add each undo file to the list
            foreach (var undoFile in undoFiles)
            {
                _undoFiles.Add(undoFile);
            }
            
            // Update the UI based on whether there are any undo files
            if (_undoFiles.Count == 0)
            {
                NoItemsMessage.Visibility = Visibility.Visible;
                UndoFilesListView.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoItemsMessage.Visibility = Visibility.Collapsed;
                UndoFilesListView.Visibility = Visibility.Visible;
            }
            
            // Update button state
            UpdateButtonState();
        }
        
        /// <summary>
        /// Updates the enabled state of buttons
        /// </summary>
        private void UpdateButtonState()
        {
            UndoButton.IsEnabled = UndoFilesListView.SelectedItem != null;
        }
        
        /// <summary>
        /// Handles the selection changed event of the undo files list view
        /// </summary>
        private void UndoFilesListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateButtonState();
        }
        
        /// <summary>
        /// Handles the refresh button click
        /// </summary>
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadUndoFiles();
        }
        
        /// <summary>
        /// Handles the undo button click
        /// </summary>
        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedUndoFile = UndoFilesListView.SelectedItem as UndoFileInfo;
            if (selectedUndoFile == null) return;
            
            // Ask for confirmation
            var result = MessageBox.Show(
                $"Are you sure you want to undo the rename operation from {selectedUndoFile.OperationTime:g}?\n\nThis will attempt to restore {selectedUndoFile.ItemCount} files to their original names.",
                "Confirm Undo",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
                
            if (result != MessageBoxResult.Yes) return;
            
            // Process the undo file
            UndoResult undoResult = UndoManager.ProcessUndoFile(selectedUndoFile.FilePath);
            
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
                
                // Refresh the list of undo files
                LoadUndoFiles();
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
    }
}