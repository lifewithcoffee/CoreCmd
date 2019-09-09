using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoreCmd.CommandLoading
{
    interface IAssemblyLoadable
    {
        IEnumerable<string> GetConflictComands(IEnumerable<Type> types, string dllPath);
        IEnumerable<string> GetConflictComands(IEnumerable<Type> types, Assembly asb);

        bool FindConflict(IEnumerable<Type> types, string dllPath);
        bool FindConflict(IEnumerable<Type> types, Assembly asb);
    }

    class AssemblyLoadable : IAssemblyLoadable
    {
        IAssemblyCommandLoader _assemblyCommandFinder = new AssemblyCommandLoader();

        public IEnumerable<string> GetConflictComands(IEnumerable<Type> types, string dllPath)
        {
            var existingCmds = types.Select(c => c.Name);
            var assemblyCmds = _assemblyCommandFinder.GetCommandClassTypesFromAssembly(dllPath, Global.CommandPostFix).Select(c => c.Name);
            return existingCmds.Intersect(assemblyCmds);
        }

        public IEnumerable<string> GetConflictComands(IEnumerable<Type> types, Assembly asb)
        {
            var existingCmds = types.Select(c => c.Name);
            var assemblyCmds = _assemblyCommandFinder.GetCommandClassTypesFromAssembly(asb, Global.CommandPostFix).Select(c => c.Name);
            return existingCmds.Intersect(assemblyCmds);
        }

        public bool FindConflict(IEnumerable<Type> types, string dllPath)
        {
            return this.GetConflictComands(types, dllPath).Count() != 0;
        }

        public bool FindConflict(IEnumerable<Type> types, Assembly asb)
        {
            return this.GetConflictComands(types, asb).Count() != 0;
        }
    }
}
