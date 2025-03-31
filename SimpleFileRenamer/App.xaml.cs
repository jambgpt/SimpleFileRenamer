using System.Collections.Generic;
using System.IO;
using System.Windows;
using Application = System.Windows.Application;

namespace SimpleFileRenamer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Check if the application was launched with file paths as arguments
            if (e.Args.Length > 0)
            {
                // Create a list of valid file paths from the arguments
                var filePaths = new List<string>();
                foreach (var arg in e.Args)
                {
                    if (File.Exists(arg) || Directory.Exists(arg))
                    {
                        filePaths.Add(arg);
                    }
                }

                // If there are valid file paths, pass them to the main window
                if (filePaths.Count > 0)
                {
                    var mainWindow = new MainWindow(filePaths);
                    mainWindow.Show();
                    Current.MainWindow = mainWindow;

                    // The mainWindow will be shown directly, so we don't need the default startup window
                    // (Note: e.Handled is not available in StartupEventArgs in .NET Core/.NET 5+)
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Perform cleanup operations if needed
            base.OnExit(e);
        }
    }
}
