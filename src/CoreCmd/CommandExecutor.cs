using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreCmd
{
    public class CommandExecutor
    {
        public void Execute(string[] args)
        {
            Console.WriteLine($"DBG: arg number = {args.Length}");
            if (args.Length > 0)
            {
                string command = $"{args[0]}-command".ToLower();
                string method;
                string[] parameters = new string[] { };

                var allTypes = Assembly.GetEntryAssembly().GetTypes();

                Type targetType = allTypes.SingleOrDefault(t => LowerKebabCase(t.Name).Equals(command));
                if (targetType != null)
                {
                    if(args.Length > 1)
                    {
                        method = args[1];
                        parameters = args.Skip(2).ToArray();
                    }
                    else
                    {
                        method = "default-method";
                    }
                }
                else
                {
                    targetType = allTypes.SingleOrDefault(t => t.Name.Equals("DefaultCommand"));
                    method = args[0];
                    parameters = args.Skip(1).ToArray();
                }

                ExecuteCommand(targetType, method, parameters);
            }
            else
            {
                Console.WriteLine("Subcommand is missing, please specify subcommands"); // [TODO]$0329: need to print all available subcommands here
            }
        }

        private string LowerKebabCase(string inputStr)
        {
            return Regex.Replace(inputStr, @"([a-z])([A-Z])", "$1-$2").ToLower();
        }

        private void ExecuteCommand(Type commandType, string method, string[] parameters)
        {
            try
            {
                if(commandType == null)
                {
                    throw new Exception("Can't find the specified command object.");
                }

                string lowerCaseMethod = method.ToLower();
                var targetMethod = commandType.GetMethods().SingleOrDefault(m => LowerKebabCase(m.Name).Equals(lowerCaseMethod));
                if(targetMethod == null)
                {
                    throw new Exception($"Can't find method: {method}");
                }
                ParameterInfo[] paramInfo = targetMethod.GetParameters();

                if (paramInfo.Length != parameters.Length)
                {
                    throw new Exception(string.Format("Incorrect argument number, command {0}.{1} can accept {2} argument(s).", commandType.Name, method, paramInfo.Length));
                }

                object[] paramObjs = new object[parameters.Length];
                for(int i=0; i< paramInfo.Length; i++)
                {
                    Type type = paramInfo[i].ParameterType;
                    if (type.Equals(typeof(int))) 
                    { 
                        paramObjs[i] = int.Parse(parameters[i]);
                    }
                    else if (type.Equals(typeof(double)))
                    {
                        paramObjs[i] = double.Parse(parameters[i]);
                    }
                    else if (type.Equals(typeof(uint)))
                    {
                        paramObjs[i] = uint.Parse(parameters[i]);
                    }
                    else if (type.Equals(typeof(short)))
                    {
                        paramObjs[i] = short.Parse(parameters[i]);
                    }
                    else if (type.Equals(typeof(ushort)))
                    {
                        paramObjs[i] = ushort.Parse(parameters[i]);
                    }
                    else if (type.Equals(typeof(decimal)))
                    {
                        paramObjs[i] = decimal.Parse(parameters[i]);
                    }
                    else if (type.Equals(typeof(float)))
                    {
                        paramObjs[i] = float.Parse(parameters[i]);
                    }
                    else
                    {
                        paramObjs[i] = parameters[i];
                    }
                }

                var ins = Activator.CreateInstance(commandType);
                targetMethod.Invoke(ins, paramObjs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
