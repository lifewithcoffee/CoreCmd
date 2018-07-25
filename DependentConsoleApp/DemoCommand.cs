using CoreCmd.CliUtils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DependentConsoleApp
{
    class DemoCommand
    {
        public void ProgressBar()
        {
            Console.WriteLine("Processing...");

            var progressBar = new CommandLineProgressBar(25);
            do
            {
                Thread.Sleep(100);
                progressBar.Report();
            }
            while (!progressBar.Finished);
            Console.WriteLine("Done!");
        }
    }
}
