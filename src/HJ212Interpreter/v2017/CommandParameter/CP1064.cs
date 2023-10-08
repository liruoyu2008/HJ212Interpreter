namespace HJ212Interpreter.v2017.CommandParameters
{
    /// <summary>
    /// 设置分钟数据间隔
    /// </summary>
    public class CP1064 : CP
    {
        /// <summary>
        /// 分钟数据上报间隔
        /// </summary>
        public byte? MinInterval { get; set; }
    }
}
