namespace HJ212Interpreter.v2017.Enum
{
    /// <summary>
    /// 执行结果定义表
    /// </summary>
    public enum ExecuteResult
    {
        /// <summary>
        /// 执行成功
        /// </summary>
        SUCCESS = 1,

        /// <summary>
        /// 执行失败，但不知道原因
        /// </summary>
        FAIL_NO_REASON = 2,

        /// <summary>
        /// 命令请求条件错误
        /// </summary>
        CONDITON_ERROR = 3,

        /// <summary>
        /// 通讯超时
        /// </summary>
        TIME_OUT = 4,

        /// <summary>
        /// 系统繁忙不能执行
        /// </summary>
        SYSTEM_BUSY = 5,

        /// <summary>
        /// 系统故障
        /// </summary>
        SYSTEM_ERROR = 6,

        /// <summary>
        /// 没有数据
        /// </summary>
        NO_DATA = 100
    }
}
