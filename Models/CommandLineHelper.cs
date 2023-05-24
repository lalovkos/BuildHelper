using BuilderHelperOnWPF.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderHelperOnWPF.Models
{
    /// <summary>
    /// Class for working with cmd.exe
    /// </summary>
    internal static class CommandLineHelper
    {
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
    }

    internal class CommandLineSettings 
    {
        public readonly List<FileToCopyInfo> FileToCopyInfos = new List<FileToCopyInfo>();
        public readonly List<TargetFileInfo> TargetFileInfos = new List<TargetFileInfo>();
        public readonly string CopyString;
        public readonly string IISStopString;
        public readonly string IISStartString;

        public CommandLineSettings(List<FileToCopyInfo> fileToCopyInfos, List<TargetFileInfo> targetFileInfos)
        {
            FileToCopyInfos = fileToCopyInfos;
            TargetFileInfos = targetFileInfos;
            CopyString = "xcopy /Y /Q ";
            IISStopString = "iisreset /stop /noforce localhost";
            IISStartString = "iisreset /start localhost";
        }

    }
}
