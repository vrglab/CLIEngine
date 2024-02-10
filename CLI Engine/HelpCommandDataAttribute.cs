using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIEngine
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class HelpCommandDataAttribute : Attribute
    {
        public string description { get; }
        public string[] requiredInput { get; }
        public string[] validOptionNames { get; set; }
        public string[] validOptionValue { get; set; }

        public HelpCommandDataAttribute(string description, string[] requiredInput)
        {
            this.description = description;
            this.requiredInput = requiredInput; 
        }
    }
}
