namespace BuilderHelperOnWPF.Models
{
    public class CopyCommand : BaseCommand
    {
        #region Public Constructors

        public CopyCommand(string copyCommand, string pathFrom, string pathTo) :
            base(GenerateBody(copyCommand, pathFrom, pathTo))
        {
        }

        #endregion Public Constructors

        #region Private Methods

        private static string GenerateBody(string copyCommand, string pathFrom, string pathTo) => copyCommand + " " + pathFrom + " " + pathTo;

        #endregion Private Methods
    }
}