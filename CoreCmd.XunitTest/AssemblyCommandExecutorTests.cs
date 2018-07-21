﻿using CoreCmd.Attributes;
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
    static class FooBar3DefaultValues
    {
        public static int Num1 { get; set; } = 0;
        public static int Num2 { get; set; } = 0;

        public static void Reset()
        {
            Num1 = 0;
            Num2 = 0;
        }
    }

    public class AsbDummyCommand
    {
        [OptionalParam("Something", typeof(int), "aa", "bb", "cc")]
        public void FooBar1() { AssemblyCommandExecutorTests.HitCounter.Hit("1"); }

        [OptionalParam("Something", typeof(int))]
        [OptionalParam("Something", typeof(int))]
        public void FooBar2(string str) { AssemblyCommandExecutorTests.HitCounter.Hit("21"); }
        public void FooBar2(string str, int num) { AssemblyCommandExecutorTests.HitCounter.Hit("22"); }
        public void FooBar2(int num, string str) { AssemblyCommandExecutorTests.HitCounter.Hit("23"); }

        public void FooBar3(string str, int num1=1, int num2=2)
        {
            FooBar3DefaultValues.Num1 = num1;
            FooBar3DefaultValues.Num2 = num2;

            AssemblyCommandExecutorTests.HitCounter.Hit("3");
        }

        public void FooBar4(int num) { AssemblyCommandExecutorTests.HitCounter.Hit("41"); }
        public void FooBar4(double num) { AssemblyCommandExecutorTests.HitCounter.Hit("42"); }
        public void FooBar4(uint num) { AssemblyCommandExecutorTests.HitCounter.Hit("43"); }
        public void FooBar4(short num) { AssemblyCommandExecutorTests.HitCounter.Hit("44"); }
        public void FooBar4(ushort num) { AssemblyCommandExecutorTests.HitCounter.Hit("45"); }
        public void FooBar4(decimal num) { AssemblyCommandExecutorTests.HitCounter.Hit("46"); }
        public void FooBar4(float num) { AssemblyCommandExecutorTests.HitCounter.Hit("47"); }

        public void FooBar5(char p) { AssemblyCommandExecutorTests.HitCounter.Hit("51"); }
        public void FooBar5(string p) { AssemblyCommandExecutorTests.HitCounter.Hit("52"); }
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
        public void Parameters_with_default_values_1()
        {
            FooBar3DefaultValues.Reset();

            executor.Execute(new string[] { "asb-dummy", "foo-bar3", "hello" });
            Assert.Equal(1, HitCounter.GetHitCount("3"));
            Assert.Equal(1, FooBar3DefaultValues.Num1);
            Assert.Equal(2, FooBar3DefaultValues.Num2);
        }

        [Fact]
        public void Parameters_with_default_values_2()
        {
            FooBar3DefaultValues.Reset();

            executor.Execute(new string[] { "asb-dummy", "foo-bar3", "hello", "123" });
            Assert.Equal(1, HitCounter.GetHitCount("3"));
            Assert.Equal(123, FooBar3DefaultValues.Num1);
            Assert.Equal(2, FooBar3DefaultValues.Num2);
        }

        [Fact]
        public void Parameters_with_default_values_3()
        { 
            FooBar3DefaultValues.Reset();

            executor.Execute(new string[] { "asb-dummy", "foo-bar3", "hello", "123", "456"});
            Assert.Equal(1, HitCounter.GetHitCount("3"));
            Assert.Equal(123, FooBar3DefaultValues.Num1);
            Assert.Equal(456, FooBar3DefaultValues.Num2);
        }

        [Fact]
        public void Parameters_with_default_values_mismatching_case()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar3", "hello", "string"});
            Assert.Equal(0, HitCounter.GetHitCount("3"));
        }

        [Fact]
        public void Numeric_paramter_matching_1()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar4", "4"});
            Assert.Equal(1, HitCounter.GetHitCount("41"));
            Assert.Equal(1, HitCounter.GetHitCount("42"));
            Assert.Equal(1, HitCounter.GetHitCount("43"));
            Assert.Equal(1, HitCounter.GetHitCount("44"));
            Assert.Equal(1, HitCounter.GetHitCount("45"));
            Assert.Equal(1, HitCounter.GetHitCount("46"));
            Assert.Equal(1, HitCounter.GetHitCount("47"));
        }

        [Fact]
        public void Numeric_paramter_matching_2()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar4", "4.1"});
            Assert.Equal(0, HitCounter.GetHitCount("41"));
            Assert.Equal(1, HitCounter.GetHitCount("42"));
            Assert.Equal(0, HitCounter.GetHitCount("43"));
            Assert.Equal(0, HitCounter.GetHitCount("44"));
            Assert.Equal(0, HitCounter.GetHitCount("45"));
            Assert.Equal(1, HitCounter.GetHitCount("46"));
            Assert.Equal(1, HitCounter.GetHitCount("47"));
        }

        [Fact]
        public void String_char_matching_1()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar5", "1"});
            Assert.Equal(1, HitCounter.GetHitCount("51"));  // char
            Assert.Equal(1, HitCounter.GetHitCount("52"));  // string
        }

        [Fact]
        public void String_char_matching_2()
        {
            executor.Execute(new string[] { "asb-dummy", "foo-bar5", "11"});
            Assert.Equal(0, HitCounter.GetHitCount("51")); // char
            Assert.Equal(1, HitCounter.GetHitCount("52")); // string
        }
    }
}
