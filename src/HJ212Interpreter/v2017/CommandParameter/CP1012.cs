using System;

namespace HJ212Interpreter.v2017.CommandParameters
{
    /// <summary>
    /// 设置现场机时间
    /// </summary>
    public class CP1012 : CP
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
