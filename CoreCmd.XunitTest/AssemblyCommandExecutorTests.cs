using CoreCmd.BuiltinCommands;
using CoreCmd.CommandExecution;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoreCmd.XunitTest
{
    public class HitCounter
    {
        Dictionary<string, int> hitDict = new Dictionary<string, int>();

        public void Hit(string key)
        {
            if (hitDict.ContainsKey(key))
                hitDict[key]++;
            else
                hitDict.Add(key, 1);
        }

        public int GetHitCount(string key)
        {
            if (hitDict.ContainsKey(key))
                return hitDict[key];
            else
                return 0;
        }

        public void ResetDict()
        {
            this.hitDict = new Dictionary<string, int>();
        }
    }

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
