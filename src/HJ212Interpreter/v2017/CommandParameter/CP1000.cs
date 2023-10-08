namespace HJ212Interpreter.v2017.CommandParameters
{
    /// <summary>
    /// 设置超时时间及重发次数
    /// </summary>
    public class CP1000 : CP
    {
        /// <summary>
        /// 超时时间
        /// </summary>
        public byte? OverTime { get; set; }

        /// <summary>
        /// 重发次数
        /// </summary>
        public byte? ReCount { get; set; }
    }
}
