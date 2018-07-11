using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

namespace CoreCmd
{
    interface ICommandFinder
    {
        IEnumerable<Type> GetCommandClassTypes(string assemblyPrefix = "command", string commandPostfix = "command");
    }

    class CommandFinder : ICommandFinder
    {
        private string[] GetCommandAssembly(string dir, string assemblyPrefix)
        {

            var targetDlls = new DirectoryInfo(dir).EnumerateFiles()
                                .Where(f => f.Name.ToLower().StartsWith(assemblyPrefix) && f.Extension.ToLower().Equals(".dll"))
                                .OrderBy(f => f.Name)
                                .Select(f => f.FullName)
                                .ToArray();
            return targetDlls;
        }

        private List<Type> GetCommandClassTypesFromAssembly(Assembly assembly, string commandPostfix)
        {
            if (assembly != null)
            {
                List<Type> types = assembly.GetTypes().Where(t => t.IsClass && t.Name.ToLower().EndsWith(commandPostfix)).ToList();
                if (types.Count() < 1)
                    return null;
                else
                    return types;
            }
            else
            {
                Console.WriteLine("Assembly is not specified");
                return null;
            }
        }

        private List<Type> GetCommandClassTypesFromAssembly(string dll, string commandPostfix)
        {
            return GetCommandClassTypesFromAssembly(Assembly.LoadFile(dll), commandPostfix);
        }

        public IEnumerable<Type> GetCommandClassTypes(string commandPostfix = "command", string assemblyPrefix = "command")
        {
            var allTypeList = new List<List<Type>>();
            allTypeList.Append(GetCommandClassTypesFromAssembly(Assembly.GetEntryAssembly(), commandPostfix));

            var dlls = this.GetCommandAssembly(Directory.GetCurrentDirectory(), assemblyPrefix);
            foreach (var dll in dlls)
                allTypeList.Append(GetCommandClassTypesFromAssembly(dll, commandPostfix));

            return allTypeList.SelectMany(r => r);
        }
    }
}
