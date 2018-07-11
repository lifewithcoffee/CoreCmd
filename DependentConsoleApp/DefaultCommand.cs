using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependentConsoleApp
{
    public class DefaultCommand
    {
        public void TestMethod1(string param1, string param2, double number)
        {
            Console.WriteLine("TestMethod1: param1 = {0}, param2 = {1}, number = {2}", param1, param2, number);
        }
    }
}
