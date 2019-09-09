using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CoreCmd
{
    static class Global
    {
        public const string CommandPostFix = "command";
        public const string AssemblyPrefix = "corecmd.";
        public const string DefaultSubcommandMethodName = "default";
        public const string DefaultCommandName = "DefaultCommand";
        public const string indentSpaces = "    ";

        public static string ConfigFileName { get; set; }
        public static string ConfigFileFullPath
        {
            get
            {
                if(string.IsNullOrWhiteSpace(Global.ConfigFileName))
                     Global.ConfigFileName = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);

                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), $"{Global.ConfigFileName}.config.xml");
            }
        }
    }
}
