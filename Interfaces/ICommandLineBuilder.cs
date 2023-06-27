using System.Collections.Generic;

namespace BuilderHelperOnWPF.Models
{
    internal interface ICommandLineBuilder
    {
        string GenerateCommandLine(List<(string, string)> pathsFromTo);
    }
}