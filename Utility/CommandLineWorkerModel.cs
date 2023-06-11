using BuilderHelperOnWPF.Models;
using BuilderHelperOnWPF.Models.SaveModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace BuilderHelperOnWPF.ViewModels
{
    public class CommandLineWorkerModel : INotifyPropertyChanged, ISaveable<CommandLineWorkerSettingsSave>
    {
        #region Public Fields

        public const string DEFAULT_COPY_STRING = "xcopy /Y /Q ";
        public const string DEFAULT_IIS_START_STRING = "iisreset /start localhost";
        public const string DEFAULT_IIS_STOP_STRING = "iisreset /stop /noforce localhost";
        public const bool RESTART_IIS = true;

        #endregion Public Fields

        #region Private Fields

        private string _commandLineText;
        private string _copyCommandString;
        private string _iISStartString;
        private string _iISStopString;
        private bool _restartIIS;

        #endregion Private Fields

        #region Public Constructors

        public CommandLineWorkerModel()
        {
            CopyCommandString = DEFAULT_COPY_STRING;
            IISStopString = DEFAULT_IIS_STOP_STRING;
            IISStartString = DEFAULT_IIS_START_STRING;
            RestartIIS = RESTART_IIS;
            CommandLineTextToCopy = "";
            CommandLineTextToCopy = "";
            GenerateCommandLineStringForExecuting();
            GenerateCommandLineStringForFile();
        }

        #endregion Public Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public string CommandLineTextToCopy
        { get => _commandLineText; private set { _commandLineText = value; NotifyPropertyChanged(nameof(CommandLineTextToCopy)); } }
        public string CommandLineTextToExecute
        { get => _commandLineText; private set { _commandLineText = value; NotifyPropertyChanged(nameof(CommandLineTextToExecute)); } }

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

        public void GenerateCommandLineStringForExecuting(List<(string, string)> fileToCopyInfos = null)
        {
            StringBuilder sb = new StringBuilder();
            if (RestartIIS)
            {
                sb.Append(IISStopString + " & ");
            }

            if (fileToCopyInfos != null)
            {
                for (int i = 0; i < fileToCopyInfos.Count; i++)
                {
                    var st = CopyCommandString + " " + fileToCopyInfos[i].Item1 + " " + fileToCopyInfos[i].Item2 + "* & ";
                    sb.Append(st);
                }
            }

            if (RestartIIS)
            {
                sb.Append(IISStartString);
            }
            CommandLineText = sb.ToString();
        }

        public void GenerateCommandLineStringForFile(List<(string, string)> fileToCopyInfos = null)
        {
            StringBuilder sb = new StringBuilder();
            if (RestartIIS)
            {
                sb.AppendLine(IISStopString);
            }

            if (fileToCopyInfos != null)
            {
                for (int i = 0; i < fileToCopyInfos.Count; i++)
                {
                    var st = CopyCommandString + " " + fileToCopyInfos[i].Item1 + " " + fileToCopyInfos[i].Item2;
                    sb.AppendLine(st);
                }
            }

            if (RestartIIS)
            {
                sb.AppendLine(IISStartString);
            }
            CommandLineText = sb.ToString();
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