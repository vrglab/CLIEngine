using CLIEngine.PremadeCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CLIEngine
{
    public class CommandManager
    {
        public static void ProcessCommands(string[] args)
        {
            bool foundCommand = false;
            foreach (var item in Utils.GetTypesMarkedWithAttrib(typeof(CommandAttribute)))
            {
                var object_Created = (ICommand)item.GetConstructors()[0].Invoke(new object[] { });
                var attribute = (CommandAttribute)item.GetCustomAttribute(typeof(CommandAttribute));

                if (args != null)
                {
                    if (args.Length > 0)
                    {
                        if (args[0] == attribute.Name)
                        {
                            foundCommand = true;
                            List<string> commandArgs = new List<string>();
                            for (int i = 1; i < args.Length; i++)
                            {
                                if (!args[i].StartsWith("--"))
                                {
                                    commandArgs.Add(args[i]);
                                }
                            }

                            if (commandArgs.Count < attribute.requiredInputAmount)
                            {
                                Console.Error.WriteLine("Required command field's not provided");
                                return;
                            }

                            List<KeyValuePair<string, string>> commandOptions = new List<KeyValuePair<string, string>>();
                            for (int i = 1; i < args.Length; i++)
                            {
                                if (args[i].StartsWith("--"))
                                {
                                    string[] splitOption = args[i].Split('=');
                                    string optionName = splitOption[0].Replace("--", "");
                                    if (attribute.validOptions.Contains(optionName))
                                    {
                                        try
                                        {
                                            commandOptions.Add(new KeyValuePair<string, string>(optionName, splitOption[1]));
                                        }
                                        catch (Exception e)
                                        {
                                            commandOptions.Add(new KeyValuePair<string, string>(optionName, ""));
                                        }
                                    }
                                    else
                                    {
                                        Console.Error.WriteLine($"Option \"{args[i]}\" is not a valid option");
                                        return;
                                    }
                                }
                            }
                            object_Created.Execute(commandArgs.ToArray(), commandOptions.ToArray());
                            return;
                        }
                    }
                }
            }

            if (foundCommand == false && args.Length > 0)
            {
                HelpCommand helpCommand = new HelpCommand();
                Console.Error.WriteLine($"{args[0]} is not a valid command");
                helpCommand.Execute(new string[] { }, new KeyValuePair<string, string>[] { });
            }
            else
            {
                HelpCommand helpCommand = new HelpCommand();
                Console.Error.WriteLine("No command provided");
                helpCommand.Execute(new string[] { }, new KeyValuePair<string, string>[] { });
            }
        }
    }
}
