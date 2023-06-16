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
        #region Private Fields

        private readonly MainWindowViewModel _viewModel;
        private readonly string TitlePrefix = "BuildHelperOnWPF";

        #endregion Private Fields

        #region Public Constructors

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;
            Title = TitlePrefix + " : Unsaved project";
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

        private void CopyAllToClipBoard(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(_viewModel.CommandLineText);
        }

        private async void ExecuteCommandLine(object sender, RoutedEventArgs e)
        {
            await _viewModel.ExecuteCommandLine();
        }

        private void GenerateCommandLine(object sender, RoutedEventArgs e)
        {
            _viewModel.GenerateCommandLine();
        }

        private void NewProjectMenuClick(object sender, RoutedEventArgs e)
        {
            _viewModel.NewProject();
            Title = TitlePrefix + " : Unsaved project";
        }

        private void OpenProjectMenuClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.InitialDirectory = "C:\\Users";
                dialog.IsFolderPicker = false;
                dialog.Multiselect = false;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    _viewModel.OpenProjectFromFile(dialog.FileName);
                    Title = TitlePrefix + " : " + Path.GetFileNameWithoutExtension(dialog.FileName);
                }
            }
        }

        private void RemoveSourceRow(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as System.Windows.Controls.Button;
                _viewModel.RemoveSourceRow(button.DataContext);
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
                _viewModel.RemoveTargetRow(button.DataContext);
            }
            catch (Exception)
            {
            }
        }

        private async void SaveProjectMenuClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonSaveFileDialog())
            {
                dialog.InitialDirectory = "C:\\Users";
                dialog.DefaultExtension = "bhpj";
                dialog.DefaultFileName = _viewModel.ProjectName;
                dialog.Title = "Select file to save project data";
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    await _viewModel.SaveFileIntoProject(dialog.FileName);
                }
            }
        }

        #endregion Private Methods
    }
}