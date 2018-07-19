using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using CoreCmd.MethodMatching;
using CoreCmd.BuiltinCommands;

namespace CoreCmd.CommandExecution
{
    public interface ITargetCommand
    {
        void PrintHelp();
        int Execute();
    }

    public class TargetCommand : ITargetCommand
    {
        private IMethodMatcher _methodMatcher = new MethodMatcher();
        private IParameterMatcher _parameterMatcher = new ParameterMatcher();

        public Type CommandType { get; set; }

        /// <summary>
        /// Lower kebab-case method name.
        /// 
        /// When receive the relevant argument from console, the format has already been
        /// kebab-case,so when pass into the internal variable, just need to turn it into
        /// lower case for the later comparison.
        /// </summary>
        private string methodSubcommand;
        public string MethodSubcommand {
            get { return methodSubcommand; }
            set { methodSubcommand = value.ToLower(); }
        }

        public string[] Parameters { get; set; } = new string[] { };

        public void PrintHelp()
        {
            var helpInfo = (HelpAttribute)this.CommandType.GetCustomAttribute(typeof(HelpAttribute));
            Console.WriteLine(helpInfo.Description);
        }

        public int Execute()
        {
            int invocationCount = 0;
            if (this.CommandType == null)
                throw new Exception("Command type is null");

            string errmsg = $"Can't find method: {this.MethodSubcommand}";
            var methods = _methodMatcher.GetMethodInfo(this.CommandType, this.MethodSubcommand);

            if(methods == null)
                Console.WriteLine(errmsg);
            else
            {
                foreach (var m in methods)
                {
                    var paramObjs = _parameterMatcher.Match(m.GetParameters(), this.Parameters);
                    if(paramObjs != null)
                    {
                        m.Invoke(Activator.CreateInstance(this.CommandType), paramObjs);
                        invocationCount++;
                    }
                }

                if (invocationCount == 0)
                    Console.WriteLine(errmsg);
            }

            return invocationCount;
        }
    }
}
