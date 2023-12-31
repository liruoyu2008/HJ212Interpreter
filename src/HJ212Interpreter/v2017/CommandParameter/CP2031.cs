﻿using HJ212Interpreter.v2017.CommandParameters.SubCommandParameter;
using System;
using System.Collections.Generic;

namespace HJ212Interpreter.v2017.CommandParameters
{
    /// <summary>
    /// 取污染物（或噪声声级）日历史数据
    /// </summary>
    public class CP2031_Request : CP
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    /// <summary>
    /// 上传污染物（或噪声声级）日历史数据
    /// </summary>
    public class CP2031_Upload : CP
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
