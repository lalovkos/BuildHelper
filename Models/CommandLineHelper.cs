using System.Collections.Generic;
using System.Text;

namespace BuilderHelperOnWPF.Models
{
    /// <summary>
    /// Class for working with cmd.exe
    /// </summary>
    internal static class CommandLineHelper
    {
        #region Public Methods

        public static string GenerateCommandLineString(CommandLineSettings settings)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(settings.IISStopString + ";");

            for (int i = 0; i < settings.FileToCopyInfos.Count; i++)
            {
                var st = settings.CopyString + " " + settings.FileToCopyInfos[i].Path + " " + settings.FileToCopyInfos[i].Path + ";";
                sb.AppendLine(st);
            }

            sb.AppendLine(settings.IISStartString + ";");
            return sb.ToString();
        }

        #endregion Public Methods
    }

    internal class CommandLineSettings
    {
        #region Public Fields

        public readonly string CopyString;
        public readonly List<FileToCopyInfo> FileToCopyInfos = new List<FileToCopyInfo>();
        public readonly string IISStartString;
        public readonly string IISStopString;
        public readonly List<TargetFileInfo> TargetFileInfos = new List<TargetFileInfo>();

        #endregion Public Fields

        #region Public Constructors

        public CommandLineSettings(List<FileToCopyInfo> fileToCopyInfos, List<TargetFileInfo> targetFileInfos)
        {
            FileToCopyInfos = fileToCopyInfos;
            TargetFileInfos = targetFileInfos;
            CopyString = "xcopy /Y /Q ";
            IISStopString = "iisreset /stop /noforce localhost";
            IISStartString = "iisreset /start localhost";
        }

        #endregion Public Constructors
    }
}