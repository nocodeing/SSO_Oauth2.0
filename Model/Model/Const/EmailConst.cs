
namespace Model.Const
{
    public static class EmailConst
    {
        public const int ServerPort = 25;
        public const string FromName = "system@5tha.com";
        public const string EmailPwd = "Enjoylife5";
        public const string Subject = "第五大街";
        public const string MailBody =
            "亲爱的用户您好:<br />&nbsp;&nbsp;&nbsp;&nbsp;感谢您注册第五大街</a>.<br />&nbsp;&nbsp;&nbsp;&nbsp;此邮件为系统发送,请勿回复。为保障您的帐户安全,请在激活后及时删除此邮件,您本次的验证码是:";
        public const bool MailBodyHtml = true;
        public const string SmtpServer = "smtp.exmail.qq.com";

        //到货通知
        public const string AogNoticeHead = "【第五大街】好消息！你订购的商品 \"";
        public const string AogNoticeFoot = "\" 已经到货啦，数量有限，欲购从速！";

        //咖啡厅支付密码
        public const string PayMailBody =
            "亲爱的用户您好:<br />&nbsp;&nbsp;&nbsp;&nbsp;感谢您光临第五大街</a>.<br />&nbsp;&nbsp;&nbsp;&nbsp;此邮件为系统发送,请勿回复。您本次的消费的支付验证码是:";
    }
}
