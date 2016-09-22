using Model.Const;
using System;
using System.Web;
using System.Web.Security;

namespace CommonTools
{
    public static class CookieHelper
    {
        public static void ClearLoginCookie(string domain)
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();

            HttpCookie cookies = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookies.Expires = DateTime.Now.AddYears(-2);
            cookies.Domain = string.IsNullOrEmpty(domain) ? FormsAuthentication.CookieDomain : domain;
            HttpContext.Current.Response.Cookies.Add(cookies);
        }

        public static void ClearReferCookie(string domain)
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            HttpCookie cookies = new HttpCookie(CookieConst.ReferCookie,"");
            cookies.Expires = DateTime.Now.AddYears(-2);
            cookies.Domain = domain;            
            HttpContext.Current.Response.Cookies.Add(cookies);
        }
    }
}
