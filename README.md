# Simple File Renamer

A powerful yet easy-to-use batch file renaming utility for Windows with advanced pattern-based rules and Windows Explorer integration.

![Simple File Renamer Screenshot](screenshots/main-window.png)

## Features

- **Intuitive User Interface**: Clean, modern design that makes batch renaming simple
- **Pattern-Based Renaming**: Rename files using customizable patterns
- **Windows Explorer Integration**: Right-click on files to rename them directly
- **Preview Before Renaming**: See the new filenames before applying changes
- **Undo Capability**: Easily undo rename operations
- **Regular Expression Support**: Use regex for advanced pattern matching
- **Customizable Settings**: Save favorite patterns and preferences
- **Theme Support**: Light and dark mode themes

## Renaming Patterns

Simple File Renamer supports various patterns to help you rename your files:

- **Text Replacement**: Replace specific text with new text
- **Prefix/Suffix**: Add text to the beginning or end of filenames
- **Sequential Numbering**: Add sequential numbers to filenames
- **Case Conversion**: Change to UPPERCASE, lowercase, or Title Case
- **Regular Expressions**: Use regex patterns for advanced renaming

## System Requirements

- Windows 10 or higher
- .NET 8.0 Runtime (included in installer)
- 50MB disk space

## Installation

### Option 1: Installer

1. Download the latest installer from the Releases page (will be available after first GitHub release)
2. Run the installer and follow the prompts
3. Launch Simple File Renamer from the Start menu

### Option 2: Portable Version

1. Download the portable zip from the Releases page (will be available after first GitHub release)
2. Extract the zip to a location of your choice
3. Run `SimpleFileRenamer.exe` from the extracted folder
4. (Optional) Install the shell extension by running `install_shell_extension.bat` as administrator

## Windows Explorer Integration

Right-click on selected files in Windows Explorer to see the "Rename with Simple File Renamer" option. This will launch the application with the selected files loaded, ready for renaming.

## Building from Source

### Prerequisites

- Visual Studio 2022 or higher
- .NET 8.0 SDK
- SharpShell library

### Build Steps

1. Clone the repository
   ```
   git clone https://github.com/YOUR-USERNAME/SimpleFileRenamer.git
   ```
   (Replace YOUR-USERNAME with your actual GitHub username after creating the repository)

2. Open the solution in Visual Studio
   ```
   cd SimpleFileRenamer
   start SimpleFileRenamer.sln
   ```

3. Build the solution
   ```
   dotnet build
   ```

4. Run the application
   ```
   dotnet run --project SimpleFileRenamer/SimpleFileRenamer.csproj
   ```

## Project Structure

- **SimpleFileRenamer**: Main WPF application
- **SimpleFileRenamer.ShellExtension**: Windows Explorer shell extension
- **SimpleFileRenamer.Tests**: Unit tests

## How to Contribute

Contributions are welcome! Here's how you can contribute:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/my-new-feature`)
3. Commit your changes (`git commit -am 'Add some feature'`)
4. Push to the branch (`git push origin feature/my-new-feature`)
5. Create a new Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- [SharpShell](https://github.com/dwmkerr/sharpshell) for Windows Shell integration
- [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) for MVVM implementation