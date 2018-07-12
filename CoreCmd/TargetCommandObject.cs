using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace CoreCmd
{
    class TargetCommandObject
    {
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

        public void Execute()
        {
            try
            {
                if (this.CommandType == null)
                    throw new Exception("Command type is null");

                var targetMethod = this.CommandType.GetMethods().SingleOrDefault(m => Utils.LowerKebabCase(m.Name).Equals(this.MethodName));
                if (targetMethod == null)
                    throw new Exception($"Can't find method: {this.MethodName}");

                ParameterInfo[] paramInfo = targetMethod.GetParameters();

                if (paramInfo.Length != this.Parameters.Length)
                {
                    Console.WriteLine($"Incorrect argument number, command {this.CommandType.Name}.{this.MethodName} can accept {paramInfo.Length} argument(s).");
                    return;
                }

                object[] paramObjs = new object[this.Parameters.Length];
                for (int i = 0; i < paramInfo.Length; i++)
                {
                    Type type = paramInfo[i].ParameterType;
                    if (type.Equals(typeof(int)))
                        paramObjs[i] = int.Parse(this.Parameters[i]);
                    else if (type.Equals(typeof(double)))
                        paramObjs[i] = double.Parse(this.Parameters[i]);
                    else if (type.Equals(typeof(uint)))
                        paramObjs[i] = uint.Parse(this.Parameters[i]);
                    else if (type.Equals(typeof(short)))
                        paramObjs[i] = short.Parse(this.Parameters[i]);
                    else if (type.Equals(typeof(ushort)))
                        paramObjs[i] = ushort.Parse(this.Parameters[i]);
                    else if (type.Equals(typeof(decimal)))
                        paramObjs[i] = decimal.Parse(this.Parameters[i]);
                    else if (type.Equals(typeof(float)))
                        paramObjs[i] = float.Parse(this.Parameters[i]);
                    else
                        paramObjs[i] = this.Parameters[i];
                }

                var ins = Activator.CreateInstance(this.CommandType);
                targetMethod.Invoke(ins, paramObjs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
