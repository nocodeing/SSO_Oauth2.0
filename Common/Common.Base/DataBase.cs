using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Buffer;
using CommonTools;
using Model;

namespace Common.Base
{
    /// <summary>
    /// 数据访问层基类
    /// </summary>
    public abstract class DataBase
    {


        private static string _modelNameSpace;

        protected DataBase()
        {
            if (_modelNameSpace == null)
            {
                _modelNameSpace = System.Configuration.ConfigurationManager.AppSettings["ModelNameSpace"].ToUpper();
            }
        }

        // private DbManager _dbManager;
        public DbManager DbManager
        {
            get { return (DbManager)CallContext.GetData("DatabaseManager"); }
            set { CallContext.SetData("DatabaseManager", value); }
        }
        public string SqlText { set; get; }

        public string TableName
        {
            get { return GetType().GetTableName(GetModelType(this)); }
        }

        public string Fields
        {
            get { return string.Join(",", GetType().GetFieldName(GetModelType(this))); }
        }

        private static IList<Type> _modelTypes;

        private static object _syncRoot = new object();

        private static IEnumerable<Type> GetModelTypes()
        {
            if (_modelTypes == null)
            {
                lock (_syncRoot)
                {
                    if (_modelTypes == null)
                    {
                        var assembly = AppDomain.CurrentDomain.GetAssemblies()
                            .FirstOrDefault(asm => asm.GetName().Name.ToUpper() == _modelNameSpace);
                        if (assembly != null)
                        {
                            _modelTypes = assembly.GetTypes();
                        }
                    }
                }
            }

            return _modelTypes;


        }

        private static Type GetModelType(object repositoryObject)
        {
            var typeName = repositoryObject.GetType().Name;
            var modelName = typeName.ToUpper().Replace("REPOSITORY", "");
            var types = GetModelTypes();
            var type = types.FirstOrDefault(t => t.Name.ToUpper() == modelName);
            return type;
        }

        public IDbDataParameter[] Parameters { set; get; }

        public List<IDbDataParameter> ListParameters { get; set; }

        protected string FieldsToParameters(string fields)
        {
            fields = fields.ToUpper().Replace(string.Format("{0}.", TableName.ToUpper()), "");
            var paramers = BufferHelp.Get<string>(fields);
            if (paramers == null)
            {
                var columns = fields.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (var i = 0; i < columns.Length; i++)
                {
                    columns[i] = "@" + columns[i].Replace("[", "").Replace("]", "").Trim();
                }
                paramers = string.Join(",", columns);
                BufferHelp.Add(fields, paramers, int.MaxValue);
            }
            return paramers;
        }

        private string GetModelPkId()
        {
            var pKey = string.Format("{0}PKID", GetType().FullName);
            var pkid = BufferHelp.Get<string>(pKey);
            if (pkid == null)
            {
                var type = GetModelType(this);
                var memberInfos = type.GetMembers();

                foreach (var memberInfo in memberInfos)
                {
                    var atts = memberInfo.GetCustomAttributes(false);
                    if (atts.Length == 0) continue;
                    if (atts.OfType<PrimaryKeyAttribute>().Any())
                    {
                        pkid = memberInfo.Name;
                    }
                }
            }
            BufferHelp.Add(pKey, pkid, int.MaxValue);
            return pkid;
        }


        /// <summary>
        /// 字段字符串转换update字段以及参数列表
        /// </summary>
        /// <param name="fields">字段字符串</param>
        /// <param name="ignoreFields">忽略字段</param>
        /// <returns>转换后的字符串</returns>
        protected string GetUpdateFieldsAndParmeters(string fields, string ignoreFields)
        {
            fields = fields.ToUpper().Replace(string.Format("{0}.", TableName.ToUpper()), "");
            var sourceArray = fields.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var ignoreArray = ignoreFields.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            sourceArray = sourceArray.Select(s => s.Trim().ToUpper()).ToArray();
            ignoreArray = ignoreArray.Select(i => i.Trim().ToUpper()).ToArray();
            sourceArray =
                sourceArray.Where(
                    s => !ignoreArray.ToList().Exists(n => TrimChar(n, "[", "]") == TrimChar(s, "[", "]"))).ToArray();
            sourceArray =
                sourceArray.Select(s => string.Format(" {0}=@{1}", s, TrimChar(s, "[", "]"))).ToArray();
            return " SET " + string.Join(",", sourceArray);
        }

        protected string GetUpdateFieldsAndParmeters<TEntity>(TEntity t, List<string> ignoreFieldList) where TEntity : class
        {
            var type = t.GetType();
            var propertys = type.GetProperties();
            var memberAccess = DynamicMethodMemberAccessor.GeteMemberAccessor();
            var tempList = new List<string>();

            string where = GetWhere(t, ignoreFieldList);
            Parameters = GetValueSqlParameters(t);
            SqlText = string.Format("SELECT {0} FROM {1} {2}", Fields, TableName, where);
            var entity = DbManager.SetCommand(SqlText, Parameters).ExecuteObject<TEntity>();
            foreach (var pro in propertys)
            {
                if (ignoreFieldList.Exists(
                    i => i.Equals(pro.Name)) || typeof(ModelBase).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).ToList().Exists(m => m.Name.Contains(pro.Name)))
                    continue;
                var value = memberAccess.GetValue(t, pro.Name) ?? DBNull.Value;
                if (value == DBNull.Value) continue;
                //比对待修改属性值与数据库中数据是否相同，如果相同则不进行修改
                var isUnUpdate =
                    entity.GetType()
                        .GetProperties()
                        .Any(x => x.Name == pro.Name && (memberAccess.GetValue(entity, pro.Name) ?? DBNull.Value) == value);
                if (isUnUpdate)
                {
                    continue;
                }
                //var tempValueType = value.GetType();
                //object obj = tempValueType.IsValueType ? Activator.CreateInstance(tempValueType) : null;

                //if (obj != null && obj.Equals(value))
                //{
                //    memberAccess.SetValue(t, pro.Name, memberAccess.GetValue(entity, pro.Name));
                //}
                tempList.Add(" [" + pro.Name + "] = @" + pro.Name);
            }
            return " SET " + string.Join(",", tempList);
        }

        private string TrimChar(string source, params string[] chars)
        {
            return chars.Aggregate(source, (current, c) => current.Replace(c, ""));
        }

        //获得实体的参数化集合
        protected IDbDataParameter[] GetSqlParameters(object obj)
        {
            var type = obj.GetType();
            var propertys = type.GetProperties();
            var memberAccess = DynamicMethodMemberAccessor.GeteMemberAccessor();
            // ReSharper disable once CoVariantArrayConversion
            return propertys.Select(pro =>
            {
                var value = memberAccess.GetValue(obj, pro.Name) ?? DBNull.Value;
                if (value == DBNull.Value)
                {
                    var nullAttrs = pro.GetCustomAttributes(typeof(NullableAttribute), true);
                    if (nullAttrs.Length == 0)
                    {
                        if (pro.PropertyType == typeof(string))
                            value = "";
                    }
                }
                var ignoreAttr = pro.GetCustomAttributes(typeof(MapIgnoreAttribute), true);
                if (ignoreAttr.Length != 0)
                {
                    return null;
                }
                return new SqlParameter("@" + pro.Name.Trim().ToUpper(), value);
            }).Where(p => p != null).ToArray();
        }
        protected IDbDataParameter[] GetInsertSqlParameters(object obj)
        {
            var type = obj.GetType();
            var propertys = type.GetProperties();
            var memberAccess = DynamicMethodMemberAccessor.GeteMemberAccessor();
            // ReSharper disable once CoVariantArrayConversion
            return propertys.Select(pro =>
            {
                var value = memberAccess.GetValue(obj, pro.Name) ?? DBNull.Value;
                if (value == DBNull.Value)
                {
                    var nullAttrs = pro.GetCustomAttributes(typeof(NullableAttribute), true);
                    if (nullAttrs.Length == 0)
                    {
                        if (pro.PropertyType == typeof(string))
                            value = "";
                    }
                }
                var ignoreAttr = pro.GetCustomAttributes(typeof(MapIgnoreAttribute), true);
                if (ignoreAttr.Length != 0)
                {
                    return null;
                }
                return new SqlParameter("@" + pro.Name.Trim().ToUpper(), value);
            }).Where(p => p != null).ToArray();
        }
        /// <summary>
        /// 获得实体中存在值的参数化集合
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected IDbDataParameter[] GetValueSqlParameters(object obj)
        {
            var type = obj.GetType();
            var propertys = type.GetProperties();
            var memberAccess = DynamicMethodMemberAccessor.GeteMemberAccessor();

            // ReSharper disable once CoVariantArrayConversion
            return propertys.Select(pro =>
            {
                var value = memberAccess.GetValue(obj, pro.Name) ?? DBNull.Value;
                if (value == DBNull.Value)
                {
                    return null;
                }
                var ignoreAttr = pro.GetCustomAttributes(typeof(MapIgnoreAttribute), true);
                if (ignoreAttr.Length != 0)
                {
                    return null;
                }
                return new SqlParameter("@" + pro.Name.Trim().ToUpper(), value);
            }).Where(p => p != null).ToArray();
        }
        /// <summary>
        /// 根据属性获取where条件
        /// </summary>
        /// <typeparam name="TEntity">属性对应实体类型</typeparam>
        /// <param name="t">属性对应实体</param>
        /// <param name="propertyList">属性名</param>
        /// <returns>where条件</returns>
        private string GetWhere<TEntity>(TEntity t, IEnumerable<string> propertyList) where TEntity : class
        {
            var pList = propertyList.ToList();
            if (!Check(t, pList)) return " ";

            var tempList = pList.Select(pro => " [" + pro + "] = @" + pro).ToList();
            return " WHERE " + string.Join(" AND ", tempList);
        }

        public int Insert<TEntity>(TEntity entity) where TEntity : class
        {
            var insertParms = FieldsToParameters(Fields);
            SqlText = string.Format("INSERT INTO {0}({1}) VALUES({2})", TableName, Fields, insertParms);
            Parameters = GetInsertSqlParameters(entity);



            return DbManager.SetCommand(SqlText, Parameters).ExecuteNonQuery();
        }

        public int Update<TEntity>(TEntity entity) where TEntity : class
        {
            var pkId = GetModelPkId();
            var setFields = GetUpdateFieldsAndParmeters(Fields, pkId);
            SqlText = string.Format("UPDATE {0} {1} WHERE {2}=@{2}", TableName, setFields, pkId);
            Parameters = GetSqlParameters(entity);
            return
                DbManager.SetCommand(SqlText, Parameters).ExecuteNonQuery();
        }

        /// <summary>
        /// 根据条件进行修改
        /// </summary>
        /// <typeparam name="TEntity">实体对象类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="condition">指定实体中哪个字段作为修改条件（仅支持“=”）</param>
        /// <returns>修改结果</returns>
        public int Update<TEntity>(TEntity entity, string condition) where TEntity : class
        {
            if (!Check(entity, new List<string>() { condition })) return 0;
            var setFields = GetUpdateFieldsAndParmeters(entity, new List<string> { condition });
            SqlText = string.Format("UPDATE {0} {1} WHERE {2}=@{2}", TableName, setFields, condition);
            Parameters = GetValueSqlParameters(entity);
            return DbManager.SetCommand(SqlText, Parameters).ExecuteNonQuery();
        }

        /// <summary>
        /// 根据条件进行修改
        /// </summary>
        /// <typeparam name="TEntity">实体对象类型</typeparam>
        /// <param name="t">实体对象</param>
        /// <param name="propertyList">指定实体中哪个字段作为修改条件（仅支持“=”）</param>
        /// <returns>修改结果</returns>
        public int Update<TEntity>(TEntity t, IEnumerable<string> propertyList) where TEntity : class
        {
            var propertyListTemp = propertyList.ToList();
            string where = GetWhere(t, propertyListTemp);
            var setFields = GetUpdateFieldsAndParmeters(t, propertyListTemp);

            SqlText = string.Format("UPDATE {0} {1} {2}", TableName, setFields, where);
            Parameters = GetValueSqlParameters(t);
            return DbManager.SetCommand(SqlText, Parameters).ExecuteNonQuery();
        }


        /// <summary>
        /// 判断字段是否存在于实体内
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="t"></param>
        /// <param name="propertyList"></param>
        /// <returns></returns>
        private bool Check<TEntity>(TEntity t, IEnumerable<string> propertyList)
        {
            var type = t.GetType();
            var properties = type.GetProperties().ToList();
            return propertyList.All(property => properties.Exists(p => p.Name == property));
        }

        public IList<TEntity> FindAll<TEntity>(bool isCache = false) where TEntity : class
        {
            return FindAll<TEntity>(10, isCache);
        }

        public IList<TEntity> FindAll<TEntity>(int bufferTime, bool isCache = false) where TEntity : class
        {
            SqlText = string.Format("SELECT {0} FROM {1}", Fields, TableName);
            if (!isCache)
            {
                var list = DbManager.SetCommand(SqlText).ExecuteList<TEntity>();
                return list;
            }
            var entitys = BufferHelp.Get<IList<TEntity>>(string.Format("ALL{0}", typeof(TEntity).Name));
            if (entitys != null) return entitys;
            entitys = DbManager.SetCommand(SqlText).ExecuteList<TEntity>();
            BufferHelp.Add(string.Format("ALL{0}", typeof(TEntity).Name), entitys, bufferTime);
            return entitys;
        }

        public IList<TEntity> FindAll<TEntity>(string currentValue, int endLevel, int startLevel) where TEntity : class
        {
            SqlText = string.Format("with temp as" +
                                    "(" +
                                    "select *,0 level from {3} where Id = '{0}' " +
                                    "union all " +
                                    "select m.*,level+1 from temp c inner join {3}  m " +
                                    "on c.Referrer = m.id where level<{1}  and m.TypeValue=3 AND m.Status=1" +
                                    ")" +
                                    "select * from temp where level>={2}", currentValue, endLevel, startLevel, TableName);
            var entitys = DbManager.SetCommand(SqlText).ExecuteList<TEntity>();
            return entitys;
        }

        public IList<TEntity> FindAll<TEntity>(int pageIndex, int pageSize, string sortFiled, string sortDir, string where) where TEntity : class
        {
            SqlText = string.Format(@"BEGIN
                                    DECLARE @PAGE_INDEX INT
                                    SET @PAGE_INDEX={0}
                                    DECLARE @PAGE_SIZE INT 
                                    SET @PAGE_SIZE={1}
                                    ;WITH TMP AS
                                    (
                                    SELECT {3},
                                    ROW_NUMBER() OVER(ORDER BY {2} {6}) PAGEINDEX, 
                                    COUNT(*) OVER() TOTALNUMBER
                                    FROM {4} where 1=1 {5}
                                    )
                                    SELECT *,TOTALNUMBER
                                    FROM TMP
                                    WHERE PAGEINDEX BETWEEN (@PAGE_SIZE * (@PAGE_INDEX-1)+1) AND @PAGE_SIZE * @PAGE_INDEX ORDER BY PAGEINDEX
                                    END", pageIndex, pageSize, sortFiled, Fields, TableName, where, sortDir);

            var entitys = DbManager.SetCommand(SqlText).ExecuteDataTable();

            return DataTableHelper.List<TEntity>(entitys);
        }
        public IList<TEntity> FindAll<TEntity>(List<string> idList) where TEntity : class
        {
            SqlText = string.Format("SELECT {0} FROM {1} WHERE ID in ('{2}')", Fields, TableName, string.Join("','", idList));
            return DbManager.SetCommand(SqlText).ExecuteList<TEntity>();
        }

        public TEntity Find<TEntity>(string id) where TEntity : class
        {
            SqlText = string.Format("SELECT {0} FROM {1} WHERE ID='{2}'", Fields, TableName, id);
            return DbManager.SetCommand(SqlText).ExecuteObject<TEntity>();
        }

        public int Delete(object id)
        {
            var pkId = GetModelPkId();
            SqlText = string.Format("DELETE {0} WHERE {1}=@{1}", TableName, pkId);
            return DbManager.SetCommand(SqlText, new SqlParameter("@" + pkId, id)).ExecuteNonQuery();
        }

        public int InsertBatch<T>(IEnumerable<T> entities)
        {
            return DbManager.InsertBatch(entities);
        }

        public int InsertOrReplaceBatch<T>(IEnumerable<T> entities)
        {
            return DbManager.InsertOrReplace(entities);
        }

        /// <summary>
        /// 用于生成序列号
        /// </summary>
        /// <param name="prefix">序列前缀</param>
        /// <param name="length">包含前缀的序列号长度</param>
        /// <param name="fieldName">该序列号的对应的字段名</param>
        /// <returns>序列号</returns>
        public string GenerateSerial(string prefix, int length, string fieldName)
        {
            var rd = new KenceryValidateCode().CreateValidateCode(6);
            
            if (prefix.Contains("OR"))
                prefix = prefix + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Convert.ToString(rd);
            if (prefix.Contains("RE"))
                prefix = prefix + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Convert.ToString(rd);
                           
            SqlText = "Pro_GenerateSerial";
            return DbManager.SetCommand(CommandType.StoredProcedure, SqlText, new IDbDataParameter[]{
                new SqlParameter("@SequencePrefix",prefix),
                new SqlParameter("@SequenceLength",length),
                new SqlParameter("@KeyName",fieldName)
            }).ExecuteScalar<string>();
        }

        public string GenerateId(string tableName)
        {
            string prefix = "1"; int length = 12;
            string fieldName = tableName + "Id";
            SqlText = "Pro_GenerateSerial";
            return DbManager.SetCommand(CommandType.StoredProcedure, SqlText, new IDbDataParameter[]{
                new SqlParameter("@SequencePrefix",prefix),
                new SqlParameter("@SequenceLength",length),
                new SqlParameter("@KeyName",fieldName)
            }).ExecuteScalar<string>();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="tableName">要显示的表或多个表的连接</param>
        /// <param name="fields">要显示的字段列表</param>
        /// <param name="sortfield">排序字段</param>
        /// <param name="singleSortType">单表所用排序字段，false为升序，true为降序</param>
        /// <param name="pageSize">每页显示的记录个数</param>
        /// <param name="pageIndex">要显示那一页的记录</param>
        /// <param name="condition">查询条件,不需where</param>
        /// <param name="count">查询到的记录数</param>
        /// <returns>List<T></returns>
        public List<T> TablesPage<T>(string tableName, string fields, string sortfield, bool singleSortType, int pageSize, int pageIndex, string condition, out int count)
        {
            var outParmeter = new SqlParameter("@Counts", DbType.Int32) { Direction = ParameterDirection.Output };
            var parameters = new IDbDataParameter[]
            {
                new SqlParameter("@tblName", tableName),
                new SqlParameter("@fields", fields),
                new SqlParameter("@sortfields", sortfield),
                new SqlParameter("@singleSortType", singleSortType ? "1" : "0"),
                new SqlParameter("@pageSize", pageSize),
                new SqlParameter("@pageIndex", pageIndex),
                new SqlParameter("@strCondition", condition),
                outParmeter
            };
            var list = DbManager.SetCommand(CommandType.StoredProcedure, "sp_TablesPage", parameters).ExecuteList<T>();
            count = (int)outParmeter.Value;
            return list;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="tableName">要显示的表或多个表的连接</param>
        /// <param name="fields">要显示的字段列表</param>
        /// <param name="sortfield">排序字段</param>
        /// <param name="singleSortType">单表所用排序字段，false为升序，true为降序</param>
        /// <param name="pageSize">每页显示的记录个数</param>
        /// <param name="pageIndex">要显示那一页的记录</param>
        /// <param name="condition">查询条件,不需where</param>
        /// <param name="count">查询到的记录数</param>
        /// <returns>DataTable</returns>
        protected DataTable TablesPage(string tableName, string fields, string sortfield, bool singleSortType, int pageSize, int pageIndex, string condition, out int count)
        {

            var outParmeter = new SqlParameter("@Counts", DbType.Int32) { Direction = ParameterDirection.Output };
            var parameters = new IDbDataParameter[]
            {
                new SqlParameter("@tblName", tableName),
                new SqlParameter("@fields", fields),
                new SqlParameter("@sortfields", sortfield),
                new SqlParameter("@singleSortType", singleSortType ? "1" : "0"),
                new SqlParameter("@pageSize", pageSize),
                new SqlParameter("@pageIndex", pageIndex),
                new SqlParameter("@strCondition", condition),
                outParmeter
            };
            var dt = DbManager.SetCommand(CommandType.StoredProcedure, "sp_TablesPage", parameters).ExecuteDataTable();
            count = (int)outParmeter.Value;
            return dt;
        }
    }
}