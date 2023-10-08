using System;

namespace HJ212Interpreter.v2017.CommandParameters
{
    /// <summary>
    /// 提取采样时间周期
    /// </summary>
    public class CP3017_Request : CP
    {
        /// <summary>
        /// 污染因子编码
        /// </summary>
        public string PolId { get; set; }
    }

    /// <summary>
    /// 上传采样时间周期
    /// </summary>
    public class CP3017_Upload : CP
    {
        /// <summary>
        /// 污染因子编码
        /// </summary>
        public string PolId { get; set; }

        /// <summary>
        /// 设备采样起始时间
        /// </summary>
        public DateTime? CstartTime { get; set; }

        /// <summary>
        /// 采样周期
        /// </summary>
        public byte? Ctime { get; set; }
    }
}
