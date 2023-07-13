using System.Collections.Generic;
using System.Linq;

namespace BuilderHelperOnWPF.Interfaces
{
    public static class CommandLineHelper
    {
        #region Public Methods

        public static string FormCopingCommand(string copyCommandString, (string, string) pathFromTo)
        {
            return copyCommandString + " " + pathFromTo.Item1 + " " + pathFromTo.Item2;
        }

        public static IEnumerable<string> FormCopingCommands(string copyCommandString, IEnumerable<(string, string)> pathsFromTo)
        {
            return pathsFromTo.Select(path => FormCopingCommand(copyCommandString, path));
        }

        #endregion Public Methods
    }
}