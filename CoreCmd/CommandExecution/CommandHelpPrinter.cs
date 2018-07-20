using CoreCmd.BuiltinCommands;
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
            Console.WriteLine(helpInfo.Description);
        }

        public void PrintAllMethodHelp(Type commandClassType)
        {
            var methods = commandClassType.GetMethods();
            foreach(var m in methods)
            {
                this.PrintMethodHelp(m);
            }
        }

        public string GetParameterListText(MethodInfo methodInfo)
        {
            StringBuilder sb = new StringBuilder();

            var parameters = methodInfo.GetParameters();
            foreach(var p in parameters)
            {
                if (p.IsOptional)
                    sb.Append($"  [{p.Name}]");
                else
                    sb.Append($"  <{p.Name}>");
            }

            return sb.ToString();
        }

        public void PrintMethodHelp(MethodInfo methodInfo)
        {
            var helpInfo = methodInfo.GetCustomAttribute<HelpAttribute>();
            if (helpInfo != null)
            {
                Console.WriteLine($"{Utils.LowerKebabCase(methodInfo.Name)}{GetParameterListText(methodInfo)}");
                Console.WriteLine($"\t{helpInfo.Description}");
            }
        }
    }
}
