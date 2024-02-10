using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIEngine
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class CommandAttribute : Attribute
    {
        public string Name { get; }
        public int requiredInputAmount { get; }
        public string[] validOptions { get;}
        public CommandAttribute(string command_name, int requiredInputAmount,params string[] validOptions)
        {
            Name = command_name;
            this.requiredInputAmount = requiredInputAmount;
            this.validOptions = validOptions;
        }
    }
}
