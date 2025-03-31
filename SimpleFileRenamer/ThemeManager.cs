using System;
using System.Windows;

namespace SimpleFileRenamer
{
    /// <summary>
    /// Manages application themes
    /// </summary>
    public static class ThemeManager
    {
        /// <summary>
        /// Applies the specified theme to the application
        /// </summary>
        /// <param name="themeName">The name of the theme to apply</param>
        public static void ApplyTheme(string themeName)
        {
            // Get the application's resource dictionary
            var resources = System.Windows.Application.Current.Resources.MergedDictionaries;
            
            // Clear any existing theme dictionaries
            for (int i = resources.Count - 1; i >= 0; i--)
            {
                var dictionary = resources[i];
                var source = dictionary.Source?.ToString() ?? string.Empty;
                if (source.Contains("LightTheme.xaml") || source.Contains("DarkTheme.xaml"))
                {
                    resources.RemoveAt(i);
                }
            }
            
            // Add the new theme dictionary
            ResourceDictionary themeDictionary = new ResourceDictionary();
            try
            {
                switch (themeName?.ToLower())
                {
                    case "dark":
                        themeDictionary.Source = new Uri("/SimpleFileRenamer;component/Resources/DarkTheme.xaml", UriKind.Relative);
                        break;
                    case "light":
                    default:
                        themeDictionary.Source = new Uri("/SimpleFileRenamer;component/Resources/LightTheme.xaml", UriKind.Relative);
                        break;
                }
                
                System.Windows.Application.Current.Resources.MergedDictionaries.Add(themeDictionary);
            }
            catch (Exception ex)
            {
                // Log or handle the error
                Console.WriteLine($"Error applying theme: {ex.Message}");
            }
        }
    }
}