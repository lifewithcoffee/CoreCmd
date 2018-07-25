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
        void SetAdditionalSearchAssembly(Assembly assembly);
        IEnumerable<Type> GetAllCommandClasses(string assemblyPrefix = "command", string commandPostfix = "command");
        ISingleCommandExecutor GetSingleCommandExecutor(IEnumerable<Type> targetTypes, string[] args);
    }

    internal class CommandFinder : ICommandFinder
    {

        public List<Assembly> additionalAssemblies = new List<Assembly>();
        private CommandClassLoader _commandClassLoader = new CommandClassLoader();

        public void SetAdditionalSearchAssembly(Assembly assembly)
        {
            this.additionalAssemblies.Add(assembly);
        }

        public ISingleCommandExecutor GetSingleCommandExecutor(IEnumerable<Type> targetTypes, string[] args)
        {
            SingleCommandExecutor result = null;
            if (args.Length > 0)
            {
                result = new SingleCommandExecutor();
                string command = $"{args[0]}-command".ToLower();

                Type targetType = targetTypes.SingleOrDefault(t => Utils.LowerKebabCase(t.Name).Equals(command));
                if (targetType != null)
                {
                    result.CommandClassType = targetType;
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
                    result.CommandClassType = targetTypes.SingleOrDefault(t => t.Name.Equals("DefaultCommand"));
                    result.MethodSubcommand = args[0];
                    result.Parameters = args.Skip(1).ToArray();
                }
            }
            return result;
        }

        public IEnumerable<Type> GetAllCommandClasses(string commandPostfix = "command", string assemblyPrefix = "corecmd.")
        {
            var allTypeLists = new List<List<Type>>();

            _commandClassLoader.LoadFromEntry(allTypeLists, commandPostfix);
            _commandClassLoader.LoadFromCoreCmd(allTypeLists, commandPostfix);
            _commandClassLoader.LoadFromCurrentDir(allTypeLists, assemblyPrefix, commandPostfix);
            _commandClassLoader.LoadFromAdditionalAssemblies(allTypeLists, commandPostfix, additionalAssemblies);

            return allTypeLists.SelectMany(r => r);
        }
    }

    class CommandClassLoader
    {
        IAssemblyCommandFinder _assemblyCommandFinder = new AssemblyCommandFinder();
        IAssemblyFinder _assemblyFinder = new AssemblyFinder();

        public void LoadFromEntry(List<List<Type>> lists, string postfix)
        {
            lists.Add(_assemblyCommandFinder.GetCommandClassTypesFromAssembly(Assembly.GetEntryAssembly(), postfix));
        }

        public void LoadFromCoreCmd(List<List<Type>> lists, string postfix)
        {
            var coreCmdAssembly = Assembly.GetAssembly(typeof(AssemblyCommandExecutor));
            string entryAssemblyFileName = Assembly.GetEntryAssembly().GetName().Name;

            // make sure the entry assembly is not CoreCmd.dll itself, otherwise the commands in CoreCmd.dll will be printed twice
            if (!entryAssemblyFileName.Equals(coreCmdAssembly.GetName().Name))
                lists.Add(_assemblyCommandFinder.GetCommandClassTypesFromAssembly(coreCmdAssembly, postfix));
        }

        public void LoadFromCurrentDir(List<List<Type>> lists, string assemblyPrefix, string commandPostfix)
        {
            // the assemblies in the current dir need to match some naming pattern
            var dlls = _assemblyFinder.GetCommandAssembly(Directory.GetCurrentDirectory(), assemblyPrefix);
            foreach (var dll in dlls)
                lists.Add(_assemblyCommandFinder.GetCommandClassTypesFromAssembly(dll, commandPostfix));
        }

        public void LoadFromAdditionalAssemblies(List<List<Type>> lists, string commandPostfix, List<Assembly> additionalAssemblies)
        {
            foreach (var additioal in additionalAssemblies)
                lists.Add(_assemblyCommandFinder.GetCommandClassTypesFromAssembly(additioal, commandPostfix));
        }
    }
}
