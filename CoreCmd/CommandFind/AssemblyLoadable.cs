using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreCmd.CommandFind
{
    interface IAssemblyLoadable
    {
        IEnumerable<string> GetConflictComands(IEnumerable<Type> types, string dllPath);
        bool FindConflict(IEnumerable<Type> types, string dllPath);
    }

    class AssemblyLoadable : IAssemblyLoadable
    {
        IAssemblyCommandFinder _assemblyCommandFinder = new AssemblyCommandFinder();

        public IEnumerable<string> GetConflictComands(IEnumerable<Type> types, string dllPath)
        {
            var existingCmds = types.Select(c => c.Name);
            var assemblyCmds = _assemblyCommandFinder.GetCommandClassTypesFromAssembly(dllPath, GlobalConsts.CommandPostFix).Select(c => c.Name);
            return existingCmds.Intersect(assemblyCmds);
        }

        public bool FindConflict(IEnumerable<Type> types, string dllPath)
        {
            return this.GetConflictComands(types, dllPath).Count() != 0;
        }
    }
}
