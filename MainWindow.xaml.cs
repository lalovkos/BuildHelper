using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace BuilderHelperOnWPF
{
    public class FileToCopyInfo
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
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

        private void btn_OnClickAsync(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel == null) throw new InvalidDataException();

            // create a file dialog instance
            var dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.ShowDialog();


            foreach (var file in dialog.FileNames)
            {
                viewModel.SelectedPaths.Add(new FileToCopyInfo() {  Name = Path.GetFileName(file), Path = file, Time = File.GetLastWriteTime(file)});
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

        private void ChangeText(object sender, RoutedEventArgs e)
        {
            try 
            {
                var button = sender as Button;
                ((MainWindowViewModel)DataContext).SelectedPaths.Remove((FileToCopyInfo)button.DataContext);
            }
            catch { }
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
            rootNode.Subfolders = GetSubfolders(strFolder);

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

        public ObservableCollection<Node> GetSubfolders(string strPath)
        {
            ObservableCollection<Node> subfolders = new ObservableCollection<Node>();
            string[] subdirs = Directory.GetDirectories(strPath, "*", SearchOption.TopDirectoryOnly);

            foreach (string dir in subdirs)
            {
                Node thisnode = new Node(dir);

                if (Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly).Length > 0)
                {
                    thisnode.Subfolders = new ObservableCollection<Node>();

                    thisnode.Subfolders = GetSubfolders(dir);
                }

                subfolders.Add(thisnode);
            }

            return subfolders;
        }

        #endregion Public Methods

        #region Public Classes

        public class Node
        {
            #region Public Constructors

            public Node(string _strFullPath)
            {
                strFullPath = _strFullPath;
                strNodeText = System.IO.Path.GetFileName(_strFullPath);
            }

            #endregion Public Constructors

            #region Public Properties

            public string strFullPath { get; }
            public string strNodeText { get; }
            public ObservableCollection<Node> Subfolders { get; set; }

            #endregion Public Properties
        }

        #endregion Public Classes
    }
}