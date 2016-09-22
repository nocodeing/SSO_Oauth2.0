﻿
namespace Model.Enum
{
    public enum ErrorCodeType
    {
        成功 = 0,
        系统异常 = -1,
        客户端异常 = -2,
        命令不被支持 = -101,
        RegistryTransInfo删除信息失败 = -102,
        RegistryInfo更新信息失败 = -103,
        请求超过限制 = -104,

        号码注册激活失败 = -110,
        企业注册失败 = -111,

        充值失败 = -113,

        发送短信失败 = -117,
        接收MO失败 = -118,
        接收Report失败 = -119,
        修改密码失败 = -120,

        号码注销激活失败 = -122,
        查询单价失败 = -123,
        查询余额失败 = -124,
        设置MO转发失败 = -125,
        路由信息失败 = -126,
        计费失败0余额 = -127,
        计费失败余额不足 = -128,

        数据操作失败 = -190,

        序列号错误序列号不存在内存中或尝试攻击的用户 = -1100,

        序列号密码错误 = -1102,
        序列号Key错误 = -1103,
        路由失败请联系系统管理员 = -1104,
        注册号状态异常未用1 = -1105,

        注册号状态异常停用3 = -1107,
        注册号状态异常停止5 = -1108,

        充值卡无效 = -1131,
        充值密码无效 = -1132,
        充值卡绑定异常 = -1133,
        充值状态无效 = -1134,
        充值金额无效 = -1135,

        数据库插入操作失败 = -1901,
        数据库更新操作失败 = -1902,
        数据库删除操作失败 = -1903,

        数据格式错误数据超出数据库允许范围 = -9000,
        序列号格式错误 = -9001,
        密码格式错误 = -9002,
        客户端Key格式错误 = -9003,
        设置转发格式错误 = -9004,
        公司地址格式错误 = -9005,
        企业中文名格式错误 = -9006,
        企业中文名简称格式错误 = -9007,
        邮件地址格式错误 = -9008,
        企业英文名格式错误 = -9009,
        企业英文名简称格式错误 = -9010,
        传真格式错误 = -9011,
        联系人格式错误 = -9012,
        联系电话 = -9013,
        邮编格式错误 = -9014,
        新密码格式错误 = -9015,
        发送短信包大小超出范围 = -9016,
        发送短信内容格式错误 = -9017,
        发送短信扩展号格式错误 = -9018,
        发送短信优先级格式错误 = -9019,
        发送短信手机号格式错误 = -9020,
        发送短信定时时间格式错误 = -9021,
        发送短信唯一序列值错误 = -9022,
        充值卡号格式错误 = -9023,
        充值密码格式错误 = -9024,
        客户端请求sdk5超时 = -9025
    }
}
