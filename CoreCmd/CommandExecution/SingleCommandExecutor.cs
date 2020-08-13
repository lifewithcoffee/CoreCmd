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
using Microsoft.Extensions.DependencyInjection;

namespace CoreCmd.CommandExecution
{
    public interface ISingleCommandExecutor
    {
        Type CommandClassType { get; set; }
        Task<int> ExecuteAsync(IServiceProvider serviceProvider);
    }

    public class SingleCommandExecutor : ISingleCommandExecutor
    {
        private IMethodMatcher _methodMatcher = new MethodMatcher();
        private IParameterMatcher _parameterMatcher = new ParameterMatcher();
        private IHelpInfoService _helpSvc = new HelpInfoService();

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

        /// <summary>
        /// If the serviceProvider is null, this method will use normal
        /// Activator.CreateInstance() to create an instance.
        /// 
        /// If the target class requires dependency injection, using
        /// Activator.CreateInstance() may throw out an exception or have
        /// unexpected behavior.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(IServiceProvider serviceProvider)
        {
            int invocationCount = 0;
            if (this.CommandClassType == null)
            {
                throw new Exception("Command type is null");
            }

            if(this.MethodSubcommand == "help")
            {
                Console.WriteLine("Help command called");
                _helpSvc.PrintClassHelp(this.CommandClassType);
                return await Task.FromResult<int>(1);
            }

            string errmsg = $"Invalid subcommand: Either there is no subcommand '{this.MethodSubcommand}' or parameter mismatches.";
            var methods = _methodMatcher.GetMethodInfo(this.CommandClassType, this.MethodSubcommand);

            if (methods.Count() == 0)
            {
                Console.WriteLine(errmsg);
            }
            else
            {
                foreach (var m in methods)
                {
                    var paramObjs = _parameterMatcher.Match(m.GetParameters(), this.Parameters);
                    if(paramObjs != null)
                    {
                        object instance = this.GetInstance(serviceProvider, this.CommandClassType);

                        if (IsAsyncMethod(m))
                        {
                            Task result = (Task)m.Invoke(instance, paramObjs);
                            await result.ConfigureAwait(false);
                        }
                        else
                        {
                            m.Invoke(instance, paramObjs);
                        }
                        invocationCount++;
                    }
                }

                if (invocationCount == 0)
                {
                    Console.WriteLine(errmsg);
                }
            }

            return await Task.FromResult<int>(invocationCount);
        }

        private object GetInstance(IServiceProvider serviceProvider, Type type)
        {
            object instance = serviceProvider?.GetService(this.CommandClassType);
            if (instance == null)
            {
                instance = Activator.CreateInstance(this.CommandClassType);
            }
            return instance;
        }
    }
}
