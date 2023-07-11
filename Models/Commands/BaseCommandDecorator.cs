using BuilderHelperOnWPF.Interfaces;
using System.Text;

namespace BuilderHelperOnWPF.Models
{
    internal class BaseCommandDecorator : CommandLineDecorator
    {
        #region Private Fields

        private ICLCommand _decoration;

        #endregion Private Fields

        #region Public Constructors

        public BaseCommandDecorator(CommandLineElement comp, ICLCommand decoration) : base(comp)
        {
            _decoration = decoration;
        }

        #endregion Public Constructors

        #region Public Methods

        public override string FormCommandLine()
        {
            return new StringBuilder(base.FormCommandLine() + _decoration.GetCommand()).ToString();
        }

        #endregion Public Methods
    }
}