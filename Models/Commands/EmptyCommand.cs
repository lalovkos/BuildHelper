namespace BuilderHelperOnWPF.Models
{
    internal class EmptyCommand : BaseCommand
    {
        #region Public Constructors

        public EmptyCommand() : base("")
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override string FormCommandLine()
        {
            return base.FormCommandLine();
        }

        #endregion Public Methods
    }
}