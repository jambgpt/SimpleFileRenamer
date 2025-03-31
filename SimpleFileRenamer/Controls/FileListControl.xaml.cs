using SimpleFileRenamer.Core;
using SimpleFileRenamer.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using UserControl = System.Windows.Controls.UserControl;

namespace SimpleFileRenamer.Controls
{
    /// <summary>
    /// Interaction logic for FileListControl.xaml
    /// </summary>
    public partial class FileListControl : UserControl
    {
        private MainViewModel? _viewModel;

        public FileListControl()
        {
            InitializeComponent();
            
            // Add converters to resources
            Resources.Add("BooleanToVisibilityConverter", new BooleanToVisibilityConverter());
            Resources.Add("CountToVisibilityConverter", new CountToVisibilityConverter());
        }

        /// <summary>
        /// Sets the view model for the control
        /// </summary>
        /// <param name="viewModel">The view model</param>
        public void SetViewModel(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            FilesDataGrid.ItemsSource = _viewModel.Files;
        }

        /// <summary>
        /// Refreshes the file list display
        /// </summary>
        public void RefreshList()
        {
            FilesDataGrid.Items.Refresh();
        }

        /// <summary>
        /// Handles the removal of selected files from the list
        /// </summary>
        private void RemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = FilesDataGrid.SelectedItems.Cast<FileItem>().ToList();
            if (selectedItems.Count == 0 || _viewModel == null) return;

            foreach (var item in selectedItems)
            {
                _viewModel.Files.Remove(item);
            }

            // Update indices for remaining files
            for (int i = 0; i < _viewModel.Files.Count; i++)
            {
                _viewModel.Files[i].Index = i;
            }

            RefreshList();
        }
    }

    /// <summary>
    /// Converts boolean values to Visibility
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }
    }

    /// <summary>
    /// Converts collection count to Visibility (shows element when count is 0)
    /// </summary>
    public class CountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int count)
            {
                return count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
