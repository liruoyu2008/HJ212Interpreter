using HJ212Interpreter.v2017.Enum;
using System;
using System.Globalization;
using System.Text;

namespace HJ212Interpreter.v2017
{
    /// <summary>
    /// 公共帮助类
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// 计算CRC16校验
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static ushort ToCRC16(this string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            int crcRegister = 0xFFFF;
            for (int i = 0; i < bytes.Length; i++)
            {
                crcRegister = (crcRegister >> 8) ^ bytes[i];
                for (int j = 0; j < 8; j++)
                {
                    int check = crcRegister & 0x0001;
                    crcRegister >>= 1;
                    if (check == 0x0001)
                    {
                        crcRegister ^= 0xA001;
                    }
                }
            }
            return (ushort)crcRegister;
        }

        /// <summary>
        /// CP指令值字符串校验
        /// </summary>
        /// <param name="keyOrValue"></param>
        public static string ValidateCP(this string keyOrValue)
        {
            // 非法字符校验
            if (keyOrValue != null)
            {
                if (keyOrValue.Contains(",") || keyOrValue.Contains(";") || keyOrValue.Contains("="))
                {
                    new ArgumentException("hj212 CP's key and value shouldn't contain ',' or ';' or '='");
                }
            }

            return keyOrValue;
        }

        /// <summary>
        /// 将项目值转换为指令字符串值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ToCPString(this object val, string name)
        {
            switch (val)
            {
                case DateTime time:
                    return name == "CstartTime"
                        ? time.ToString("HHmmss")
                        : time.ToString("yyyyMMddHHmmss");
                case RequestResult rr: return ((int)rr).ToString();
                case ExecuteResult er: return ((int)er).ToString();
                default: return val?.ToString();
            }
        }

        /// <summary>
        /// 值类型转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object ToCPValue(this string value, string key, Type conversionType)
        {
            var baseType = Nullable.GetUnderlyingType(conversionType) ?? conversionType;
            if (baseType == typeof(DateTime))
            {
                switch (key)
                {
                    case "CstartTime": return DateTime.ParseExact(value, "HHmmss", CultureInfo.InvariantCulture);
                    default: return DateTime.ParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                }
            }
            else if (baseType.IsEnum)
            {
                return System.Enum.Parse(baseType, value);
            }
            else
            {
                return Convert.ChangeType(value, baseType);
            }
        }
    }
}
