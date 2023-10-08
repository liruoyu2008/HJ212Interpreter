namespace HJ212Interpreter.v2017.CommandParameters
{
    /// <summary>
    /// 提取出样时间
    /// </summary>
    public class CP3018_Request : CP
    {
        /// <summary>
        /// 污染因子编码
        /// </summary>
        public string PolId { get; set; }
    }

    /// <summary>
    /// 上传出样时间
    /// </summary>
    public class CP3018_Upload : CP
    {
        /// <summary>
        /// 污染因子编码
        /// </summary>
        public string PolId { get; set; }

        /// <summary>
        /// 设备采样起始时间
        /// </summary>
        public ushort? Stime { get; set; }
    }
}
