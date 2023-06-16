using System.Diagnostics;
using System.Threading.Tasks;

namespace BuilderHelperOnWPF.Utility
{
    internal static class CommandLineExecutor
    {
        #region Public Methods
        public async static Task ExecuteFromStringAsync(string command)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.UseShellExecute = true;
            startInfo.FileName = "cmd.exe";
            startInfo.Verb = "runas";
            startInfo.Arguments = "/user:Administrator \"cmd /K" + " " + command + "\"";
            process.StartInfo = startInfo;
            await Task.Run ( () => process.Start());
        }

        #endregion Public Methods
    }
}