using Xunit;
using CoreCmd;

namespace CoreCmd.XunitTest
{
    public class UtilsTests
    {
        [Theory]
        [InlineData("hello-world","HelloWorld")]
        [InlineData("hello-ibm","HelloIBM")]
        [InlineData("hello2-world","Hello2World")]
        [InlineData("hello2_world","Hello2_World")]
        [InlineData("hello_world","Hello_World")]
        public void Test_LowerKebabCase(string output, string input)
        {
            Assert.Equal(output, Utils.LowerKebabCase(input));
        }
    }
}