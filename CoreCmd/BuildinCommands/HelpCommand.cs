using CoreCmd.CommandExecution;
using CoreCmd.CommandLoading;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCmd.BuildinCommands
{
    class HelpCommand
    {
        public void Show(string command)
        {
            ICommandExecutorCreate _exeCreator = new CommandExecutorCreator();
            ICommandClassLoader _loader = new CommandClassLoader();

            var allClassTypes = _loader.LoadAllCommandClasses(null);
            var singleCommandExecutor = _exeCreator.GetSingleCommandExecutor(allClassTypes, new string[]{ command });

            if(singleCommandExecutor != null)
                singleCommandExecutor.PrintHelp();
            else
                Console.WriteLine("No command object found");
        }
    }
}
