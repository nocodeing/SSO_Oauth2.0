using System;
using Common.Base;
using Common.IService;
using CommonTools;
using Model.Const;
using Model.Enum;

namespace Common.Service
{
    public class SmsService : BussinessBase, ISmsService
    {
        private readonly EmaySDKService.SDKService _service = new EmaySDKService.SDKService();

        public ErrorCodeType SerialNumberRegister()
        {
            return (ErrorCodeType)_service.registEx(SmsConst.SoftwareSerialNo, SmsConst.Key, SmsConst.SerialPass);
        }

        public ErrorCodeType EnterInfoRegister()
        {
            return (ErrorCodeType)_service.registDetailInfo(SmsConst.SoftwareSerialNo, SmsConst.Key, SmsConst.EnterName, SmsConst.LinkMan, SmsConst.PhoneNum, SmsConst.Mobile, SmsConst.Email, SmsConst.Fax, SmsConst.Address, SmsConst.PostCode);
        }

        public ErrorCodeType SerialNumberLogOff()
        {
            return (ErrorCodeType)_service.logout(SmsConst.SoftwareSerialNo, SmsConst.Key);
        }

        public double GetEachFee()
        {
            return _service.getEachFee(SmsConst.SoftwareSerialNo, SmsConst.Key);
        }

        public double GetBalance()
        {
            return _service.getBalance(SmsConst.SoftwareSerialNo, SmsConst.Key);
        }

        public ErrorCodeType ChargeUp(string cardNo, string cardPass)
        {
            return (ErrorCodeType)_service.chargeUp(SmsConst.SoftwareSerialNo, SmsConst.Key, cardNo, cardPass);
        }

        public ErrorCodeType SendSms(string msg, params string[] mobiles)
        {
            if (RequestHelper.GetServerIp() != "203.171.233.12")
            {
                return ErrorCodeType.成功;
            }
            var s = (ErrorCodeType)_service.sendSMS(SmsConst.SoftwareSerialNo, SmsConst.Key, "", mobiles, msg, "", "GBK", SmsConst.SmsPriority, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            return s;
        }

        public void SendSmsAsync(string msg, string[] mobiles, object userState = null)
        {
            _service.sendSMSAsync(SmsConst.SoftwareSerialNo, SmsConst.Key, "", mobiles, msg, "", "GBK", SmsConst.SmsPriority, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")), userState);
        }


        public ErrorCodeType SerialUpdatePass(string serialNewPass)
        {
            return (ErrorCodeType)_service.serialPwdUpd(SmsConst.SoftwareSerialNo, SmsConst.Key, SmsConst.SerialPass, serialNewPass);
        }
    }
}
