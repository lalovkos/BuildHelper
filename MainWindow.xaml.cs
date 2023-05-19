using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace BuilderHelperOnWPF
{
    public class FileToCopyInfo
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
    }

    public class Node
    {

        public Node(string strFullPath, Node parent = null)
        {
            StrFullPath = strFullPath;
            Parent = parent;
            StrNodeText = Path.GetFileName(strFullPath);
            Subfolders = SetSubfolders(StrFullPath, this);
        }

        public string StrFullPath { get; }
        public string StrNodeText { get; }
        public ObservableCollection<Node> Subfolders { get; set; }
        public Node Parent { get; set; }

        private ObservableCollection<Node> SetSubfolders(string folderPath, Node parent = null)
        {
            ObservableCollection<Node> subfolders = new ObservableCollection<Node>();
            string[] subdirs = Directory.GetDirectories(folderPath, "*", SearchOption.TopDirectoryOnly);

            foreach (string dir in subdirs)
            {
                Node thisnode = new Node(dir, parent);
                if (Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly).Length > 0)
                {
                    thisnode.Subfolders = SetSubfolders(dir, this);
                }
                subfolders.Add(thisnode);
            }

            return subfolders;
        }
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


        private void RemoveSourceRow(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as System.Windows.Controls.Button;
                ((MainWindowViewModel)DataContext).SelectedPaths.Remove((FileToCopyInfo)button.DataContext);
            }
            catch (Exception ex)
            {
                var i = ex;
            }
        }

        private void RemoveTargetRow(object sender, RoutedEventArgs e)
        {
            try 
            {
                var button = sender as System.Windows.Controls.Button;
                ((Node)button.DataContext).Parent.Subfolders.Remove((Node)button.DataContext);
            }
            catch(Exception ex) 
            {
                var i = ex;
            }
        }

        private void AddTargetFolders(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel == null) throw new InvalidDataException();

            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK) viewModel.Items.Add(new Node(dialog.SelectedPath));
            }
        }

        private void AddSourceFiles(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel == null) throw new InvalidDataException();

            // create a file dialog instance
            var dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.ShowDialog();


            foreach (var file in dialog.FileNames)
            {
                viewModel.SelectedPaths.Add(new FileToCopyInfo() { Name = Path.GetFileName(file), Path = file, Time = File.GetLastWriteTime(file) });
            }

            //// check if a folder was selected
            //if (!string.IsNullOrEmpty(result))
            //{
            //    // do something with the selected folder
            //    Console.WriteLine($"Selected folder: {result}");
            //}
            //else
            //{
            //    // no folder was selected
            //    Console.WriteLine("No folder was selected.");
            //}
        }
    }
    public class MainWindowViewModel
    {
        #region Public Constructors

        public MainWindowViewModel()
        {
            strFolder = @"C:\Users\engineer\source\repos"; // EDIT THIS FOR AN EXISTING FOLDER

            Items = new ObservableCollection<Node>();
            SelectedItems = new ObservableCollection<Node>();

            Node rootNode = new Node(strFolder);

            Items.Add(rootNode);
        }

        #endregion Public Constructors

        #region Public Properties

        public ObservableCollection<FileToCopyInfo> SelectedPaths { get; set; } = new ObservableCollection<FileToCopyInfo>();
        public string Greeting => "Welcome to Avalonia!";

        public ObservableCollection<Node> Items { get; }
        public ObservableCollection<Node> SelectedItems { get; }
        public string strFolder { get; set; }

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods

        #region Public Classes

        #endregion Public Classes
    }
}