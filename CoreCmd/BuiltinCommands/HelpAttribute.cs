using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCmd.BuiltinCommands
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HelpAttribute : Attribute
    {
        public HelpAttribute(String description_in)
        {
            this.description = description_in;
        }

        private String description;
        public String Description { get { return this.description; } }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class OptionalParam : Attribute
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }
        public string[] Values { get; private set; }

        public OptionalParam(string name, Type type, params string[] values)
        {
            this.Name = name;
            this.Type = type;
            this.Values = values;
        }
    }
}
