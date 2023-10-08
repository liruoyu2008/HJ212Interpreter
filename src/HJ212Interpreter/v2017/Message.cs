using HJ212Interpreter.v2017.Enum;
using System;
using System.Text.RegularExpressions;

namespace HJ212Interpreter.v2017
{
    /// <summary>
    /// HJ212-2017协议通讯包--通讯包
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 包头
        /// </summary>
        public const string Head = "##";

        /// <summary>
        /// 数据段长度
        /// </summary>
        public int Length { get => (Command?.ToString() ?? string.Empty).Length; }

        /// <summary>
        /// 数据段
        /// </summary>
        public Command Command { get; set; }

        /// <summary>
        /// 数据段CRC校验值
        /// </summary>
        public ushort CRC { get => (Command?.ToString() ?? string.Empty).ToCRC16(); }

        /// <summary>
        /// 包尾
        /// </summary>
        public const string Tail = "\r\n";


        #region 构建、打包、解析

        // 通讯包基础正则格式
        private static Regex _pattern = new("^##[0-9]{4}.{1,1024}[0-9a-fA-F]{4}\r\n$");

        /// <summary>
        /// 构造通讯包
        /// </summary>
        public Message() { }

        /// <summary>
        /// 构造通讯包
        /// </summary>
        /// <param name="content">数据段字符串</param>
        public Message(Command content)
        {
            Command = content;
        }

        /// <summary>
        /// 打包成通讯包字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var msg = $"{Head}{Length:D4}{Command}{CRC:X2}{Tail}";
            return msg;
        }

        /// <summary>
        /// 将通讯包字符串解析为对象
        /// </summary>
        /// <param name="sourceType">消息源的类型</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Message Parse(SourceType sourceType, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("hj212 message is null or empty.");
            }

            if (!_pattern.IsMatch(message))
            {
                throw new ArgumentException("hj212 message format is incorrect.");
            }

            // 摘取内容
            var len = Convert.ToInt32(message.Substring(2, 4));
            var content = message.Substring(6, message.Length - 12);
            var crc = Convert.ToUInt16(message.Substring(message.Length - 6, 4), 16);

            // 长度校验
            if (len != content.Length)
            {
                throw new ArgumentException("hj212 message length is incorrect.");
            }

            // CRC校验
            if (crc != content.ToCRC16())
            {
                throw new ArgumentException("hj212 message crc16 is incorrect.");
            }

            var msg = new Message()
            {
                Command = Command.Parse(sourceType, content)
            };
            return msg;
        }

        #endregion
    }
}