using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Base;
using Common.IService;
using CommonTools;
using Model.Enum;
using Top.Api;
using Top.Api.Request;

namespace Common.Service
{
   public class AlismsService: BussinessBase, IAlismsService
    {
        /// <summary>
        /// 发送短信方法
        /// </summary>
        /// <param name="name">发送类型MemberNoticeType</param>
        /// <param name="smsParams">参数</param>
        /// <param name="mobiles">手机号</param>
        /// <returns></returns>
        public bool ALiSendSms(MemberNoticeType name, Dictionary<string, string> smsParams, params string[] mobiles)
        {
            ITopClient client = new DefaultTopClient("http://gw.api.taobao.com/router/rest", "", "", "json");  //需要提供apikey 和appsecret
            var req = new AlibabaAliqinFcSmsNumSendRequest
            {
                SmsType = "normal",
                SmsFreeSignName = "第五大街",
                SmsParam = smsParams == null || smsParams.Count < 1 ? "" : smsParams.Serializer(),
                RecNum = string.Join(",", mobiles),
                SmsTemplateCode = GetTemplateCode(name)
            };
            return client.Execute(req).Body.Deserialize<AlismsResponse>().alibaba_aliqin_fc_sms_num_send_response.result.success;
        }
        private static string GetTemplateCode(MemberNoticeType name)   //返回的参数需要在后台设置
        {
            switch (name)
            {
                case MemberNoticeType.短信营销:
                    return "SMS_6695905";
                default:
                    return "";
            }
        }
    }

    public class AlismsResult
    {
        public AlismsResultCode result { get; set; }
    }
    public class AlismsResponse
    {
        public AlismsResult alibaba_aliqin_fc_sms_num_send_response { get; set; }
    }
    public class AlismsResultCode
    {
        public string err_code { get; set; }
        public string model { get; set; }
        public bool success { get; set; }
        public string msg { get; set; }
    }

}
