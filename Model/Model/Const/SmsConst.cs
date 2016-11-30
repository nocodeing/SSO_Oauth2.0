
using System;

namespace Model.Const
{
    public static class SmsConst
    {
        public const string SoftwareSerialNo = "";
        public const string Key = "";
        public const string SerialPass = "";
        public const string EnterName = "";
        public const string LinkMan = "";
        public const string PhoneNum = "";
        public const string Mobile = "";
        public const string Email = "";
        public const string Fax = "";
        public const string Address = "";
        public const string PostCode = "";
        public const int SmsPriority = 1;  //短信等级1~5,数值越高发送等级越高,默认为1

        public const string Msg = "短信验证码为:";
        public static readonly string ModiPayPasswordfypassword = "您于{0}修改了支付密码,若非本人操作请及时联系管理员";  //修改支付密码提醒内容
        public static readonly string ModifyLoginPassword = "您于{0}修改了登录密码,若非本人操作请及时联系管理员";  //修改登录密码提醒内容

        public static string ClubMember = "您已达到俱乐部会员标准,恭喜您成为俱乐部会员!";

        //到货通知
        public const string AogNoticeHead = "好消息！你订购的商品 \"";
        public const string AogNoticeFoot = "\" 已经到货啦，数量有限，欲购从速！";

        //咖啡厅支付密码
        public const string MsgPayWord = "支付密码验证码为:";
    }
}
