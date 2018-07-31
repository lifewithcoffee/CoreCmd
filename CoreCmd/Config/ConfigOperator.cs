using CoreCmd.CommandFind;
using NetCoreUtils.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreCmd.Config
{
    class ConfigOperator
    {
        ICommandFinder _commandFinder = new CommandFinder();
        IAssemblyCommandFinder _assemblyCommandFinder = new AssemblyCommandFinder();

        CoreCmdConfig config = null;

        public void LoadConfig(string path)
        {
            var xmlUtil = new XmlUtil<CoreCmdConfig>();
            config = xmlUtil.ReadFromFile(path);
        }

        /// <summary>
        /// If the commands in the specified dll do not exist, add the dll to the config
        /// </summary>
        public void AddCommandAssembly(string dllPath)
        {
            if (config != null)
            {
                string lowerPath = dllPath.ToLower();

                // add wehn the assembly does not exist
                if (config.CommandAssemblies.Where(c => c.Path.ToLower().Equals(lowerPath)).FirstOrDefault() != null)
                {
                    var existingCmds = _commandFinder.GetAllCommandClasses().Select(c => c.Name);
                    var assemblyCmds = _assemblyCommandFinder.GetCommandClassTypesFromAssembly(dllPath, GlobalConsts.CommandPostFix).Select(c => c.Name);
                    var intersect = existingCmds.Intersect(assemblyCmds);

                    if (intersect.Count() == 0)
                        config.AddCommandAssembly(dllPath);
                    else
                    {
                        foreach (var cmd in intersect)
                            Console.WriteLine($"Add command asssembly denied: {cmd} already exists");
                    }
                }
                else
                    Console.WriteLine("Command assembly already exsits.");
            }
            else
                Console.WriteLine("Error: configuration not loaded");
        }

        public void RemoveCommandAssembly(string dllPath)
        {
            if (config != null)
                config.CommandAssemblies.RemoveAll(a => a.Path.ToLower().Equals(dllPath.ToLower()));
            else
                Console.WriteLine("Error: configuration not loaded");
        }
    }
}
