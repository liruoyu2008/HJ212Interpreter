using HJ212Interpreter.v2017.Enum;

namespace HJ212Interpreter.v2017.CommandParameters.SubCommandParameter
{
    /// <summary>
    /// 历史数据（污染物、噪声等）
    /// </summary>
    public class SubCP_Hd : SubCP
    {
        /// <summary>
        /// 污染物指定时间内最小值
        /// </summary>
        public double? Min { get; set; }

        /// <summary>
        /// 污染物指定时间内平均值
        /// </summary>
        public double? Avg { get; set; }

        /// <summary>
        /// 污染物指定时间内最大值
        /// </summary>
        public double? Max { get; set; }

        /// <summary>
        /// 污染物指定时间内最大折算值
        /// </summary>
        public double? ZsMin { get; set; }

        /// <summary>
        /// 污染物指定时间内平均折算值
        /// </summary>
        public double? ZsAvg { get; set; }

        /// <summary>
        /// 污染物指定时间内最大折算值
        /// </summary>
        public double? ZsMax { get; set; }

        /// <summary>
        /// 监测仪器数据标记
        /// </summary>
        public DataFlag? Flag { get; set; }

        /// <summary>
        /// 监测仪器扩充数据标记
        /// </summary>
        public string EFlag { get; set; }

        /// <summary>
        /// 污染物指定时间内累计值
        /// </summary>
        public double? Cou { get; set; }

        /// <summary>
        /// 噪声监测时间段内数据
        /// </summary>
        public double? Data { get; set; }

        /// <summary>
        /// 噪声昼间数据
        /// </summary>
        public double? DayData { get; set; }

        /// <summary>
        /// 噪声夜间数据
        /// </summary>
        public double? NightData { get; set; }


        public SubCP_Hd(string name) : base(name) { }
    }
}
