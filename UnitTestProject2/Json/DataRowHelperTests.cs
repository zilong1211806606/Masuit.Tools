using Microsoft.VisualStudio.TestTools.UnitTesting;
using Masuit.Tools.Core.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Masuit.Tools.Core.Json.Tests
{
    [TestClass()]
    public class DataRowHelperTests
    {
        [TestMethod()]
        public void DataRowsToJsonTest()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("name");
            dt.Columns.Add("age");
            DataRow dr = dt.NewRow();
            dr["name"] = "张三";
            dr["age"] = "18";
            dt.Rows.Add(dr);

            _ = DataRowHelper.DataRowsToJson(dt.Select());

            Assert.IsTrue(true);
        }
    }
}