using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using CoreCmd.CommandExecution;
using CoreCmd.MethodMatching;
using CoreCmd.Help;
using CoreCmd.Attributes;

namespace CoreCmd.CommandExecution
{
    interface ICommandExecutorCreate
    {
        ISingleCommandExecutor GetSingleCommandExecutor(IEnumerable<Type> targetTypes, string[] args);
    }

    internal class CommandExecutorCreator : ICommandExecutorCreate
    {
        private bool CommandMatched(Type commandType, string commandString)
        {
            string commandName = Utils.LowerKebabCase(commandType.Name);
            if (commandName.Equals(commandString))
                return true;

            var aliasInfo = commandType.GetCustomAttribute<AliasAttribute>();
            if (aliasInfo != null && $"{aliasInfo.Alias}-command".ToLower() == commandString)
            {
                Console.WriteLine($"'{commandString}' is an alias command for '{commandName}'");
                return true;
            }

            return false;
        }

        public ISingleCommandExecutor GetSingleCommandExecutor(IEnumerable<Type> targetTypes, string[] args)
        {
            IMethodMatcher _methodMatcher = new MethodMatcher();

            SingleCommandExecutor result = null;
            if (args.Length > 0)
            {
                result = new SingleCommandExecutor();
                string command = $"{args[0]}-command".ToLower();

                Type targetType = targetTypes.SingleOrDefault(t => this.CommandMatched(t,command));
                if (targetType != null)
                {
                    result.CommandClassType = targetType;
                    if (args.Length > 1)
                    {
                        result.MethodSubcommand = args[1];

                        if ( result.MethodSubcommand.ToLower() != "help")   // every command should have a "help" sub-command
                        {
                            // if can't find the subcommand use default subcommand
                            if( _methodMatcher.GetMethodInfo(result.CommandClassType, result.MethodSubcommand).Count() != 0)
                            {
                                result.Parameters = args.Skip(2).ToArray();
                            }
                            else
                            {
                                result.MethodSubcommand = Global.DefaultSubcommandMethodName;
                                result.Parameters = args.Skip(1).ToArray();
                            }
                        }
                    }
                    else
                        result.MethodSubcommand = Global.DefaultSubcommandMethodName;
                }
                else
                {
                    result.CommandClassType = targetTypes.SingleOrDefault(t => t.Name.Equals(Global.DefaultCommandName));
                    result.MethodSubcommand = args[0];
                    result.Parameters = args.Skip(1).ToArray();
                }

                if (result.CommandClassType == null)
                {
                    Console.WriteLine($"Invalid command: {command.Replace("-command","")}");
                    result = null;
                }
            }
            return result;
        }
    }

    
}
