using System.Collections.Generic;
using Webdiyer.WebControls.Mvc;

namespace CommonTools.WebHelper
{
    /// <summary>
    /// 页面模型
    /// </summary>
    public class PageModel<T>
    {
        private object _pageData;
        public PagedList<T> PageList { set; get; }
        public List<T> List { set; get; }
        public object PageData
        {
            set { _pageData = value; }
        }
        public object GetData(string name)
        {
            return _pageData.GetDataByProperty(name);
        }
    }
}