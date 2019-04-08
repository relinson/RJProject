using System;
using System.Runtime.Serialization;

namespace System
{
    /// <summary>
    /// 取消操作异常消息.
    /// 可用于对ThreadAbortException异常的友好包装
    /// </summary>
    public class CanceledException : ApplicationException
    {
        public CanceledException(string message = "用户取消了当前操作！", Exception ex = null)
            : base(message, ex)
        {

        }

        public CanceledException(Exception ex)
            : base("用户取消了当前操作！", ex)
        {

        }
         

        private CanceledException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}