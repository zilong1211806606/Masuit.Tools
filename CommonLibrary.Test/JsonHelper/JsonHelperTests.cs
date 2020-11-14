using CommonLibrary.JsonHelper;
using CommonLibrary.Test.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using CommonLibrary.JsonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.JsonHelper.Tests
{
    [TestClass()]
    public class JsonHelperTests
    {
        [TestMethod()]
        public void SerializeObjectTest()
        {
            Preson p = new Preson();
            p.name ="张三";
            p.age = 20;
            string str = p.SerializeObject();
            Assert.AreEqual("{\"name\":\"张三\",\"age\":20}", str);
        }

        [TestMethod()]
        public void SerializeObjectNotNullTest()
        {
            Preson p = new Preson();
            string str = p.SerializeObjectNotNull();
            Assert.AreEqual("{\"name\":\"\",\"age\":0}", str);
        }

        [TestMethod()]
        public void JsonToObjectTest()
        {
            string str = "{\"name\":\"张三\",\"age\":20}";
            Preson p = str.JsonToObject<Preson>();
            Assert.AreNotEqual(p,new Preson());
        }

        [TestMethod()]
        public void DeserializeJsonToObjectTest()
        {
            string str = "{\"name\":\"张三\",\"age\":20}";
            Preson p = str.DeserializeJsonToObject<Preson>();
            Assert.AreNotEqual(p, new Preson());
        }

        [TestMethod()]
        public void DeserializeJsonToListTest()
        {
            string str = "[{\"name\":\"张三\",\"age\":20},{\"name\":\"李四\",\"age\":21}]";
            List<Preson> p = str.DeserializeJsonToList<Preson>();
            Assert.AreNotEqual(p, new List<Preson>());
        }
    }
}