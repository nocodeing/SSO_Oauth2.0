using System;

namespace Model
{
    /// <summary>
    /// DTO
    /// </summary>
    public class ResponseBase
    {
        //响应标识
        public string Identity { set; get; }
        public Exception ResponseException { set; get; }
        public int ErrorCode { set; get; }
        public bool IsSuccess { set; get; }
        public string Message { set; get; }
        public object ReturnData { set; get; }

    }
}
