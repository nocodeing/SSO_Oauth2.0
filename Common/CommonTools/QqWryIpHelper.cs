using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace CommonTools
{
    public class IPService
    {
        private string _IP;
        private string _Country;
        private string _Local;
        public string IP
        {
            get { return _IP; }
            set { _IP = value; }
        }
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }
        public string Local
        {
            get { return _Local; }
            set { _Local = value; }
        }
    }
    public class QQWryLocator
    {
        private byte[] data;
        Regex regex = new Regex(@"(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))");
        long firstStartIpOffset;
        long lastStartIpOffset;
        long ipCount;
        public long Count { get { return ipCount; } }
        public QQWryLocator(string dataPath)
        {
            using (FileStream fs = new FileStream(dataPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
            }
            byte[] buffer = new byte[8];
            Array.Copy(data, 0, buffer, 0, 8);
            firstStartIpOffset = ((buffer[0] + (buffer[1] * 0x100)) + ((buffer[2] * 0x100) * 0x100)) + (((buffer[3] * 0x100) * 0x100) * 0x100);
            lastStartIpOffset = ((buffer[4] + (buffer[5] * 0x100)) + ((buffer[6] * 0x100) * 0x100)) + (((buffer[7] * 0x100) * 0x100) * 0x100);
            ipCount = Convert.ToInt64((double)(((double)(lastStartIpOffset - firstStartIpOffset)) / 7.0));

            if (ipCount <= 1L)
            {
                throw new ArgumentException("ip FileDataError");
            }
        }
        private static long IpToInt(string ip)
        {
            char[] separator = new char[] { '.' };
            if (ip.Split(separator).Length == 3)
            {
                ip = ip + ".0";
            }
            string[] strArray = ip.Split(separator);
            long num2 = ((long.Parse(strArray[0]) * 0x100L) * 0x100L) * 0x100L;
            long num3 = (long.Parse(strArray[1]) * 0x100L) * 0x100L;
            long num4 = long.Parse(strArray[2]) * 0x100L;
            long num5 = long.Parse(strArray[3]);
            return (((num2 + num3) + num4) + num5);
        }
        private static string IntToIP(long ip_Int)
        {
            long num = (long)((ip_Int & 0xff000000L) >> 0x18);
            if (num < 0L)
            {
                num += 0x100L;
            }
            long num2 = (ip_Int & 0xff0000L) >> 0x10;
            if (num2 < 0L)
            {
                num2 += 0x100L;
            }
            long num3 = (ip_Int & 0xff00L) >> 8;
            if (num3 < 0L)
            {
                num3 += 0x100L;
            }
            long num4 = ip_Int & 0xffL;
            if (num4 < 0L)
            {
                num4 += 0x100L;
            }
            return (num.ToString() + "." + num2.ToString() + "." + num3.ToString() + "." + num4.ToString());
        }
        public IPService Query(string ip)
        {
            if (!regex.Match(ip).Success)
            {
                throw new ArgumentException("IP格式错误");
            }
            IPService ipLocation = new IPService();
            ipLocation.IP = ip;
            long intIP = IpToInt(ip);
            if ((intIP >= IpToInt("127.0.0.1") && (intIP <= IpToInt("127.255.255.255"))))
            {
                ipLocation.Country = "本机内部环回地址";
                ipLocation.Local = "";
            }
            else
            {
                if ((((intIP >= IpToInt("0.0.0.0")) && (intIP <= IpToInt("2.255.255.255"))) || ((intIP >= IpToInt("64.0.0.0")) && (intIP <= IpToInt("126.255.255.255")))) ||
                ((intIP >= IpToInt("58.0.0.0")) && (intIP <= IpToInt("60.255.255.255"))))
                {
                    ipLocation.Country = "网络保留地址";
                    ipLocation.Local = "";
                }
            }
            long right = ipCount;
            long left = 0L;
            long middle = 0L;
            long startIp = 0L;
            long endIpOff = 0L;
            long endIp = 0L;
            int countryFlag = 0;
            while (left < (right - 1L))
            {
                middle = (right + left) / 2L;
                startIp = GetStartIp(middle, out endIpOff);
                if (intIP == startIp)
                {
                    left = middle;
                    break;
                }
                if (intIP > startIp)
                {
                    left = middle;
                }
                else
                {
                    right = middle;
                }
            }
            startIp = GetStartIp(left, out endIpOff);
            endIp = GetEndIp(endIpOff, out countryFlag);
            if ((startIp <= intIP) && (endIp >= intIP))
            {
                string local;
                ipLocation.Country = GetCountry(endIpOff, countryFlag, out local);
                ipLocation.Local = local;
            }
            else
            {
                ipLocation.Country = "未知";
                ipLocation.Local = "";
            }
            return ipLocation;
        }
        private long GetStartIp(long left, out long endIpOff)
        {
            long leftOffset = firstStartIpOffset + (left * 7L);
            byte[] buffer = new byte[7];
            Array.Copy(data, leftOffset, buffer, 0, 7);
            endIpOff = (Convert.ToInt64(buffer[4].ToString()) + (Convert.ToInt64(buffer[5].ToString()) * 0x100L)) + ((Convert.ToInt64(buffer[6].ToString()) * 0x100L) * 0x100L);
            return ((Convert.ToInt64(buffer[0].ToString()) + (Convert.ToInt64(buffer[1].ToString()) * 0x100L)) + ((Convert.ToInt64(buffer[2].ToString()) * 0x100L) * 0x100L)) + (((Convert.ToInt64(buffer[3].ToString()) * 0x100L) * 0x100L) * 0x100L);
        }
        private long GetEndIp(long endIpOff, out int countryFlag)
        {
            byte[] buffer = new byte[5];
            Array.Copy(data, endIpOff, buffer, 0, 5);
            countryFlag = buffer[4];
            return ((Convert.ToInt64(buffer[0].ToString()) + (Convert.ToInt64(buffer[1].ToString()) * 0x100L)) + ((Convert.ToInt64(buffer[2].ToString()) * 0x100L) * 0x100L)) + (((Convert.ToInt64(buffer[3].ToString()) * 0x100L) * 0x100L) * 0x100L);
        }
        /// <summary>   
        /// Gets the country.   
        /// </summary>   
        /// <param name="endIpOff">The end ip off.</param>   
        /// <param name="countryFlag">The country flag.</param>   
        /// <param name="local">The local.</param>   
        /// <returns>country</returns>   
        private string GetCountry(long endIpOff, int countryFlag, out string local)
        {
            string country = "";
            long offset = endIpOff + 4L;
            switch (countryFlag)
            {
                case 1:
                case 2:
                    country = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    offset = endIpOff + 8L;
                    local = (1 == countryFlag) ? "" : GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    break;
                default:
                    country = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    local = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    break;
            }
            return country;
        }
        private string GetFlagStr(ref long offset, ref int countryFlag, ref long endIpOff)
        {
            int flag = 0;
            byte[] buffer = new byte[3];

            while (true)
            {
                //用于向前累加偏移量   
                long forwardOffset = offset;
                flag = data[forwardOffset++];
                //没有重定向   
                if (flag != 1 && flag != 2)
                {
                    break;
                }
                Array.Copy(data, forwardOffset, buffer, 0, 3);
                forwardOffset += 3;
                if (flag == 2)
                {
                    countryFlag = 2;
                    endIpOff = offset - 4L;
                }
                offset = (Convert.ToInt64(buffer[0].ToString()) + (Convert.ToInt64(buffer[1].ToString()) * 0x100L)) + ((Convert.ToInt64(buffer[2].ToString()) * 0x100L) * 0x100L);
            }
            if (offset < 12L)
            {
                return "";
            }
            return GetStr(ref offset);
        }
        private string GetStr(ref long offset)
        {
            byte lowByte = 0;
            byte highByte = 0;
            StringBuilder stringBuilder = new StringBuilder();
            byte[] bytes = new byte[2];
            Encoding encoding = Encoding.GetEncoding("GB2312");
            while (true)
            {
                lowByte = data[offset++];
                if (lowByte == 0)
                {
                    return stringBuilder.ToString();
                }
                if (lowByte > 0x7f)
                {
                    highByte = data[offset++];
                    bytes[0] = lowByte;
                    bytes[1] = highByte;
                    if (highByte == 0)
                    {
                        return stringBuilder.ToString();
                    }
                    stringBuilder.Append(encoding.GetString(bytes));
                }
                else
                {
                    stringBuilder.Append((char)lowByte);
                }
            }
        }
    }

    public static class QqWryIpHelper
    {
        /// <summary>
        /// 根据ip得到归属地
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetIPAddress(string ip)
        {
            try
            {
                QQWryLocator qqWry = new QQWryLocator(HttpContext.Current.Server.MapPath("~/Mydata/qqwry.dat"));
                IPService ips = qqWry.Query(ip);
                string CZIPCity = ips.Country;//得到IP归属地
                return CZIPCity;
            }
            catch (Exception)
            {

                return "";
            }

        }

        /// <summary>
        /// 根据ip得到归属网络供应商：联通，电信，移动等
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetIPAddressProvince(string ip)
        {
            QQWryLocator qqWry = new QQWryLocator(HttpContext.Current.Server.MapPath("~/Mydata/qqwry.dat"));
            IPService ips = qqWry.Query(ip);
            string CZIPCity = ips.Local;//得到IP归属地
            return CZIPCity;
        }

        /// <summary>
        /// 根据ip得到归属地:国家，省，市
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string[] GetIpAddressDetail(string ip)
        {
            try
            {
                //LogHelper.Instance.Info("QQWryLocator获得IP归属地--begin ");

                QQWryLocator qqWry = new QQWryLocator(HttpContext.Current.Server.MapPath("~/Mydata/qqwry.dat"));
                IPService ips = qqWry.Query(ip);
                string CZIPCity = ips.Country;//

                //LogHelper.Instance.Info("获得Ip:" + ip + " IP归属地: " + CZIPCity);

                var state = "";
                var province = "";
                var city = "";
                //解析
                if (!string.IsNullOrEmpty(CZIPCity))
                {                    
                    if (CZIPCity == "局域网")
                    {
                        state = "中国";
                        province = "河南省";
                        city = "郑州市";
                    }
                    else
                    {
                        var strState = CZIPCity.Split('国');
                        if (strState.Length > 1 && !string.IsNullOrEmpty(strState[0]))
                        {
                            state = strState[0].Trim() + "国";
                            if (!string.IsNullOrEmpty(strState[1]))
                            {
                                var strProvince = strState[1].Split('省');
                                if (strProvince.Length > 1 && !string.IsNullOrEmpty(strProvince[0]))
                                {
                                    province = strProvince[0].Trim() + "省";
                                    if (!string.IsNullOrEmpty(strProvince[1]))
                                    {
                                        city = strProvince[1].Trim();
                                    }
                                }
                                else if (strProvince.Length == 1 && !string.IsNullOrEmpty(strProvince[0]))
                                {
                                    //有可能是自治区或者特别行政区,或者市直辖市
                                    if (strProvince[0].Contains("自治区"))
                                    {
                                        var strProvincez = strProvince[0].Split('区');
                                        if (strProvincez.Length > 1 && !string.IsNullOrEmpty(strProvincez[0]))
                                        {
                                            province = strProvincez[0].Trim() + "区";
                                            if (!string.IsNullOrEmpty(strProvincez[1]))
                                            {
                                                city = strProvincez[1].Trim();
                                            }
                                        }
                                    }
                                    else if (strProvince[0].Contains("特别行政区"))
                                    {
                                        province = strProvince[0].Trim();
                                    }
                                    else if (strProvince[0].Contains("北京市") || strProvince[0].Contains("上海市") || strProvince[0].Contains("天津市") || strProvince[0].Contains("重庆市"))
                                    {
                                        province = strProvince[0].Trim();
                                    }
                                }
                            }
                        }
                        else if (strState.Length == 1 && !string.IsNullOrEmpty(strState[0]))
                        {
                            state = "中国";
                            var strProvince = strState[0].Split('省');
                            if (strProvince.Length > 1 && !string.IsNullOrEmpty(strProvince[0]))
                            {
                                province = strProvince[0].Trim() + "省";
                                if (!string.IsNullOrEmpty(strProvince[1]))
                                {
                                    city = strProvince[1].Trim();
                                }
                            }
                            else if (strProvince.Length == 1 && !string.IsNullOrEmpty(strProvince[0]))
                            {
                                //有可能是自治区或者特别行政区,或者市直辖市
                                if (strProvince[0].Contains("自治区"))
                                {
                                    var strProvincez = strProvince[0].Split('区');
                                    if (strProvincez.Length > 1 && !string.IsNullOrEmpty(strProvincez[0]))
                                    {
                                        province = strProvincez[0].Trim() + "区";
                                        if (!string.IsNullOrEmpty(strProvincez[1]))
                                        {
                                            city = strProvincez[1].Trim();
                                        }
                                    }
                                }
                                else if (strProvince[0].Contains("特别行政区"))
                                {
                                    province = strProvince[0].Trim();
                                }
                                else if (strProvince[0].Contains("北京市") || strProvince[0].Contains("上海市") || strProvince[0].Contains("天津市") || strProvince[0].Contains("重庆市"))
                                {
                                    province = strProvince[0].Trim();
                                }
                            }
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    //LogHelper.Instance.Info(" IP归属地国家: " + state + " IP归属地省: " + province + " IP归属地市: " + city);
                    return new string[] { state, province, city };
                }
                else
                {
                    throw new Exception();
                }
               
            }
            catch (Exception)
            {
                return new string[]{"","",""};
            }

        }
    }
}
