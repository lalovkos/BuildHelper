using System.Diagnostics;
using System.Threading.Tasks;

namespace BuilderHelperOnWPF.Utility
{
    public class WindowsCommandLineExecutor : ICommandLineExecutor
    {
        #region Public Methods

        public async Task ExecuteFromStringAsync(string command)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.UseShellExecute = true;
            startInfo.FileName = "cmd.exe";
            startInfo.Verb = "runas";
            startInfo.Arguments = "/user:Administrator \"cmd /K" + " " + command + "\"";
            process.StartInfo = startInfo;
            await Task.Run(() => process.Start());
        }

        #endregion Public Methods
    }
}