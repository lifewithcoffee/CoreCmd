using CoreCmd.Attributes;
using CoreCmd.BuiltinCommands;
using CoreCmd.CommandExecution;
using CoreCmd.XunitTest.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoreCmd.XunitTest
{
    public class AsbDummyCommand
    {
        [OptionalParam("Something", typeof(int), "aa", "bb", "cc")]
        public void FooBar1() { AssemblyCommandExecutorTests.HitCounter.Hit(nameof(FooBar1)); }

        [OptionalParam("Something", typeof(int))]
        [OptionalParam("Something", typeof(int))]
        public void FooBar2() { AssemblyCommandExecutorTests.HitCounter.Hit(nameof(FooBar2)); }
    }

    public class AssemblyCommandExecutorTests
    {
        static public HitCounter HitCounter { get; set; } = new HitCounter();

        [Fact]
        public void Do_test()
        {
            HitCounter.ResetDict();

            string[] args = { "asb-dummy", "foo-bar1" };
            new AssemblyCommandExecutor(typeof(AsbDummyCommand)).Execute(args);
            Assert.Equal(1, HitCounter.GetHitCount(nameof(AsbDummyCommand.FooBar1)));
        }
    }
}
