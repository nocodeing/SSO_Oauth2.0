using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace CommonTools
{
    public class DataTableHelper
    {
        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T">datatable对应序列的实体</typeparam>
        /// <param name="dt">datatable</param>
        /// <returns>转换后实体的list</returns>
        public static List<T> List<T>(DataTable dt)
        {
            var list = new List<T>();
            //Type t = typeof(T);
            var plist =
                new List<PropertyInfo>(
                    typeof(T).GetProperties(BindingFlags.IgnoreReturn | BindingFlags.Public | BindingFlags.Instance));

            foreach (DataRow item in dt.Rows)
            {
                T t = Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name.ToUpper() == dt.Columns[i].ColumnName.ToUpper());
                    if (info == null) continue;
                    if (Convert.IsDBNull(item[i])) continue;
                    var type = item[i].GetType().Name;   
                    switch (type)
                    {
                        case "Int64":
                            info.SetValue(t, Convert.ToInt32(item[i]));
                            break;
                        case "Double":
                            info.SetValue(t, Convert.ToSingle(item[i]));
                            break;                       
                        default:
                            info.SetValue(t,item[i]);
                            break;
                    }
                }
                list.Add(t);
            }
            return list;
        }

    }
}