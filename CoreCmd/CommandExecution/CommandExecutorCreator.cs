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
    public class CommandExecutorCreator
    {
        private bool CommandMatched(Type commandType, string commandString)
        {
            string commandName = Utils.LowerKebabCase(commandType.Name);
            if (commandName.Equals(commandString))
                return true;

            var aliasInfo = commandType.GetCustomAttribute<AliasAttribute>();
            if (aliasInfo != null && $"{aliasInfo.Alias}-command".ToLower() == commandString)
                return true;

            return false;
        }

        public SingleCommandExecutor GetSingleCommandExecutor(IEnumerable<Type> targetTypes, string[] args)
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
                            result.MethodInfo = _methodMatcher.GetMethodInfo(result.CommandClassType, result.MethodSubcommand);
                            if( result.MethodInfo.Count() != 0 )
                            {
                                // this is a normal non-default subcommand
                                result.Parameters = args.Skip(2).ToArray();
                            }
                            else
                            {
                                // can't find the specified subcommand, try to find the "default" subcommand instead
                                result.MethodSubcommand = Global.DefaultSubcommandMethodName;
                                result.MethodInfo = _methodMatcher.GetMethodInfo(result.CommandClassType, result.MethodSubcommand);
                                result.Parameters = args.Skip(1).ToArray();
                            }
                        }
                    }
                    else
                    {
                        result.MethodSubcommand = Global.DefaultSubcommandMethodName;
                        result.MethodInfo = _methodMatcher.GetMethodInfo(result.CommandClassType, result.MethodSubcommand);
                    }
                }
                else
                {
                    // try to find a DefaultComand class
                    result.CommandClassType = targetTypes.SingleOrDefault(t => t.Name.Equals(Global.DefaultCommandName));
                    if(result.CommandClassType != null)
                    {
                        result.MethodSubcommand = args[0];
                        result.MethodInfo = _methodMatcher.GetMethodInfo(result.CommandClassType, result.MethodSubcommand);
                        result.Parameters = args.Skip(1).ToArray();
                    }
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
