using System;

namespace HJ212Interpreter.v2017.CommandParameters
{
    /// <summary>
    /// 超标留样
    /// </summary>
    public class CP3015_Request : CP
    {
    }

    /// <summary>
    /// 上传超标留样
    /// </summary>
    public class CP3015_Upload : CP
    {
        /// <summary>
        /// 数据时间信息
        /// </summary>
        public DateTime? DataTime { get; set; }

        /// <summary>
        /// 留杨瓶编号
        /// </summary>
        public byte? VaseNo { get; set; }
    }
}
