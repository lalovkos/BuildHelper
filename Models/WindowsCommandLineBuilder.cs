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
        private List<ICLCommand> _endCommands = new List<ICLCommand>();
        private List<ICLCommand> _mainCommandBlocks = new List<ICLCommand>();
        private List<ICLCommand> _startingCommands = new List<ICLCommand>();

        #endregion Private Fields

        #region Public Methods

        public string GetCommand()
        {
            if (_commandLine is EmptyCommand) ReconstructCommand();
            return _commandLine.FormCommandLine();
        }

        public void SetCommandBetweenCommands(string command)
        {
            _commandBetweenCommands.SetCommand(command);
        }

        public void SetEndCommands(IEnumerable<string> endCommands)
        {
            _endCommands.Clear();
            _endCommands.AddRange(endCommands.Select(command => new BaseCommand(command)));
        }

        public void SetHeaderCommands(IEnumerable<string> headerCommands)
        {
            _startingCommands.Clear();
            _startingCommands.AddRange(headerCommands.Select(command => new BaseCommand(command)));
        }

        public void SetMainCommands(IEnumerable<string> mainCommands)
        {
            _mainCommandBlocks.Clear();
            _mainCommandBlocks.AddRange(mainCommands.Select(command => new BaseCommand(command)));
        }

        #endregion Public Methods

        #region Private Methods

        private void ReconstructCommand()
        {
            _commandLine = new EmptyCommand();
            var commandBlocks = new List<ICLCommand>
            {
                new CommandBlock(_startingCommands, _commandBetweenCommands),
                new CommandBlock(_mainCommandBlocks, _commandBetweenCommands),
                new CommandBlock(_endCommands, _commandBetweenCommands)
            };
            _commandLine = new BlockDecorator(_commandLine, new CommandBlock(commandBlocks.ToArray(), _commandBetweenCommands));
        }

        #endregion Private Methods
    }
}