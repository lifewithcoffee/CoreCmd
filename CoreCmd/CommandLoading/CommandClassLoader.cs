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
        void LoadFromEntry(List<List<Type>> lists, string postfix);
        void LoadFromCoreCmd(List<List<Type>> lists, string postfix);
        void LoadFromCurrentDir(List<List<Type>> lists, string assemblyPrefix, string commandPostfix);
        void LoadFromAdditionalAssemblies(List<List<Type>> lists, string commandPostfix, List<Assembly> additionalAssemblies);
        IEnumerable<Type> LoadAllCommandClasses(List<Assembly> additionalAssemblies);
    }

    class CommandClassLoader : ICommandClassLoader
    {
        IAssemblyCommandFinder _assemblyCommandFinder = new AssemblyCommandFinder();

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
            IAssemblyFinder _assemblyFinder = new AssemblyFinder();
            IAssemblyLoadable _assemblyLoadable = new AssemblyLoadable();

            var dlls = _assemblyFinder.GetCommandAssembly(Directory.GetCurrentDirectory(), assemblyPrefix);
            foreach (var dll in dlls)
            {
                if (!_assemblyLoadable.FindConflict(lists.SelectMany(c => c), dll))
                    lists.Add(_assemblyCommandFinder.GetCommandClassTypesFromAssembly(dll, commandPostfix));
            }
        }

        public void LoadFromAdditionalAssemblies(List<List<Type>> lists, string commandPostfix, List<Assembly> additionalAssemblies)
        {
            if( additionalAssemblies != null )
            {
                foreach (var additioal in additionalAssemblies)
                    lists.Add(_assemblyCommandFinder.GetCommandClassTypesFromAssembly(additioal, commandPostfix));
            }
        }

        public IEnumerable<Type> LoadAllCommandClasses(List<Assembly> additionalAssemblies)
        {
            string commandPostfix = GlobalConsts.CommandPostFix;
            string assemblyPrefix = GlobalConsts.AssemblyPrefix;

            var allTypeLists = new List<List<Type>>();

            this.LoadFromEntry(allTypeLists, commandPostfix);
            this.LoadFromCoreCmd(allTypeLists, commandPostfix);
            this.LoadFromAdditionalAssemblies(allTypeLists, commandPostfix, additionalAssemblies);
            this.LoadFromCurrentDir(allTypeLists, assemblyPrefix, commandPostfix);

            return allTypeLists.SelectMany(r => r);
        }
    }
}
