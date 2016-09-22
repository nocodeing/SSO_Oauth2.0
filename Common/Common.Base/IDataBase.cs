using System;
using System.Collections.Generic;
using System.Data;

namespace Common.Base
{
    /// <summary>
    /// 数据访问层基类接口
    /// </summary>
    public interface IDataBase
    {
        int Insert<TEntity>(TEntity entity) where TEntity : class;
        int Update<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// 根据条件进行修改
        /// </summary>
        /// <typeparam name="TEntity">实体对象类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="condition">指定实体中哪个字段作为修改条件（仅支持“=”）</param>
        /// <returns>修改结果</returns>
        int Update<TEntity>(TEntity entity, string condition) where TEntity : class;

        /// <summary>
        /// 根据条件进行修改
        /// </summary>
        /// <typeparam name="TEntity">实体对象类型</typeparam>
        /// <param name="t">实体对象</param>
        /// <param name="propertyList">指定实体中哪个字段作为修改条件（仅支持“=”）</param>
        /// <returns>修改结果</returns>
        int Update<TEntity>(TEntity t, IEnumerable<string> propertyList) where TEntity : class;

        int Delete(object id);
        IList<TEntity> FindAll<TEntity>(bool isCache = false) where TEntity : class;
        IList<TEntity> FindAll<TEntity>(int cacheTimeLength, bool isCache = false) where TEntity : class;

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TEntity">返回列表对应实体</typeparam>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">每页显示数据量</param>
        /// <param name="sortFiled">排序字段</param>
        /// <param name="sortDir">正序asc,倒序desc</param>
        /// <param name="where">对应返回实体的查询条件</param>
        /// <returns></returns>
        [Obsolete("此方法有BUG,请使用TablesPage", false)]
        IList<TEntity> FindAll<TEntity>(int pageIndex, int pageSize, string sortFiled, string sortDir, string where)
            where TEntity : class;

        /// <summary>
        /// 向上递归查询层级数据
        /// </summary>
        /// <param name="currentValue">查询条件字段对应值</param>
        /// <param name="endLevel">结束递归查询级别（此查询结果级别向上排序，从本级向上依次为:0，1，2，3……）</param>
        /// <param name="startLevel">开始递归查询级别本级为1</param>
        /// <returns></returns>
        IList<TEntity> FindAll<TEntity>(string currentValue, int endLevel,
            int startLevel) where TEntity : class;

        /// <summary>
        /// 根据Id列表获取相应实体列表
        /// </summary>
        /// <typeparam name="TEntity">列表中实体类型</typeparam>
        /// <param name="idList"></param>
        /// <returns></returns>
        IList<TEntity> FindAll<TEntity>(List<string> idList) where TEntity : class;

        int InsertBatch<T>(IEnumerable<T> entities);
        int InsertOrReplaceBatch<T>(IEnumerable<T> entities);
        TEntity Find<TEntity>(string id) where TEntity : class;

        /// <summary>
        /// 用于生成序列号
        /// </summary>
        /// <param name="prefix">序列前缀</param>
        /// <param name="length">包含前缀的序列号长度</param>
        /// <param name="fieldName">该序列号的对应的字段名</param>
        /// <returns>序列号</returns>
        string GenerateSerial(string prefix, int length, string fieldName);

        /// <summary>
        /// 生成相关表ID
        /// </summary>                                                                                 
        /// <param name="tableName">表名</param>
        /// <returns>ID</returns>
        string GenerateId(string tableName);


        List<T> TablesPage<T>(string tableName, string fields, string sortfield, bool singleSortType, int pageSize, int pageIndex, string condition, out int count);
    }
}