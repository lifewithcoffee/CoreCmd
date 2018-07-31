using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CoreCmd.Config
{
    public class CoreCmdConfig
    {
        List<CommandAssembly> CommandAssemblies { get; set; } = new List<CommandAssembly>();

        public void AddCommandAssembly(string dllPath)
        {
            CommandAssemblies.Add(new CommandAssembly { Path = dllPath });
        }
    }

    public class CommandAssembly
    {
        [XmlAttribute]
        public string Path { get; set; }
    }
}
