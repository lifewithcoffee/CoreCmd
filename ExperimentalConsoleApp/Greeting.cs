using System;
using System.Collections.Generic;
using System.Text;

namespace ExperimentalConsoleApp
{
    class Greeting
    {
        public void Hello(string param1, string param2)
        {
            Console.WriteLine($"Greeting.Hello({param1},{param2}) called");
        }
    }
}
