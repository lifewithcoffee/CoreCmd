using CoreCmd.CommandExecution;
using System;

namespace CoreProject
{
    class Program
    {
        static void Main(string[] args)
        {
            new AssemblyCommandExecutor().Execute(args);
        }
    }
}
