using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonLibrary.JsonHelper
{
    public static class JsonHelper
    {
        /// <summary>
        /// 将对象序列化成JSON字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>josn字符串</returns>
        public static string SerializeObject(this object obj)
        {
            return JsonConvert.SerializeObject(obj); ;
        }

        /// <summary>
        /// 将对象序列化成JSON字符串 且 不带null值
        /// </summary>
        /// <param name="obj">入参对象</param>
        /// <returns>字符串</returns>
        public static string SerializeObjectNotNull(this object obj)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };
            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// json字符串转指定对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">字符串</param>
        /// <returns></returns>
        public static T JsonToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 将json字符串解析成实体类
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">字符串</param>
        /// <returns>一个对象</returns>
        public static T DeserializeJsonToObject<T>(this string json) where T : class 
        {
            JsonSerializer serializer = new JsonSerializer();
            T t = serializer.Deserialize(new JsonTextReader(new StringReader(json)),typeof(T)) as T;
            return t;
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(this string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            List<T> list = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>)) as List<T>;
            return list;
        }

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
