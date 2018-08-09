using CoreCmd.BuildinCommands;
using CoreCmd.CommandExecution;
using CoreCmd.XunitTest.TestUtils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace AssemblyCommandExecutorTests
{
    public class AsbDummyCommand
    {
        public void FooBar1() { BasicTests.HitCounter.Hit("1"); }
        public void FooBar2(string str) { BasicTests.HitCounter.Hit("2_string"); }
        public void FooBar2(string str, int num) { BasicTests.HitCounter.Hit("2_string_int"); }
        public void FooBar2(int num, string str) { BasicTests.HitCounter.Hit("2_int_string"); }

        #region Foobar4
        public void FooBar4(byte num) { BasicTests.HitCounter.Hit("4_byte"); }
        public void FooBar4(sbyte num) { BasicTests.HitCounter.Hit("4_sbyte"); }

        public void FooBar4(bool param) { BasicTests.HitCounter.Hit("4_bool"); }

        public void FooBar4(long num) { BasicTests.HitCounter.Hit("4_long"); }
        public void FooBar4(ulong num) { BasicTests.HitCounter.Hit("4_ulong"); }

        public void FooBar4(int num) { BasicTests.HitCounter.Hit("4_int"); }
        public void FooBar4(uint num) { BasicTests.HitCounter.Hit("4_uint"); }

        public void FooBar4(short num) { BasicTests.HitCounter.Hit("4_short"); }
        public void FooBar4(ushort num) { BasicTests.HitCounter.Hit("4_ushort"); }

        public void FooBar4(double num) { BasicTests.HitCounter.Hit("4_double"); }
        public void FooBar4(decimal num) { BasicTests.HitCounter.Hit("4_decimal"); }
        public void FooBar4(float num) { BasicTests.HitCounter.Hit("4_float"); }
        #endregion

        public void FooBar5(char p) { BasicTests.HitCounter.Hit("5_char"); }
        public void FooBar5(string p) { BasicTests.HitCounter.Hit("5_string"); }
    }

    public class BasicTests
    {
        static public HitCounter HitCounter { get; set; } = new HitCounter();
        AssemblyCommandExecutor executor = new AssemblyCommandExecutor(typeof(BasicTests));

        public BasicTests() { HitCounter.ResetDict(); }

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
            Assert.Equal(1, HitCounter.GetHitCount("2_string"));
            Assert.Equal(0, HitCounter.GetHitCount("2_string_int"));
            Assert.Equal(0, HitCounter.GetHitCount("2_int_string"));
        }

        [Fact]
        public void Method_overloading_2()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar2", "hello", "123" });
            Assert.Equal(0, HitCounter.GetHitCount("1"));
            Assert.Equal(0, HitCounter.GetHitCount("2_string"));
            Assert.Equal(1, HitCounter.GetHitCount("2_string_int"));
            Assert.Equal(0, HitCounter.GetHitCount("2_int_string"));
        }

        [Fact]
        public void Bool_paramter_matching_1()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar4", "true"});
            Assert.Equal(1, HitCounter.GetHitCount("4_bool"));
        }

        [Fact]
        public void Bool_paramter_matching_2()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar4", "1"});
            Assert.Equal(0, HitCounter.GetHitCount("4_bool"));
        }

        [Fact]
        public void Numeric_paramter_matching_1()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar4", "4"});
            Assert.Equal(1, HitCounter.GetHitCount("4_byte"));
            Assert.Equal(1, HitCounter.GetHitCount("4_sbyte"));
            Assert.Equal(1, HitCounter.GetHitCount("4_long"));
            Assert.Equal(1, HitCounter.GetHitCount("4_ulong"));

            Assert.Equal(1, HitCounter.GetHitCount("4_int"));
            Assert.Equal(1, HitCounter.GetHitCount("4_double"));
            Assert.Equal(1, HitCounter.GetHitCount("4_uint"));
            Assert.Equal(1, HitCounter.GetHitCount("4_short"));
            Assert.Equal(1, HitCounter.GetHitCount("4_ushort"));
            Assert.Equal(1, HitCounter.GetHitCount("4_decimal"));
            Assert.Equal(1, HitCounter.GetHitCount("4_float"));
        }

        [Fact]
        public void Numeric_paramter_matching_2()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar4", "4.1"});
            Assert.Equal(0, HitCounter.GetHitCount("4_long"));
            Assert.Equal(0, HitCounter.GetHitCount("4_ulong"));

            Assert.Equal(0, HitCounter.GetHitCount("4_int"));
            Assert.Equal(1, HitCounter.GetHitCount("4_double"));
            Assert.Equal(0, HitCounter.GetHitCount("4_uint"));
            Assert.Equal(0, HitCounter.GetHitCount("4_short"));
            Assert.Equal(0, HitCounter.GetHitCount("4_ushort"));
            Assert.Equal(1, HitCounter.GetHitCount("4_decimal"));
            Assert.Equal(1, HitCounter.GetHitCount("4_float"));
        }

        [Fact]
        public void String_char_matching_1()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar5", "1"});
            Assert.Equal(1, HitCounter.GetHitCount("5_char"));  // char
            Assert.Equal(1, HitCounter.GetHitCount("5_string"));  // string
        }

        [Fact]
        public void String_char_matching_2()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar5", "11"});
            Assert.Equal(0, HitCounter.GetHitCount("5_char")); // char
            Assert.Equal(1, HitCounter.GetHitCount("5_string")); // string
        }
    }
}
