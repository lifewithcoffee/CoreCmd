using CoreCmd.Attributes;
using NetCoreUtils.Text.Indent;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CoreCmd.Help
{
    public interface IHelpInfoService
    {
        void PrintClassHelp(IEnumerable<Type> commandClassTypes);
        void PrintClassHelp(Type commandClassType);
        void PrintVerboseMethodHelp(Type commandClassType);
    }

    public class HelpInfoService : IHelpInfoService
    {
        public void PrintClassHelp(IEnumerable<Type> commandClassTypes)
        {
            foreach (var cmdType in commandClassTypes) 
                PrintClassHelp(cmdType);
        }

        public void PrintClassHelp(Type commandClassType)
        {
            // print command name
            string commandName = Utils.LowerKebabCase(commandClassType.Name).Replace("-command", "");
            var aliasinfo = commandClassType.GetCustomAttribute<AliasAttribute>();
            if(aliasinfo != null)
                commandName = $"{commandName}|{aliasinfo.Alias}";

            // print command description, if available
            var helpInfo = commandClassType.GetCustomAttribute<HelpAttribute>();
            Console.WriteLine($"{commandName,-10}\t{helpInfo?.Description}");

            // print subcommand info
            this.PrintVerboseMethodHelp(commandClassType);
        }

        public void PrintVerboseMethodHelp(Type commandClassType)
        {
            var methods = commandClassType.GetMethods( BindingFlags.Public | BindingFlags.Instance );
            foreach(var m in methods)
            {
                if( m.Name != "GetType" 
                    && m.Name != "ToString" 
                    && m.Name != "Equals"
                    && m.Name != "GetHashCode")     // exclude methods inherits from the Object class
                {
                    this.PrintMethodHelp(m);
                }
            }
        }

        private void PrintDllLocation(Type commandClassType)
        {
            string dllPath = commandClassType.Assembly.Location;
            Console.WriteLine($"{Global.indentSpaces}{dllPath}");
        }

        /// <summary>
        /// Example:
        ///     <param1:String> <param2:String>
        /// </summary>
        private string GetParameterListText(MethodInfo methodInfo)
        {
            StringBuilder sb = new StringBuilder();

            var parameters = methodInfo.GetParameters();
            foreach(var p in parameters)
            {
                if (p.IsOptional)
                    sb.Append($" [{p.Name}:{p.ParameterType.Name}]");
                else
                    sb.Append($" <{p.Name}:{p.ParameterType.Name}>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Print:
        ///     {method-name}
        ///         {method-help-info (from the [help] attribute)}
        /// </summary>
        /// <param name="methodInfo"></param>
        public void PrintMethodHelp(MethodInfo methodInfo)
        {
            const string indentSpaces = "    ";
            var helpInfo = methodInfo.GetCustomAttribute<HelpAttribute>();
            string methodName = Utils.LowerKebabCase(methodInfo.Name);
            string description;
            if(methodName == "default")
                description = "The omissible default subcommand";
            else
                description = helpInfo?.Description;
            Console.WriteLine($"{indentSpaces}{methodName,-10}\t{description}");
        }
    }
}
