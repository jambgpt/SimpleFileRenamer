using SimpleFileRenamer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace SimpleFileRenamer
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        /// <summary>
        /// Gets the updated settings after the dialog is closed
        /// </summary>
        public AppSettings UpdatedSettings { get; private set; }
        
        /// <summary>
        /// Gets a value indicating whether the default pattern should be loaded
        /// </summary>
        public bool ShouldLoadDefaultPattern { get; private set; }
        
        private ObservableCollection<SavedPattern> _patterns = new ObservableCollection<SavedPattern>();
        private RenamePattern _currentPattern;
        
        /// <summary>
        /// Creates a new instance of the SettingsWindow
        /// </summary>
        /// <param name="settings">The current application settings</param>
        /// <param name="currentPattern">The current pattern being used</param>
        public SettingsWindow(AppSettings settings, RenamePattern currentPattern = null)
        {
            InitializeComponent();
            
            // Create a working copy of the settings
            UpdatedSettings = new AppSettings
            {
                Theme = settings.Theme,
                CreateUndoFileByDefault = settings.CreateUndoFileByDefault
            };
            
            // Copy patterns from the original settings
            if (settings.DefaultPatterns != null)
            {
                UpdatedSettings.DefaultPatterns = new List<RenamePattern>();
                foreach (var pattern in settings.DefaultPatterns)
                {
                    UpdatedSettings.DefaultPatterns.Add(pattern.Clone());
                }
            }
            
            // Store current pattern
            _currentPattern = currentPattern;
            
            // Apply settings to UI
            LightThemeRadio.IsChecked = settings.Theme.Equals("Light", StringComparison.OrdinalIgnoreCase);
            DarkThemeRadio.IsChecked = settings.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase);
            CreateUndoFileCheckbox.IsChecked = settings.CreateUndoFileByDefault;
            
            // Load saved patterns into list view
            LoadPatternList();
            
            // Update button state
            UpdateButtonState();
        }
        
        /// <summary>
        /// Loads the pattern list into the list view
        /// </summary>
        private void LoadPatternList()
        {
            _patterns.Clear();
            
            if (UpdatedSettings.DefaultPatterns != null)
            {
                foreach (var pattern in UpdatedSettings.DefaultPatterns)
                {
                    _patterns.Add(new SavedPattern
                    {
                        Name = GetPatternName(pattern),
                        Description = pattern.ToString(),
                        Pattern = pattern
                    });
                }
            }
            
            PatternsListView.ItemsSource = _patterns;
        }
        
        /// <summary>
        /// Gets a suitable name for the pattern
        /// </summary>
        private string GetPatternName(RenamePattern pattern)
        {
            if (pattern == null) return "Unknown";
            
            // Generate a name based on pattern properties
            string name = "";
            
            if (!string.IsNullOrEmpty(pattern.Prefix))
                name += "Prefix";
                
            if (!string.IsNullOrEmpty(pattern.Suffix))
                name += (name.Length > 0 ? "+" : "") + "Suffix";
                
            if (!string.IsNullOrEmpty(pattern.FindText) && !string.IsNullOrEmpty(pattern.ReplaceText))
                name += (name.Length > 0 ? "+" : "") + "Replace";
                
            if (pattern.UseSequence)
                name += (name.Length > 0 ? "+" : "") + "Sequence";
                
            if (string.IsNullOrEmpty(name))
                name = "Empty Pattern";
                
            return name;
        }
        
        /// <summary>
        /// Updates the enabled state of buttons
        /// </summary>
        private void UpdateButtonState()
        {
            bool hasSelection = PatternsListView.SelectedItem != null;
            DeletePatternButton.IsEnabled = hasSelection;
            LoadPatternButton.IsEnabled = hasSelection;
            SaveCurrentPatternButton.IsEnabled = _currentPattern != null;
        }
        
        /// <summary>
        /// Handles the save button click
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Update settings from UI
            UpdatedSettings.Theme = LightThemeRadio.IsChecked == true ? "Light" : "Dark";
            UpdatedSettings.CreateUndoFileByDefault = CreateUndoFileCheckbox.IsChecked == true;
            
            // Close dialog
            DialogResult = true;
        }
        
        /// <summary>
        /// Handles the save current pattern button click
        /// </summary>
        private void SaveCurrentPatternButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPattern == null) return;
            
            // Get a name for the pattern
            var dialog = new InputDialog("Enter a name for this pattern:", "Save Pattern", GetPatternName(_currentPattern));
            dialog.Owner = this;
            
            if (dialog.ShowDialog() == true)
            {
                var name = dialog.Result;
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Please enter a valid name for the pattern.", "Save Pattern", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                // Clone the current pattern and add it to the list
                var savedPattern = _currentPattern.Clone();
                
                // Initialize the patterns list if needed
                UpdatedSettings.DefaultPatterns = UpdatedSettings.DefaultPatterns ?? new List<RenamePattern>();
                UpdatedSettings.DefaultPatterns.Add(savedPattern);
                
                // Add to the UI list
                _patterns.Add(new SavedPattern
                {
                    Name = name,
                    Description = savedPattern.ToString(),
                    Pattern = savedPattern
                });
                
                // Update button state
                UpdateButtonState();
            }
        }
        
        /// <summary>
        /// Handles the delete pattern button click
        /// </summary>
        private void DeletePatternButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = PatternsListView.SelectedItem as SavedPattern;
            if (selectedItem == null) return;
            
            var result = MessageBox.Show($"Are you sure you want to delete the pattern '{selectedItem.Name}'?", "Delete Pattern", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                // Remove from the settings
                UpdatedSettings.DefaultPatterns.RemoveAll(p => p == selectedItem.Pattern);
                
                // Remove from the UI list
                _patterns.Remove(selectedItem);
                
                // Update button state
                UpdateButtonState();
            }
        }
        
        /// <summary>
        /// Handles the load pattern button click
        /// </summary>
        private void LoadPatternButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = PatternsListView.SelectedItem as SavedPattern;
            if (selectedItem == null) return;
            
            var result = MessageBox.Show($"Load the pattern '{selectedItem.Name}' when closing settings?", "Load Pattern", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                ShouldLoadDefaultPattern = true;
                DialogResult = true;
            }
        }
    }
    
    /// <summary>
    /// Represents a saved pattern in the list
    /// </summary>
    public class SavedPattern
    {
        /// <summary>
        /// Gets or sets the name of the pattern
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the description of the pattern
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the actual pattern object
        /// </summary>
        public required RenamePattern Pattern { get; set; }
    }
}