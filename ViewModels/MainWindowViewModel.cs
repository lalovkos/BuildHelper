using BuilderHelperOnWPF.Models;
using MS.WindowsAPICodePack.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BuilderHelperOnWPF.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Public Constructors

        public MainWindowViewModel()
        {
            TargetFolders = new ObservableCollection<FolderNode>();
            SourceFiles = new ObservableCollection<FileInfo>();
            FilesPathsCopyFromTo = new List<(string, string)>();
            CommandLineText = "";
        }

        #endregion Public Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public string CommandLineText { get; set; }
        public ObservableCollection<FileInfo> SourceFiles { get; set; }
        public ObservableCollection<FolderNode> TargetFolders { get; }

        #endregion Public Properties

        #region Private Properties

        private List<(string, string)> FilesPathsCopyFromTo { get; set; }

        #endregion Private Properties

        #region Public Methods

        public void GenerateCommandLine()
        {
            CommandLineText = CommandLineHelper.GenerateCommandLineString(new CommandLineSettings(FilesPathsCopyFromTo));
            NotifyPropertyChanged("CommandLineText");
        }

        #endregion Public Methods

        #region Internal Methods

        internal void AddSelectedPaths(IEnumerable<string> fileNames)
        {
            var newAddedSourcesFiles = new List<FileInfo>();
            foreach (var file in fileNames)
            {
                var newFileInfo = new FileInfo(file);
                SourceFiles.Add(newFileInfo);
                newAddedSourcesFiles.Add(newFileInfo);
            }
            RecalculateTargetPaths(newAddedSourcesFiles, null);
        }

        internal void AddTargetFolders(IEnumerable<string> fileNames)
        {
            var newAddedFolders = new List<FolderNode>();
            foreach (var file in fileNames)
            {
                var newTargetFolder = new FolderNode(file);
                TargetFolders.Add(newTargetFolder);
                newAddedFolders.Add(newTargetFolder);
            }
            RecalculateTargetPaths(null, newAddedFolders);
        }

        internal void RemoveSourceRow(object dataContext)
        {
            var fileInfo = dataContext as FileInfo ?? throw new InvalidDataException();
            FilesPathsCopyFromTo.RemoveAll(o => o.Item1 == fileInfo.FullName);
            SourceFiles.Remove(fileInfo);
        }

        internal void RemoveTargetRow(object nodeDataContext)
        {
            var node = nodeDataContext as FolderNode ?? throw new InvalidDataException();
            FilesPathsCopyFromTo.RemoveAll(o => o.Item2 == node.FullName);
            if (node.Parent == null)
            {
                TargetFolders.Remove(node);
            }
            else
            {
                node.Parent.Children.Remove(node);
            }
        }

        #endregion Internal Methods

        #region Private Methods

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RecalculateTargetPaths(List<FileInfo> sourceFiles = null, List<FolderNode> targetFolders = null) 
        {
            var sources = sourceFiles ?? SourceFiles.ToList();
            var targets = GetTargetFilesAsList(targetFolders);
            var query = (from s in sources
                         join t in targets on s.Name equals t.Name
                         select ( s.FullName, t.FullName)).ToList();
            FilesPathsCopyFromTo.AddRange(query);
        }

        private List<FileInfo> GetTargetFilesAsList(List<FolderNode> targetFolders = null) 
        {
            var result = new List<FileInfo>();
            var targets = targetFolders ?? TargetFolders.ToList();
            foreach (var targetFolder in targets)
            {
                if (targetFolder.IsFile)
                {
                    result.Add(targetFolder.FileInfo);
                }
                else
                {
                    foreach (var subfolder in targetFolder.Children)
                    {
                        AddToListIfFile(subfolder, result);
                    }
                }
            }
            return result;
        }

        private void AddToListIfFile(FolderNode node, List<FileInfo> files) 
        {
            if (node.IsFile)
            {
                files.Add(node.FileInfo);
            }
            else
            {
                foreach (var subfolder in node.Children)
                {
                    AddToListIfFile(subfolder, files);
                }
            }
        }

        #endregion Private Methods
    }
}