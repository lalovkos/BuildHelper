using BuilderHelperOnWPF.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace BuilderHelperOnWPF.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Public Fields

        public string ProjectName = "NewProject";

        #endregion Public Fields

        #region Private Fields

        private MainModel _mainModel;

        #endregion Private Fields

        #region Internal Constructors

        internal MainWindowViewModel()
        {
            _mainModel = new MainModel();
            _mainModel.PropertyChanged += ModelChanged;
        }

        #endregion Internal Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public string CommandLineText => _mainModel.CommandLineText;
        public ObservableCollection<FileInfo> SourceFiles => new ObservableCollection<FileInfo>(_mainModel.SourceFiles);
        public ObservableCollection<FolderNode> TargetFolders => new ObservableCollection<FolderNode>(_mainModel.TargetFolders);

        #endregion Public Properties

        #region Internal Methods

        internal void AddSelectedPaths(IEnumerable<string> fileNames)
        {
            _mainModel.AddSelectedPaths(fileNames);
        }

        internal void AddTargetFolders(IEnumerable<string> fileNames)
        {
            _mainModel.AddTargetFolders(fileNames);
        }

        internal void GenerateCommandLine()
        {
            _mainModel.GenerateCommandLine();
        }

        internal void NewProject()
        {
            _mainModel.Clear();
        }

        internal void OpenProjectFromFile(string fileName)
        {
            var file = new FileInfo(fileName);
            using (var rS = file.OpenText())
            {
                _mainModel.LoadFromSave(rS.ReadToEnd());
            }
        }

        internal void RemoveSourceRow(object dataContext)
        {
            var fileInfo = dataContext as FileInfo ?? throw new InvalidDataException();
            _mainModel.RemoveSourceRow(fileInfo);
        }

        internal void RemoveTargetRow(object nodeDataContext)
        {
            var node = nodeDataContext as FolderNode ?? throw new InvalidDataException();
            _mainModel.RemoveTargetRow(node);
        }

        internal async Task SaveFileIntoProject(string fileName)
        {
            var file = new FileInfo(fileName);
            using (var wS = file.CreateText())
            {
                await wS.WriteAsync(_mainModel.GetSave());
            }
        }

        #endregion Internal Methods

        #region Private Methods

        private void ModelChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
        }

        #endregion Private Methods
    }
}