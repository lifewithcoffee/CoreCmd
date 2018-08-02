using CoreCmd.CommandExecution;
using CoreCmd.XunitTest.TestUtils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AssemblyCommandExecutorTests
{
    public class DefaultMethodTestCommand
    {
        public void Default() { DefaultMethodTests.HitCounter.Hit("1"); } 
        public void Default(string p) { DefaultMethodTests.HitCounter.Hit("2"); } 
        public void Default(int p) { DefaultMethodTests.HitCounter.Hit("3"); } 
    }

    public class DefaultMethodTests
    {
        static public HitCounter HitCounter { get; set; } = new HitCounter();
        AssemblyCommandExecutor executor = new AssemblyCommandExecutor(typeof(DefaultMethodTests));

        public DefaultMethodTests() { HitCounter.ResetDict(); }

        [Fact]
        public void No_parameter()
        {
            executor.Execute(new string[] { "default-method-test" });
            Assert.Equal(1, HitCounter.GetHitCount("1"));
            Assert.Equal(0, HitCounter.GetHitCount("2"));
            Assert.Equal(0, HitCounter.GetHitCount("3"));
        }

        [Fact]
        public void Int_parameter()
        {
            executor.Execute(new string[] { "default-method-test", "1" });
            Assert.Equal(0, HitCounter.GetHitCount("1"));
            Assert.Equal(1, HitCounter.GetHitCount("2"));
            Assert.Equal(1, HitCounter.GetHitCount("3"));   // because "1" can be matched to string as well
        }

        [Fact]
        public void String_parameter()
        {
            executor.Execute(new string[] { "default-method-test", "abc" });
            Assert.Equal(0, HitCounter.GetHitCount("1"));
            Assert.Equal(1, HitCounter.GetHitCount("2"));
            Assert.Equal(0, HitCounter.GetHitCount("3"));
        }
    }
}
