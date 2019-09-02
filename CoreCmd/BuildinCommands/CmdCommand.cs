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
        public void Default()
        {
            this.List();
        }

        public void Add(string filename, bool local=false)
        {
            IConfigOperator _configOperator = new ConfigOperator();
            _configOperator.AddCommandAssembly(filename);
            _configOperator.SaveChanges();

        }

        public void List()
        {
            Console.WriteLine("Cmd.List() called");
        }

        public void Remove(int[] dllIds)
        {
            Console.WriteLine("Cmd.Remove() called");
        }

        public void Disable(int[] dllIds)
        {
            Console.WriteLine("Cmd.Disable() called");
        }

        //public void Test(string name, string param1 = "param1", string param2 = "param2", double value = 100.25)
        //{
        //    Console.WriteLine($"Cmd.Test() called: name1={name}, param1={param1} ,param2={param2}, value={value}");
        //}
    }
}
