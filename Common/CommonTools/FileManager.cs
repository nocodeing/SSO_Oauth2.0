using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace CommonTools
{
    //文件管理类
    public static class FileManager
    {
        #region 写文件
        /****************************************
         * 函数名称：WriteFile
         * 功能说明：当文件不存时，则创建文件，并追加文件
         * 参    数：Path:文件路径,Strings:文本内容

        *****************************************/
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">文件内容</param>
        public static void Write(string path, string content, bool append)
        {

            if (!File.Exists(path))
            {
                FileStream f = File.Create(path);
                f.Close();
                f.Dispose();
            }
            var f2 = new StreamWriter(path, append, Encoding.UTF8);
            f2.WriteLine(content);
            f2.Close();
            f2.Dispose();


        }
        #endregion

        #region 读文件
        /****************************************
         * 函数名称：ReadFile
         * 功能说明：读取文本内容
         * 参    数：Path:文件路径
        *****************************************/
        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string Read(string path)
        {
            string s = "";
            if (!File.Exists(path))
                s = "不存在相应的目录";
            else
            {
                var f2 = new StreamReader(path, Encoding.GetEncoding("gb2312"));
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }

            return s;
        }
        #endregion



        public static XmlNode ReadXmlFile(string xmlPath, string rootName)
        {
            var document = new XmlDocument();
            document.Load(xmlPath);
            return document.SelectSingleNode(rootName);
        }

        public static string GetApplicationBinPath()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var path = string.Format("{0}bin/", basePath);
            if (Directory.Exists(path))
                return path;
            return basePath;
        }
    }
}
