using HJ212Interpreter.v2017.CommandParameters.SubCommandParameter;
using System.Collections.Generic;

namespace HJ212Interpreter.v2017.CommandParameters
{
    /// <summary>
    /// 设置现场机参数
    /// </summary>
    public class CP3021 : CP
    {
        /// <summary>
        /// 污染因子编码
        /// </summary>
        public string PolId { get; set; }

        /// <summary>
        /// 在线监控（监测）设备信息编码
        /// </summary>
        public string InfoId { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<SubCP> SubCP { get; set; }
    }
}
