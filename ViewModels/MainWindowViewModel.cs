using BuilderHelperOnWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace BuilderHelperOnWPF.ViewModels
{
    public class MainWindowViewModel
    {
        #region Public Constructors

        public MainWindowViewModel()
        {
            StrFolder = @"C:\Users\engineer\source\repos";

            TargetFolders = new ObservableCollection<Node>();
            SelectedItems = new ObservableCollection<Node>();
            SelectedPaths = new ObservableCollection<FileToCopyInfo>();
            TargetFilesFullPaths = new ObservableCollection<string>();

            Node rootNode = new Node(StrFolder);

            TargetFolders.Add(rootNode);
        }

        #endregion Public Constructors

        #region Public Properties

        public ObservableCollection<Node> SelectedItems { get; }
        public ObservableCollection<FileToCopyInfo> SelectedPaths { get; set; }
        public string StrFolder { get; set; }
        public ObservableCollection<string> TargetFilesFullPaths { get; set; }
        public ObservableCollection<Node> TargetFolders { get; }

        #endregion Public Properties

        #region Public Methods

        public void FindFileInNode(Node node, string fileName)
        {
            if (node.IsFile)
            {
                if (node.StrNodeText == fileName) TargetFilesFullPaths.Add(node.StrFullPath);
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

        #endregion Public Methods

        #region Internal Methods

        internal void AddSelectedPaths(IEnumerable<string> fileNames)
        {
            foreach (var file in fileNames)
            {
                var newFileInfo = new FileToCopyInfo() { Name = Path.GetFileName(file), Path = file, Time = File.GetLastWriteTime(file) };
                SelectedPaths.Add(newFileInfo);
                FindFileInTargetFolders(newFileInfo.Name);
            }
        }

        internal void AddTargetFolders(IEnumerable<string> fileNames)
        {
            foreach (var file in fileNames)
            {
                TargetFolders.Add(new Node(file));
            }
        }

        internal void RemoveSourceRow(object dataContext)
        {
            SelectedPaths.Remove((FileToCopyInfo)dataContext);
        }

        internal void RemoveTargetPath(object dataContext)
        {
            TargetFilesFullPaths.Remove((string)dataContext);
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
    }
}