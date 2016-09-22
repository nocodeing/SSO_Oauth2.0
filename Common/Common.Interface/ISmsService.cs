
using Common.Base;
using Model.Enum;

namespace Common.IService
{
    public interface ISmsService : IBussinessBase
    {
        /// <summary>
        /// 序列号注册
        /// </summary>
        /// <returns>ErrorCode</returns>
        ErrorCodeType SerialNumberRegister();

        /// <summary>
        /// 企业信息注册
        /// </summary>
        /// <returns>ErrorCode</returns>
        ErrorCodeType EnterInfoRegister();

        /// <summary>
        /// 序列号注销
        /// </summary>
        /// <returns>ErrorCode</returns>
        ErrorCodeType SerialNumberLogOff();

        /// <summary>
        /// 获取短信单价
        /// </summary>
        /// <returns></returns>
        double GetEachFee();

        /// <summary>
        /// 获取余额
        /// </summary>
        /// <returns>ErrorCode</returns>
        double GetBalance();

        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="cardNo">充值卡号</param>
        /// <param name="cardPass">充值卡密码</param>
        /// <returns>ErrorCode</returns>
        ErrorCodeType ChargeUp(string cardNo, string cardPass);

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="msg">短信内容</param>
        /// <param name="mobiles">手机号</param>
        /// <returns></returns>
        ErrorCodeType SendSms(string msg, params string[] mobiles);


        /// <summary>
        /// 异步发送短信
        /// </summary>
        /// <param name="msg">短信内容</param>
        /// <param name="mobiles">手机号</param>
        /// <param name="userState"></param>
        /// <returns></returns>
        void SendSmsAsync(string msg, string[] mobiles, object userState = null);

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="serialNewPass">新密码</param>
        /// <returns></returns>
        ErrorCodeType SerialUpdatePass(string serialNewPass);
    }
}
