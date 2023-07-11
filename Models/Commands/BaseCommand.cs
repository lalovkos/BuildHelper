using BuilderHelperOnWPF.Interfaces;

namespace BuilderHelperOnWPF.Models
{
    public class BaseCommand : CommandLineElement, ICLCommand
    {
        #region Private Fields

        private string _body;

        #endregion Private Fields

        #region Public Constructors

        public BaseCommand(string body)
        {
            _body = body;
        }

        public BaseCommand(ref string body)
        {
            _body = body;
            body = "sdadsd";
        }

        #endregion Public Constructors

        #region Public Methods

        public override string FormCommandLine()
        {
            return _body;
        }

        public string GetCommand()
        {
            return _body;
        }

        public void SetCommand(string command) 
        {
            _body = command;
        }

        #endregion Public Methods
    }
}