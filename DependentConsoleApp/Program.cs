using CoreCmd;
using CoreCmd.CommandExecution;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DependentConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new AssemblyCommandExecutor().ExecuteAsync(args, services => {
                services.AddScoped<IGreeting, Greeting>();  // see demo at: GoodMorningCommand.Greet();
            });
        }
    }
}
