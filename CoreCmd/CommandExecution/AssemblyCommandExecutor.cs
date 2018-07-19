using CoreCmd.CommandFind;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CoreCmd.CommandExecution
{
    public interface IAssemblyCommandExecutor
    {
        void Execute(string[] args);
    }

    public class AssemblyCommandExecutor : IAssemblyCommandExecutor
    {
        ICommandFinder _commandFinder = new CommandFinder();

        public void Execute(string[] args)
        {
            try
            {
                const string commandPostfix = "command";
                var allClassTypes = _commandFinder.GetAllCommandClasses(commandPostfix);
                if (args.Length > 0)
                {
                    var singleCommandExecutor = _commandFinder.GetSingleCommandExecutor(allClassTypes, args);

                    if(singleCommandExecutor != null)
                        singleCommandExecutor.Execute();
                    else
                        Console.WriteLine("No command object found");
                }
                else
                {
                    Console.WriteLine("Subcommand is missing, please specify subcommands:");

                    // print all available commands
                    foreach(var cmd in allClassTypes)
                        Console.WriteLine(Utils.LowerKebabCase(cmd.Name.Substring(0,cmd.Name.Length - commandPostfix.Length)));
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        
    }
}
