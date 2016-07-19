using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CoreCmd.Commands
{
    public class CommandExecutor
    {
        internal void Execute(string[] args)
        {
            if (ValidateArguments(args))
            {
                string command = args[0];
                string method = args[1];

                string[] parameters = args.Skip(2).ToArray();
                ExecuteCommand(command, method, parameters);
            }
            else
            {
                throw new NotImplementedException();
            }

        }

        private bool ValidateArguments(string[] args)
        {
            return args.Count() > 0;
        }

        private void ExecuteCommand(string command, string method, string[] parameters)
        {
            try
            {
                if (FindCommand(command))
                {
                    string fullTypeWithNamespace = string.Format("{0}.Commands.{1}", typeof(Program).Namespace, command);
                    Type commandType = Type.GetType(fullTypeWithNamespace);
                    var ins = Activator.CreateInstance(commandType);

                    object[] paramObjs = new object[parameters.Length];
                    ParameterInfo[] paramInfo = commandType.GetMethod(method).GetParameters();

                    if (paramInfo.Length != parameters.Length)
                    {
                        throw new Exception(string.Format("Incorrect argument number, command {0}.{1} can accept {2} argument(s).", command, method, paramInfo.Length));
                    }

                    for(int i=0; i< paramInfo.Length; i++)
                    {
                        switch (Type.GetTypeCode(paramInfo[i].ParameterType))
                        {
                            case TypeCode.Byte:
                            case TypeCode.SByte:
                            case TypeCode.UInt16:
                            case TypeCode.UInt32:
                            case TypeCode.UInt64:
                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.Decimal:
                            case TypeCode.Double:
                            case TypeCode.Single:
                                paramObjs[i] = int.Parse(parameters[i]);
                                break;
                            default:
                                paramObjs[i] = parameters[i];
                                break;
                        }
                    }

                    commandType.GetMethod(method).Invoke(ins, paramObjs);
                }
                else
                {
                    Console.WriteLine("Command {0} not found", command);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private bool FindCommand(string command)
        {
            Console.WriteLine("FindCommand: not implemented");
            return true;
        }
    }
}
