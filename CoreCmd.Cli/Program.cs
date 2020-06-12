using CoreCmd.CommandExecution;
using System;
using System.Threading.Tasks;

namespace CoreCmd.Cli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new AssemblyCommandExecutor("corecmd").ExecuteAsync(args); // specify the config file name to be "corecmd.config.xml"
        }
    }
}
