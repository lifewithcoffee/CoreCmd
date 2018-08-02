using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExperimentalConsoleApp.Commands
{
    public class MyTestCommand
    {
        public void Default()
        {
            Console.WriteLine("MyTestCommand.Default() is called");
        }

        public void MyTestMethod(string param1, double param2)
        {
            Console.WriteLine("MyTestMethod: param1 = {0}; param2 = {1}", param1, param2);
        }
    }
}
