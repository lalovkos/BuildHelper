using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BuilderHelperOnWPF.Models
{
    internal class MainModel : INotifyPropertyChanged
    {
        #region Private Fields

        private string _commandLineText = "";
        private List<FileInfo> _sourceFiles;
        private List<FolderNode> _targetFolders;

        #endregion Private Fields

        #region Public Constructors

        public MainModel()
        {
            TargetFolders = new List<FolderNode>();
            SourceFiles = new List<FileInfo>();
            _filesPathsCopyFromTo = new List<(string, string)>();
        }

        #endregion Public Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public string CommandLineText
        { get => _commandLineText; set { _commandLineText = value; NotifyPropertyChanged("CommandLineText"); } }

        public List<FileInfo> SourceFiles
        { get => _sourceFiles; private set { _sourceFiles = value; NotifyPropertyChanged("SourceFiles"); } }

        public List<FolderNode> TargetFolders
        { get => _targetFolders; private set { _targetFolders = value; NotifyPropertyChanged("TargetFolders"); } }

        #endregion Public Properties

        #region Private Properties

        private List<(string, string)> _filesPathsCopyFromTo { get; set; }

        #endregion Private Properties

        #region Internal Methods

        internal void AddSelectedPaths(IEnumerable<string> fileNames)
        {
            var newAddedSourcesFiles = fileNames.Select(file => new FileInfo(file)).ToList();
            SourceFiles.AddRange(newAddedSourcesFiles);
            RecalculateTargetPaths(newAddedSourcesFiles, null);
            NotifyPropertyChanged("SourceFiles");
        }

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        internal void AddTargetFolders(IEnumerable<string> fileNames)
        {
            var newAddedFolders = fileNames.Select(file => new FolderNode(file)).ToList();
            TargetFolders.AddRange(newAddedFolders);
            RecalculateTargetPaths(null, newAddedFolders);
            NotifyPropertyChanged("TargetFolders");
        }

        internal void GenerateCommandLine()
        {
            CommandLineText = CommandLineHelper.GenerateCommandLineString(new CommandLineSettings(_filesPathsCopyFromTo));
        }

        internal void RemoveSourceRow(FileInfo fileInfo)
        {
            _filesPathsCopyFromTo.RemoveAll(o => o.Item1 == fileInfo.FullName);
            SourceFiles.Remove(fileInfo);
            NotifyPropertyChanged("SourceFiles");
        }

        internal void RemoveTargetRow(FolderNode node)
        {
            _filesPathsCopyFromTo.RemoveAll(o => o.Item2 == node.FullName);
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

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RecalculateTargetPaths(List<FileInfo> sourceFiles = null, List<FolderNode> targetFolders = null)
        {
            var sources = sourceFiles ?? SourceFiles.ToList();
            var targets = GetTargetFilesAsList(targetFolders);

            if (sources.Count == 0 || targets.Count == 0)
                return;

            var query = (from s in sources
                         join t in targets on s.Name equals t.Name
                         where s.FullName != t.FullName
                         select (s.FullName, t.FullName)).ToList();

            _filesPathsCopyFromTo.AddRange(query);
        }

        #endregion Private Methods
    }
}