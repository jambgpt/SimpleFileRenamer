using System.Windows;

namespace SimpleFileRenamer
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        /// <summary>
        /// Gets the result entered by the user
        /// </summary>
        public string Result { get; private set; } = string.Empty;

        /// <summary>
        /// Creates a new instance of the InputDialog
        /// </summary>
        public InputDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a new instance of the InputDialog with the specified prompt
        /// </summary>
        /// <param name="prompt">The prompt text to display</param>
        /// <param name="title">The dialog title</param>
        /// <param name="defaultValue">The default value for the input box</param>
        public InputDialog(string prompt, string title = "Input", string defaultValue = "")
            : this()
        {
            Title = title;
            PromptText.Text = prompt;
            InputTextBox.Text = defaultValue;
        }

        /// <summary>
        /// Handles the OK button click
        /// </summary>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Result = InputTextBox.Text;
            DialogResult = true;
        }
    }
}