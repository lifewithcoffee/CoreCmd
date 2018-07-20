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
    }

    public class AssemblyCommandExecutorTests
    {
        static public HitCounter HitCounter { get; set; } = new HitCounter();

        [Fact]
        public void Do_test()
        {
            HitCounter.ResetDict();
            var executor = new AssemblyCommandExecutor(typeof(AssemblyCommandExecutorTests));

            executor.Execute(new string[] { "asb-dummy", "foo-bar1" });
            Assert.Equal(1, HitCounter.GetHitCount("1"));

            executor.Execute(new string[] { "asb-dummy", "foo-bar2", "hello" });
            Assert.Equal(1, HitCounter.GetHitCount("21"));

            executor.Execute(new string[] { "asb-dummy", "foo-bar2", "hello", "123"});
            Assert.Equal(1, HitCounter.GetHitCount("22"));
        }
    }
}
