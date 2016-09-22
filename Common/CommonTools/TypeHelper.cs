using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace CommonTools
{
    public static class TypeHelper
    {
        static readonly ConcurrentDictionary<string, string> TableBuffer = new ConcurrentDictionary<string, string>();
        static readonly ConcurrentDictionary<string, IList<string>> ColumnBuffer = new ConcurrentDictionary<string, IList<string>>();


        public static string GetTableName(this object obj, Type type = null)
        {
           
            type = type ?? obj.GetType(); 
            return TableBuffer.GetOrAdd(type.FullName, _ =>
            {
                var attrs = type.GetCustomAttributes(typeof(TableNameAttribute), true);
                var attr = attrs.FirstOrDefault(a => a is TableNameAttribute) as TableNameAttribute;
                if (attr == null)
                    return null;
              return string.Format("[{0}]", attr.Name);
            }); 
        }
        public static IList<string> GetFieldName(this object obj, Type type = null)
        {
            type = type ?? obj.GetType();
            var tableName = GetTableName(null, type);
            if (ColumnBuffer.ContainsKey(type.FullName)) return ColumnBuffer[type.FullName];
            var listColumnName = new List<string>();

            //取属性上的自定义特性
            foreach (var propInfo in type.GetProperties())
            {

                var objAttrs = propInfo.GetCustomAttributes(typeof(MapFieldAttribute), true);
                if (objAttrs.Length > 0)
                {
                    var attr = objAttrs[0] as MapFieldAttribute;
                    if (attr != null)
                    {
                        continue;
                        //listColumnName.Add(tableName + ".[" + attr.MapName + "]"); //列名
                    }
                }
                else
                {
                    var igNos = propInfo.GetCustomAttributes(typeof(MapIgnoreAttribute), true);
                    if (igNos.Length == 0)
                    {
                        listColumnName.Add(tableName + ".[" + propInfo.Name + "]");
                    }
                }
            }
            ColumnBuffer.TryAdd(type.FullName, listColumnName);
            return listColumnName;
        }
    }
}
