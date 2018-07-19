using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using CoreCmd.Commands;
using CoreCmd.MethodMatching;

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
        /// Lower kebab-case method name
        /// </summary>
        private string method_name;
        public string MethodName {
            get { return method_name; }
            set { method_name = value.ToLower(); }
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

            string errmsg = $"Can't find method: {this.MethodName}";
            var methods = _methodMatcher.GetMethodInfo(this.CommandType, this.MethodName);

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
