using CoreCmd;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            new CommandExecutor().Execute(args);
        }
    }
}
