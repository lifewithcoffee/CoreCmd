using CoreCmd.CommandExecution;
using System;

namespace CoreCmd.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            new AssemblyCommandExecutor("corecmd").Execute(args); // specify the config file name to be "corecmd.config.xml"
        }
    }
}
