using CoreCmd.CommandExecution;
using CoreCmd.XunitTest.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoreCmd.XunitTest
{
    public class DefaultParameterCommand
    {
        static public int Num1 { get; set; } = 0;
        static public int Num2 { get; set; } = 0;
        static public string Code { get; set; }
        static public HitCounter HitCounter { get; set; } = new HitCounter();

        static public void Reset()
        {
            Num1 = 0;
            Num2 = 0;
            Code = "";
        }

        public void FooBar3(string str, int num1=1, int num2=2, string code="apple")
        {
            Num1 = num1;
            Num2 = num2;
            Code = code;

            HitCounter.Hit("3");
        }
    }

    public class AssemblyCommandExecutorTests_DefaultParameters
    {
        AssemblyCommandExecutor executor = new AssemblyCommandExecutor(typeof(AssemblyCommandExecutorTests_DefaultParameters));

        public AssemblyCommandExecutorTests_DefaultParameters()
        {
            DefaultParameterCommand.HitCounter.ResetDict();
        }

        [Fact]
        public void Parameters_with_default_values_mismatching_case()
        {
            DefaultParameterCommand.Reset();

            executor.Execute(new string[] { "default-parameter", "foo-bar3", "hello", "-num1:some-string" });
            Assert.Equal(0, DefaultParameterCommand.HitCounter.GetHitCount("3"));
        }

        [Fact]
        public void Parameters_with_default_values_omit_all_default()
        {
            DefaultParameterCommand.Reset();

            executor.Execute(new string[] { "default-parameter", "foo-bar3", "hello" });
            Assert.Equal(1, DefaultParameterCommand.HitCounter.GetHitCount("3"));
            Assert.Equal(1, DefaultParameterCommand.Num1);
            Assert.Equal(2, DefaultParameterCommand.Num2);
            Assert.Equal("apple", DefaultParameterCommand.Code);
        }

        [Fact]
        public void Parameters_with_default_values_partial_defaults()
        {
            DefaultParameterCommand.Reset();

            executor.Execute(new string[] { "default-parameter", "foo-bar3", "hello", "-num1:123" });
            Assert.Equal(1, DefaultParameterCommand.HitCounter.GetHitCount("3"));
            Assert.Equal(123, DefaultParameterCommand.Num1);
            Assert.Equal(2, DefaultParameterCommand.Num2);
            Assert.Equal("apple", DefaultParameterCommand.Code);
        }

        [Fact]
        public void Parameters_with_default_values_partial_orderless_defaults()
        {
            DefaultParameterCommand.Reset();

            executor.Execute(new string[] { "default-parameter", "foo-bar3", "-num1:123", "hello", "--c:orange" });
            Assert.Equal(1, DefaultParameterCommand.HitCounter.GetHitCount("3"));
            Assert.Equal(123, DefaultParameterCommand.Num1);
            Assert.Equal(2, DefaultParameterCommand.Num2);
            Assert.Equal("orange", DefaultParameterCommand.Code);
        }

    }
}
