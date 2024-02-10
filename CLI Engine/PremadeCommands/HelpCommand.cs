using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CLIEngine.PremadeCommands
{
    [Command("help", 0)]
    public class HelpCommand : ICommand
    {
        public void Execute(string[] args, KeyValuePair<string, string>[] options)
        {
            StringBuilder helpData = new StringBuilder();
            foreach (var item in Utils.GetTypesMarkedWithAttrib(typeof(CommandAttribute)))
            {
                CommandAttribute attribute = (CommandAttribute)item.GetCustomAttribute(typeof(CommandAttribute));
                HelpCommandDataAttribute helpDataAttrib = (HelpCommandDataAttribute)item.GetCustomAttribute(typeof(HelpCommandDataAttribute));
                StringBuilder commandData = new StringBuilder();

                commandData.Append(attribute.Name + "\t");
                if (helpDataAttrib != null)
                {
                    if (helpDataAttrib.requiredInput.Length >= attribute.requiredInputAmount)
                    {
                        foreach (var input in helpDataAttrib.requiredInput)
                        {
                            commandData.Append($"[{input}]\t");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < attribute.requiredInputAmount; i++)
                        {
                            commandData.Append("[FIELD]\t");
                        }
                    }
                    

                    if (helpDataAttrib.validOptionNames != null)
                    {
                        for (int i = 0; i < helpDataAttrib.validOptionNames.Length; i++)
                        {
                            if (helpDataAttrib.validOptionValue != null)
                            {
                                if (string.IsNullOrEmpty(helpDataAttrib.validOptionValue[i]))
                                {
                                    commandData.Append($"<{helpDataAttrib.validOptionNames[i]}>\t");
                                }
                                else
                                {
                                    commandData.Append($"<{helpDataAttrib.validOptionNames[i]}:{helpDataAttrib.validOptionValue[i]}>\t");
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var validOption in attribute.validOptions)
                        {
                            commandData.Append($"<{validOption}>\t");
                        }
                    }
                    

                    commandData.Append($"{helpDataAttrib.description}");
                }
                else
                {
                    for (int i = 0; i < attribute.requiredInputAmount; i++)
                    {
                        commandData.Append("[FIELD]\t");
                    }

                    foreach (var validOption in attribute.validOptions)
                    {
                        commandData.Append($"<{validOption}>\t");
                    }
                }

                
                helpData.AppendLine(commandData.ToString());
            }
            Console.WriteLine(helpData.ToString());
        }
    }
}
