using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SimpleFileRenamer.ShellExtension
{
    /// <summary>
    /// Shell extension to add a context menu for renaming files
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    [COMServerAssociation(AssociationType.Directory)]
    public class FileRenamerContextMenu : SharpContextMenu
    {
        /// <summary>
        /// Path to the SimpleFileRenamer application
        /// </summary>
        private string RenamerAppPath
        {
            get
            {
                // In a real deployment, the path should be determined based on the installation location
                // For development, we'll assume it's in the same directory as the extension
                string assemblyLocation = typeof(FileRenamerContextMenu).Assembly.Location;
                string assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
                return Path.Combine(assemblyDirectory, "SimpleFileRenamer.exe");
            }
        }

        /// <summary>
        /// Determines if the context menu should be shown for the selected items
        /// </summary>
        /// <returns>True if the menu should be shown, false otherwise</returns>
        protected override bool CanShowMenu()
        {
            // Show the menu if at least one file or folder is selected
            return SelectedItemPaths.Any();
        }

        /// <summary>
        /// Creates the context menu to be shown
        /// </summary>
        /// <returns>The context menu strip</returns>
        protected override ContextMenuStrip CreateMenu()
        {
            // Create the menu strip
            var menu = new ContextMenuStrip();

            // Create the menu item
            var renameItem = new ToolStripMenuItem
            {
                Text = "Rename with Simple File Renamer",
                Image = null, // Could add an icon here
                ToolTipText = "Open selected files/folders in Simple File Renamer"
            };

            // Add a handler for the click event
            renameItem.Click += (sender, args) => LaunchRenamerWithSelectedItems();

            // Add the item to the menu
            menu.Items.Add(renameItem);

            return menu;
        }

        /// <summary>
        /// Launches the renamer application with the selected files/folders
        /// </summary>
        private void LaunchRenamerWithSelectedItems()
        {
            try
            {
                // Check if the application exists
                if (!File.Exists(RenamerAppPath))
                {
                    MessageBox.Show(
                        "The Simple File Renamer application could not be found. Please reinstall the application.",
                        "Application Not Found",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // Build the command-line arguments (quote each path to handle spaces)
                var args = string.Join(" ", SelectedItemPaths.Select(path => $"\"{path}\""));

                // Launch the application with the selected items
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = RenamerAppPath,
                    Arguments = args,
                    UseShellExecute = true
                };
                
                Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while launching the renamer application:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
