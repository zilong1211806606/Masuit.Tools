using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masuit.Tools.Json
{
    /// <summary>
    /// DataRow 转json
    /// </summary>
    public static class DataRowHelper
    {
        /// <summary>
        /// DataRow[] 转换成Json格式
        /// </summary>
        /// <param name="drArr"></param>
        /// <returns></returns>
        public static string DataRowsToJson(this DataRow[] drArr)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");
            if (drArr.Length > 0)
            {
                foreach (DataRow dr in drArr)
                {
                    jsonBuilder.Append("{");
                    for (int drIndex = 0; drIndex < dr.ItemArray.Length; drIndex++)
                    {
                        jsonBuilder.AppendFormat("\"{0}\":\"{1}\",",
                            dr.Table.Columns[drIndex].ColumnName, dr[drIndex].ToString().Replace('"', '‘').Replace("'", "‘").Trim());
                    }
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    jsonBuilder.Append("},");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }
            jsonBuilder.Append("]");
            return jsonBuilder.ToString();
        }

        /// <summary>
        /// DataRow 转json
        /// </summary>
        /// <param name="drArr"></param>
        /// <returns></returns>
        public static string DataRowToJson(this DataRow drArr)
        {
            StringBuilder jsonBuilder = new StringBuilder();

            jsonBuilder.Append("{");
            for (int drIndex = 0; drIndex < drArr.ItemArray.Length; drIndex++)
            {
                jsonBuilder.AppendFormat("\"{0}\":\"{1}\",",
                    drArr.Table.Columns[drIndex].ColumnName, drArr[drIndex].ToString().Replace('"', '‘').Replace("'", "‘").Trim());
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
    }
}
