using System;
using Xunit;
using Xunit.Abstractions;

namespace CoreCmd.XunitTest
{
    public class TargetCommandObjectTests
    {
        private readonly ITestOutputHelper output;
        public TargetCommandObjectTests(ITestOutputHelper output) { this.output = output; }

        //[Theory]
        //[InlineData(-1)]
        //[InlineData(0)]
        //[InlineData(1)]
        //public void Test1(int n)
        //{
        //    Assert.False(true, $"n={n}");
        //}

        [Fact]
        public void Test1()
        {
            var cmd = new TargetCommandObject { };
        }
    }
}
