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

        public object[] Match(ParameterInfo[] info, string[] parameters)
        {
            object[] result = null;

            int minParamNumber = info.Where(i => !i.HasDefaultValue).Count(); // parameters with default values are optional

            if(parameters.Length >= minParamNumber && parameters.Length <= info.Length)
            {
                result = new object[info.Length];
                for (int i = 0; i < info.Length; i++)
                {
                    if (i >= parameters.Length) // these missing parameters may have default values
                        result[i] = info[i].DefaultValue??Type.Missing;
                    else
                    {
                        result[i] = MatchOneParameter(info[i].ParameterType, parameters[i]);
                        if (result[i] == null)
                            return null;
                    }
                }
            }
            return result;
        }
    }
}
