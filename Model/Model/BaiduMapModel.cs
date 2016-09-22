// ReSharper disable All
namespace Model
{
    public class BaiduMapModel
    {
        public string address { get; set; }
        public BaiduMapContent content { get; set; }
        public int status { get; set; }
    }

    public class BaiduMapContent
    {
        public string address { get; set; }
        public BaiduMapAddressDetail address_detail { get; set; }
        public Point point { get; set; }
    }

    public class BaiduMapAddressDetail
    {
        public string city { get; set; }
        public int city_code { get; set; }
        public string district { get; set; }
        public string province { get; set; }
        public string street { get; set; }
        public string street_number { get; set; }
    }

    public class Point
    {
        public string x { get; set; }
        public string y { get; set; }
    }
}
