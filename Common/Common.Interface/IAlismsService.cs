using System.Collections.Generic;
using Common.Base;
using Model.Enum;

namespace Common.IService
{
    public interface IAlismsService: IBussinessBase
   {
        /// <summary>
        /// 阿里大鱼发送短信接口
        /// </summary>
        /// <param name="name"></param>
        /// <param name="smsParams"></param>
        /// <param name="mobiles"></param>
        /// <returns></returns>
       bool ALiSendSms(MemberNoticeType name, Dictionary<string, string> smsParams, params string[] mobiles);
   }
}
