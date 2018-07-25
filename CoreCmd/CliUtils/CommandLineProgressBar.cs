using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCmd.CliUtils
{
    public class CommandLineProgressBar
    {
        private string bar;
        public bool Finished { get; set; } = false;

        public CommandLineProgressBar(int length = 10)
        {
            bar = string.Format("[{0}]", new string(' ', length));
            Console.Write(bar);
            Console.CursorLeft = 1;
        }

        public void Report()
        {
            if (!Finished)
            {
                Console.Write("=");
                if (Console.CursorLeft > bar.Length - 2)
                {
                    Finished = true;
                    Console.WriteLine("");
                }
            }
        }
    }
}
