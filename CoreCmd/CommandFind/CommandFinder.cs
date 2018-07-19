using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using CoreCmd.CommandExecution;

namespace CoreCmd.CommandFind
{
    interface ICommandFinder
    {
        IEnumerable<Type> GetAllCommandClasses(string assemblyPrefix = "command", string commandPostfix = "command");
        ITargetCommand GetTargetCommand(IEnumerable<Type> targetTypes, string[] args);
    }

    internal class CommandFinder : ICommandFinder
    {
        IAssemblyFinder _assemblyFinder = new AssemblyFinder();
        IAssemblyCommandFinder _assemblyCommandFinder = new AssemblyCommandFinder();

        public ITargetCommand GetTargetCommand(IEnumerable<Type> targetTypes, string[] args)
        {
            TargetCommand result = null;
            if (args.Length > 0)
            {
                result = new TargetCommand();
                string command = $"{args[0]}-command".ToLower();

                Type targetType = targetTypes.SingleOrDefault(t => Utils.LowerKebabCase(t.Name).Equals(command));
                if (targetType != null)
                {
                    result.CommandType = targetType;
                    if (args.Length > 1)
                    {
                        result.MethodSubcommand = args[1];
                        result.Parameters = args.Skip(2).ToArray();
                    }
                    else
                        result.MethodSubcommand = "default-method";
                }
                else
                {
                    result.CommandType = targetTypes.SingleOrDefault(t => t.Name.Equals("DefaultCommand"));
                    result.MethodSubcommand = args[0];
                    result.Parameters = args.Skip(1).ToArray();
                }
            }
            return result;
        }

        public IEnumerable<Type> GetAllCommandClasses(string commandPostfix = "command", string assemblyPrefix = "command")
        {
            var allTypeList = new List<List<Type>>();

            var entryAssembly = Assembly.GetEntryAssembly();
            var coreCmdAssembly = Assembly.GetAssembly(typeof(CommandExecutor));

            allTypeList.Add(_assemblyCommandFinder.GetCommandClassTypesFromAssembly(entryAssembly, commandPostfix)); // the console app itself's assembly

            if(!entryAssembly.GetName().Name.Equals(coreCmdAssembly.GetName().Name))    // it's not CoreCmd.dll itself, otherwise the commands in CoreCmd.dll will be printed twice
                allTypeList.Add(_assemblyCommandFinder.GetCommandClassTypesFromAssembly(coreCmdAssembly,commandPostfix));    // the dependent CoreCmd package's assembly

            var dlls = _assemblyFinder.GetCommandAssembly(Directory.GetCurrentDirectory(), assemblyPrefix);    // the assemblies in the current dir match some naming pattern
            foreach (var dll in dlls)
                allTypeList.Add(_assemblyCommandFinder.GetCommandClassTypesFromAssembly(dll, commandPostfix));

            return allTypeList.SelectMany(r => r);
        }
    }
}
