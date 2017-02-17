using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;

namespace ZY.Common.Tools
{
    /// <summary>
    /// 泛型相关工具类（可用Linq替代）
    /// </summary>
    public static class GenericTool
    {
        /// <summary>
        /// 去除Dictionary中Value的重复值
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns>Value中无重复的Dictionary</returns>
        public static Dictionary<T1, T2> DistinctValue<T1, T2>(Dictionary<T1, T2> dictionary)
        {
            Contract.Requires(dictionary != null);
            Dictionary<T1, T2> distinct = new Dictionary<T1, T2>();
            foreach (var item in dictionary)
            {
                if (!distinct.ContainsValue(item.Value))
                    distinct.Add(item.Key, item.Value);
            }

            return distinct;
        }

        /// <summary>
        /// 根据column名称获取指定DataRow的单元值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="partName"></param>
        /// <returns>如果DataRow包含该列名，则返回字符串，否则返回空字符串</returns>
        public static string GetCellValuebyPartConlumeName(DataRow row, string partName)
        {
            Contract.Requires(row != null);
            foreach (DataColumn item in row.Table.Columns)
                if (item.ColumnName.Contains(partName))
                    return row[item.ColumnName].ToString();

            return string.Empty;
        }

        /// <summary>
        /// 将list中的T2类型经过fun转换为T1类型，并返回T1类型的值
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="list"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static List<T1> ToList<T1, T2>(List<T2> list, Func<T2, T1> fun)
        {
            if (list == null)
                return null;

            List<T1> t = new List<T1>();
            foreach (var item in list)
            {
                t.Add(fun(item));
            }

            return t;
        }

        /// <summary>
        /// 获取list中fun函数为真的部分
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> GetPartial<T>(List<T> list, Func<T, bool> fun)
        {
            Contract.Requires(list != null);
            List<T> Templist = new List<T>();
            foreach (var item in list)
                if (fun(item))
                    Templist.Add(item);

            return Templist;
        }
    }
}
