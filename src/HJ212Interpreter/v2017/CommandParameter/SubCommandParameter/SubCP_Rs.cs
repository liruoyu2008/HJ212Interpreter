namespace HJ212Interpreter.v2017.CommandParameters.SubCommandParameter
{
    /// <summary>
    /// 设备运行状态数据
    /// </summary>
    public class SubCP_Rs : SubCP
    {
        /// <summary>
        /// 污染治理设施运行状态实时采样
        /// </summary>
        public byte? RS { get; set; }


        public SubCP_Rs(string name) : base(name) { }
    }
}
