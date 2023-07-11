using BuilderHelperOnWPF.Interfaces;
using System;
using System.Text;

namespace BuilderHelperOnWPF.Models
{
    internal class CommandBlock : CommandLineElement
    {
        #region Public Fields

        public ICLCommand[] Commands = new ICLCommand[] { };
        public ICLCommand СommandBetween = new BaseCommand(" ");

        #endregion Public Fields

        #region Public Constructors

        public CommandBlock(ICLCommand commandBetweenLines)
        {
            СommandBetween = commandBetweenLines;
        }

        public CommandBlock(ICLCommand[] commands, ICLCommand commandBetweenLines) : this(commandBetweenLines)
        {
            Commands = commands;
        }

        #endregion Public Constructors

        #region Public Methods

        public override string FormCommandLine()
        {
            if (Commands.Length > 0)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < Commands.Length - 1; i++)
                {
                    sb.Append(Commands[0].GetCommand() + СommandBetween.GetCommand());
                }
                sb.Append(Commands[Commands.Length - 1].GetCommand());
                return sb.ToString();
            }
            else
            {
                return "";
            }
        }

        #endregion Public Methods
    }
}