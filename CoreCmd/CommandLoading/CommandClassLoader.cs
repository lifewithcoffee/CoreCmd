using CoreCmd.CommandExecution;
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
        void LoadFromEntry(List<Type> lists, string postfix);
        void LoadFromCoreCmd(List<Type> lists, string postfix);
        void LoadFromCurrentDir(List<Type> lists, string assemblyPrefix, string commandPostfix);
        void LoadFromAdditionalAssemblies(List<Type> lists, string commandPostfix, List<Assembly> additionalAssemblies);
        IEnumerable<Type> LoadAllCommandClasses(List<Assembly> additionalAssemblies);
    }

    class CommandClassLoader : ICommandClassLoader
    {
        IAssemblyCommandFinder _assemblyCommandFinder = new AssemblyCommandFinder();

        public void LoadFromEntry(List<Type> lists, string postfix)
        {
            var entryCmds = _assemblyCommandFinder.GetCommandClassTypesFromAssembly(Assembly.GetEntryAssembly(), postfix);
            this.AddCommandsIfNotExist(lists, entryCmds);
        }

        public void LoadFromCoreCmd(List<Type> lists, string postfix)
        {
            var coreCmdAssembly = Assembly.GetAssembly(typeof(AssemblyCommandExecutor));
            string entryAssemblyFileName = Assembly.GetEntryAssembly().GetName().Name;

            // make sure the entry assembly is not CoreCmd.dll itself, otherwise the commands in CoreCmd.dll will be printed twice
            if (!entryAssemblyFileName.Equals(coreCmdAssembly.GetName().Name))
                this.AddCommandsIfNotExist(lists, _assemblyCommandFinder.GetCommandClassTypesFromAssembly(coreCmdAssembly, postfix));
        }

        public void LoadFromAdditionalAssemblies(List<Type> lists, string commandPostfix, List<Assembly> additionalAssemblies)
        {
            if (additionalAssemblies != null)
            {
                foreach (var additional in additionalAssemblies)
                    this.AddCommandsIfNotExist(lists, _assemblyCommandFinder.GetCommandClassTypesFromAssembly(additional, commandPostfix));
            }
        }

        public void LoadFromCurrentDir(List<Type> lists, string assemblyPrefix, string commandPostfix)
        {
            var dlls = new AssemblyFinder().GetCommandAssembly(Directory.GetCurrentDirectory(), assemblyPrefix);
            foreach (var dll in dlls)
                this.AddCommandsIfNotExist(lists, _assemblyCommandFinder.GetCommandClassTypesFromAssembly(dll, commandPostfix));
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

        public IEnumerable<Type> LoadAllCommandClasses(List<Assembly> additionalAssemblies)
        {
            string commandPostfix = GlobalConsts.CommandPostFix;
            string assemblyPrefix = GlobalConsts.AssemblyPrefix;

            var allTypeLists = new List<Type>();

            // note: the order matters, it's by design
            this.LoadFromEntry(allTypeLists, commandPostfix);
            this.LoadFromCoreCmd(allTypeLists, commandPostfix);
            this.LoadFromAdditionalAssemblies(allTypeLists, commandPostfix, additionalAssemblies);
            this.LoadFromCurrentDir(allTypeLists, assemblyPrefix, commandPostfix);

            return allTypeLists;
        }
    }
}
