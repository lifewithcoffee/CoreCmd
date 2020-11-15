using CoreCmd.Attributes;
using CoreCmd.CliUtils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DependentConsoleApp
{
    class DemoCommandBase
    {
        public void Fn1()
        {
            Console.WriteLine("DemoCommandBase.Fn1() called");
        }
    }

    class DemoCommand : DemoCommandBase
    {
        [Help("Demonstrate console progress bar")]
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
