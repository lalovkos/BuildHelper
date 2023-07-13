using System.Collections.Generic;

namespace BuilderHelperOnWPF.Models
{
    internal interface ICommandLineBuilder
    {
        #region Public Methods

        void SetMainCommands(IEnumerable<string> MainCommands);

        string GenerateCommandLine();

        void SetCommandBetweenCommands(string command);

        void SetEndCommands(IEnumerable<string> EndCommands);

        void SetHeaderCommands(IEnumerable<string> EndCommands);

        #endregion Public Methods
    }
}