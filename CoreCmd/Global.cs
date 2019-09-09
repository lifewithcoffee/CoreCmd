using System;
using System.Collections.Generic;
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

        /// <summary>
        /// The extension will be ".config.xml" 
        /// </summary>
        public static string ConfigFileName { get; set; }
    }
}
