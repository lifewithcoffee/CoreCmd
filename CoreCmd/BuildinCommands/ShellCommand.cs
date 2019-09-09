using CoreCmd.Attributes;
using CoreCmd.CommandExecution;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCmd.BuildinCommands
{
    [Help("Enable interactive mode")]
    [Alias("sh")]
    class ShellCommand
    {
        /// <summary>
        /// Enter interactive shell mode
        /// </summary>
        public void Default()
        {
            while (true)
            {
                Console.Write(":> ");
                var input = Console.ReadLine().ToLower().Trim();

                if (input == "exit")
                    break;
                else
                {
                    var executor = new AssemblyCommandExecutor();
                    if (string.IsNullOrEmpty(input))
                        executor.Execute(new string[] { } );    // will print all available commands
                    else
                        executor.Execute(input.Split());
                }
            }
        }
    }
}
