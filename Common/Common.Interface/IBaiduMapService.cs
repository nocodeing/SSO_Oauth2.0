using Model;

namespace Common.IService
{
    public interface IBaiduMapService
    {
        /// <summary>
        /// 获取百度IP定位信息
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns></returns>
        BaiduMapModel GetAddress(string ip);
    }
}
