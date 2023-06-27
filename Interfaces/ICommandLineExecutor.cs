using System.Threading.Tasks;

namespace BuilderHelperOnWPF.Utility
{
    public interface ICommandLineExecutor
    {
        Task ExecuteFromStringAsync(string command);
    }
}