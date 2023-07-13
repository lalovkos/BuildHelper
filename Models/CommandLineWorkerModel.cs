using BuilderHelperOnWPF.Interfaces;
using BuilderHelperOnWPF.Models.SaveModels;
using BuilderHelperOnWPF.Utility;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BuilderHelperOnWPF.Models
{
    public class CommandLineWorkerModel : INotifyPropertyChanged, ISaveable<CommandLineWorkerSettingsSave>
    {
        #region Private Fields

        private readonly ICommandLineBuilder _commandLineBuilder = new WindowsCommandLineBuilder();
        private readonly ICommandLineExecutor _commandLineExecutor = new WindowsCommandLineExecutor();
        private string _commandLineTextToCopy;
        private string _commandLineTextToExecute;
        private string _copyCommandString;
        private string _iISStartString;
        private string _iISStopString;
        private bool _restartIIS;

        #endregion Private Fields

        #region Public Constructors

        public CommandLineWorkerModel() : this(new CommandLineWorkerSettingsSave())
        {
        }

        public CommandLineWorkerModel(CommandLineWorkerSettingsSave commandLineWorkerSettingsSave)
        {
            LoadFromSave(commandLineWorkerSettingsSave);
        }

        #endregion Public Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public string CommandLineTextToCopy
        { get => _commandLineTextToCopy; private set { _commandLineTextToCopy = value; NotifyPropertyChanged(nameof(CommandLineTextToCopy)); } }

        public string CommandLineTextToExecute
        { get => _commandLineTextToExecute; private set { _commandLineTextToExecute = value; NotifyPropertyChanged(nameof(CommandLineTextToExecute)); } }

        public string CopyCommandString
        { get => _copyCommandString; set { _copyCommandString = value; NotifyPropertyChanged(nameof(CopyCommandString)); } }

        public string IISStartString
        { get => _iISStartString; set { _iISStartString = value; NotifyPropertyChanged(nameof(IISStartString)); } }

        public string IISStopString
        { get => _iISStopString; set { _iISStopString = value; NotifyPropertyChanged(nameof(IISStopString)); } }

        public bool RestartIIS
        { get => _restartIIS; set { _restartIIS = value; NotifyPropertyChanged(nameof(RestartIIS)); } }

        #endregion Public Properties

        #region Public Methods

        public void ExecuteCommandLine()
        {
            _commandLineExecutor.ExecuteFromStringAsync(_commandLineTextToExecute);
        }

        public async Task ExecuteCommandLineAsync()
        {
            await _commandLineExecutor.ExecuteFromStringAsync(_commandLineTextToExecute);
        }

        public void GenerateCommandLine(List<(string, string)> filesPathsCopyFromTo = null)
        {
            _commandLineBuilder.SetHeaderCommands(RestartIIS ? new string[] { IISStartString } : new string[] { });
            _commandLineBuilder.SetEndCommands(RestartIIS ? new string[] { IISStopString } : new string[] { });

            if (filesPathsCopyFromTo != null) _commandLineBuilder.SetMainCommands(CommandLineHelper.FormCopingCommands("CopyCommandString ", filesPathsCopyFromTo));

            _commandLineBuilder.SetCommandBetweenCommands(" *& ");
            CommandLineTextToExecute = _commandLineBuilder.GenerateCommandLine();

            _commandLineBuilder.SetCommandBetweenCommands("\n");
            CommandLineTextToCopy = _commandLineBuilder.GenerateCommandLine();
        }

        public CommandLineWorkerSettingsSave GetSave()
        {
            var save = new CommandLineWorkerSettingsSave()
            {
                CopyCommandString = CopyCommandString,
                IISStartString = IISStartString,
                IISStopString = IISStopString,
                RestartIIS = RestartIIS
            };
            return save;
        }

        public void LoadFromSave(CommandLineWorkerSettingsSave save)
        {
            CopyCommandString = save.CopyCommandString;
            IISStartString = save.IISStartString;
            IISStopString = save.IISStopString;
            RestartIIS = save.RestartIIS;
            GenerateCommandLine();
        }

        #endregion Public Methods

        #region Private Methods

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Private Methods
    }
}