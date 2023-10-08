using System;

namespace HJ212Interpreter.v2017
{
    /// <summary>
    /// HJ212-2017协议通讯包--标志位
    /// </summary>
    public class Flag
    {
        /// <summary>
        /// 版本号,0--HJ/T212-2005 1--HJ212-2017
        /// </summary>
        public byte V { get; private set; }

        /// <summary>
        /// 命令是否需要应答
        /// </summary>
        public bool A { get; private set; }

        /// <summary>
        /// 命令是否分了包
        /// </summary>
        public bool D { get; private set; }


        #region 构建、打包、解析

        /// <summary>
        /// 构建Flag对象
        /// </summary>
        /// <param name="a">是否需要应答</param>
        public Flag(bool a) : this(1, false, a)
        { }

        /// <summary>
        /// 构建Flag对象
        /// </summary>
        /// <param name="v">协议版本，0--HJ/T212-2005，1--HJ212-2007</param>
        /// <param name="d">是否已分包并拥有包号PNO和包数PNUM字段</param>
        /// <param name="a">是否需要应答</param>
        public Flag(byte v, bool d, bool a)
        {
            V = v;
            D = d;
            A = a;
        }

        /// <summary>
        /// 将Flag编码为字节
        /// </summary>
        /// <returns></returns>
        public byte ToByte()
        {
            int byteD = D ? 2 : 0;
            int byteA = A ? 1 : 0;
            byte flag = (byte)((V << 2) | byteD | byteA);
            return flag;
        }

        /// <summary>
        /// 将Flag编码为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToByte().ToString();
        }

        /// <summary>
        /// 将Flag字节转换为对象
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static Flag Parse(byte flag)
        {
            byte v = (byte)(flag >> 2);
            bool d = (flag & 2) == 2;
            bool a = (flag & 1) == 1;
            return new Flag(v, d, a);
        }

        /// <summary>
        /// 将Flag字符串转换为对象
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static Flag Parse(string flagStr)
        {
            byte flag = Convert.ToByte(flagStr);
            return Parse(flag);
        }

        #endregion
    }
}