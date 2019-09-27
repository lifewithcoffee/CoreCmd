using CoreCmd.CommandExecution;
using System;

namespace CoreProject
{
    class HelloCommand
    {
        public void Ben()
        {
            Console.WriteLine("Ben() is called");
        }
    }

    class GoodMorningCommand
    {
        public void HarryPotter(string param1, int param2)
        {
            Console.WriteLine($"HarryPotter() is called, param1={param1}, param2={param2}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            new AssemblyCommandExecutor().Execute(args);
        }
    }
}
