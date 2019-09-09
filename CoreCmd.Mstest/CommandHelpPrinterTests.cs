using CoreCmd.Attributes;
using CoreCmd.BuildinCommands;
using CoreCmd.CommandExecution;
using CoreCmd.Help;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CoreCmd.XunitTest
{
    [TestClass]
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
            public void FooBarBo3(double a, double b) { }
            public void FooBarBo4(float a, string b) { }
        }


        [TestMethod]
        public void Do_test()
        {
            var helpInfo = new HelpInfoService();
            helpInfo.PrintClassHelp(typeof(DummyCommand2));
            helpInfo.PrintAllMethodHelp(typeof(DummyCommand2));
        }
    }
}
