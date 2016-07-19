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
                    commandType.GetMethod(method).Invoke(ins, new object[] { "hello2", 134 });
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
