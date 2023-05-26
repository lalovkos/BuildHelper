using System.Collections.ObjectModel;
using System.IO;

namespace BuilderHelperOnWPF.Models
{
    public class FolderNode
    {
        #region Public Constructors

        public FolderNode(string strFullPath, FolderNode parent = null)
        {
            FullName = strFullPath;
            Parent = parent;
            Name = Path.GetFileName(strFullPath);
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

        #endregion Public Constructors

        #region Public Properties

        public ObservableCollection<FolderNode> Children { get; set; }
        public bool IsFile { get; }
        public FolderNode Parent { get; set; }
        public string FullName { get; }
        public string Name { get; }

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