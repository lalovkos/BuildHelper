using BuilderHelperOnWPF.Models.SaveModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BuilderHelperOnWPF.Models
{
    public class PathFinderModel : INotifyPropertyChanged, ISaveable<PathFinderSave>
    {
        #region Private Fields

        private bool _copyFilesWithSamePath;
        private List<(string, string)> _filesPathsCopyFromTo;
        private bool _removeDuplicates;
        private List<FileInfo> _sourceFiles;
        private List<FolderNode> _targetFolders;

        #endregion Private Fields

        #region Public Constructors

        public PathFinderModel() : this(new PathFinderSave())
        {
        }

        public PathFinderModel(PathFinderSave save)
        {
            LoadFromSave(save);
        }

        #endregion Public Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public bool CopyFilesWithSamePath
        { get => _copyFilesWithSamePath; set { _copyFilesWithSamePath = value; NotifyPropertyChanged(nameof(CopyFilesWithSamePath)); } }

        public List<(string, string)> FilesPathsCopyFromTo
        { get => _filesPathsCopyFromTo; private set { _filesPathsCopyFromTo = value; NotifyPropertyChanged(nameof(FilesPathsCopyFromTo)); } }

        public bool RemoveDuplicates
        { get => _removeDuplicates; set { _removeDuplicates = value; NotifyPropertyChanged(nameof(RemoveDuplicates)); } }

        public List<FileInfo> SourceFiles
        { get => _sourceFiles; private set { _sourceFiles = value; NotifyPropertyChanged(nameof(SourceFiles)); } }

        public List<FolderNode> TargetFolders
        { get => _targetFolders; private set { _targetFolders = value; NotifyPropertyChanged(nameof(TargetFolders)); } }

        #endregion Public Properties

        #region Public Methods

        public void AddSelectedPaths(IEnumerable<string> fileNames)
        {
            var newAddedSourcesFiles = fileNames.Select(file => new FileInfo(file)).ToList();
            SourceFiles.AddRange(newAddedSourcesFiles);
            if (RemoveDuplicates) SourceFiles = SourceFiles.GroupBy(s => s.FullName).Select(grp => grp.FirstOrDefault()).ToList();
            RecalculateTargetPaths(newAddedSourcesFiles, null);
            NotifyPropertyChanged(nameof(SourceFiles));
        }

        public void AddTargetFolders(IEnumerable<string> fileNames)
        {
            var newAddedFolders = fileNames.Select(file => new FolderNode(file)).ToList();
            TargetFolders.AddRange(newAddedFolders);
            RecalculateTargetPaths(null, newAddedFolders);
            NotifyPropertyChanged(nameof(TargetFolders));
        }

        public PathFinderSave GetSave()
        {
            var save = new PathFinderSave()
            {
                TargetFolders = TargetFolders,
                SourceFiles = SourceFiles,
                RemoveDuplicates = RemoveDuplicates,
                CopyFilesWithSamePath = CopyFilesWithSamePath
            };
            return save;
        }

        public void LoadFromSave(PathFinderSave save)
        {
            TargetFolders = save.TargetFolders;
            SourceFiles = save.SourceFiles;
            RemoveDuplicates = save.RemoveDuplicates;
            CopyFilesWithSamePath = save.CopyFilesWithSamePath;
            RecalculateTargetPaths();
        }

        public void RemoveSourceRow(FileInfo fileInfo)
        {
            FilesPathsCopyFromTo.RemoveAll(o => o.Item1 == fileInfo.FullName);
            SourceFiles.Remove(fileInfo);
            NotifyPropertyChanged(nameof(SourceFiles));
        }

        public void RemoveTargetRow(FolderNode node)
        {
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

        #endregion Public Methods

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

            IEnumerable<(string, string)> query;
            if (!CopyFilesWithSamePath)
            {
                query = (from s in sources
                         join t in targets on s.Name equals t.Name
                         select (s.FullName, t.FullName));
            }
            else
            {
                query = (from s in sources
                         join t in targets on s.Name equals t.Name
                         where s.FullName != t.FullName
                         select (s.FullName, t.FullName));
            }

            FilesPathsCopyFromTo.AddRange(query);
            NotifyPropertyChanged(nameof(FilesPathsCopyFromTo));
        }

        #endregion Private Methods
    }
}