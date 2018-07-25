using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CoreCmd
{
    public class Utils
    {
        static public string LowerKebabCase(string inputStr)
        {
            return Regex.Replace(inputStr, @"([a-z0-9])([A-Z])", "$1-$2").ToLower();
        }
    }
}
