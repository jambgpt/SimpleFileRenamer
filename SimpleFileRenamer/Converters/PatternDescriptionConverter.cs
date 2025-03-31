using SimpleFileRenamer.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Text;

namespace SimpleFileRenamer.Converters
{
    /// <summary>
    /// Converts a RenamePattern to a human-readable description
    /// </summary>
    public class PatternDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not RenamePattern pattern)
                return string.Empty;

            var description = new StringBuilder();
            
            if (!string.IsNullOrEmpty(pattern.Prefix))
                description.Append($"Prefix: '{pattern.Prefix}' ");
                
            if (!string.IsNullOrEmpty(pattern.Suffix))
                description.Append($"Suffix: '{pattern.Suffix}' ");
                
            if (!string.IsNullOrEmpty(pattern.FindText) && !string.IsNullOrEmpty(pattern.ReplaceText))
                description.Append($"Replace '{pattern.FindText}' with '{pattern.ReplaceText}'" + 
                                   (pattern.UseRegex ? " (Regex)" : "") + " ");
                
            if (pattern.UseSequence)
                description.Append($"Sequence: Start={pattern.SequenceStart}, " +
                                   $"Format='{pattern.SequenceFormat}', " +
                                   $"Position={pattern.SequencePosition}");
                
            return description.ToString().Trim();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}