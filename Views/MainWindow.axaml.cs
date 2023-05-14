using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApplicationMVVM.ViewModels;

namespace AvaloniaApplicationMVVM.Views
{
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

        private async void btn_OnClickAsync(object sender, RoutedEventArgs e)
        {
            // create a file dialog instance
            var dialog = new OpenFileDialog();
            dialog.AllowMultiple = true;

            // show the file dialog
            var result = await dialog.ShowAsync(this);

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

        #endregion Private Methods
    }
}