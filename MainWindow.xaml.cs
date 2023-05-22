using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace BuilderHelperOnWPF
{
    public class FileToCopyInfo
    {
        #region Public Properties

        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime Time { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Public Constructors

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        #endregion Public Constructors

        #region Private Methods

        private void AddSourceFiles(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel == null) throw new InvalidDataException();

            var dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            var result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK) return;

            foreach (var file in dialog.FileNames)
            {
                var newFileInfo = new FileToCopyInfo() { Name = Path.GetFileName(file), Path = file, Time = File.GetLastWriteTime(file) };
                viewModel.SelectedPaths.Add(newFileInfo);
                viewModel.FindFileInTargetFolders(newFileInfo.Name);
            }
        }

        private void AddTargetFolders(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel == null) throw new InvalidDataException();

            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK) viewModel.TargetFolders.Add(new Node(dialog.SelectedPath));
            }
        }

        private void RemoveSourceRow(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as System.Windows.Controls.Button;
                ((MainWindowViewModel)DataContext).SelectedPaths.Remove((FileToCopyInfo)button.DataContext);
            }
            catch (Exception)
            {
            }
        }

        private void RemoveTargetRow(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as System.Windows.Controls.Button;
                var node = (Node)button.DataContext;
                if (node.Parent == null)
                {
                    ((MainWindowViewModel)DataContext).TargetFolders.Remove(node);
                }
                else
                {
                    node.Parent.Children.Remove((Node)button.DataContext);
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Private Methods
    }

    public class MainWindowViewModel
    {
        #region Public Constructors

        public MainWindowViewModel()
        {
            strFolder = @"C:\Users\engineer\source\repos";

            TargetFolders = new ObservableCollection<Node>();
            SelectedItems = new ObservableCollection<Node>();
            SelectedPaths = new ObservableCollection<FileToCopyInfo>();
            TargetFilesFullPaths = new ObservableCollection<string>();

            Node rootNode = new Node(strFolder);

            TargetFolders.Add(rootNode);
        }

        #endregion Public Constructors

        #region Public Properties

        public ObservableCollection<Node> SelectedItems { get; }
        public ObservableCollection<FileToCopyInfo> SelectedPaths { get; set; }
        public string strFolder { get; set; }
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
    }

    public class Node
    {
        #region Public Constructors

        public Node(string strFullPath, Node parent = null, bool isFile = false)
        {
            StrFullPath = strFullPath;
            Parent = parent;
            StrNodeText = Path.GetFileName(strFullPath);
            if (!isFile) SetSubfolders(StrFullPath, this);
            IsFile = isFile;
        }

        #endregion Public Constructors

        #region Public Properties

        public ObservableCollection<Node> Children { get; set; }
        public bool IsFile { get; }
        public Node Parent { get; set; }
        public string StrFullPath { get; }
        public string StrNodeText { get; }

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
                Children.Add(new Node(file.FullName, parent, true));
            }
        }

        #endregion Private Methods
    }
}