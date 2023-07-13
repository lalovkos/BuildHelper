using BuilderHelperOnWPF.Interfaces;
using System.Collections.Generic;

namespace BuilderHelperOnWPF.Models
{
    internal interface ICommandLineBuilder : ICLCommand
    {
        #region Public Methods

        void SetCommandBetweenCommands(string command);

        void SetEndCommands(IEnumerable<string> EndCommands);

        void SetHeaderCommands(IEnumerable<string> EndCommands);

        void SetMainCommands(IEnumerable<string> MainCommands);

        #endregion Public Methods
    }
}