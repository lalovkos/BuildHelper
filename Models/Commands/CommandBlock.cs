using BuilderHelperOnWPF.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuilderHelperOnWPF.Models
{
    internal class CommandBlock : CommandLineElement, ICLCommand
    {
        #region Public Fields

        public IList<ICLCommand> Commands = new List<ICLCommand>();
        public ICLCommand СommandBetween = new BaseCommand(" ");

        #endregion Public Fields

        #region Public Constructors

        public CommandBlock(ICLCommand commandBetweenLines)
        {
            СommandBetween = commandBetweenLines;
        }

        public CommandBlock(IList<ICLCommand> commands, ICLCommand commandBetweenLines) : this(commandBetweenLines)
        {
            Commands = commands;
        }

        #endregion Public Constructors

        #region Public Methods

        public override string FormCommandLine()
        {
            if (Commands.Count() > 0)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < Commands.Count() - 1; i++)
                {
                    var command = Commands[i].GetCommand();
                    sb.Append(command);
                    if (command != "") sb.Append(СommandBetween.GetCommand());
                }
                sb.Append(Commands[Commands.Count() - 1].GetCommand());
                return sb.ToString();
            }
            else
            {
                return "";
            }
        }

        public string GetCommand()
        {
            return FormCommandLine();
        }

        #endregion Public Methods
    }
}