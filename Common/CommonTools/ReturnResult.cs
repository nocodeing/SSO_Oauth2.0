namespace CommonTools
{
    public class ReturnResult
    {
        /// <summary>
        ///默认
        /// </summary>
        public ReturnResult()
        {
            Message = "系统错误";
        }

        public ReturnResult(bool flag, string message)
        {
            Flag = flag;
            Message = message;
        }

        /// <summary>
        /// 处理成功与否标志
        /// </summary>
        public bool Flag { get; set; }

        /// <summary>
        /// 返回的处理信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 用来标识返回信息 限定位数为6位
        /// 000000：成功
        /// 以下为是否回滚事务标识
        /// 010000：不回滚事务——如果无标识则默认回滚事务
        /// 以下为会员登录注册错误标识
        /// 100001：
        /// </summary>
        public string  Code { get; set; }
    }
    public class ReturnResult<T>
    {
        /// <summary>
        ///默认
        /// </summary>
        public ReturnResult()
        {
            Message = "系统错误";
        }

        public ReturnResult(bool flag, string message)
        {
            Flag = flag;
            Message = message;
        }
        public ReturnResult(bool flag, string message, string code)
        {
            Flag = flag;
            Message = message;
            Code = code;
        }
        /// <summary>
        /// 处理成功与否标志
        /// </summary>
        public bool Flag { get; set; }

        /// <summary>
        /// 返回的处理信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 用来标识返回信息 限定位数为6位
        /// 000000：成功
        /// 以下为是否回滚事务标识
        /// 010000：不回滚事务——如果无标识则默认回滚事务
        /// 以下为订单错误代码
        /// 100001：跨境直邮产品需要实名认证
        /// 110000-119999错误码为交易流程错误代码
        /// 110001：当前订单不在登录用户的订单列表中
        /// 110002: 当前订单结算额外佣金时，未找到对应服务站，但事务不回滚
        /// 以下为新增或修改数据错误代码
        /// 120000-129999错误码为新增或修改数据错误代码
        /// 120001：新增数据数量与需要新增数据数量不相符
        /// 120002：修改数据数量与需要修改数据数量不相符
        /// 120003：此数据中会员信息要求为已经实名认证，录入信息未实名
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public T Date { get; set; }
    }
}