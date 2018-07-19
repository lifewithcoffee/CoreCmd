using CoreCmd.BuiltinCommands;
using CoreCmd.CommandExecution;
using CoreCmd.XunitTest.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace CoreCmd.XunitTest
{
    public class CommandHelpPrinterTests
    {
        const string dummyCmd2HelpText = "This is the help info for DummyCommand2";
        const string foo1HelpText = "This is the help info for DummyCommand2.Foo1()";
        const string foo2HelpText = "This is the help info for DummyCommand2.Foo2()";

        [Help(dummyCmd2HelpText)]
        class DummyCommand2
        {
            [Help(foo1HelpText)]
            public void FooBar1(string hello, int num=0) { }

            [Help(foo2HelpText)]
            public int FooBarBo2() { return 123; }
        }

        private readonly ITestOutputHelper output;
        public CommandHelpPrinterTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Do_test()
        {
            var xunitTestOutputWriter = new XunitTestOutputWriter(output);
            Console.SetOut(xunitTestOutputWriter);

            var printer = new CommandHelpPrinter();
            printer.PrintClassHelp(typeof(DummyCommand2));
            printer.PrintAllMethodHelp(typeof(DummyCommand2));
        }
    }
}
