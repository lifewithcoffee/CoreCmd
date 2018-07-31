using CoreCmd.CommandFind;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCmd.BuildinCommands
{
    class HelpCommand
    {
        public void Show(string command)
        {
            ICommandFinder _commandFinder = new CommandFinder();
            ICommandClassLoader _loader = new CommandClassLoader();

            var allClassTypes = _loader.LoadAllCommandClasses(null);
            var singleCommandExecutor = _commandFinder.GetSingleCommandExecutor(allClassTypes, new string[]{ command });

            if(singleCommandExecutor != null)
                singleCommandExecutor.PrintHelp();
            else
                Console.WriteLine("No command object found");
        }
    }
}
