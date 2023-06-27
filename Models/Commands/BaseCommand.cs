using BuilderHelperOnWPF.Interfaces;

namespace BuilderHelperOnWPF.Models
{
    public class BaseCommand : ICommand
    {
        #region Private Fields

        private readonly string _body;

        #endregion Private Fields

        #region Public Constructors

        public BaseCommand(string body)
        {
            _body = body;
        }

        #endregion Public Constructors

        #region Public Methods

        public string GetCommand()
        {
            return _body;
        }

        #endregion Public Methods
    }
}