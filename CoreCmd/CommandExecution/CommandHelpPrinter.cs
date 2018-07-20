using CoreCmd.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CoreCmd.CommandExecution
{
    public class CommandHelpPrinter
    {
        public void PrintClassHelp(Type commandClassType)
        {
            var helpInfo = commandClassType.GetCustomAttribute<HelpAttribute>();

            string commandName = Utils.LowerKebabCase(commandClassType.Name);
            commandName = commandName.Replace("-command", "");

            Console.WriteLine($"{commandName}: {helpInfo.Description}");
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

        public string GetParameterListText(MethodInfo methodInfo)
        {
            StringBuilder sb = new StringBuilder();

            var parameters = methodInfo.GetParameters();
            foreach(var p in parameters)
            {
                if (p.IsOptional)
                    sb.Append($"  [{p.Name}:{p.ParameterType.Name}]");
                else
                    sb.Append($"  <{p.Name}:{p.ParameterType.Name}>");
            }

            return sb.ToString();
        }

        public void PrintMethodHelp(MethodInfo methodInfo)
        {
            Console.WriteLine($"\t{Utils.LowerKebabCase(methodInfo.Name)}{GetParameterListText(methodInfo)}");

            var helpInfo = methodInfo.GetCustomAttribute<HelpAttribute>();
            if (helpInfo != null)
            {
                Console.WriteLine($"\t\t{helpInfo.Description}");
            }
        }
    }
}
