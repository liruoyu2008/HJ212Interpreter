namespace HJ212Interpreter.v2017.Enum
{
    /// <summary>
    /// 请求命令返回表
    /// </summary>
    public enum RequestResult
    {

        /// <summary>
        /// 准备执行请求
        /// </summary>
        READY = 1,

        /// <summary>
        /// 请求被拒绝
        /// </summary>
        REFUSE = 2,

        /// <summary>
        /// PW 错误
        /// </summary>
        PW_ERROR = 3,

        /// <summary>
        /// MN 错误
        /// </summary>
        MN_ERROR = 4,

        /// <summary>
        /// ST 错误
        /// </summary>
        ST_ERROR = 5,

        /// <summary>
        /// Flag 错误
        /// </summary>
        FLAG_ERROR = 6,

        /// <summary>
        /// QN 错误
        /// </summary>
        QN_ERROR = 7,

        /// <summary>
        /// CN错误
        /// </summary>
        CN_ERROR = 8,

        /// <summary>
        /// CRC 校验错误
        /// </summary>
        CRC_ERROR = 9,

        /// <summary>
        /// 未知错误
        /// </summary>
        UNDEFINED_ERROR = 100
    }
}
