
using System.Collections.Concurrent;
using BLToolkit.Data;
using CommonTools;
using Factory;

namespace Common.Base
{

    public class SequenceKeyRepository : DataBase, ISequenceKeyRepository
    {
        //public string GenerateId(string tableName)
        //{
        //    return GenerateSerial("1", 12, tableName + "Id");
        //}
    }

    public interface ISequenceKeyRepository : IDataBase
    {
        //string GenerateId(string tableName);
    }
    //public class BussinessHelper : IBussinessHelper
    //{
    //    /// <summary>
    //    /// 用于生成序列号
    //    /// </summary>
    //    /// <param name="prefix">序列前缀</param>
    //    /// <param name="length">包含前缀的序列号长度</param>
    //    /// <param name="fieldName">该序列号的对应的字段名</param>
    //    /// <returns>序列号</returns>
    //    public string GenerateSerial(string prefix, int length, string fieldName)
    //    {
    //        var r = new DalFactoryRepository();
    //        return r.CreateInstance<ISequenceKeyRepository>().GenerateSerial(prefix, length, fieldName);
    //    }

    //    /// <summary>
    //    /// 生成相关表ID
    //    /// </summary>                                                                                 
    //    /// <param name="tableName">表名</param>
    //    /// <returns>ID</returns>
    //    public string GenerateId(string tableName)
    //    {
    //        return GenerateSerial("1", 10, tableName + "Id");
    //    }
    //}

 
}
