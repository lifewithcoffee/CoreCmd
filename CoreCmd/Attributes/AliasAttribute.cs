using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCmd.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AliasAttribute : Attribute
    {
        private string _alias;

        public string Alias { get { return _alias; } }

        public AliasAttribute(string alias)
        {
            _alias = alias;
        }
    }
}
