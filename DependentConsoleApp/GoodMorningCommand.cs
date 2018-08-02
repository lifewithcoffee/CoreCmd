﻿using CoreCmd;
using CoreCmd.Help;
using System;

namespace DependentConsoleApp
{
    [Help("This is the help info of command " + nameof(GoodMorningCommand) + ".")]
    public class GoodMorningCommand
    {
        public void Default()
        {
            Console.WriteLine("GreetingCommand.Default() called");
        }

        [Help("This is the help info of method " + nameof(SayHello) + ".")]
        public void SayHello()
        {
            Console.WriteLine("GreetingCommand.SayHello() called");
        }

        [Help("This is the help info of method " + nameof(Hello) + ".")]
        public void Hello(string param1, double param2)
        {
            Console.WriteLine($"GreetingCommand.Hello({param1},{param2}) called");
        }
    }
}
