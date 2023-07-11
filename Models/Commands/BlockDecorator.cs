using System.Text;

namespace BuilderHelperOnWPF.Models
{
    internal class BlockDecorator : CommandLineDecorator
    {
        #region Private Fields

        private readonly CommandLineElement _decoration;

        #endregion Private Fields

        #region Public Constructors

        public BlockDecorator(CommandLineElement comp, CommandBlock decoration) : base(comp)
        {
            _decoration = decoration;
        }

        #endregion Public Constructors

        #region Public Methods

        public override string FormCommandLine()
        {
            return new StringBuilder(base.FormCommandLine() + _decoration.FormCommandLine()).ToString();
        }

        #endregion Public Methods
    }
}