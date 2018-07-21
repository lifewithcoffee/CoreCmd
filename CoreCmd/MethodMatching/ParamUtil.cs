using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreCmd.MethodMatching
{
    interface IParamUtil
    {
        (List<string>, List<string>) GroupdParameters(string[] parameters);
        Dictionary<string, string> GetOptionalParamDict(List<string> optionalParamList);
    }

    class ParamUtil : IParamUtil
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

        // All optional parameters start with '-'
        // if found multiple parameters have the same key, an exception will be thrown out
        public Dictionary<string, string> GetOptionalParamDict(List<string> optionalParamList)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            var paramsWithoutLeadingDash = optionalParamList.Select(p => p.Substring(1, p.Length));
            foreach (var param in paramsWithoutLeadingDash)
            {
                int index = param.IndexOf(':');

                if (index < 0)
                    index = param.Length - 1;

                string key = param.Substring(0, index);
                if (result.ContainsKey(key))
                    throw new Exception($"Duplicated key '{key}' found when parsing optional parameters");
                else if (index == param.Length - 1)
                    result[key] = "";
                else
                    result[key] = param.Substring(index + 1, param.Length);
            }
            return null;
        }
    }

}
