using CoreCmd.Attributes;
using CoreCmd.Help;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace CoreCmd.BuiltinCommands
{
    [Alias("ver")]
    class VersionCommand
    {
        private void PrintAssemblyInfo(Assembly assembly)
        {
            string version= FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            Console.WriteLine(assembly.Location);
            Console.WriteLine(version);
        }

        public void Default()
        {
            PrintAssemblyInfo(Assembly.GetEntryAssembly());
            Console.WriteLine("");
            PrintAssemblyInfo(Assembly.GetExecutingAssembly());
        }
    }
}
