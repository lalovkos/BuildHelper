using BuilderHelperOnWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace BuilderHelperOnWPF.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Public Constructors

        public MainWindowViewModel()
        {
            TargetFolders = new ObservableCollection<Node>();
            SelectedItems = new ObservableCollection<Node>();
            SelectedPaths = new ObservableCollection<FileToCopyInfo>();
            TargetFilesFullPaths = new ObservableCollection<TargetFileInfo>();
            CommandLineText = "";
        }

        #endregion Public Constructors

        #region Public Properties

        public ObservableCollection<Node> SelectedItems { get; }
        public ObservableCollection<FileToCopyInfo> SelectedPaths { get; set; }
        public ObservableCollection<TargetFileInfo> TargetFilesFullPaths { get; set; }
        public ObservableCollection<Node> TargetFolders { get; }
        public string CommandLineText { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Public Properties

        #region Public Methods

        public void FindFileInNode(Node node, string fileName)
        {
            if (node.IsFile)
            {
                if (node.StrNodeText == fileName) TargetFilesFullPaths.Add(new TargetFileInfo(node.StrFullPath));
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
                FindFileInTargetFolders(file);
            }
        }

        internal void RemoveSourceRow(object dataContext)
        {
            SelectedPaths.Remove((FileToCopyInfo)dataContext);
        }

        internal void RemoveTargetPath(object dataContext)
        {
            TargetFilesFullPaths.Remove((TargetFileInfo)dataContext);
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

        public void GenerateCommandLine() 
        {
            CommandLineText = CommandLineHelper.GenerateCommandLineString(new CommandLineSettings(SelectedPaths.ToList(), TargetFilesFullPaths.ToList()));
            NotifyPropertyChanged("CommandLineText");
        }

        #endregion Internal Methods
    }
}