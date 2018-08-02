using System;

namespace CommandsInSeparateDll
{
    public class GreetingCommand
    {
        public void Default()
        {
            Console.WriteLine("GreetingCommand.Default() called");
        }

        public void SayHello()
        {
            Console.WriteLine("GreetingCommand.SayHello() called");
        }

        public void Hello(string param1, double param2)
        {
            Console.WriteLine($"GreetingCommand.Hello({param1},{param2}) called");
        }
    }
}
