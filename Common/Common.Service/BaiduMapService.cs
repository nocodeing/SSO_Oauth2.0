using Common.IService;
using CommonTools;
using Model;

namespace Common.Service
{
    public class BaiduMapService : IBaiduMapService
    {
        public BaiduMapModel GetAddress(string ip)
        {
            var json = HttpHelper.GetHttpResponse("http://api.map.baidu.com/location/ip?ak=W9rqU0KmU3P2r6ttZYLI994C&coor=bd09ll&ip=" + ip);
            return SerializerHelper.Deserialize<BaiduMapModel>(json);
        }
    }
}
