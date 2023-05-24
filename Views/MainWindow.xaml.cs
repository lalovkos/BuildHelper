using BuilderHelperOnWPF.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Windows;

namespace BuilderHelperOnWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Public Constructors
        private readonly MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;
        }

        #endregion Public Constructors

        #region Private Methods

        private void AddSourceFiles(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.InitialDirectory = "C:\\Users";
                dialog.IsFolderPicker = false;
                dialog.Multiselect = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    _viewModel.AddSelectedPaths(dialog.FileNames);
                }
            }
        }

        private void AddTargetFolders(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.InitialDirectory = "C:\\Users";
                dialog.IsFolderPicker = true;
                dialog.Multiselect = false;

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    _viewModel.AddTargetFolders(dialog.FileNames);
                }
            }
        }

        private void RemoveSourceRow(object sender, RoutedEventArgs e)
        {
            try
            {
                var viewModel = DataContext as MainWindowViewModel;
                var button = sender as System.Windows.Controls.Button;
                _viewModel.RemoveSourceRow(button.DataContext);
            }
            catch (Exception)
            {
            }
        }

        private void RemoveTargetPath(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            _viewModel.RemoveTargetPath(button.DataContext);
        }

        private void RemoveTargetRow(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as System.Windows.Controls.Button;
                _viewModel.RemoveTargetRow(button.DataContext);
            }
            catch (Exception)
            {
            }
        }



        #endregion Private Methods

        private void GenerateCommandLine(object sender, RoutedEventArgs e)
        {
            _viewModel.GenerateCommandLine();
        }
    }
}