using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ExpensesManager.Services.Map.Common
{
    public class DataNamesMapper<T> where T : class, new()
    {
        public T Map(DataRow row)
        {
            //Step 1 - Get the Column Names
            List<string> columnNames = row.Table.Columns
                                       .Cast<DataColumn>()
                                       .Select(x => x.ColumnName)
                                       .ToList();

            //Step 2 - Get the Property Data Names
            List<PropertyInfo> properties = (typeof(T)).GetProperties()
                                              .Where(x => x.GetCustomAttributes(typeof(DataNamesAttribute), true).Any())
                                              .ToList();

            //Step 3 - Map the data
            T entity = new T();
            foreach (PropertyInfo prop in properties)
            {
                PropertyMapHelper.Map(typeof(T), row, prop, entity);
            }

            return entity;
        }

        public IEnumerable<T> Map(DataTable table)
        {
            List<T> entities = new List<T>();
            List<string> columnNames = table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            List<PropertyInfo> properties = (typeof(T)).GetProperties()
                                              .Where(x => x.GetCustomAttributes(typeof(DataNamesAttribute), true).Any())
                                              .ToList();
            foreach (DataRow row in table.Rows)
            {
                T entity = new T();
                foreach (PropertyInfo prop in properties)
                {
                    PropertyMapHelper.Map(typeof(T), row, prop, entity);
                }
                entities.Add(entity);
            }

            return entities;
        }
    }
}
