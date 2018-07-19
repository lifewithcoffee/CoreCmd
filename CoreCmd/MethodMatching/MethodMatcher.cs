using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoreCmd.MethodMatching
{
    public interface IMethodMatcher
    {
        IEnumerable<MethodInfo> GetMethodInfo(Type classType, string methodName);
    }

    public class MethodMatcher : IMethodMatcher
    {
        /// <param name="classType">The host class type of the method.</param>
        /// <param name="methodName">Must be the lower kebab-case.</param>
        /// <returns>Null if not find, otherwise return the relevant MethodInfo objects.</returns>
        public IEnumerable<MethodInfo> GetMethodInfo(Type classType, string methodName)
        {
            return classType.GetMethods().Where(m => Utils.LowerKebabCase(m.Name).Equals(methodName));
        }
    }
}
