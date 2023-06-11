using BuilderHelperOnWPF.Models;
using BuilderHelperOnWPF.Models.SaveModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace BuilderHelperOnWPF.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Public Fields

        public string ProjectName = "NewProject";

        #endregion Public Fields

        #region Private Fields

        private CommandLineWorkerModel _commandLineModel;
        private PathFinderModel _pathFinderModel;

        #endregion Private Fields

        #region Public Constructors

        public MainWindowViewModel()
        {
            _pathFinderModel = new PathFinderModel();
            _pathFinderModel.PropertyChanged += ModelChanged;

            _commandLineModel = new CommandLineWorkerModel();
            _commandLineModel.PropertyChanged += ModelChanged;
        }

        #endregion Public Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public string CommandLineText => _commandLineModel.CommandLineTextToCopy;

        public string CopyCommandString
        { get { return _commandLineModel.CopyCommandString; } set { _commandLineModel.CopyCommandString = value; } }

        public string IISStartString
        { get { return _commandLineModel.IISStartString; } set { _commandLineModel.IISStartString = value; } }

        public string IISStopString
        { get { return _commandLineModel.IISStopString; } set { _commandLineModel.IISStopString = value; } }

        public bool RestartIIS
        { get { return _commandLineModel.RestartIIS; } set { _commandLineModel.RestartIIS = value; } }
        
        public bool CopyFilesWithSamePath
        { get { return _pathFinderModel.CopyFilesWithSamePath; } set { _pathFinderModel.CopyFilesWithSamePath = value; } }
        public bool RemoveDuplicates 
        { get { return _pathFinderModel.RemoveDuplicates; } set { _pathFinderModel.RemoveDuplicates = value; } }

        public ObservableCollection<FileInfo> SourceFiles => new ObservableCollection<FileInfo>(_pathFinderModel.SourceFiles);
        public ObservableCollection<FolderNode> TargetFolders => new ObservableCollection<FolderNode>(_pathFinderModel.TargetFolders);

        #endregion Public Properties

        #region Public Methods

        public void AddSelectedPaths(IEnumerable<string> fileNames)
        {
            _pathFinderModel.AddSelectedPaths(fileNames);
        }

        public void AddTargetFolders(IEnumerable<string> fileNames)
        {
            _pathFinderModel.AddTargetFolders(fileNames);
        }

        public void NewProject()
        {
            _pathFinderModel.Clear();
        }

        public void OpenProjectFromFile(string fileName)
        {
            var file = new FileInfo(fileName);
            using (var readStream = file.OpenText())
            {
                ProjectSave pS = JsonConvert.DeserializeObject<ProjectSave>(readStream.ReadToEnd());
                _pathFinderModel.LoadFromSave(pS.PathFinderSave);
                _commandLineModel.LoadFromSave(pS.CommandLineWorkerSettingsSave);
                _commandLineModel.GenerateCommandLineString(_pathFinderModel.FilesPathsCopyFromTo);
            }
        }

        public void RemoveSourceRow(object dataContext)
        {
            var fileInfo = dataContext as FileInfo ?? throw new InvalidDataException();
            _pathFinderModel.RemoveSourceRow(fileInfo);
        }

        public void RemoveTargetRow(object nodeDataContext)
        {
            var node = nodeDataContext as FolderNode ?? throw new InvalidDataException();
            _pathFinderModel.RemoveTargetRow(node);
        }

        public async Task SaveFileIntoProject(string fileName)
        {
            var file = new FileInfo(fileName);
            using (var writeStream = file.CreateText())
            {
                ProjectSave pS = new ProjectSave() { CommandLineWorkerSettingsSave = _commandLineModel.GetSave(), PathFinderSave = _pathFinderModel.GetSave() };
                await writeStream.WriteAsync(JsonConvert.SerializeObject(pS));
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal void GenerateCommandLine()
        {
            _commandLineModel.GenerateCommandLineString(_pathFinderModel.FilesPathsCopyFromTo);
        }

        #endregion Internal Methods

        #region Private Methods

        private void ModelChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
            if (e.PropertyName == nameof(_pathFinderModel.FilesPathsCopyFromTo))
            {
                _commandLineModel.GenerateCommandLineString(_pathFinderModel.FilesPathsCopyFromTo);
            }
        }

        #endregion Private Methods
    }
}