using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpensesManager.Services.Map.Common
{
  
        [AttributeUsage(AttributeTargets.Property)]
        public class DataNamesAttribute : Attribute
        {
            protected List<string> valueNames { get; set; }

            public List<string> ValueNames
            {
                get
                {
                    return valueNames;
                }
                set
                {
                    valueNames = value;
                }
            }

            public DataNamesAttribute()
            {
                valueNames = new List<string>();
            }

            public DataNamesAttribute(params string[] valueNames)
            {
                this.valueNames = valueNames.ToList();
            }
        }
}