using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HJ212Interpreter.v2017.CommandParameters.SubCommandParameter;
using HJ212Interpreter.v2017.Enum;

namespace HJ212Interpreter.v2017.CommandParameters
{
    /// <summary>
    /// 空参CP指令
    /// </summary>
    public abstract class CP
    {
        /// <summary>
        /// 得到一个<属性名,属性值>字典,其中SubCP被处理为子字典<项名，<属性名，属性值>>.注意：值为null的属性会被排除在外
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> ToDictionary()
        {
            var res = new Dictionary<string, object>();
            var props = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var k = prop.Name.ValidateCP();
                var v = prop.GetValue(this);
                if (v == null)
                {
                    continue;
                }

                if (v is not List<SubCP>)
                {
                    res[k] = v;
                }
                else
                {
                    var list = v as List<SubCP>;
                    foreach (var subCP in list)
                    {
                        var dic = subCP.ToDictionary();
                        if (dic != null)
                        {
                            res[subCP.Name] = dic;
                        }
                    }
                }
            }

            return res.Count == 0 ? null : res;
        }

        /// <summary>
        /// 转换为指令参数字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var props = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (props.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new();
            foreach (var prop in props)
            {
                if (prop.Name != "SubCP")
                {
                    var k = prop.Name.ValidateCP();
                    var v = prop.GetValue(this).ToCPString(prop.Name).ValidateCP();
                    if (v != null)
                    {
                        sb.Append($"{k}={v};");
                    }
                }
                // SubCP
                else
                {
                    var list = prop.GetValue(this) as List<SubCP>;
                    if (list == null)
                    {
                        continue;
                    }

                    foreach (var k in list)
                    {
                        var subCPStr = k.ToString();
                        if (subCPStr != null)
                        {
                            sb.Append($"{subCPStr};");
                        }
                    }
                    continue;
                }
            }
            return sb.ToString().TrimEnd(';');
        }

        /// <summary>
        /// 将字符串转换为指定的指令参数对象
        /// </summary>
        /// <param name="cpStr"></param>
        /// <returns></returns>
        public static T Parse<T>(string cpStr) where T : CP, new()
        {
            var cp = new T();
            List<SubCP> subCPDic = new List<SubCP>();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var list1 = cpStr.Split(';');
            foreach (var item in list1)
            {
                if (!item.Contains(','))
                {
                    // 不含等于号跳过
                    if (!item.Contains('='))
                    {
                        continue;
                    }

                    var kv = item.Split('=');
                    var k = kv[0].ValidateCP();
                    var v = kv[1].ValidateCP();
                    var prop = props.FirstOrDefault(it => it.Name == k);
                    if (prop != null)
                    {
                        prop.SetValue(cp, v.ToCPValue(prop.Name, prop.PropertyType));
                    }
                }
                // SubCP
                else
                {
                    var subCP = SubCP.Parse(typeof(T), item);
                    if (subCP != null)
                    {
                        subCPDic.Add(subCP);
                    }
                }
            }

            if (subCPDic.Count > 0)
            {
                var prop = props.FirstOrDefault(it => it.Name == "SubCP");
                if (prop != null)
                {
                    prop.SetValue(cp, subCPDic);
                }
            }

            return cp;
        }


        #region 对照表

        /// <summary>
        /// 将字符串转换为指定命令号、命令类型所对应的指令参数对象
        /// </summary>
        /// <param name="cn"></param>
        /// <param name="ct"></param>
        /// <param name="cpStr"></param>
        /// <returns></returns>
        public static CP Parse(int cn, CommandType ct, string cpStr)
        {
            switch ((cn, ct))
            {
                // 初始化命令
                case (1000, _): return Parse<CP1000>(cpStr);
                case ( >= 1001 and <= 1010, _): return null;

                // 参数命令
                case (1011, CommandType.REQUEST): return Parse<CP1011_Request>(cpStr);
                case (1011, CommandType.UPLOAD): return Parse<CP1011_Upload>(cpStr);
                case (1012, _): return Parse<CP1012>(cpStr);
                case (1013, _): return Parse<CP1013>(cpStr);
                case (1061, CommandType.REQUEST): return Parse<CP1061_Request>(cpStr);
                case (1061, CommandType.UPLOAD): return Parse<CP1061_Upload>(cpStr);
                case (1062, _): return Parse<CP1062>(cpStr);
                case (1063, CommandType.REQUEST): return Parse<CP1063_Request>(cpStr);
                case (1063, CommandType.UPLOAD): return Parse<CP1063_Upload>(cpStr);
                case (1064, _): return Parse<CP1064>(cpStr);
                case (1072, _): return Parse<CP1072>(cpStr);
                case ( >= 1073 and <= 1999, _): return null;

                // 数据命令
                case (2011, CommandType.REQUEST): return Parse<CP2011_Request>(cpStr);
                case (2011, CommandType.UPLOAD): return Parse<CP2011_Upload>(cpStr);
                case (2012, _): return Parse<CP2012>(cpStr);
                case (2021, CommandType.REQUEST): return Parse<CP2021_Request>(cpStr);
                case (2021, CommandType.UPLOAD): return Parse<CP2021_Upload>(cpStr);
                case (2022, _): return Parse<CP2022>(cpStr);
                case (2031, CommandType.REQUEST): return Parse<CP2031_Request>(cpStr);
                case (2031, CommandType.UPLOAD): return Parse<CP2031_Upload>(cpStr);
                case (2041, CommandType.REQUEST): return Parse<CP2041_Request>(cpStr);
                case (2041, CommandType.UPLOAD): return Parse<CP2041_Upload>(cpStr);
                case (2051, CommandType.REQUEST): return Parse<CP2051_Request>(cpStr);
                case (2051, CommandType.UPLOAD): return Parse<CP2051_Upload>(cpStr);
                case (2061, CommandType.REQUEST): return Parse<CP2061_Request>(cpStr);
                case (2061, CommandType.UPLOAD): return Parse<CP2061_Upload>(cpStr);
                case (2081, _): return Parse<CP2081>(cpStr);
                case ( >= 2082 and <= 2999, _): return null;

                // 控制命令
                case (3011, _): return Parse<CP3011>(cpStr);
                case (3012, _): return Parse<CP3012>(cpStr);
                case (3013, _): return Parse<CP3013>(cpStr);
                case (3014, _): return Parse<CP3014>(cpStr);
                case (3015, CommandType.REQUEST): return Parse<CP3015_Request>(cpStr);
                case (3015, CommandType.UPLOAD): return Parse<CP3015_Upload>(cpStr);
                case (3016, _): return Parse<CP3016>(cpStr);
                case (3017, CommandType.REQUEST): return Parse<CP3017_Request>(cpStr);
                case (3017, CommandType.UPLOAD): return Parse<CP3017_Upload>(cpStr);
                case (3018, CommandType.REQUEST): return Parse<CP3018_Request>(cpStr);
                case (3018, CommandType.UPLOAD): return Parse<CP3018_Upload>(cpStr);
                case (3019, CommandType.REQUEST): return Parse<CP3019_Request>(cpStr);
                case (3019, CommandType.UPLOAD): return Parse<CP3019_Upload>(cpStr);
                case (3020, CommandType.REQUEST): return Parse<CP3020_Request>(cpStr);
                case (3020, CommandType.UPLOAD): return Parse<CP3020_Upload>(cpStr);
                case (3021, _): return Parse<CP3021>(cpStr);
                case ( >= 3022 and <= 3999, _): return null;

                // 交互命令
                case (9011, _): return Parse<CP9011>(cpStr);
                case (9012, _): return Parse<CP9012>(cpStr);
                case (9013, _): return Parse<CP9013>(cpStr);
                case (9014, _): return Parse<CP9014>(cpStr);
                case ( >= 9015 and <= 9999, _): return null;

                // 其他
                default: return null;
            }
        }

        #endregion
    }
}
