using CoreCmd.CommandFind;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public List<Assembly> additionalAssemblies = new List<Assembly>();

        public void SetAdditionalSearchAssembly(Assembly assembly)
        {
            this.additionalAssemblies.Add(assembly);
        }

        public AssemblyCommandExecutor() { }

        public AssemblyCommandExecutor(params Type[] types)
        {
            if (types != null)
            {
                foreach(var type in types)
                    this.SetAdditionalSearchAssembly(Assembly.GetAssembly(type));
            }
        }

        public void Execute(string[] args)
        {
            try
            {
                ICommandClassLoader _loader = new CommandClassLoader();

                var allClassTypes = _loader.LoadAllCommandClasses(additionalAssemblies);

                if (args.Length > 0)
                    ExecuteFirstCommand(allClassTypes);
                else
                    PrintHelpMessage(allClassTypes, GlobalConsts.CommandPostFix);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            /** local functions **/

            void ExecuteFirstCommand(IEnumerable<Type> allClassTypes)
            {
                ICommandExecutorCreate _commandFinder = new CommandExecutorCreator();
                var singleCommandExecutor = _commandFinder.GetSingleCommandExecutor(allClassTypes, args);

                if (singleCommandExecutor != null)
                    singleCommandExecutor.Execute();
                else
                    Console.WriteLine("No command object found");
            }

            void PrintHelpMessage(IEnumerable<Type> allClassTypes, string commandPostfix)
            {
                Console.WriteLine("Subcommand is missing, please specify subcommands:");

                // print all available commands
                foreach (var cmd in allClassTypes)
                    Console.WriteLine(Utils.LowerKebabCase(cmd.Name.Substring(0, cmd.Name.Length - commandPostfix.Length)));
            }
        }
    }
}
