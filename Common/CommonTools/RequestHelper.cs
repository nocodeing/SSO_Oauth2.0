using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace CommonTools
{
    /// <summary>
    /// HTTP请求类
    /// </summary>
    public static class RequestHelper
    {
        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIp()
        {
            try
            {
                string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(result))
                {
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.IsNullOrEmpty(result))
                {
                    result = HttpContext.Current.Request.UserHostAddress;
                }

                if (string.IsNullOrEmpty(result) ||
                    !Regex.IsMatch(result, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
                {
                    return "0.0.0.0";
                }

                return result;
            }
            catch
            {
                return "0.0.0.0";
            }

        }

        /// <summary>
        /// 获取服务器IP
        /// </summary>
        /// <returns></returns>
        public static string GetServerIp()
        {
            var hostName = Dns.GetHostName();
            var addressList = Dns.GetHostAddresses(hostName);
            var list = addressList.Select(ip => ip.ToString()).ToList();
            foreach (var item in list.Where(item => StringHelper.StringHelper.IsIp(item) && !item.Contains("0.0.0.0") && !item.Contains("255.255.255.255")))
            {
                return item;
            }
            return "0.0.0.0";
        }
        /// <summary>
        /// 创建HTTP请求
        /// </summary>
        /// <param name="url">URL绝对地址</param>
        /// <returns></returns>
        public static string GetHttpResponse(string url)
        {
            string content = "";
            //其中,HttpWebRequest实例不使用HttpWebRequest的构造函数来创建,二是使用WebRequest的Create方法来创建.
            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //不维持与服务器的请求状态
            myHttpWebRequest.KeepAlive = true;
            try
            {
                //创建一个HttpWebRequest对象
                var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                //设置页面的编码模式
                System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
                var streamResponse = myHttpWebResponse.GetResponseStream();
                if (streamResponse != null)
                {
                    var streamRead = new StreamReader(streamResponse, utf8);

                    var readBuff = new Char[256];
                    //这里使用了StreamReader的Read()方法,参数意指从0开始读取256个char到readByff中.
                    //Read()方法返回值为指定的字符串数组,当达到文件或流的末尾使,方法返回0
                    int count = streamRead.Read(readBuff, 0, 256);
                    while (count > 0)
                    {
                        var outputData = new String(readBuff, 0, count);
                        content += outputData;
                        count = streamRead.Read(readBuff, 0, 256);
                    }
                }
                myHttpWebResponse.Close();
                return content;
            }
            catch (WebException)
            {
                return content;
            }
        }
    }
}
