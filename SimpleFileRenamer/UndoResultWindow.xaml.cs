using System.Windows;
using SimpleFileRenamer.Core;

namespace SimpleFileRenamer
{
    /// <summary>
    /// Interaction logic for UndoResultWindow.xaml
    /// </summary>
    public partial class UndoResultWindow : Window
    {
        /// <summary>
        /// Creates a new instance of the UndoResultWindow
        /// </summary>
        /// <param name="undoResult">The undo result to display</param>
        public UndoResultWindow(UndoResult undoResult)
        {
            InitializeComponent();
            
            // Set the result summary
            ResultSummary.Text = undoResult.Message;
            
            // Set the list view's data source
            FailedItemsListView.ItemsSource = undoResult.FailedItems;
        }
    }
}