using BuilderHelperOnWPF.Interfaces;
using System;
using System.Text;

namespace BuilderHelperOnWPF.Models
{
    internal class CommandBlock : CommandLineElement, IComparable<CommandBlock>
    {
        #region Public Fields

        public ICommand[] Commands = new ICommand[] { };
        public int Order = 0;
        public string StringBetweenLines = " ";

        #endregion Public Fields

        #region Public Constructors

        public CommandBlock()
        { }

        public CommandBlock(ICommand[] commands, int order = 0, string stringBetweenLines = " ")
        {
            Commands = commands;
            Order = order;
            StringBetweenLines = stringBetweenLines;
        }

        #endregion Public Constructors

        #region Public Methods

        public int CompareTo(CommandBlock other)
        {
            return other.Order - this.Order;
        }

        public override string FormCommandLine()
        {
            if (Commands.Length > 0)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < Commands.Length - 1; i++)
                {
                    sb.Append(Commands[0].GetCommand() + StringBetweenLines);
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