using CoreCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExperimentalConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Experimantal console app start");
            new CommandExecutor().Execute(args);
        }
    }
}
