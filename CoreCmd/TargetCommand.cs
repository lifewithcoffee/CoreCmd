using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace CoreCmd
{
    public class MethodMatcher
    {
        /// <param name="classType">The host class type of the method.</param>
        /// <param name="methodName">Must be the lower kebab-case.</param>
        /// <returns>Null if not find, otherwise return the relevant MethodInfo objects.</returns>
        public IEnumerable<MethodInfo> GetMethodInfo(Type classType, string methodName)
        {
            return classType.GetMethods().Where(m => Utils.LowerKebabCase(m.Name).Equals(methodName));
        }
    }

    class ParameterMatcher
    {
        public object[] Match(ParameterInfo[] info, string[] parameters)
        {
            object[] result = null;
            if (info.Length == parameters.Length)
            {
                result = new object[info.Length];
                for (int i = 0; i < info.Length; i++)
                {
                    Type type = info[i].ParameterType;
                    if (type.Equals(typeof(string)))
                        result[i] = parameters[i];
                    else if (type.Equals(typeof(char)) && parameters.Length == 1)
                        result[i] = parameters[0];
                    else if (type.Equals(typeof(int)))
                    {
                        if (int.TryParse(parameters[i], out int p))
                            result[i] = p;
                        else
                            return null;
                    }
                    else if (type.Equals(typeof(double)))
                    {
                        if (double.TryParse(parameters[i], out double p))
                            result[i] = p;
                        else
                            return null;
                    }
                    else if (type.Equals(typeof(uint)))
                    {
                        if (uint.TryParse(parameters[i], out uint p))
                            result[i] = p;
                        else
                            return null;
                    }
                    else if (type.Equals(typeof(short)))
                    {
                        if (short.TryParse(parameters[i], out short p))
                            result[i] = p;
                        else
                            return null;
                    }
                    else if (type.Equals(typeof(ushort)))
                    {
                        if (ushort.TryParse(parameters[i], out ushort p))
                            result[i] = p;
                        else
                            return null;
                    }
                    else if (type.Equals(typeof(decimal)))
                    {
                        if (decimal.TryParse(parameters[i], out decimal p))
                            result[i] = p;
                        else
                            return null;
                    }
                    else if (type.Equals(typeof(float)))
                    {
                        if (float.TryParse(parameters[i], out float p))
                            result[i] = p;
                        else
                            return null;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return result;
        }
    }

    public class TargetCommand
    {
        private MethodMatcher _methodMatcher = new MethodMatcher();
        private ParameterMatcher _parameterMatcher = new ParameterMatcher();

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
