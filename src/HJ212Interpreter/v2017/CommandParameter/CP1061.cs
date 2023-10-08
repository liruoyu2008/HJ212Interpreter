namespace HJ212Interpreter.v2017.CommandParameters
{
    /// <summary>
    /// 提取实时数据间隔
    /// </summary>
    public class CP1061_Request : CP
    {
    }

    /// <summary>
    /// 上传实时数据间隔
    /// </summary>
    public class CP1061_Upload : CP
    {
        /// <summary>
        /// 实时采样数据上报间隔
        /// </summary>
        public ushort? RtdInterval { get; set; }
    }
}
