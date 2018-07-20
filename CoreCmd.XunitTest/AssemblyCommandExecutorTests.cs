using CoreCmd.Attributes;
using CoreCmd.BuiltinCommands;
using CoreCmd.CommandExecution;
using CoreCmd.XunitTest.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace CoreCmd.XunitTest
{
    public class AsbDummyCommand
    {
        [OptionalParam("Something", typeof(int), "aa", "bb", "cc")]
        public void FooBar1() { AssemblyCommandExecutorTests.HitCounter.Hit("1"); }

        [OptionalParam("Something", typeof(int))]
        [OptionalParam("Something", typeof(int))]
        public void FooBar2(string str) { AssemblyCommandExecutorTests.HitCounter.Hit("21"); }
        public void FooBar2(string str, int num) { AssemblyCommandExecutorTests.HitCounter.Hit("22"); }
        public void FooBar2(int num, string str) { AssemblyCommandExecutorTests.HitCounter.Hit("23"); }

        public void FooBar3(string str, int num1=1, int num2=2) { AssemblyCommandExecutorTests.HitCounter.Hit("3"); }
    }

    public class AssemblyCommandExecutorTests
    {
        static public HitCounter HitCounter { get; set; } = new HitCounter();
        AssemblyCommandExecutor executor = new AssemblyCommandExecutor(typeof(AssemblyCommandExecutorTests));

        public AssemblyCommandExecutorTests()
        {
            HitCounter.ResetDict();
        }

        [Fact]
        public void Basic_usage()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar1" });
            Assert.Equal(1, HitCounter.GetHitCount("1"));
        }

        [Fact]
        public void Method_overloading_1()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar2", "hello" });
            Assert.Equal(1, HitCounter.GetHitCount("21"));
            Assert.Equal(0, HitCounter.GetHitCount("22"));
            Assert.Equal(0, HitCounter.GetHitCount("23"));
        }

        [Fact]
        public void Method_overloading_2()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar2", "hello", "123" });
            Assert.Equal(0, HitCounter.GetHitCount("1"));
            Assert.Equal(0, HitCounter.GetHitCount("21"));
            Assert.Equal(1, HitCounter.GetHitCount("22"));
            Assert.Equal(0, HitCounter.GetHitCount("23"));
        }

        [Fact]
        public void Method_with_default_values_1()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar3", "hello" });
            Assert.Equal(1, HitCounter.GetHitCount("3"));
        }

        [Fact]
        public void Method_with_default_values_2()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar3", "hello", "123" });
            Assert.Equal(1, HitCounter.GetHitCount("3"));
        }

        [Fact]
        public void Method_with_default_values_3()
        { 
            executor.Execute(new string[] { "asb-dummy", "foo-bar3", "hello", "123", "456"});
            Assert.Equal(1, HitCounter.GetHitCount("3"));
        }
    }
}
