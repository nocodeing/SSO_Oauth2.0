
using BLToolkit.Mapping;

namespace Model
{
    /// <summary>
    /// 所有数据库表对应实体基类
    /// </summary>
    public class ModelBase
    {
        [MapIgnore]
        public int PageIndex { get; set; }

        [MapIgnore]
        public int PageSize { get; set; }

        [MapIgnore]
        public int TotalNumber { get; set; }
    }
}
