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
        TargetCommandObject GetTargetCommandObject(IEnumerable<Type> targetTypes, string[] args);
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
            var result = new List<Type>();

            if (assembly != null)
                result = assembly.GetTypes().Where(t => t.IsClass && t.Name.ToLower().EndsWith(commandPostfix)).ToList();
            else
                Console.WriteLine("Assembly is not specified");

            return result;
        }

        private List<Type> GetCommandClassTypesFromAssembly(string dll, string commandPostfix)
        {
            return GetCommandClassTypesFromAssembly(Assembly.LoadFile(dll), commandPostfix);
        }

        public TargetCommandObject GetTargetCommandObject(IEnumerable<Type> targetTypes, string[] args)
        {
            TargetCommandObject result = null;
            if (args.Length > 0)
            {
                result = new TargetCommandObject();
                string command = $"{args[0]}-command".ToLower();

                Type targetType = targetTypes.SingleOrDefault(t => Utils.LowerKebabCase(t.Name).Equals(command));
                if (targetType != null)
                {
                    result.CommandType = targetType;
                    if (args.Length > 1)
                    {
                        result.MethodName = args[1];
                        result.Parameters = args.Skip(2).ToArray();
                    }
                    else
                        result.MethodName = "default-method";
                }
                else
                {
                    result.CommandType = targetTypes.SingleOrDefault(t => t.Name.Equals("DefaultCommand"));
                    result.MethodName = args[0];
                    result.Parameters = args.Skip(1).ToArray();
                }
            }
            return result;
        }

        public IEnumerable<Type> GetCommandClassTypes(string commandPostfix = "command", string assemblyPrefix = "command")
        {
            var allTypeList = new List<List<Type>>();
            allTypeList.Add(GetCommandClassTypesFromAssembly(Assembly.GetEntryAssembly(), commandPostfix)); // the console app itself's assembly
            allTypeList.Add(GetCommandClassTypesFromAssembly(Assembly.GetAssembly(typeof(CommandExecutor)),commandPostfix));    // the dependent CoreCmd package's assembly

            var dlls = this.GetCommandAssembly(Directory.GetCurrentDirectory(), assemblyPrefix);    // the assemblies in the current dir match some naming pattern
            foreach (var dll in dlls)
                allTypeList.Add(GetCommandClassTypesFromAssembly(dll, commandPostfix));

            return allTypeList.SelectMany(r => r);
        }
    }
}
