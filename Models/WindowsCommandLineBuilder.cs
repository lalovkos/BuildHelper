using BuilderHelperOnWPF.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BuilderHelperOnWPF.Models
{
    internal class WindowsCommandLineBuilder : ICommandLineBuilder, ICLCommand
    {
        #region Private Fields

        private BaseCommand _commandBetweenCommands = new BaseCommand(" ");
        private CommandLineElement _commandLine = new EmptyCommand();
        private ICLCommand[] _endCommands = new ICLCommand[0];
        private List<CommandBlock> _mainCommandBlocks = new List<CommandBlock>();
        private ICLCommand[] _startingCommands = new ICLCommand[0];

        #endregion Private Fields

        #region Public Methods

        public string GenerateCommandLine()
        {
            if (_commandLine is EmptyCommand) ReconstructCommand();
            return _commandLine.FormCommandLine();
        }

        public string GetCommand()
        {
            return GenerateCommandLine();
        }

        public void SetCommandBetweenCommands(string command)
        {
            _commandBetweenCommands.SetCommand(command);
        }

        public void SetEndCommands(IEnumerable<string> endCommands)
        {
            _endCommands = endCommands.Select(command => new BaseCommand(command)).ToArray();
        }

        public void SetHeaderCommands(IEnumerable<string> headerCommands)
        {
            _startingCommands = headerCommands.Select(command => new BaseCommand(command)).ToArray();
        }

        public void SetMainCommands(IEnumerable<string> MainCommands)
        {
            _mainCommandBlocks = new List<CommandBlock>
            {
                new CommandBlock(GetCommandArray(MainCommands), _commandBetweenCommands)
            };
            ReconstructCommand();
        }

        #endregion Public Methods

        #region Private Methods

        private CommandBlock GenerateEndBlock()
        {
            return new CommandBlock(_endCommands, _commandBetweenCommands);
        }

        private CommandBlock GenerateHeaderBlock()
        {
            return new CommandBlock(_startingCommands, _commandBetweenCommands);
        }

        private ICLCommand[] GetCommandArray(IEnumerable<string> commands)
        {
            return commands.Select(command => new BaseCommand(command)).ToArray();
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

        #endregion Private Methods
    }
}