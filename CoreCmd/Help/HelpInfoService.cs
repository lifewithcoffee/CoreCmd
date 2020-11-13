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
            //Console.WriteLine(commandName);

            // print command description, if available
            var helpInfo = commandClassType.GetCustomAttribute<HelpAttribute>();
            //string helpText = helpInfo == null ? null : $"{helpInfo?.Description}";
            //if(helpText != null)
            //{ 
            //    Console.WriteLine($"{Global.indentSpaces}{helpText}");
            //}
            Console.WriteLine($"{commandName,-10}\t{helpInfo?.Description}");

            // print dll location info
            //string dllPath = commandClassType.Assembly.Location;
            //Console.WriteLine($"{Global.indentSpaces}{dllPath}");

            // print subcommand info
            //Console.WriteLine($"{Global.indentSpaces}----- subcommands -----");
            this.PrintVerboseMethodHelp(commandClassType);
        }

        public void PrintVerboseMethodHelp(Type commandClassType)
        {
            //Sections sections = new Sections(4);

            var methods = commandClassType.GetMethods( BindingFlags.Public | BindingFlags.Instance );
            foreach(var m in methods)
            {
                if( m.Name != "GetType" 
                    && m.Name != "ToString" 
                    && m.Name != "Equals"
                    && m.Name != "GetHashCode")     // exclude methods inherits from the Object class
                {
                    this.PrintMethodHelp(m);
                    //string commandName = Utils.LowerKebabCase(m.Name);
                    //string commandDescription = m.GetCustomAttribute<HelpAttribute>()?.Description;

                    //var Content = sections.AddSection(commandName);

                    //if(commandName == "default")
                    //{
                    //    Content.AddLine("The omissible default subcommand");
                    //}
                    //Content.AddLine(commandDescription);
                }
            }

            //sections.Print();
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
            //Console.WriteLine($"{indentSpaces}{Utils.LowerKebabCase(methodInfo.Name)}{GetParameterListText(methodInfo)}");

            var helpInfo = methodInfo.GetCustomAttribute<HelpAttribute>();
            //if (helpInfo != null)
            //{
            //    Console.WriteLine($"{indentSpaces}{indentSpaces}{helpInfo.Description}");
            //}
            Console.WriteLine($"{indentSpaces}{Utils.LowerKebabCase(methodInfo.Name),-10}\t{helpInfo?.Description}");
        }
    }
}
