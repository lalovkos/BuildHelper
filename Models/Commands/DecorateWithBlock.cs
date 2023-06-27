using System.Text;

namespace BuilderHelperOnWPF.Models
{
    internal class DecorateWithBlock : CommandLineDecorator 
    {
        private CommandBlock _commandBlock = null;
        public DecorateWithBlock(CommandLineElement comp, CommandBlock commandBlock) : base(comp)
        {
            _commandBlock = commandBlock;
        }

        public override string FormCommandLine()
        {
            return base.FormCommandLine() + _commandBlock.FormCommandLine();
        }
    }
}