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
        void PrintAllMethodHelp(Type commandClassType);
        void PrintMethodHelp(MethodInfo methodInfo);
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
            const string indentSpaces = "    ";

            // print command name
            string commandName = Utils.LowerKebabCase(commandClassType.Name).Replace("-command", "");
            Console.WriteLine(commandName);

            // print command description, if available
            var helpInfo = commandClassType.GetCustomAttribute<HelpAttribute>();
            string helpText = helpInfo == null ? null : $"{helpInfo?.Description}";
            if(helpText != null)
                Console.WriteLine($"{indentSpaces}{helpText}");

            // print dll location info
            string dllPath = commandClassType.Assembly.Location;
            Console.WriteLine($"{indentSpaces}{dllPath}");
            Console.WriteLine($"{indentSpaces}----- subcommands -----");

            // print subcommand info
            this.PrintAllMethodHelp(commandClassType);
            Console.WriteLine("");
        }

        public void PrintAllMethodHelp(Type commandClassType)
        {
            var methods = commandClassType.GetMethods( BindingFlags.DeclaredOnly
                                                     | BindingFlags.Public
                                                     | BindingFlags.Instance
                                                     );
            foreach(var m in methods)
                this.PrintMethodHelp(m);
        }

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
            Console.WriteLine($"{indentSpaces}{Utils.LowerKebabCase(methodInfo.Name)}{GetParameterListText(methodInfo)}");

            var helpInfo = methodInfo.GetCustomAttribute<HelpAttribute>();
            if (helpInfo != null)
            {
                Console.WriteLine($"{indentSpaces}{indentSpaces}{helpInfo.Description}");
            }
        }
    }
}
