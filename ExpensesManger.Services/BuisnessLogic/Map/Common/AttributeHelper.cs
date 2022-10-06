using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpensesManager.Services.Map.Common
{
    internal class AttributeHelper
    {
        public static List<string> GetDataNames(Type type, string propertyName)
        {
            object property = type
                           .GetProperty(propertyName)
                           .GetCustomAttributes(false)
                           .Where(x => x.GetType().Name == "DataNamesAttribute")
                           .FirstOrDefault();

            if (property != null)
            {
                return ((DataNamesAttribute)property).ValueNames;
            }
            return new List<string>();
        }
    }
}
