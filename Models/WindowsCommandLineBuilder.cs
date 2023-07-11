using BuilderHelperOnWPF.Interfaces;
using System.Collections.Generic;
namespace BuilderHelperOnWPF.Models
{
    internal class WindowsCommandLineBuilder : ICommandLineBuilder
    {
        #region Public Properties

        private ICLCommand[] _endCommands = new ICLCommand[0];
        private ICLCommand[] _startingCommands = new ICLCommand[0];
        private List<CommandBlock> _mainCommandBlocks = new List<CommandBlock>();
        private ICLCommand _commandBetweenCommands = new BaseCommand(" ");
        private CommandLineElement _commandLine = new EmptyCommand();

        #endregion Public Properties

        #region Private Methods

        private CommandBlock GenerateEndBlock()
        {
            return new CommandBlock(_endCommands, _commandBetweenCommands);
        } 

        private CommandBlock GenerateHeaderBlock()
        {
            return new CommandBlock(_startingCommands, _commandBetweenCommands);
        }

        private ICLCommand[] GetCopyCommandArray(IEnumerable<(string, string)> pathsFromTo)
        {
            var result = new List<CopyCommand>();
            foreach (var path in pathsFromTo)
            {
                result.Add(new CopyCommand("Copy ", path.Item1, path.Item2));
            }
            return result.ToArray();
        }

        private void ReconstructCommand() 
        {
            _commandLine = new EmptyCommand();
            _commandLine = new BlockDecorator(_commandLine, GenerateHeaderBlock());
            if (_startingCommands.Length > 0) _commandLine = new BaseCommandDecorator(_commandLine, _commandBetweenCommands);
            foreach (var block in _mainCommandBlocks) 
            {
                _commandLine = new BlockDecorator(_commandLine, block);
            }

            if (_mainCommandBlocks.Count > 0) _commandLine = new BaseCommandDecorator(_commandLine, _commandBetweenCommands);
            _commandLine = new BlockDecorator(_commandLine, GenerateEndBlock());
        }

        public void SetHeaderCommands(ICLCommand[] headerCommands)
        {
            _startingCommands = headerCommands;
        }

        public void AddCopyingCommands(IEnumerable<(string, string)> pathsFromTo)
        {
            _mainCommandBlocks.Add(new CommandBlock(GetCopyCommandArray(pathsFromTo), _commandBetweenCommands));
            ReconstructCommand();
        }

        public void SetCommandBetweenCommands(ICLCommand command)
        {
            _commandBetweenCommands = command;
        }

        public void SetEndCommands(ICLCommand[] endCommands)
        {
            _endCommands = endCommands;
        }

        public string GenerateCommandLine()
        {
            if (_commandLine is EmptyCommand) ReconstructCommand();
            return _commandLine.FormCommandLine();
        }

        #endregion Private Methods
    }
}