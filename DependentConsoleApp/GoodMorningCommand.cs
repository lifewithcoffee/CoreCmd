using CoreCmd;
using CoreCmd.Attributes;
using System;

namespace DependentConsoleApp
{
    public interface IGreeting
    {
        void Greet();
    }

    public class Greeting : IGreeting
    {
        public void Greet()
        {
            Console.WriteLine("Greeting.Greet() called");
        }
    }

    [Help("This is the help info of command " + nameof(GoodMorningCommand) + ".")]
    public class GoodMorningCommand
    {
        IGreeting _greeting;

        public GoodMorningCommand(IGreeting greeting)
        {
            _greeting = greeting;
        }

        public void Default()
        {
            Console.WriteLine("GreetingCommand.Default() called");
        }

        [Help("This is the help info of method " + nameof(SayHello) + ".")]
        public void SayHello()
        {
            Console.WriteLine("GreetingCommand.SayHello() called");
        }

        [Help("Demo of Dependency Injection")]
        public void Greet()
        {
            _greeting.Greet();
        }

        [Help("This is the help info of method " + nameof(Hello) + ".")]
        public void Hello(string param1, double param2)
        {
            Console.WriteLine($"GreetingCommand.Hello({param1},{param2}) called");
        }
    }
}
