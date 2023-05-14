using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace AvaloniaApplicationMVVM.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public async void btn_OnClickAsync()
        {
            //// create a file dialog instance
            //var dialog = new OpenFolderDialog();

            //// show the file dialog
            //var result = await dialog.ShowAsync();
            await Task.Delay(1000);
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


        public ObservableCollection<Node> Items { get; }
        public ObservableCollection<Node> SelectedItems { get; }
        public string strFolder { get; set; }

        public MainWindowViewModel()
        {
            strFolder = @"C:\Users\lalov\Desktop\Работа"; // EDIT THIS FOR AN EXISTING FOLDER

            Items = new ObservableCollection<Node>();
            SelectedItems = new ObservableCollection<Node>();

            Node rootNode = new Node(strFolder);
            rootNode.Subfolders = GetSubfolders(strFolder);

            Items.Add(rootNode);
        }

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

        public class Node
        {
            public ObservableCollection<Node> Subfolders { get; set; }

            public string strNodeText { get; }
            public string strFullPath { get; }

            public Node(string _strFullPath)
            {
                strFullPath = _strFullPath;
                strNodeText = Path.GetFileName(_strFullPath);
            }
        }
    }
}