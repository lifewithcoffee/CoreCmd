using CoreCmd.CommandExecution;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CoreCmd.XunitTest
{
    class DummyCommand
    {
        static public int State { get; set; } = 0;

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

        public async Task FooFoo(int param)
        {
            State = 0;
            await Task.Delay(1000);
            State = 1234;
        }
    }

    public class SingleCommandExecutorTests
    {
        private readonly ITestOutputHelper output;
        public SingleCommandExecutorTests(ITestOutputHelper output) { this.output = output; }

        [Fact]
        public async Task ExecutAsync_without_DI_should_match_parameter_types_as_well()
        {
            var cmd = new SingleCommandExecutor { CommandClassType = typeof(DummyCommand), MethodSubcommand = "foo-foo"};

            cmd.Parameters = new string[] { "1", "hello" };
            Assert.Equal(2, await cmd.ExecuteAsync(null));

            cmd.Parameters = new string[] { "1.2", "hello" };
            Assert.Equal(1, await cmd.ExecuteAsync(null));

            cmd.Parameters = new string[] { "3.3", "hello", "9" };
            Assert.Equal(1, await cmd.ExecuteAsync(null));

            cmd.Parameters = new string[] { "hello", "hello" };
            Assert.Equal(0, await cmd.ExecuteAsync(null));

            cmd.Parameters = new string[] { "123" };
            Assert.Equal(1, await cmd.ExecuteAsync(null));
            Assert.Equal(1234, DummyCommand.State);
        }
    }
}
