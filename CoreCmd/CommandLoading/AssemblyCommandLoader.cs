using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoreCmd.CommandLoading
{
    interface IAssemblyCommandLoader
    {
        List<Type> GetCommandClassTypesFromAssembly(Assembly assembly, string commandPostfix);
        List<Type> GetCommandClassTypesFromAssembly(string dll, string commandPostfix);
    }

    class AssemblyCommandLoader : IAssemblyCommandLoader
    {
        public List<Type> GetCommandClassTypesFromAssembly(Assembly assembly, string commandPostfix)
        {
            var result = new List<Type>();

            if (assembly != null)
                result = assembly.GetTypes().Where(t => t.IsClass && t.Name.ToLower().EndsWith(commandPostfix)).ToList();
            else
                Console.WriteLine("Assembly is not specified");

            return result;
        }

        public List<Type> GetCommandClassTypesFromAssembly(string dll, string commandPostfix)
        {
            try
            {
                return GetCommandClassTypesFromAssembly(Assembly.LoadFrom(dll), commandPostfix);  // use LoadFrom() instead of LoadFile() as the former can also load the dependent dlls
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(ex.Message);
                return new List<Type>();
            }
        }
    }
}
