using CoreCmd.CommandExecution;
using System;
using Xunit;
using Xunit.Abstractions;

namespace CoreCmd.XunitTest
{
    class DummyCommand
    {
        public void FooFoo(int p1, string p2)
        {
            Console.WriteLine("FooFoo_1() called");
        }

        public void FooFoo(double p1, string p2)
        {
            Console.WriteLine("FooFoo_2() called");
        }

        public void FooFoo(double p1, string p2, int p3)
        {
            Console.WriteLine("FooFoo_3() called");
        }
    }

    public class TargetCommandTests
    {
        private readonly ITestOutputHelper output;
        public TargetCommandTests(ITestOutputHelper output) { this.output = output; }

        [Fact]
        public void Execute_should_match_parameter_types_as_well()
        {
            var cmd = new TargetCommand { CommandType = typeof(DummyCommand), MethodSubcommand = "foo-foo"};

            cmd.Parameters = new string[] { "1", "hello" };
            Assert.Equal(2, cmd.Execute());

            cmd.Parameters = new string[] { "1.2", "hello" };
            Assert.Equal(1, cmd.Execute());

            cmd.Parameters = new string[] { "3.3", "hello", "9" };
            Assert.Equal(1, cmd.Execute());

            cmd.Parameters = new string[] { "hello", "hello" };
            Assert.Equal(0, cmd.Execute());
        }
    }
}
