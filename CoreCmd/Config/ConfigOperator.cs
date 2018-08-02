using CoreCmd.CommandLoading;
using NetCoreUtils.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CoreCmd.Config
{
    interface IConfigOperator
    {
        /// <param name="dllfile">Either full path or just file name with extension in the current directory</param>
        void AddCommandAssembly(string dllfile);

        /// <param name="dllfile">Either full path or just file name with extension in the current directory</param>
        void RemoveCommandAssembly(string dllfile);

        /// <returns>Full path of all registered assembly DLLs</returns>
        IEnumerable<string> ListCommandAssemblies();

        void SaveChanges();
    }

    class ConfigOperator : IConfigOperator
    {
        CoreCmdConfig config = null;
        string configFileFullPath;

        public ConfigOperator()
        {
            var _xmlUtil = new XmlUtil<CoreCmdConfig>();

            var userDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            configFileFullPath = Path.Combine(userDir, GlobalConsts.ConfigFileName);

            if (File.Exists(configFileFullPath))
                config = _xmlUtil.ReadFromFile(configFileFullPath);
            else
                config = new CoreCmdConfig();
        }

        /// <summary>
        /// If the commands in the specified dll do not exist, add the dll to the config
        /// </summary>
        public void AddCommandAssembly(string dllfile)
        {
            ICommandClassLoader _loader = new CommandClassLoader();
            IAssemblyLoadable _assemblyLoadable = new AssemblyLoadable();

            if (config != null)
            {
                // if dllname is not full path, compose the current dir to make a full path
                string targetFilePath = Path.IsPathFullyQualified(dllfile) ? dllfile : Path.Combine(Directory.GetCurrentDirectory(), dllfile);
                if (File.Exists(targetFilePath))
                {
                    // add wehn the assembly does not exist
                    if (config.CommandAssemblies.Where(c => c.Path.ToLower().Equals(targetFilePath.ToLower())).Count() == 0)
                    {
                        config.AddCommandAssembly(targetFilePath);
                        Console.WriteLine($"Successfully added assembly: {targetFilePath}");
                    }
                    else
                        Console.WriteLine($"Command assembly already exsits: {targetFilePath}");
                }
                else
                    Console.WriteLine($"Assembly file not exists: {targetFilePath}");
            }
            else
                Console.WriteLine("Error: configuration not loaded");
        }

        public IEnumerable<string> ListCommandAssemblies()
        {
            if( config != null)
                return config.CommandAssemblies.Select(a => a.Path);
            else
            {
                Console.WriteLine("Error: configuration not loaded");
                return null;
            }
        }

        public void RemoveCommandAssembly(string dllPath)
        {
            if (config != null)
                config.CommandAssemblies.RemoveAll(a => a.Path.ToLower().Equals(dllPath.ToLower()));
            else
                Console.WriteLine("Error: configuration not loaded");
        }

        public void SaveChanges()
        {
            var _xmlUtil = new XmlUtil<CoreCmdConfig>();
            _xmlUtil.WriteToFile(config,configFileFullPath);
        }
    }
}
