#if DEBUG
using CoreCmd.Attributes;
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

        public void ViewCSproj()
        {
            //new CsprojFileService().GetRootNamespace(@"e:\rp\git\CoreCmdPlayground\CoreCmdPlayground\CoreCmdPlayground.csproj");
            Console.WriteLine("Searching .csproj file...");
            Console.WriteLine(new CsprojFileService().FindCsprojFile());
        }

        public void ViewVersion()
        {
            try
            {
                var svc = new CsprojFileService();
                var projFile = svc.FindCsprojFile();
                var ver = svc.GetVersion(projFile);
                Console.WriteLine($"Version from {projFile} : {ver}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ViewRootNamespace()
        {
            try
            {
                var svc = new CsprojFileService();
                Console.WriteLine($"Version from {svc.FindCsprojFile()} : {svc.GetRootNamespace()}");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
#endif
