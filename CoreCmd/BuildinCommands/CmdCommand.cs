using CoreCmd.Config;
using NetCoreUtils.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreCmd.BuildinCommands
{
    class CmdCommand
    {
        IConfigOperator _configOperator = new ConfigOperator();

        public void Default()
        {
            this.List();
        }

        public void Add(string filename) // need to add a parameter of "bool local=false" as well?
        {
            _configOperator.AddCommandAssembly(filename);
            _configOperator.SaveChanges();
        }

        public void List()
        {
            Console.WriteLine("Registered global command assemblies:");
            var list = _configOperator.ListCommandAssemblies();

            foreach(var dllPath in list)
                Console.WriteLine(dllPath);
        }

        /// <param name="targetDllString">
        /// A partial string of the target DLL's full path.
        /// If multiple DLLs are matched, no DLL will be removed, and a prompt message will be displayed.
        /// </param>
        public void Remove(string targetDllString)
        {
            Console.WriteLine($"Removing a dll containing '{targetDllString}' in its full path.");
            var lowerDllString = targetDllString.ToLower();
            List<string> matchedDll = new List<string>();
            var list = _configOperator.ListCommandAssemblies();
            foreach(var dll in list)
            {
                if (dll.ToLower().Contains(lowerDllString))
                    matchedDll.Add(dll);
            }

            if(matchedDll.Count > 1)
            {
                Console.WriteLine("Multiple DLLs are matched:");
                foreach (var dll in matchedDll)
                    Console.WriteLine($"{GlobalConsts.indentSpaces}{dll}");
                Console.WriteLine("No DLL is removed from the global command registry.");
            }
            else if(matchedDll.Count == 1)
            {
                string targetDll = matchedDll[0];
                _configOperator.RemoveCommandAssembly(targetDll);
                Console.WriteLine($"{targetDll} removed");
                _configOperator.SaveChanges();
            }
            else
                Console.WriteLine($"No DLL is matched for '{targetDllString}'.");
        }

        //public void Disable(int[] dllIds)
        //{
        //    Console.WriteLine("Cmd.Disable() called");
        //}

        //public void Test(string name, string param1 = "param1", string param2 = "param2", double value = 100.25)
        //{
        //    Console.WriteLine($"Cmd.Test() called: name1={name}, param1={param1} ,param2={param2}, value={value}");
        //}
    }
}
