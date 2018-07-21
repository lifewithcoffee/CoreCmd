using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoreCmd.MethodMatching
{
    interface IParameterMatcher
    {
        object[] Match(ParameterInfo[] info, string[] parameters);
    }

    class ParameterMatcher : IParameterMatcher
    {
        private object MatchOneParameter(Type type, string parameter)
        {
            object result = null;

            if (type.Equals(typeof(string)))
                result = parameter;
            else if (type.Equals(typeof(char)) && parameter.Length == 1)
                result = parameter[0];
            else if (type.Equals(typeof(int)))
            {
                if (int.TryParse(parameter, out int p))
                    result = p;
            }
            else if (type.Equals(typeof(double)))
            {
                if (double.TryParse(parameter, out double p))
                    result = p;
            }
            else if (type.Equals(typeof(uint)))
            {
                if (uint.TryParse(parameter, out uint p))
                    result = p;
            }
            else if (type.Equals(typeof(short)))
            {
                if (short.TryParse(parameter, out short p))
                    result = p;
            }
            else if (type.Equals(typeof(ushort)))
            {
                if (ushort.TryParse(parameter, out ushort p))
                    result = p;
            }
            else if (type.Equals(typeof(decimal)))
            {
                if (decimal.TryParse(parameter, out decimal p))
                    result = p;
            }
            else if (type.Equals(typeof(float)))
            {
                if (float.TryParse(parameter, out float p))
                    result = p;
            }

            return result;
        }

        private IParamUtil _paramUtil = new ParamUtil();

        // All optional parameters start with '-'
        private object MatchOptionalParameters(ParameterInfo info, List<string> optionalParameters)
        {
            var dict = _paramUtil.GetOptionalParamDict(optionalParameters);
            if (dict == null)
                return null;
            else
            {
                string parameterName = info.Name;
                var param = dict.Where(d => parameterName.StartsWith(d.Key)).SingleOrDefault();
                if (param.Equals(default(KeyValuePair<string, string>)))
                    return MatchOneParameter(info.ParameterType, param.Value);
                else
                    return info.DefaultValue ?? Type.Missing;
            }
        }

        public object[] Match(ParameterInfo[] info, string[] parameters)
        {
            List<object> result = new List<object>();
            (List<string> required, List<string> optional) = _paramUtil.GroupdParameters(parameters);

            int requiredParamNumber = info.Where(i => !i.HasDefaultValue).Count(); // parameters with default values are optional
            int requiredParamCount = required.Count();

            bool requiredParamMatched = false;
            int paramProcessedCount = 0;

            // match required parameters
            if (requiredParamCount == requiredParamNumber)
            {
                foreach (string param in required)
                {
                    var paramObj = MatchOneParameter(info[paramProcessedCount].ParameterType, param);
                    if (paramObj != null)
                        result.Add(paramObj);
                    else
                        return null;

                    paramProcessedCount++;
                }
                requiredParamMatched = true;
            }
            else
                return null;

            // match optional parameters, i.e. parameters with defaul values
            if(requiredParamMatched)
            {
                var optionalParams = info.Where(i => i.HasDefaultValue);
                foreach(var param in optionalParams)
                {
                    var paramObj = MatchOptionalParameters(info[paramProcessedCount], optional);

                    if (paramObj != null)
                        result.Add(paramObj);
                    else
                        result.Add(info[paramProcessedCount].DefaultValue ?? Type.Missing);

                    paramProcessedCount++;
                }
            }

            // must return null if no result got
            if (result.Count() == 0)
                return null;
            else
                return result.ToArray();
        }
    }
}
