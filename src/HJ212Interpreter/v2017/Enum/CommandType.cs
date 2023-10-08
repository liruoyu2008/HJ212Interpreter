namespace HJ212Interpreter.v2017.Enum
{
    /// <summary>
    /// 命令类型
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        /// 请求命令
        /// </summary>
        REQUEST = 1,

        /// <summary>
        /// 上传命令
        /// </summary>
        UPLOAD = 2,

        /// <summary>
        /// 通知命令
        /// </summary>
        NOTICE = 3,

        /// <summary>
        /// 其他
        /// </summary>
        OTHER = 4,

        /// <summary>
        /// 不支持
        /// </summary>
        NONE = 5
    }
}
