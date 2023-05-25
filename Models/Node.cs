using System.Collections.ObjectModel;
using System.IO;

namespace BuilderHelperOnWPF.Models
{
    public class Node
    {
        #region Public Constructors

        public Node(string strFullPath, Node parent = null)
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

        public ObservableCollection<Node> Children { get; set; }
        public bool IsFile { get; }
        public Node Parent { get; set; }
        public string FullName { get; }
        public string Name { get; }

        #endregion Public Properties

        #region Private Methods

        private void SetSubfolders(string folderPath, Node parent = null)
        {
            Children = new ObservableCollection<Node>();

            var dirInfo = new DirectoryInfo(folderPath);

            foreach (var dir in dirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly))
            {
                Children.Add(new Node(dir.FullName, parent));
            }

            foreach (var file in dirInfo.GetFiles("*", SearchOption.TopDirectoryOnly))
            {
                Children.Add(new Node(file.FullName, parent));
            }
        }

        #endregion Private Methods
    }
}