using System;
using System.Collections.Generic;
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
        public object[] Match(ParameterInfo[] info, string[] parameters)
        {
            object[] result = null;
            if (info.Length == parameters.Length)
            {
                result = new object[info.Length];
                for (int i = 0; i < info.Length; i++)
                {
                    Type type = info[i].ParameterType;
                    if (type.Equals(typeof(string)))
                        result[i] = parameters[i];
                    else if (type.Equals(typeof(char)) && parameters.Length == 1)
                        result[i] = parameters[0];
                    else if (type.Equals(typeof(int)))
                    {
                        if (int.TryParse(parameters[i], out int p))
                            result[i] = p;
                        else
                            return null;
                    }
                    else if (type.Equals(typeof(double)))
                    {
                        if (double.TryParse(parameters[i], out double p))
                            result[i] = p;
                        else
                            return null;
                    }
                    else if (type.Equals(typeof(uint)))
                    {
                        if (uint.TryParse(parameters[i], out uint p))
                            result[i] = p;
                        else
                            return null;
                    }
                    else if (type.Equals(typeof(short)))
                    {
                        if (short.TryParse(parameters[i], out short p))
                            result[i] = p;
                        else
                            return null;
                    }
                    else if (type.Equals(typeof(ushort)))
                    {
                        if (ushort.TryParse(parameters[i], out ushort p))
                            result[i] = p;
                        else
                            return null;
                    }
                    else if (type.Equals(typeof(decimal)))
                    {
                        if (decimal.TryParse(parameters[i], out decimal p))
                            result[i] = p;
                        else
                            return null;
                    }
                    else if (type.Equals(typeof(float)))
                    {
                        if (float.TryParse(parameters[i], out float p))
                            result[i] = p;
                        else
                            return null;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return result;
        }
    }
}
