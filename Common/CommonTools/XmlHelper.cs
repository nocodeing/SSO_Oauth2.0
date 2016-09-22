using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CommonTools
{
    /// <summary>
    /// xml帮助类 
    /// </summary>
    public class XmlHelper
    {
         /// <summary>
        /// 追加节点
        /// </summary>
        /// <param name="filePath">XML文档绝对路径</param>
        /// <param name="xPath">范例: @"Skill/First/SkillItem"</param>
        /// <param name="xmlNode">XmlNode节点</param>
        /// <returns></returns>
        public static bool AppendChild(string filePath, string xPath, XmlNode xmlNode)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNode xn = doc.SelectSingleNode(xPath);
                XmlNode n = doc.ImportNode(xmlNode, true);
                xn.AppendChild(n);
                doc.Save(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 修改节点的InnerText的值
        /// </summary>
        /// <param name="filePath">XML文件绝对路径</param>
        /// <param name="xPath">范例: @"Skill/First/SkillItem"</param>
        /// <param name="value">节点的值</param>
        /// <returns></returns>
        public static bool UpdateNodeInnerText(string filePath, string xPath, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNode xn = doc.SelectSingleNode(xPath);
                XmlElement xe = (XmlElement)xn;
                xe.InnerText = value;
                doc.Save(filePath);
                return true;
            }
            catch
            {
                return false;
            }
         
        }

        public static string ReadNodeInnerText(string xmlString, string xPath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load( xPath );
                XmlNode xn = doc.SelectSingleNode( xmlString );
                XmlElement xe = (XmlElement)xn;
                return xe.InnerText;
            }
            catch
            {
                return "";
            }
        }
        //生成短码读取xml
        public static string ReadShortenNodeInnerText(string xmlString, string xPath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode xn = doc.SelectSingleNode(xPath);
                XmlElement xe = (XmlElement)xn;
                return xe.InnerText;
            }
            catch
            {
                return "";
            }
        }
    

    }
}
