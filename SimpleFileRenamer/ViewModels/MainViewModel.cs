using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleFileRenamer.Core;
using SimpleFileRenamer.Models;
using System.Collections.ObjectModel;

namespace SimpleFileRenamer.ViewModels
{
    /// <summary>
    /// ViewModel for the main window
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<FileItem> _files = new ObservableCollection<FileItem>();
        private RenamePattern _renamePattern = new RenamePattern();
        private bool _isRenaming = false;

        /// <summary>
        /// Collection of files to be renamed
        /// </summary>
        public ObservableCollection<FileItem> Files
        {
            get => _files;
            set => SetProperty(ref _files, value);
        }

        /// <summary>
        /// The current rename pattern
        /// </summary>
        public RenamePattern RenamePattern
        {
            get => _renamePattern;
            set => SetProperty(ref _renamePattern, value);
        }

        /// <summary>
        /// Indicates whether a rename operation is in progress
        /// </summary>
        public bool IsRenaming
        {
            get => _isRenaming;
            set => SetProperty(ref _isRenaming, value);
        }

        /// <summary>
        /// Command to add files to the list
        /// </summary>
        public IRelayCommand AddFilesCommand { get; }

        /// <summary>
        /// Command to clear the file list
        /// </summary>
        public IRelayCommand ClearFilesCommand { get; }

        /// <summary>
        /// Command to execute the rename operation
        /// </summary>
        public IRelayCommand RenameFilesCommand { get; }

        public MainViewModel()
        {
            AddFilesCommand = new RelayCommand(AddFiles);
            ClearFilesCommand = new RelayCommand(ClearFiles);
            RenameFilesCommand = new RelayCommand(RenameFiles, CanRenameFiles);
        }

        /// <summary>
        /// Adds files to the list for renaming
        /// </summary>
        /// <param name="files">The files to add</param>
        public void AddFiles(List<FileItem> files)
        {
            if (files == null || files.Count == 0)
                return;

            // Assign indices to the files for sequencing
            for (int i = 0; i < files.Count; i++)
            {
                files[i].Index = Files.Count + i;
            }

            foreach (var file in files)
            {
                Files.Add(file);
            }

            OnPropertyChanged(nameof(Files));
        }

        /// <summary>
        /// Clears the file list
        /// </summary>
        public void ClearFiles()
        {
            Files.Clear();
            OnPropertyChanged(nameof(Files));
        }

        /// <summary>
        /// Adds files to the list (for command binding)
        /// </summary>
        private void AddFiles()
        {
            // This method would typically show a file dialog
            // Since this is handled in the view, this is just a placeholder
        }

        /// <summary>
        /// Determines whether the rename operation can be executed
        /// </summary>
        /// <returns>True if the rename operation can be executed, false otherwise</returns>
        private bool CanRenameFiles()
        {
            return Files.Count > 0 && !IsRenaming;
        }

        /// <summary>
        /// Executes the rename operation
        /// </summary>
        private void RenameFiles()
        {
            // This method would execute the rename operation
            // Since this is handled in the view, this is just a placeholder
            IsRenaming = true;
            // Rename logic would go here
            IsRenaming = false;
        }

        /// <summary>
        /// Updates the pattern and refreshes file previews
        /// </summary>
        /// <param name="pattern">The new pattern to apply</param>
        public void UpdatePattern(RenamePattern pattern)
        {
            RenamePattern = pattern;
            OnPropertyChanged(nameof(RenamePattern));
        }
    }
}
