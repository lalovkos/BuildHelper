using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;

namespace BuilderHelperOnWPF.Models
{
    [JsonObject]
    public class FolderNode
    {
        #region Public Constructors

        public FolderNode(string strFullPath, FolderNode parent = null)
        {
            FileInfo = new FileInfo(strFullPath);
            Parent = parent;
            if (File.Exists(strFullPath))
            {
                IsFile = true;
                Children = null;
            }
            else if (Directory.Exists(strFullPath))
            {
                IsFile = false;
                SetSubfolders(FullName, this);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        [JsonConstructor]
        public FolderNode(FileInfo fileInfo, ObservableCollection<FolderNode> children, bool isFile)
        {
            FileInfo = fileInfo;
            Children = children;
            IsFile = isFile;
            if (children != null) foreach (var ch in children) ch.Parent = this;
        }

        #endregion Public Constructors

        #region Public Properties

        [JsonProperty("сhildren")]
        public ObservableCollection<FolderNode> Children { get; set; }

        [JsonProperty("fileInfo")]
        public FileInfo FileInfo { get; }

        [JsonIgnore]
        public string FullName => FileInfo.FullName;

        [JsonProperty("isFile")]
        public bool IsFile { get; }

        [JsonIgnore]
        public string Name => FileInfo.Name;

        [JsonIgnore]
        public FolderNode Parent { get; set; }

        #endregion Public Properties

        #region Private Methods

        private void SetSubfolders(string folderPath, FolderNode parent = null)
        {
            Children = new ObservableCollection<FolderNode>();

            var dirInfo = new DirectoryInfo(folderPath);

            foreach (var dir in dirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly))
            {
                Children.Add(new FolderNode(dir.FullName, parent));
            }

            foreach (var file in dirInfo.GetFiles("*", SearchOption.TopDirectoryOnly))
            {
                Children.Add(new FolderNode(file.FullName, parent));
            }
        }

        #endregion Private Methods
    }
}