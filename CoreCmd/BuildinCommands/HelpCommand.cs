using CoreCmd.CommandFind;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCmd.BuildinCommands
{
    class HelpCommand
    {
        ICommandFinder _commandFinder = new CommandFinder();

        public void Show(string command)
        {
            var allClassTypes = _commandFinder.GetAllCommandClasses();
            var singleCommandExecutor = _commandFinder.GetSingleCommandExecutor(allClassTypes, new string[]{ command });

            if(singleCommandExecutor != null)
                singleCommandExecutor.PrintHelp();
            else
                Console.WriteLine("No command object found");
        }
    }
}
