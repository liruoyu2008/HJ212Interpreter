namespace HJ212Interpreter.v2017.CommandParameters.SubCommandParameter
{
    /// <summary>
    /// 设备运行时间日历史数据
    /// </summary>
    public class SubCP_Rt : SubCP
    {
        /// <summary>
        /// 污染治理设施一日内的运行时间
        /// </summary>
        public double? RT { get; set; }


        public SubCP_Rt(string name) : base(name) { }
    }
}
