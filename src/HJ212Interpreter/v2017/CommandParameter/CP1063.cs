namespace HJ212Interpreter.v2017.CommandParameters
{
    /// <summary>
    /// 提取分钟数据间隔
    /// </summary>
    public class CP1063_Request : CP
    {
    }

    /// <summary>
    /// 上传分钟数据间隔
    /// </summary>
    public class CP1063_Upload : CP
    {
        /// <summary>
        /// 分钟数据上报间隔
        /// </summary>
        public byte? MinInterval { get; set; }
    }
}
