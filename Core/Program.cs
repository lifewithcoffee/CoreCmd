using CoreCmd.CommandExecution;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Core
{
    class Program
    {
        static void Main(string[] args)
        {
            new AssemblyCommandExecutor("corecmd").Execute(args); // specify the config file name to be "corecmd.config.xml"
        }
    }
}
