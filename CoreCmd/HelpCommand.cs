using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCmd
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method )]
    public class HelpAttribute : Attribute
    {
        public HelpAttribute(String description_in)
        {
            this.description = description_in;
        }

        protected String description;
        public String Description { get { return this.description; } }
    }

    class HelpCommand
    {
        ICommandFinder _commandFinder = new CommandFinder();

        public void Show(string command)
        {
            const string commandPostfix = "command";
            var allClassTypes = _commandFinder.GetCommandClassTypes(commandPostfix);
            var targetCommand = _commandFinder.GetTargetCommandObject(allClassTypes, new string[]{ command });

            if(targetCommand != null)
                targetCommand.PrintHelp();
            else
                Console.WriteLine("No command object found");
        }
    }
}
