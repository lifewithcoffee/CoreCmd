using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CoreCmd
{
    class Utils
    {
        static public string LowerKebabCase(string inputStr)
        {
            return Regex.Replace(inputStr, @"([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}
