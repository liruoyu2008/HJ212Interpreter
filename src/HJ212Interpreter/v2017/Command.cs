using HJ212Interpreter.v2017.CommandParameters;
using HJ212Interpreter.v2017.Enum;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace HJ212Interpreter.v2017
{
    /// <summary>
    /// HJ212-2017协议通讯包--数据段
    /// </summary>
    public class Command
    {
        #region 非内建字段

        /// <summary>
        /// 允许的最大长度
        /// </summary>
        private const int MAX_LENGTH = 1024;

        /// <summary>
        /// 命令类型
        /// </summary>
        public CommandType CT { get; set; }

        /// <summary>
        /// 指令参数字符串(用于暂存分包命令的指令参数字符串，不含开始与结束符&&)
        /// </summary>
        public string CPStr { get; private set; }

        #endregion


        /// <summary>
        /// 请求编码
        /// </summary>
        public string QN { get; set; }

        /// <summary>
        /// 命令编码
        /// </summary>
        public int CN { get; set; }

        /// <summary>
        /// 系统编码
        /// </summary>
        public SystemType ST { get; set; }

        /// <summary>
        /// 访问密码
        /// </summary>
        public string PW { get; set; }

        /// <summary>
        /// 设备唯一标识
        /// </summary>
        public string MN { get; set; }

        /// <summary>
        /// 拆分包及应答标志
        /// </summary>
        public Flag Flag { get; private set; }

        /// <summary>
        /// 总包数
        /// </summary>
        public int PNUM { get; private set; }

        /// <summary>
        /// 包号
        /// </summary>
        public int PNO { get; private set; }

        /// <summary>
        /// 指令参数
        /// </summary>
        public CP CP { get; set; }


        #region 构建、打包、解析

        // 数据段基础正则格式
        private static Regex _pattern = new("^QN=([0-9]{17});ST=([0-9]{2});CN=([0-9]{4});PW=([0-9a-zA-Z]{0,6});MN=([0-9a-zA-Z]{1,24});Flag=([0-9]{1,4});(PNUM=([0-9]{1,4});PNO=([0-9]{1,4});)?CP=&&(.*)&&$");

        /// <summary>
        /// 构建数据段
        /// </summary>
        /// <param name="sourceType">源类型，即此消息的发送端类型</param>
        /// <param name="st">系统编码</param>
        /// <param name="cn">命令编码</param>
        /// <param name="pw">访问密码</param>
        /// <param name="mn">设备唯一标识</param>
        /// <param name="needResponse">是否需要应答</param>
        /// <param name="cp">指令参数字符串（不含开始与结束符&&）</param>
        public Command(SourceType sourceType, SystemType st, int cn, string pw, string mn, bool needResponse)
            : this(GetCommandType(sourceType, cn), DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                 st, cn, pw, mn, new Flag(needResponse), 0, 0)
        { }

        private Command(CommandType commandType, SystemType st, int cn, string pw, string mn, Flag flag, int pnum, int pno)
            : this(commandType, DateTime.Now.ToString("yyyyMMddHHmmssfff"), st, cn, pw, mn, flag, pnum, pno)
        { }

        private Command(SourceType sourceType, string qn, SystemType st, int cn, string pw, string mn, string flag, string pnum, string pno)
            : this(GetCommandType(sourceType, cn), qn, st, cn, pw, mn,
                Flag.Parse(flag), int.TryParse(pnum, out int a) ? a : 0, int.TryParse(pno, out int b) ? b : 0)
        { }

        private Command(CommandType ct, string qn, SystemType st, int cn, string pw, string mn, Flag flag)
            : this(ct, qn, st, cn, pw, mn, flag, 0, 0)
        { }

        private Command(CommandType commandType, string qn, SystemType st, int cn, string pw, string mn, Flag flag, int pnum, int pno)
        {
            CT = commandType;

            QN = qn;
            ST = st;
            CN = cn;
            PW = pw;
            MN = mn;
            Flag = flag;
            PNUM = pnum;
            PNO = pno;
        }

        /// <summary>
        /// 将命令打包为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var msg = PNUM == 0
                ? $"QN={QN};ST={(int)ST};CN={CN};PW={PW};MN={MN};Flag={Flag};CP=&&{CP}&&"
                : $"QN={QN};ST={(int)ST};CN={CN};PW={PW};MN={MN};Flag={Flag};PNUM={PNUM};PNO={PNO};CP=&&{CP}&&";
            return msg;
        }

        /// <summary>
        /// 将命令打包为完整的通讯包对象
        /// </summary>
        /// <returns></returns>
        public Message ToMessage()
        {
            return new Message(this);
        }

        /// <summary>
        /// 将命令打包为完整的通讯包ASCII字符串
        /// </summary>
        /// <returns></returns>
        public string ToMessageString()
        {
            return ToMessage().ToString();
        }

        /// <summary>
        /// 将数据段字符串解析为命令对象
        /// </summary>
        /// <param name="sourceType">源类型，即此命令的发送端类型</param>
        /// <param name="content">命令字符串</param>
        /// <returns></returns>
        public static Command Parse(SourceType sourceType, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException("hj212 content is null or empty.");
            }

            var match = _pattern.Match(content);
            if (!match.Success)
            {
                throw new ArgumentException("hj212 content format is incorrect.");
            }

            var cmd = new Command(
                sourceType: sourceType,
                qn: match.Groups[1].Value,
                st: (SystemType)int.Parse(match.Groups[2].Value),
                cn: int.Parse(match.Groups[3].Value),
                pw: match.Groups[4].Value,
                mn: match.Groups[5].Value,
                flag: match.Groups[6].Value,
                pnum: match.Groups[8].Value,
                pno: match.Groups[9].Value
            );

            // 若分包，则暂存字符串，否则，解析为CP对象
            if (cmd.Flag.D)
            {
                cmd.CPStr = content;
            }
            else
            {
                cmd.CP = CP.Parse(cmd.CN, cmd.CT, match.Groups[10].Value);
            }

            return cmd;
        }

        /// <summary>
        /// 将一个命令对象打包为多个命令对象，使得每个命令长度不超过规定的最大长度(一般为1024).
        /// </summary>
        /// <returns></returns>
        public Command[] Pack()
        {
            // 已分包，则返回
            if (Flag.D)
            {
                return new Command[] { this };
            }

            // 未超长，则返回(CP数据区最长950-7=943,命令最长1024)
            var cpstr = CP.ToString();
            if (CP.ToString().Length <= 943 && this.ToString().Length <= 1024)
            {
                return new Command[] { this };
            }

            // 分包(将所有字段按最长处理，得出cp数据区最长为916)
            var pnum = Convert.ToInt32(Math.Ceiling((double)cpstr.Length / 916));
            if (pnum > 9999)
            {
                throw new ArgumentException("hj212 CP'content is too large to pack.");
            }
            var res = new Command[pnum];
            for (int i = 0; i < pnum; i++)
            {
                res[i] = new Command(CT, ST, CN, PW, MN, new Flag(Flag.V, true, Flag.A), pnum, i + 1)
                {
                    CPStr = cpstr.Substring(i * 916, Math.Min(cpstr.Length - i * 916, 916)),
                };
            }

            return res;
        }

        /// <summary>
        /// 将多个命令对象打包为单个命令对象（忽略最大长度，并取头包的基础信息）.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static Command UnPack(Command[] cmds)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < cmds.Length; i++)
            {
                // 若分包规则有问题，则抛出异常
                if (!cmds[i].Flag.D || cmds[i].PNUM != cmds.Length || cmds[i].PNO != i + 1)
                {
                    throw new ArgumentException("hj212 command-fragment's D/PNUM/PNO does not match the rules.");
                }
                builder.Append(cmds[i].CPStr);
            }

            var res = new Command(cmds[0].CT, cmds[0].QN, cmds[0].ST, cmds[0].CN, cmds[0].PW, cmds[0].MN, new Flag(cmds[0].Flag.V, false, cmds[0].Flag.A))
            {
                CP = CP.Parse(cmds[0].CN, cmds[0].CT, builder.ToString())
            };
            return res;
        }

        #endregion


        #region 构造对应响应命令或执行结果反馈命令

        /// <summary>
        /// 构造源命令的响应命令(分包时需要对每包数据进行应答),响应结果默认为RequestResult.READY.若源命令无需响应，返回null.
        /// </summary>
        /// <returns></returns>
        public Command CreateRequestResponse()
        {
            if (!this.Flag.A)
            {
                return null;
            }

            switch (CT)
            {
                case CommandType.REQUEST:
                    return new Command(CommandType.OTHER, this.ST, 9011, this.PW, this.MN, new Flag(false), 0, 0) { CP = new CP9011() { QnRtn = RequestResult.READY } };
                case CommandType.NOTICE:
                    return new Command(CommandType.OTHER, this.ST, 9013, this.PW, this.MN, new Flag(false), 0, 0) { CP = new CP9013() };
                case CommandType.UPLOAD:
                case CommandType.OTHER:
                    return new Command(CommandType.OTHER, this.ST, 9014, this.PW, this.MN, new Flag(false), 0, 0) { CP = new CP9014() };
                default: return null;
            }
        }

        /// <summary>
        /// 构造源命令的执行结果返回命令(分包时仅需对总包数据进行一次响应),执行结果默认为ExecuteResult.SUCCESS.
        /// </summary>
        /// <returns></returns>
        public Command CreateExecuteResponse()
        {
            // 分包时无需要对每包数据进行执行返回
            if (this.Flag.D)
            {
                return null;
            }

            return this.CT == CommandType.REQUEST
                ? new Command(CommandType.OTHER, this.ST, 9012, this.PW, this.MN, new Flag(false), 0, 0) { CP = new CP9012() { ExeRtn = ExecuteResult.SUCCESS } }
                : null;
        }

        #endregion


        #region 对照表

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="st">源类型</param>
        /// <param name="cn">命令编码</param>
        /// <returns></returns>
        public static CommandType GetCommandType(SourceType st, int cn)
        {
            return cn switch
            {
                // 初始化命令
                1000 => CommandType.REQUEST,
                >= 1000 and <= 1010 => CommandType.NONE,

                // 参数命令
                1011 => st == SourceType.Master ? CommandType.REQUEST : CommandType.UPLOAD,
                1012 => CommandType.REQUEST,
                1013 => CommandType.NOTICE,
                1061 => st == SourceType.Master ? CommandType.REQUEST : CommandType.UPLOAD,
                1062 => CommandType.REQUEST,
                1063 => st == SourceType.Master ? CommandType.REQUEST : CommandType.UPLOAD,
                1064 => CommandType.REQUEST,
                1072 => CommandType.REQUEST,
                >= 1073 and <= 1999 => CommandType.NONE,

                // 数据命令
                // 实时数据
                2011 => st == SourceType.Master ? CommandType.REQUEST : CommandType.UPLOAD,
                2012 => CommandType.NOTICE,
                // 设备状态
                2021 => st == SourceType.Master ? CommandType.REQUEST : CommandType.UPLOAD,
                2022 => CommandType.NOTICE,
                // 日数据
                2031 => st == SourceType.Master ? CommandType.REQUEST : CommandType.UPLOAD,
                2041 => st == SourceType.Master ? CommandType.REQUEST : CommandType.UPLOAD,
                // 分钟数据
                2051 => st == SourceType.Master ? CommandType.REQUEST : CommandType.UPLOAD,
                // 小时数据
                2061 => st == SourceType.Master ? CommandType.REQUEST : CommandType.UPLOAD,
                // 其他数据
                2081 => CommandType.UPLOAD,
                >= 2082 and <= 2999 => CommandType.NONE,

                // 控制命令
                3011 => CommandType.REQUEST,
                3012 => CommandType.REQUEST,
                3013 => CommandType.REQUEST,
                3014 => CommandType.REQUEST,
                3015 => st == SourceType.Master ? CommandType.REQUEST : CommandType.UPLOAD,
                3016 => CommandType.REQUEST,
                3017 => st == SourceType.Master ? CommandType.REQUEST : CommandType.UPLOAD,
                3018 => st == SourceType.Master ? CommandType.REQUEST : CommandType.UPLOAD,
                3019 => st == SourceType.Master ? CommandType.REQUEST : CommandType.UPLOAD,
                3020 => st == SourceType.Master ? CommandType.REQUEST : CommandType.UPLOAD,
                3021 => CommandType.REQUEST,
                >= 3022 and <= 3999 => CommandType.NONE,

                // 交互命令
                9011 => CommandType.OTHER,
                9012 => CommandType.OTHER,
                9013 => st == SourceType.Master ? CommandType.OTHER : CommandType.OTHER,
                9014 => st == SourceType.Master ? CommandType.OTHER : CommandType.OTHER,
                >= 9015 and <= 9999 => CommandType.NONE,

                // 默认
                _ => CommandType.NONE
            }; ;
        }

        #endregion
    }
}
