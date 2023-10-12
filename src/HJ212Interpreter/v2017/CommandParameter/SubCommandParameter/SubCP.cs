using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HJ212Interpreter.v2017.CommandParameters.SubCommandParameter
{
    /// <summary>
    /// 二级数据项（若不含某项数据，则值为null）
    /// </summary>
    public abstract class SubCP
    {
        /// <summary>
        /// 子项名称. 非内建字段
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 构建子项
        /// </summary>
        /// <param name="name">子项名称</param>
        public SubCP(string name)
        {
            // 规范校验
            name.ValidateCP();

            // 空校验
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("hj212 subcp'name cannot be empty or null.");
            }

            Name = name;
        }

        /// <summary>
        /// 将SubCP转换为<属性名,属性值>键值对字典。注意：值为null的属性会被排除在外,若字典为空，则返回null
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> ToDictionary()
        {
            var res = new Dictionary<string, object>();
            var props = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(it => it.Name != "Name");
            foreach (var prop in props)
            {
                var k = prop.Name;
                var v = prop.GetValue(this);
                if (v != null)
                {
                    res[k] = v;
                }
            }

            return res.Count == 0 ? null : res;
        }

        /// <summary>
        /// 转化为CP子字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var props = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(it => it.Name != "Name");

            StringBuilder sb = new();
            foreach (var prop in props)
            {
                var obj = prop.GetValue(this);

                // 空值略过
                if (obj == null)
                {
                    continue;
                }

                var k = prop.Name;
                var v = obj.ToCPString(prop.Name).ValidateCP();
                if (v != null)
                {
                    sb.Append($"{Name}-{k}={v},");
                }
            }

            return sb.Length > 0 ? sb.ToString().TrimEnd(',') : null;
        }

        /// <summary>
        /// 从CP子字符串解析为指定的CP子项
        /// </summary>
        /// <param name="subCPStr"></param>
        /// <returns></returns>
        public static T Parse<T>(string subCPStr) where T : SubCP
        {
            var name = subCPStr.Substring(0, subCPStr.IndexOf('-'));
            var subCP = Activator.CreateInstance(typeof(T), name) as T;
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(it => it.Name != "Name");

            var list = subCPStr.Split(',');
            foreach (var item in list)
            {
                // 不含等于号跳过
                if (!item.Contains('='))
                    continue;
                var namek_v = item.Split('=');
                if (!namek_v[0].Contains('-'))
                    continue;
                var namek = namek_v[0].ValidateCP().Split('-');
                var k = namek[1];
                var v = namek_v[1].ValidateCP();
                var prop = props.FirstOrDefault(it => it.Name == k);
                if (prop != null)
                {
                    prop.SetValue(subCP, v.ToCPValue(prop.Name, prop.PropertyType));
                }
            }

            return subCP;
        }


        #region 对照表

        /// <summary>
        /// 从CP子字符串解析出对应CP类型的CP子项
        /// </summary>
        /// <param name="cpType"></param>
        /// <param name="subCPStr"></param>
        /// <returns></returns>
        public static SubCP Parse(Type cpType, string subCPStr)
        {
            if (cpType == typeof(CP2011_Upload))
            {
                return Parse<SubCP_Rtd>(subCPStr);
            }
            else if (cpType == typeof(CP2031_Upload)
                || cpType == typeof(CP2051_Upload)
                || cpType == typeof(CP2061_Upload))
            {
                return Parse<SubCP_Hd>(subCPStr);
            }
            else if (cpType == typeof(CP2021_Upload))
            {
                return Parse<SubCP_Rs>(subCPStr);
            }
            else if (cpType == typeof(CP2041_Upload))
            {
                return Parse<SubCP_Rt>(subCPStr);
            }
            else if (cpType == typeof(CP3019_Upload))
            {
                return Parse<SubCP_SN>(subCPStr);
            }
            else if (cpType == typeof(CP3020_Upload)
                || cpType == typeof(CP3021))
            {
                return Parse<SubCP_Info>(subCPStr);
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
