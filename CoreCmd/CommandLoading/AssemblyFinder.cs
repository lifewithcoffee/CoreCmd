using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CoreCmd.CommandLoading
{
    interface IAssemblyFinder
    {
        string[] GetCommandAssembly(string dir, string assemblyPrefix);
    }

    class AssemblyFinder : IAssemblyFinder
    {
        public string[] GetCommandAssembly(string dir, string assemblyPrefix)
        {

            var targetDlls = new DirectoryInfo(dir).EnumerateFiles()
                                .Where(f => f.Name.ToLower().StartsWith(assemblyPrefix) && f.Extension.ToLower().Equals(".dll"))
                                .OrderBy(f => f.Name)
                                .Select(f => f.FullName)
                                .ToArray();
            return targetDlls;
        }
    }
}
