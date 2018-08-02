using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace CoreCmd.BuildinCommands
{
    class VersionCommand
    {
        public void Default()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string version= FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            Console.WriteLine(assembly.Location);
            Console.WriteLine(version);
        }
    }
}
