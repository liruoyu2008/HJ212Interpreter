using HJ212Interpreter.v2017.Enum;
using System;

namespace HJ212Interpreter.v2017.CommandParameters.SubCommandParameter
{
    /// <summary>
    /// 实时数据(污染物、噪声、工况等)
    /// </summary>
    public class SubCP_Rtd : SubCP
    {
        /// <summary>
        /// 污染物采样时间
        /// </summary>
        public DateTime? SampleTime { get; set; }

        /// <summary>
        /// 污染物实时采样数据
        /// </summary>
        public double? Rtd { get; set; }

        /// <summary>
        /// 监测仪器数据标记
        /// </summary>
        public DataFlag? Flag { get; set; }

        /// <summary>
        /// 监测仪器扩充数据标记
        /// </summary>
        public string EFlag { get; set; }


        public SubCP_Rtd(string name) : base(name) { }
    }
}
