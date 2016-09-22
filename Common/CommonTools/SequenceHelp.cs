using System;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace CommonTools
{
    /// <summary>
    /// 序列号帮助类
    /// </summary>
    [System.Runtime.Remoting.Contexts.Synchronization]
    public class SequenceHelp : ContextBoundObject
    {
        public static string GetDealNumber(Func<string, string> fun = null, string prefix = "")
        {
            return fun == null ? Guid.NewGuid().ToString().ToUpper() : fun(prefix);
        }

        private static readonly object LockObj = new object();
        private static long _seraNumber;

        /// <summary>  
        /// 生成充值流水号格式：  
        /// </summary>  
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static string GetSerialNumber(string prefix = "")
        {
            lock (LockObj)
            {
                if (_seraNumber > long.MaxValue - 100000) _seraNumber = 0;
                Interlocked.Increment(ref _seraNumber);
                return string.Format("{2}{0:yyyyMMddHHmm}{1}", DateTime.Now,
                      _seraNumber.ToString(CultureInfo.InvariantCulture).PadLeft(8, '0'), prefix);
            }
        }
    }
}
