using CoreCmd.CommandExecution;
using CoreCmd.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoreCmd.CommandLoading
{
    interface ICommandClassLoader
    {
        IEnumerable<Type> LoadAllCommandClasses(List<Assembly> additionalAssemblies);
    }

    class CommandClassLoader : ICommandClassLoader
    {
        IAssemblyCommandFinder _assemblyCommandFinder = new AssemblyCommandFinder();

        public IEnumerable<Type> LoadAllCommandClasses(List<Assembly> additionalAssemblies)
        {
            string commandPostfix = GlobalConsts.CommandPostFix;
            string assemblyPrefix = GlobalConsts.AssemblyPrefix;

            var result = new List<Type>();

            // note: the order matters, it's by design
            this.LoadFromEntry(result, commandPostfix);
            this.LoadFromCoreCmd(result, commandPostfix);
            this.LoadFromAdditionalAssemblies(result, commandPostfix, additionalAssemblies);
            this.LoadFromCurrentDir(result, assemblyPrefix, commandPostfix);
            this.LoadFromGlobalConfig(result);

            return result;
        }

        private void LoadFromEntry(List<Type> lists, string postfix)
        {
            var entryCmds = _assemblyCommandFinder.GetCommandClassTypesFromAssembly(Assembly.GetEntryAssembly(), postfix);
            this.AddCommandsIfNotExist(lists, entryCmds);
        }

        private void LoadFromCoreCmd(List<Type> existing, string postfix)
        {
            var coreCmdAssembly = Assembly.GetAssembly(typeof(AssemblyCommandExecutor));
            string entryAssemblyFileName = Assembly.GetEntryAssembly().GetName().Name;

            // make sure the entry assembly is not CoreCmd.dll itself, otherwise the commands in CoreCmd.dll will be printed twice
            if (!entryAssemblyFileName.Equals(coreCmdAssembly.GetName().Name))
            {
                var more = _assemblyCommandFinder.GetCommandClassTypesFromAssembly(coreCmdAssembly, postfix);
                this.AddCommandsIfNotExist(existing, more);
            }
        }

        private void LoadFromAdditionalAssemblies(List<Type> existing, string commandPostfix, List<Assembly> additionalAssemblies)
        {
            if (additionalAssemblies != null)
            {
                foreach (var additional in additionalAssemblies)
                {
                    var more = _assemblyCommandFinder.GetCommandClassTypesFromAssembly(additional, commandPostfix);
                    this.AddCommandsIfNotExist(existing, more);
                }
            }
        }

        private void LoadFromCurrentDir(List<Type> existing, string assemblyPrefix, string commandPostfix)
        {
            IAssemblyFinder _assemblyFinder = new AssemblyFinder();

            var dlls = _assemblyFinder.GetCommandAssembly(Directory.GetCurrentDirectory(), assemblyPrefix);
            foreach (var dll in dlls)
            {
                var more = _assemblyCommandFinder.GetCommandClassTypesFromAssembly(dll, commandPostfix);
                this.AddCommandsIfNotExist(existing, more);
            }
        }

        private void LoadFromGlobalConfig(List<Type> existing)
        {
            IConfigOperator _configOperator = new ConfigOperator();

            var dlls = _configOperator.ListCommandAssemblies();
            if(dlls != null)
            {
                foreach(var dll in dlls)
                {
                    var more = _assemblyCommandFinder.GetCommandClassTypesFromAssembly(dll, GlobalConsts.CommandPostFix);
                    this.AddCommandsIfNotExist(existing, more);
                }
            }
        }

        private void AddCommandsIfNotExist(List<Type> existingCmds, List<Type> newCmds)
        {
            var existingNames = existingCmds.Select(c => c.Name).ToList();
            var newNames = newCmds.Select(c => (c.Name, c)).ToList();

            foreach(var (name,cmd) in newNames)
            {
                if (!existingNames.Contains(name))
                    existingCmds.Add(cmd);
            }
        }
    }
}
