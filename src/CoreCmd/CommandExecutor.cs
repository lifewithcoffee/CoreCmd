using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CoreCmd
{
    public class CommandExecutor
    {
        public void Execute(string[] args)
        {
            if (args.Length > 0)
            {
                string command = $"{args[0]}Command".ToLower();
                string method;
                string[] parameters;

                
                Type targetType = Assembly.GetEntryAssembly().GetTypes().SingleOrDefault(t => t.Name.ToLower().Equals(command));
                if (targetType != null)
                {
                    method = args[1];
                    parameters = args.Skip(2).ToArray();
                }
                else
                {
                    command = "DefaultCommand";
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

        private void ExecuteCommand(Type commandType, string method, string[] parameters)
        {
            try
            {

                var ins = Activator.CreateInstance(commandType);

                object[] paramObjs = new object[parameters.Length];
                ParameterInfo[] paramInfo = commandType.GetMethod(method).GetParameters();

                if (paramInfo.Length != parameters.Length)
                {
                    throw new Exception(string.Format("Incorrect argument number, command {0}.{1} can accept {2} argument(s).", commandType.Name, method, paramInfo.Length));
                }

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

                commandType.GetMethod(method).Invoke(ins, paramObjs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
