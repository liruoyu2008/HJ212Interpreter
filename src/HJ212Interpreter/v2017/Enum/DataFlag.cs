namespace HJ212Interpreter.v2017.Enum
{
    /// <summary>
    /// 数据标记表
    /// </summary>
    public enum DataFlag
    {
        /// <summary>
        /// 在线监控（监测）仪器仪表工作正常
        /// </summary>
        N,

        /// <summary>
        /// 在线监控（监测）仪器仪表停运
        /// </summary>
        F,

        /// <summary>
        /// 在线监控（监测）仪器仪表处于维护期间产生的数据
        /// </summary>
        M,

        /// <summary>
        /// 手工输入的设定值
        /// </summary>
        S,

        /// <summary>
        /// 在线监控（监测）仪器仪表故障
        /// </summary>
        D,

        /// <summary>
        /// 在线监控（监测）仪器仪表处于校准状态
        /// </summary>
        C,

        /// <summary>
        /// 在线监控（监测）仪器仪表采样数值超过测量上限
        /// </summary>
        T,

        /// <summary>
        /// 在线监控（监测）仪器仪表与数采仪通讯异常
        /// </summary>
        B,
    }
}