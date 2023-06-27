using BuilderHelperOnWPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuilderHelperOnWPF.Models
{

    internal class WindowsCommandLineBuilder : ICommandLineBuilder
    {
        public string BetweenCommandsString;
        public WindowsCommandLineBuilder(string betweenCommandsString = "")
        {

        }

        public string GenerateCommandLine(List<(string, string)> pathsFromTo = null)
        {
            //Generating header
            CommandBlock header = new CommandBlock()
            {
                Commands = new ICommand[] { new BaseCommand("Start") },
                Order = 999,
            };

            //Generating main blocks
            CommandBlock main = new CommandBlock();
            if (pathsFromTo != null)
            {

                main.Commands = GetCommandArray(pathsFromTo);
                main.StringBetweenLines = " *& ";
            }

            //Generating end
            CommandBlock end = new CommandBlock()
            {
                Commands = new ICommand[] { new BaseCommand("Finish") },
                Order = -1000
            };

            return GenerateCommandLineForCopying(new CommandBlock[] { header, main, end });
        }

        private ICommand[] GetCommandArray(List<(string, string)> pathsFromTo)
        {
            var result = new List<CopyCommand>();
            foreach (var path in pathsFromTo) 
            {
                result.Add(new CopyCommand("Copy ", path.Item1, path.Item2));
            }
            return result.ToArray();
        }

        private string GenerateCommandLineForCopying(CommandBlock[] commandBlocks)
        {
            CommandLineElement commandLineResult = new EmptyCommandLine();
            Array.Sort(commandBlocks);
            foreach (var cmdBl in commandBlocks) 
            {
                commandLineResult = new DecorateWithBlock(commandLineResult, cmdBl);
            }

            return commandLineResult.FormCommandLine();                
        }

        private string GenerateCommandLineStringForExecuting(List<(string, string)> fileToCopyInfos = null)
        {
            StringBuilder sb = new StringBuilder();
            //if (RestartIIS)
            //{
            //    sb.Append(IISStopString + " & ");
            //}

            //if (fileToCopyInfos != null)
            //{
            //    for (int i = 0; i < fileToCopyInfos.Count; i++)
            //    {
            //        var st = CopyCommandString + " " + fileToCopyInfos[i].Item1 + " " + fileToCopyInfos[i].Item2;
            //        sb.Append(st);
            //        if (i < fileToCopyInfos.Count) { sb.Append("* & "); }; //Last line shouldn't have *&
            //    }
            //}

            //if (RestartIIS)
            //{
            //    if (sb.Length == 0) sb.Append("* & ");
            //    sb.Append(IISStartString);
            //}
            return sb.ToString();
        }
    }
}