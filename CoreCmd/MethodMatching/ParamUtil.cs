using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreCmd.MethodMatching
{
    public interface IParamUtil
    {
        (List<string>, List<string>) GroupdParameters(string[] parameters);
        Dictionary<string, string> GetOptionalParamDict(List<string> optionalParamList);
    }

    public class ParamUtil : IParamUtil
    {
        public (List<string>, List<string>) GroupdParameters(string[] parameters)
        {
            List<string> requiredParameters = new List<string>();
            List<string> optionalParameters = new List<string>();

            foreach (var p in parameters)
            {
                if (p.StartsWith("-"))
                    optionalParameters.Add(p);
                else
                    requiredParameters.Add(p);
            }

            return (requiredParameters, optionalParameters);
        }


        /// <summary>
        /// - All optional parameters start with '-'
        /// - It doesn't matter how many '-'s are at the beginning of the parameter string
        /// - If found multiple parameters have the same key, an exception will be thrown out
        /// </summary>
        public Dictionary<string, string> GetOptionalParamDict(List<string> optionalParamList)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            var paramsWithoutLeadingDash = optionalParamList.Select(p => p.TrimStart('-'));
            foreach (var param in paramsWithoutLeadingDash)
            {
                int index = param.IndexOf(':');

                if (index < 0)
                    index = param.Length;

                string key = Utils.LowerKebabCase(param.Substring(0, index));
                if (result.ContainsKey(key))
                    throw new Exception($"Duplicated key '{key}' found when parsing optional parameters");
                else if (index == param.Length)
                    result[key] = "";
                else
                {
                    var value = param.Substring(index + 1, param.Length - index - 1).Trim();
                    if (value.StartsWith("'") && value.EndsWith("'")) // strip single quotes (') from a string of 'xxx'
                    {
                        value = value.TrimStart('\'');
                        value = value.TrimEnd('\'');
                    }
                    else if (value.StartsWith("\"") && value.EndsWith("\""))    // strip double quotes (") from a string of "xxx"
                    {
                        value = value.TrimStart('"');
                        value = value.TrimEnd('"');
                    }

                    if (string.IsNullOrWhiteSpace(value))
                        value = "";
                    result[key] = value;
                }
            }
            return result;
        }
    }

}
