using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Enum
{
    /// <summary>
    /// 会员重要操作消息提醒
    /// </summary>
    public enum MemberNoticeType : byte
    {
        付款 = 1,
        发货 = 2,
        充值 = 3,
        兑换充值卡 = 4,
        俱乐部会员 = 5,
        咖啡厅会员 = 6,
        到货通知 = 7,
        改登录密码 = 9,
        改支付密码 = 10,
        实名认证信息提交 = 11,
        实名认证审核通过 = 12,
        佣金提现 = 13,
        第三方登录绑定手机号 = 14,
        引荐注册通知引荐人 = 15,
        实名认证审核失败 = 16,
        商家发货审核会员身份 = 17,
        //咖啡厅收银业务
        自动注册 = 20,
        分享发放红包 = 21,
        修改手机号 = 22,
        注册 = 23,
        推广会员批量注册=24,
        短信营销=25
    }
}
