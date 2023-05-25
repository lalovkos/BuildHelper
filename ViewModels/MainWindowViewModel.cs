using BuilderHelperOnWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BuilderHelperOnWPF.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Public Constructors

        public MainWindowViewModel()
        {
            TargetFolders = new ObservableCollection<Node>();
            SelectedItems = new ObservableCollection<Node>();
            SelectedPaths = new ObservableCollection<FileInfo>();
            TargetFilesFullPaths = new ObservableCollection<FileInfo>();
            CommandLineText = "";
        }

        #endregion Public Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public string CommandLineText { get; set; }
        public ObservableCollection<Node> SelectedItems { get; }
        public ObservableCollection<FileInfo> SelectedPaths { get; set; }
        public ObservableCollection<FileInfo> TargetFilesFullPaths { get; set; }
        public ObservableCollection<Node> TargetFolders { get; }

        #endregion Public Properties

        #region Public Methods

        public void FindFileInNode(Node node, string fileName)
        {
            if (node.IsFile)
            {
                if (node.Name == fileName) TargetFilesFullPaths.Add(new FileInfo(node.FullName));
            }
            else
            {
                foreach (var subfolder in node.Children)
                {
                    FindFileInNode(subfolder, fileName);
                }
            }
        }

        public void FindFileInTargetFolders(string fileName)
        {
            foreach (var targetFolder in TargetFolders)
            {
                FindFileInNode(targetFolder, fileName);
            }
        }

        public void GenerateCommandLine()
        {
            CommandLineText = CommandLineHelper.GenerateCommandLineString(new CommandLineSettings(SelectedPaths.ToList(), TargetFilesFullPaths.ToList()));
            NotifyPropertyChanged("CommandLineText");
        }

        #endregion Public Methods

        #region Internal Methods

        internal void AddSelectedPaths(IEnumerable<string> fileNames)
        {
            foreach (var file in fileNames)
            {
                var newFileInfo = new FileInfo(Path.GetFileName(file));
                SelectedPaths.Add(newFileInfo);
                FindFileInTargetFolders(newFileInfo.Name);
            }
        }

        internal void AddTargetFolders(IEnumerable<string> fileNames)
        {
            foreach (var file in fileNames)
            {
                TargetFolders.Add(new Node(file));
                FindFileInTargetFolders(file);
            }
        }

        internal void RemoveSourceRow(object dataContext)
        {
            SelectedPaths.Remove((FileInfo)dataContext);
        }

        internal void RemoveTargetPath(object dataContext)
        {
            TargetFilesFullPaths.Remove((FileInfo)dataContext);
        }

        internal void RemoveTargetRow(object nodeDataContext)
        {
            var node = nodeDataContext as Node ?? throw new InvalidDataException();
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
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Private Methods
    }
}