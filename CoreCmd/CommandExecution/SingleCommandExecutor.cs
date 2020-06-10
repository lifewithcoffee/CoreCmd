using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using CoreCmd.MethodMatching;
using CoreCmd.Help;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CoreCmd.CommandExecution
{
    public interface ISingleCommandExecutor
    {
        Type CommandClassType { get; set; }
        Task<int> ExecuteAsync();
    }

    public class SingleCommandExecutor : ISingleCommandExecutor
    {
        private IMethodMatcher _methodMatcher = new MethodMatcher();
        private IParameterMatcher _parameterMatcher = new ParameterMatcher();

        /// <summary>
        /// There is no naming format required for the relevant class.
        /// </summary>
        public Type CommandClassType { get; set; }

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

        private bool IsAsyncMethod(MethodInfo method)
        {
            var asyncAttrib = (AsyncStateMachineAttribute)method.GetCustomAttribute(typeof(AsyncStateMachineAttribute));
            return (asyncAttrib != null);
        }

        public async Task<int> ExecuteAsync()
        {
            int invocationCount = 0;
            if (this.CommandClassType == null)
                throw new Exception("Command type is null");

            string errmsg = $"Invalid subcommand: Either there is no subcommand '{this.MethodSubcommand}' or parameter mismatches.";
            var methods = _methodMatcher.GetMethodInfo(this.CommandClassType, this.MethodSubcommand);

            if (methods.Count() == 0)
                Console.WriteLine(errmsg);
            else
            {
                foreach (var m in methods)
                {
                    var paramObjs = _parameterMatcher.Match(m.GetParameters(), this.Parameters);
                    if(paramObjs != null)
                    {
                        if (IsAsyncMethod(m))
                        {
                            Task result = (Task)m.Invoke(Activator.CreateInstance(this.CommandClassType), paramObjs);
                            await result.ConfigureAwait(false);
                        }
                        else
                            m.Invoke(Activator.CreateInstance(this.CommandClassType), paramObjs);
                        invocationCount++;
                    }
                }

                if (invocationCount == 0)
                    Console.WriteLine(errmsg);
            }

            return await Task.FromResult<int>(invocationCount);
        }
    }
}
