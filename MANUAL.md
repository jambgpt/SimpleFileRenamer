# Simple File Renamer - User Manual

## Table of Contents

1. [Introduction](#introduction)
2. [Getting Started](#getting-started)
   - [Installation](#installation)
   - [First Launch](#first-launch)
3. [User Interface Overview](#user-interface-overview)
   - [Main Window](#main-window)
   - [Pattern Input Area](#pattern-input-area)
   - [File List](#file-list)
   - [Status Bar](#status-bar)
4. [Adding Files](#adding-files)
   - [Using Add Files Button](#using-add-files-button)
   - [Using Add Folder Button](#using-add-folder-button)
   - [Using Windows Explorer Integration](#using-windows-explorer-integration)
5. [Creating Rename Patterns](#creating-rename-patterns)
   - [Text Replacement](#text-replacement)
   - [Prefix and Suffix](#prefix-and-suffix)
   - [Case Conversion](#case-conversion)
   - [Sequential Numbering](#sequential-numbering)
   - [Regular Expressions](#regular-expressions)
6. [Renaming Files](#renaming-files)
   - [Preview Mode](#preview-mode)
   - [Executing Rename Operations](#executing-rename-operations)
   - [Handling Conflicts](#handling-conflicts)
7. [Undo Operations](#undo-operations)
   - [Undoing the Last Operation](#undoing-the-last-operation)
   - [Using Undo History](#using-undo-history)
8. [Settings](#settings)
   - [Theme Options](#theme-options)
   - [Saving Patterns](#saving-patterns)
   - [Loading Patterns](#loading-patterns)
   - [General Settings](#general-settings)
9. [Advanced Features](#advanced-features)
10. [Troubleshooting](#troubleshooting)
11. [Keyboard Shortcuts](#keyboard-shortcuts)

## Introduction

Simple File Renamer is a versatile utility designed to help you rename multiple files efficiently using pattern-based rules. Whether you need to organize photos, music, documents, or any other type of files, this application makes the process quick and intuitive.

## Getting Started

### Installation

**Option 1: Using the Installer**
1. Run the installer executable (SimpleFileRenamer-Setup.exe)
2. Follow the installation wizard instructions
3. Choose whether to install for all users or just yourself
4. Select whether to add a desktop shortcut
5. Complete the installation

**Option 2: Portable Version**
1. Extract the ZIP file to any location on your computer
2. Run SimpleFileRenamer.exe directly from the extracted folder
3. Optional: Install the Windows Explorer integration by running install_shell_extension.bat as administrator

### First Launch

When you first launch Simple File Renamer, you'll see the main application window. The application starts with default settings and an empty file list.

## User Interface Overview

### Main Window

The main window is divided into several sections:
- Top toolbar with buttons for adding files, folders, and accessing settings
- Pattern input area for configuring how files will be renamed
- File list showing original and new filenames
- Status bar displaying operation information
- Action buttons for executing rename operations and undo functions

### Pattern Input Area

This section allows you to define how your files will be renamed. It includes:
- Text replacement fields
- Prefix and suffix options
- Case conversion options
- Numbering settings
- Regular expression support

### File List

The file list displays:
- Original filename
- Preview of new filename
- File type
- Size
- Last modified date
- Status indicators for conflicts or errors

### Status Bar

The status bar shows:
- Current operation status
- Count of selected files
- Error messages, if any

## Adding Files

### Using Add Files Button

1. Click the "Add Files" button in the top toolbar
2. A file selection dialog will appear
3. Navigate to the folder containing files you want to rename
4. Select one or multiple files (use Ctrl or Shift for multiple selections)
5. Click "Open" to add the selected files to the list

### Using Add Folder Button

1. Click the "Add Folder" button in the top toolbar
2. A folder selection dialog will appear
3. Navigate to and select the folder containing files you want to rename
4. Click "Select Folder" to add all files from that folder to the list

### Using Windows Explorer Integration

If you've installed the Windows Explorer integration:
1. In Windows Explorer, select one or more files you want to rename
2. Right-click on the selection
3. Choose "Rename with Simple File Renamer" from the context menu
4. The application will launch with your selected files already loaded

## Creating Rename Patterns

### Text Replacement

To replace specific text in filenames:
1. In the Pattern Input area, check the "Replace text" option
2. Enter the text you want to find in the "Find" field
3. Enter the replacement text in the "Replace with" field
4. The preview will update automatically

### Prefix and Suffix

To add text at the beginning or end of filenames:
1. Check the "Add prefix" or "Add suffix" option (or both)
2. Enter the text you want to add in the corresponding field
3. The preview will update to show filenames with added text

### Case Conversion

To change the case of filenames:
1. Check the "Change case" option
2. Select the desired case option from the dropdown:
   - UPPERCASE
   - lowercase
   - Title Case
   - Sentence case
3. The preview will update to show the case change

### Sequential Numbering

To add sequential numbers to filenames:
1. Check the "Add numbering" option
2. Configure the numbering options:
   - Starting number (default: 1)
   - Increment (default: 1)
   - Padding (number of digits, e.g., 001, 002, etc.)
   - Position (prefix, suffix, or custom position)
   - Separator (text between the number and filename)
3. The preview will update to show numbered filenames

### Regular Expressions

For advanced users who need complex pattern matching:
1. Check the "Use regular expression" option
2. Enter a regular expression pattern in the "Pattern" field
3. Enter the replacement pattern in the "Replace with" field
4. The preview will update based on regex matches

## Renaming Files

### Preview Mode

The file list always shows a preview of how files will be renamed before you execute the operation. This allows you to verify the results before making any changes.

Indicators in the preview will show:
- ✓ Green: Ready to rename (no conflicts)
- ⚠ Yellow: Warning (filename won't change)
- ❌ Red: Error or conflict (duplicate filename would be created)

### Executing Rename Operations

When you're satisfied with the preview:
1. Check "Create undo file" if you want to be able to undo the operation later (recommended)
2. Click the "Rename Files" button at the bottom of the window
3. If "Create undo file" is checked, you'll be prompted to save an undo file
4. The rename operation will execute
5. A results dialog will show success/failure information

### Handling Conflicts

If conflicts are detected (such as duplicate filenames):
1. Files with conflicts will be marked with a red indicator
2. Error messages will explain the specific issue
3. You'll need to adjust your pattern to resolve conflicts before renaming
4. Only files without conflicts will be renamed when you click "Rename Files"

## Undo Operations

### Undoing the Last Operation

To undo your most recent rename operation:
1. Click the "Undo Last Operation" button in the bottom action bar
2. Confirm that you want to proceed with the undo
3. Files will be restored to their original names
4. A results dialog will show success/failure information

### Using Undo History

To access the history of rename operations:
1. Click the "Undo History" button in the bottom action bar
2. The Undo History window will open showing past rename operations
3. Select an operation from the list
4. Click "Undo Selected Operation"
5. Confirm that you want to proceed
6. Files will be restored to their state before that operation
7. A results dialog will show success/failure information

## Settings

To access settings, click the "Settings" button in the top toolbar.

### Theme Options

1. In the Settings window, go to the "Appearance" tab
2. Choose between:
   - Light theme
   - Dark theme
   - System theme (follows Windows settings)
3. Click "Apply" to preview the theme
4. Click "OK" to save changes

### Saving Patterns

To save a pattern for future use:
1. Create your pattern in the Pattern Input area
2. Open the Settings window
3. Go to the "Patterns" tab
4. Click "Add Current Pattern"
5. Enter a name for the pattern
6. Click "Save"

### Loading Patterns

To load a saved pattern:
1. Open the Settings window
2. Go to the "Patterns" tab
3. Select a pattern from the list
4. Click "Load Selected"
5. The pattern will be loaded into the Pattern Input area

### General Settings

The "General" tab in Settings allows you to configure:
- Whether to create undo files by default
- Default file path for undo files
- Startup behavior options
- Explorer integration settings

## Advanced Features

Simple File Renamer includes several advanced features:

**Metadata-Based Renaming**:
Use file metadata such as creation date, dimensions (for images), or ID3 tags (for music) in your patterns.

**Batch Processing**:
Process large numbers of files efficiently with optimized performance.

**Pattern Testing**:
Test complex patterns on a subset of files before applying to all.

## Troubleshooting

**Common Issues and Solutions**:

1. **Files Not Appearing in List**:
   - Check if you have permission to access the files
   - Try running the application as administrator

2. **Cannot Install Shell Extension**:
   - Make sure you're running the install script as administrator
   - Check if you have the required .NET runtime installed

3. **Undo Operation Failed**:
   - Files may have been moved or deleted since the original rename
   - The destination path might be occupied by another file

4. **Pattern Not Working as Expected**:
   - Check for typos in your pattern
   - For regex patterns, verify your expression syntax
   - Test with a smaller set of files first

## Keyboard Shortcuts

- **Ctrl+O**: Add files
- **Ctrl+Shift+O**: Add folder
- **Ctrl+S**: Save current pattern
- **Ctrl+Z**: Undo last operation
- **Ctrl+Shift+Z**: Open undo history
- **F1**: Open help
- **F5**: Refresh file list
- **Ctrl+R**: Execute rename operation
- **Esc**: Close current dialog
- **Ctrl+,**: Open settings