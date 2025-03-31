using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SimpleFileRenamer.Models
{
    /// <summary>
    /// Represents application settings
    /// </summary>
    public class AppSettings
    {
        // Default settings file path
        private static readonly string SettingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SimpleFileRenamer",
            "settings.json");

        /// <summary>
        /// Gets or sets the application theme
        /// </summary>
        public string Theme { get; set; } = "Light";

        /// <summary>
        /// Gets or sets a value indicating whether to create undo file by default
        /// </summary>
        public bool CreateUndoFileByDefault { get; set; } = true;

        /// <summary>
        /// Gets or sets the list of default patterns
        /// </summary>
        public List<RenamePattern> DefaultPatterns { get; set; } = new List<RenamePattern>();

        /// <summary>
        /// Loads settings from the settings file
        /// </summary>
        /// <returns>The loaded settings</returns>
        public static AppSettings Load()
        {
            try
            {
                // Create directory if it doesn't exist
                var directory = Path.GetDirectoryName(SettingsFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // If settings file exists, load it
                if (File.Exists(SettingsFilePath))
                {
                    var json = File.ReadAllText(SettingsFilePath);
                    var settings = JsonSerializer.Deserialize<AppSettings>(json);
                    if (settings != null)
                    {
                        return settings;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error loading settings: {ex.Message}");
            }

            // Return default settings if loading fails
            return new AppSettings();
        }

        /// <summary>
        /// Saves settings to the settings file
        /// </summary>
        public void Save()
        {
            try
            {
                // Create directory if it doesn't exist
                var directory = Path.GetDirectoryName(SettingsFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Serialize and save
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(this, options);
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }
    }
}