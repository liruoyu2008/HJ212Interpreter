namespace HJ212Interpreter.v2017.CommandParameters.SubCommandParameter
{
    /// <summary>
    /// 设备唯一标识
    /// </summary>
    public class SubCP_SN : SubCP
    {
        /// <summary>
        /// 在线监控（监测）仪器仪表编码
        /// </summary>
        public string SN { get; set; }


        public SubCP_SN(string name) : base(name) { }
    }
}
