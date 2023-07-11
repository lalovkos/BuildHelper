using BuilderHelperOnWPF.Interfaces;
using System.Collections.Generic;

namespace BuilderHelperOnWPF.Models
{
    internal interface ICommandLineBuilder
    {
        #region Public Methods

        void SetHeaderCommands(ICLCommand[] EndCommands);
        void AddCopyingCommands(IEnumerable<(string, string)> pathsFromTo);
        void SetCommandBetweenCommands(ICLCommand command);
        void SetEndCommands(ICLCommand[] EndCommands);
        string GenerateCommandLine();

        #endregion Public Methods
    }
}