
using System;

namespace Model.Const
{
    public static class SmsConst
    {
        public const string SoftwareSerialNo = "9SDK-EMY-0999-JFSSP";
        public const string Key = "394946";
        public const string SerialPass = "394946";
        public const string EnterName = "河南第五大街电子商务有限公司";
        public const string LinkMan = "韩凯";
        public const string PhoneNum = "0371-56659935";
        public const string Mobile = "18838905613";
        public const string Email = "hankai125@sina.com";
        public const string Fax = "0371-56659935";
        public const string Address = "河南省郑州市郑东新区商都路站南路建正东方中心C-2202";
        public const string PostCode = "450000";
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
