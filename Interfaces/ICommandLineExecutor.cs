using System.Threading.Tasks;

namespace BuilderHelperOnWPF.Utility
{
    public interface ICommandLineExecutor
    {
        #region Public Methods

        Task ExecuteFromStringAsync(string command);

        #endregion Public Methods
    }
}