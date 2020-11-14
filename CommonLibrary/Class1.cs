using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;

namespace CommonLibrary
{
    public static class Class1
    {
        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }

        public static string SerializeObjectNew(this object o)
        {
            var jSettings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };
            string json = JsonConvert.SerializeObject(o, jSettings);
            return json;
        }

        public static T JsonToObject<T>(string strJson)
        {
            return JsonConvert.DeserializeObject<T>(strJson);
        }

        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }

        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }

        public static string TiLayer(string json)
        {
            string reValue = "";
            JArray jar = JArray.Parse(json);
            for (int i = 0; i < jar.Count; i++)
            {
                JObject job = JObject.Parse(jar[i].ToString());
                job.Merge(job["INFO"]);
            }
            return reValue;
        }

        /// <summary>
        /// Json转换DataTable
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string strJson)
        {
            DataTable dataTable = new DataTable();
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(strJson);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        //Columns
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, Type.GetType("System.String"));
                            }
                        }
                        //Rows
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }
                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch { }
            result = dataTable;
            return result;
        }

        #region 根据datatable分页获取数据

        public static DataTable GetPageDataByDataTable(DataTable dt, int PAGEINDEX, int PAGESIZE)
        {
            //对 表格数据dt 进行分页处理
            var totalCount = dt.Rows.Count;
            var totalPage = getTotalPage(totalCount, PAGESIZE);
            var currentPage = PAGEINDEX;
            currentPage = (currentPage > totalPage ? totalPage : currentPage);//如果PageNo过大，则较正PageNo=PageCount
            currentPage = (currentPage <= 0 ? 1 : currentPage);//如果PageNo<=0，则改为首页
            //----克隆表结构到新表
            var NewOutDataDt = dt.Clone();
            //----取出1页数据到新表
            var rowBegin = (currentPage - 1) * PAGESIZE;
            var rowEnd = currentPage * PAGESIZE;
            rowEnd = (rowEnd > totalCount ? totalCount : rowEnd);
            for (var i = rowBegin; i <= rowEnd - 1; i++)
            {
                var newRow = NewOutDataDt.NewRow();
                var oldRow = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newRow[column.ColumnName] = oldRow[column.ColumnName];
                }
                NewOutDataDt.Rows.Add(newRow);
            }
            return NewOutDataDt;
        }

        /// <summary>
        /// 返回分页后的总页数
        /// </summary>
        /// <param name="totalCount">总记录条数</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>总页数</returns>
        public static int getTotalPage(int totalCount, int pageSize)
        {
            var totalPage = (totalCount / pageSize) + (totalCount % pageSize > 0 ? 1 : 0);
            return totalPage;
        }
        #endregion
    }

    public class NullToEmptyStringResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return type.GetProperties()
                    .Select(p =>
                    {
                        var jp = base.CreateProperty(p, memberSerialization);
                        jp.ValueProvider = new NullToEmptyStringValueProvider(p);
                        return jp;
                    }).ToList();
        }
    }
    public class NullToEmptyStringValueProvider : IValueProvider
    {
        PropertyInfo _MemberInfo;
        public NullToEmptyStringValueProvider(PropertyInfo memberInfo)
        {
            _MemberInfo = memberInfo;
        }

        public object GetValue(object target)
        {
            object result = _MemberInfo.GetValue(target, null);
            if (_MemberInfo.PropertyType == typeof(string) && result == null) result = "";
            return result;

        }

        public void SetValue(object target, object value)
        {
            _MemberInfo.SetValue(target, value, null);
        }
    }
}
