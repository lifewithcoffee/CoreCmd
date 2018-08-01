using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CoreCmd.Help
{
    public interface IHelpInfoService
    {
        void PrintClassHelp(Type commandClassType);
        void PrintAllMethodHelp(Type commandClassType);
        void PrintMethodHelp(MethodInfo methodInfo);
    }

    public class HelpInfoService : IHelpInfoService
    {
        public void PrintClassHelp(Type commandClassType)
        {
            var helpInfo = commandClassType.GetCustomAttribute<HelpAttribute>();
            string commandName = Utils.LowerKebabCase(commandClassType.Name).Replace("-command", "");

            string helpText = helpInfo == null ? commandName : $"{commandName}: {helpInfo?.Description}";
            Console.WriteLine(helpText);
            this.PrintAllMethodHelp(commandClassType);
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
