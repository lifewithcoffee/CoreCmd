using CoreCmd.CommandExecution;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            new AssemblyCommandExecutor().Execute(args);
        }
    }
}
