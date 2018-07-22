using CoreCmd.MethodMatching;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoreCmd.XunitTest
{
    public class ParamUtils_GetOptionalParamDict_Tests
    {
        IParamUtil paramUtil = new ParamUtil();

        [Fact]
        public void Basic_usage()
        {
            var optionalParamList = new List<string> { "-a:hello" };
            var dict = paramUtil.GetOptionalParamDict(optionalParamList);
            Assert.Single(dict);
            Assert.Equal("hello", dict["a"]);
        }

        [Fact]
        public void Basic_usage_key_name_kebab_case()
        {
            var optionalParamList = new List<string> { "-SomeKey:hello" };
            var dict = paramUtil.GetOptionalParamDict(optionalParamList);
            Assert.Single(dict);
            Assert.Equal("hello", dict["some-key"]);
        }

        [Fact]
        public void Multidash_single_quote_string_with_space()
        {
            var optionalParamList = new List<string> { "--tag:'hello world'" };
            var dict = paramUtil.GetOptionalParamDict(optionalParamList);
            Assert.Single(dict);
            Assert.Equal("hello world", dict["tag"]);
        }
        
        [Fact]
        public void Multidash_double_quote_string_with_space()
        {
            var optionalParamList = new List<string> { "---tag:\"nice day\"" };
            var dict = paramUtil.GetOptionalParamDict(optionalParamList);
            Assert.Single(dict);
            Assert.Equal("nice day", dict["tag"]);
        }

        [Fact]
        public void Multiparam()
        {
            var optionalParamList = new List<string> { "--tag:\"nice day\"", "-arg:'hello fellows'" };
            var dict = paramUtil.GetOptionalParamDict(optionalParamList);
            Assert.Equal(2, dict.Count);
            Assert.Equal("nice day", dict["tag"]);
            Assert.Equal("hello fellows", dict["arg"]);
        }

        [Fact]
        public void Multiparam_conflict()
        {
            var optionalParamList = new List<string> { "--tag:\"nice day\"", "-tag:'hello fellows'" };
            Assert.Throws<Exception>(() => paramUtil.GetOptionalParamDict(optionalParamList));
        }

        [Fact]
        public void No_value_with_only_colon()
        {
            var optionalParamList = new List<string> { "----s:" };
            var dict = paramUtil.GetOptionalParamDict(optionalParamList);
            Assert.Single(dict);
            Assert.Equal("", dict["s"]);
        }

        [Fact]
        public void No_value_with_whitespace_value()
        {
            var optionalParamList = new List<string> { "-s:'   '" };
            var dict = paramUtil.GetOptionalParamDict(optionalParamList);
            Assert.Single(dict);
            Assert.Equal("", dict["s"]);
        }

        [Fact]
        public void No_value_without_colon()
        {
            var optionalParamList = new List<string> { "-s" };
            var dict = paramUtil.GetOptionalParamDict(optionalParamList);
            Assert.Single(dict);
            Assert.Equal("", dict["s"]);
        }
    }
}
