using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IService
{
    public interface IEmailService
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email">Email地址</param>
        /// <param name="content">内容</param>
        bool Send(string email, string content);
    }
}
