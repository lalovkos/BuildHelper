namespace BuilderHelperOnWPF.Models
{
    public abstract class CommandLineDecorator : CommandLineElement
    {
        #region Protected Fields

        protected CommandLineElement _component;

        #endregion Protected Fields

        #region Public Constructors

        public CommandLineDecorator(CommandLineElement component)
        {
            _component = component;
        }

        #endregion Public Constructors

        #region Public Methods

        public override string FormCommandLine()
        {
            return _component.FormCommandLine();
        }

        #endregion Public Methods
    }
}