using SimpleFileRenamer.Models;
using SimpleFileRenamer.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using UserControl = System.Windows.Controls.UserControl;
using SequencePosition = SimpleFileRenamer.Models.SequencePosition;

namespace SimpleFileRenamer.Controls
{
    /// <summary>
    /// Interaction logic for PatternInputControl.xaml
    /// </summary>
    public partial class PatternInputControl : UserControl
    {
        private MainViewModel? _viewModel;
        
        public event EventHandler? PatternChanged;

        public PatternInputControl()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Loads a pattern into the control
        /// </summary>
        /// <param name="pattern">The pattern to load</param>
        public void LoadPattern(RenamePattern pattern)
        {
            if (pattern == null) return;
            
            // Set the text inputs
            PrefixTextBox.Text = pattern.Prefix ?? string.Empty;
            SuffixTextBox.Text = pattern.Suffix ?? string.Empty;
            FindTextBox.Text = pattern.FindText ?? string.Empty;
            ReplaceTextBox.Text = pattern.ReplaceText ?? string.Empty;
            
            // Set checkboxes
            UseRegexCheckBox.IsChecked = pattern.UseRegex;
            UseSequenceCheckBox.IsChecked = pattern.UseSequence;
            
            // Set sequence options
            SequenceStartTextBox.Text = pattern.SequenceStart.ToString();
            SequenceIncrementTextBox.Text = pattern.SequenceIncrement.ToString();
            SequenceFormatTextBox.Text = pattern.SequenceFormat ?? "0";
            
            // Set sequence position
            ComboBoxItem? positionItem = null;
            switch (pattern.SequencePosition)
            {
                case SequencePosition.Prefix:
                    positionItem = SequencePositionComboBox.Items.Cast<ComboBoxItem>()
                        .FirstOrDefault(i => i.Tag.ToString() == "Prefix");
                    break;
                case SequencePosition.Suffix:
                    positionItem = SequencePositionComboBox.Items.Cast<ComboBoxItem>()
                        .FirstOrDefault(i => i.Tag.ToString() == "Suffix");
                    break;
                case SequencePosition.Replace:
                    positionItem = SequencePositionComboBox.Items.Cast<ComboBoxItem>()
                        .FirstOrDefault(i => i.Tag.ToString() == "Replace");
                    break;
            }
            
            if (positionItem != null)
                SequencePositionComboBox.SelectedItem = positionItem;
                
            // Update sequence controls state
            SequenceStartTextBox.IsEnabled = pattern.UseSequence;
            SequenceIncrementTextBox.IsEnabled = pattern.UseSequence;
            SequenceFormatTextBox.IsEnabled = pattern.UseSequence;
            SequencePositionComboBox.IsEnabled = pattern.UseSequence;
            
            // Update the pattern in the view model
            UpdatePatternFromInputs();
            
            // Trigger pattern changed event
            PatternChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sets the view model for the control
        /// </summary>
        /// <param name="viewModel">The view model</param>
        public void SetViewModel(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        /// <summary>
        /// Called when any pattern input changes
        /// </summary>
        private void Pattern_Changed(object sender, RoutedEventArgs e)
        {
            UpdatePatternFromInputs();
            PatternChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates the pattern model from the UI inputs
        /// </summary>
        private void UpdatePatternFromInputs()
        {
            if (_viewModel == null) return;

            var pattern = new RenamePattern
            {
                Prefix = PrefixTextBox.Text.Trim(),
                Suffix = SuffixTextBox.Text.Trim(),
                FindText = FindTextBox.Text,
                ReplaceText = ReplaceTextBox.Text,
                UseRegex = UseRegexCheckBox.IsChecked ?? false,
                UseSequence = UseSequenceCheckBox.IsChecked ?? false
            };

            // Parse sequence options if sequence is enabled
            if (pattern.UseSequence)
            {
                // Parse the start value (default to 1 if parsing fails)
                if (!int.TryParse(SequenceStartTextBox.Text, out int start))
                {
                    start = 1;
                    SequenceStartTextBox.Text = "1";
                }
                pattern.SequenceStart = start;

                // Parse the increment value (default to 1 if parsing fails)
                if (!int.TryParse(SequenceIncrementTextBox.Text, out int increment) || increment < 1)
                {
                    increment = 1;
                    SequenceIncrementTextBox.Text = "1";
                }
                pattern.SequenceIncrement = increment;

                // Set the format string
                pattern.SequenceFormat = SequenceFormatTextBox.Text;

                // Set the sequence position based on the selected combo box item
                var selectedItem = SequencePositionComboBox.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    pattern.SequencePosition = selectedItem.Tag.ToString() switch
                    {
                        "Prefix" => SequencePosition.Prefix,
                        "Suffix" => SequencePosition.Suffix,
                        "Replace" => SequencePosition.Replace,
                        _ => SequencePosition.Prefix
                    };
                }
            }

            _viewModel.UpdatePattern(pattern);
        }

        /// <summary>
        /// Handles the checked changed event for the sequence checkbox
        /// </summary>
        private void UseSequenceCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            bool isChecked = UseSequenceCheckBox.IsChecked ?? false;
            
            SequenceStartTextBox.IsEnabled = isChecked;
            SequenceIncrementTextBox.IsEnabled = isChecked;
            SequenceFormatTextBox.IsEnabled = isChecked;
            SequencePositionComboBox.IsEnabled = isChecked;
            
            // Also update the pattern
            Pattern_Changed(sender, e);
        }
    }
}
