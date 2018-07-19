﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoreCmd.CommandFind
{
    interface IAssemblyCommandFinder
    {
        List<Type> GetCommandClassTypesFromAssembly(Assembly assembly, string commandPostfix);
        List<Type> GetCommandClassTypesFromAssembly(string dll, string commandPostfix);
    }

    class AssemblyCommandFinder : IAssemblyCommandFinder
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
            return GetCommandClassTypesFromAssembly(Assembly.LoadFile(dll), commandPostfix);
        }
    }
}
