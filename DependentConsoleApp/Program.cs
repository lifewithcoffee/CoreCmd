using CoreCmd;
using CoreCmd.CommandExecution;
using System;

namespace DependentConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            new CommandExecutor().Execute(args);
        }
    }
}
