using CoreCmd.Attributes;
using CoreCmd.CommandExecution;
using CoreCmd.CommandLoading;
using CoreCmd.Help;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCmd.BuiltinCommands
{
    [Help("Print the help information")]
    class HelpCommand
    {
        IHelpInfoService _helpSvc = new HelpInfoService();

        public void Default(string command)
        {
            ICommandExecutorCreate _exeCreator = new CommandExecutorCreator();
            ICommandClassLoader _loader = new CommandClassLoader();

            var allClassTypes = _loader.LoadAllCommandClasses(null);
            var singleCommandExecutor = _exeCreator.GetSingleCommandExecutor(allClassTypes, new string[]{ command });

            if(singleCommandExecutor != null)
            {
                _helpSvc.PrintClassHelp(singleCommandExecutor.CommandClassType);
            }
        }
    }
}
