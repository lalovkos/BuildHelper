using BuilderHelperOnWPF.Interfaces;
using BuilderHelperOnWPF.Models.SaveModels;
using BuilderHelperOnWPF.Utility;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

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

        public void GenerateCommandLine(List<(string, string)> filesPathsCopyFromTo = null)
        {
            var commandBetween = new BaseCommand(" *& ");
            _commandLineBuilder.SetCommandBetweenCommands(commandBetween);
            _commandLineBuilder.SetHeaderCommands(new ICLCommand[] { new BaseCommand("Start")} );
            if (filesPathsCopyFromTo != null) _commandLineBuilder.AddCopyingCommands(filesPathsCopyFromTo);
            _commandLineBuilder.SetEndCommands(new ICLCommand[] { new BaseCommand("Finish") });
            var string1 = _commandLineBuilder.GenerateCommandLine();
            commandBetween.SetCommand(" ");
            var string2 = _commandLineBuilder.GenerateCommandLine();
            int i = 0;
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