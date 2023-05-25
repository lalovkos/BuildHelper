using System.Collections.Generic;
using System.IO;
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
                var st = settings.CopyString + " " + settings.FileToCopyInfos[i].FullName + " " + settings.FileToCopyInfos[i].FullName + ";";
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
        public readonly List<FileInfo> FileToCopyInfos = new List<FileInfo>();
        public readonly string IISStartString;
        public readonly string IISStopString;
        public readonly List<FileInfo> TargetFileInfos = new List<FileInfo>();

        #endregion Public Fields

        #region Public Constructors

        public CommandLineSettings(List<FileInfo> fileToCopyInfos, List<FileInfo> targetFileInfos)
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