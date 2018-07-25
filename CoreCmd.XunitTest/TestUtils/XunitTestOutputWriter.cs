using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit.Abstractions;

namespace CoreCmd.XunitTest.TestUtils
{
    class XunitTestOutputWriter : TextWriter
    {
        ITestOutputHelper output;
        public XunitTestOutputWriter(ITestOutputHelper output)
        {
            this.output = output;
        }

        public override Encoding Encoding => Encoding.UTF8;

        public override void WriteLine(string value)
        {
            output.WriteLine(value);
        }
    }
}
