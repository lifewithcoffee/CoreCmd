using CoreCmd;
using CoreCmd.CommandExecution;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DependentConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            new AssemblyCommandExecutor().Execute(args, services => {
                services.AddScoped<IGreeting, Greeting>();  // see demo at: GoodMorningCommand.Greet();
            });
        }
    }
}
