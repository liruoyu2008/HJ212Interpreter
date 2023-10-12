using HJ212Interpreter.v2017.CommandParameters.SubCommandParameter;
using System;
using System.Collections.Generic;

namespace HJ212Interpreter.v2017.CommandParameters
{
    /// <summary>
    /// 取污染物（或噪声声级、工况等）实时数据（启动命令）
    /// </summary>
    public class CP2011_Request : CP
    {
    }

    /// <summary>
    /// 上传污染物（或噪声声级、工况等）实时数据
    /// </summary>
    public class CP2011_Upload : CP
    {
        /// <summary>
        /// 数据时间信息
        /// </summary>
        public DateTime? DataTime { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<SubCP> SubCP { get; set; }
    }
}
