using CoreCmd.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCmd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CommandExecutor executor = new CommandExecutor();
            executor.Execute(args);            
        }
    }
}
