using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCmd.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HelpAttribute : Attribute
    {
        private String description;

        public String Description { get { return this.description; } }

        public HelpAttribute(String description_in)
        {
            this.description = description_in;
        }
    }
}
