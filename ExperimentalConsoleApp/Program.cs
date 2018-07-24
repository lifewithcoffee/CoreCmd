using CommandsInSeparateDll;
using CoreCmd;
using CoreCmd.CommandExecution;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExperimentalConsoleApp
{
    class Program
    {
        static IAssemblyCommandExecutor CommandExecutor = new AssemblyCommandExecutor();
        static void Main(string[] args)
        {
            CommandExecutor.Execute(args);
        }
    }
}
