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
            var allClassTypes = GetAllClassTypes();
            if (args.Length > 0)
            {
                string command = $"{args[0]}-command".ToLower();
                string method;
                string[] parameters = new string[] { };

                Type targetType = allClassTypes.SingleOrDefault(t => LowerKebabCase(t.Name).Equals(command));
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
                    targetType = allClassTypes.SingleOrDefault(t => t.Name.Equals("DefaultCommand"));
                    method = args[0];
                    parameters = args.Skip(1).ToArray();
                }

                ExecuteCommand(targetType, method, parameters);
            }
            else
            {
                Console.WriteLine("Subcommand is missing, please specify subcommands:");
                ListAllSubCommands(allClassTypes);
            }
        }

        private IEnumerable<Type> GetAllClassTypes()
        {
            return Assembly.GetEntryAssembly().GetTypes().Where(t => t.IsClass);
        }

        private void ListAllSubCommands(IEnumerable<Type> classTypes)
        {
            var allCommandTypes = classTypes.Where(t => t.Name.EndsWith("Command"));
            foreach(var cmd in allCommandTypes)
            {
                Console.WriteLine(LowerKebabCase(cmd.Name.Substring(0,cmd.Name.Length - "Command".Length)));
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
                    throw new Exception("Command type is null");
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
                    Console.WriteLine(string.Format("Incorrect argument number, command {0}.{1} can accept {2} argument(s).", commandType.Name, method, paramInfo.Length));
                    return;
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
