using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CoreCmd
{
    public class CommandExecutor
    {
        ICommandFinder _commandFinder = new CommandFinder();

        private TargetCommandObject GetTargetCommandObject(IEnumerable<Type> targetTypes, string[] args)
        {
            TargetCommandObject result = null;
            if (args.Length > 0)
            {
                result = new TargetCommandObject();
                string command = $"{args[0]}-command".ToLower();

                Type targetType = targetTypes.SingleOrDefault(t => Utils.LowerKebabCase(t.Name).Equals(command));
                if (targetType != null)
                {
                    result.CommandType = targetType;
                    if (args.Length > 1)
                    {
                        result.MethodName = args[1];
                        result.Parameters = args.Skip(2).ToArray();
                    }
                    else
                        result.MethodName = "default-method";
                }
                else
                {
                    result.CommandType = targetTypes.SingleOrDefault(t => t.Name.Equals("DefaultCommand"));
                    result.MethodName = args[0];
                    result.Parameters = args.Skip(1).ToArray();
                }
            }
            return result;
        }

        public void Execute(string[] args)
        {
            try
            {
                const string commandPostfix = "command";
                var allClassTypes = _commandFinder.GetCommandClassTypes(commandPostfix);
                if (args.Length > 0)
                {
                    var targetCommand = this.GetTargetCommandObject(allClassTypes, args);

                    if(targetCommand != null)
                        targetCommand.Execute();
                    else
                        Console.WriteLine("No command object found");
                }
                else
                {
                    Console.WriteLine("Subcommand is missing, please specify subcommands:");

                    // print all available commands
                    foreach(var cmd in allClassTypes)
                        Console.WriteLine(Utils.LowerKebabCase(cmd.Name.Substring(0,cmd.Name.Length - commandPostfix.Length)));
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        
    }
}
