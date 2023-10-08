using System;

namespace HJ212Interpreter.v2017.CommandParameters
{
    /// <summary>
    /// 设置采样时间周期
    /// </summary>
    public class CP3016 : CP
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
