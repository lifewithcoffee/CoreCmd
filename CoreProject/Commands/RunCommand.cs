using CoreProject.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreProject.Commands
{
    class RunCommand
    {
        public void AddClass(string className, string path)
        {
            new ScaffoldingService().GenerateClassFile(className, path);
        }

        public void Test()
        {
            //new CsprojFileService().GetRootNamespace(@"e:\rp\git\CoreCmdPlayground\CoreCmdPlayground\CoreCmdPlayground.csproj");
            Console.WriteLine("Searching .csproj file...");
            Console.WriteLine(new CsprojFileService().FindCsprojFile());
        }
    }
}
