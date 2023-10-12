using HJ212Interpreter.v2017.CommandParameters.SubCommandParameter;
using System;
using System.Collections.Generic;

namespace HJ212Interpreter.v2017.CommandParameters
{
    /// <summary>
    /// 提取设备唯一标识
    /// </summary>
    public class CP3019_Request : CP
    {
        /// <summary>
        /// 污染因子编码
        /// </summary>
        public string PolId { get; set; }
    }

    /// <summary>
    /// 上传设备唯一标识
    /// </summary>
    public class CP3019_Upload : CP
    {
        /// <summary>
        /// 污染因子编码
        /// </summary>
        public string PolId { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<SubCP> SubCP { get; set; }
    }
}
