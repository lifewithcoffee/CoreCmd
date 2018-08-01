using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCmd.BuildinCommands
{
    class CmdCommand
    {
        public void DefaultMethod()
        {
            this.List();
        }

        public void Add(string dllPath, bool local=false)
        {
            var userDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Console.WriteLine($"userDir: {userDir}");
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
    }
}
