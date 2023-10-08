using System;

namespace HJ212Interpreter.v2017.CommandParameters
{
    /// <summary>
    /// 提取现场机时间
    /// </summary>
    public class CP1011_Request : CP
    {
        /// <summary>
        /// 污染因子编码
        /// </summary>
        public string PolId { get; set; }
    }

    /// <summary>
    /// 上传现场机时间
    /// </summary>
    public class CP1011_Upload : CP
    {
        /// <summary>
        /// 污染因子编码
        /// </summary>
        public string PolId { get; set; }

        /// <summary>
        /// 系统时间
        /// </summary>
        public DateTime? SystemTime { get; set; }
    }
}
