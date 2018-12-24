using System;
using System.Windows.Data;
using CSTPad.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSTPad.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IValueConverter converter = new CSharpResultConverter();

            var result = converter.Convert("@using System;\r\n@{= 20 }", null, null, null);
        }

        [TestMethod]
        public void TestMethod2()
        {
            IValueConverter converter = new CSharpResultConverter();

            var result = converter.Convert("@using System;\r\n@{= 20 }", null, null, null);
            result = converter.Convert("@using System;\r\n@{= 20 }", null, null, null);
            result = converter.Convert("@using System;\r\n@{= 20 }", null, null, null);
            result = converter.Convert("@using System;\r\n@{= 20 }", null, null, null);
            result = converter.Convert("@using System;\r\n@{= 20 }", null, null, null);
            result = converter.Convert("@using System;\r\n@{= 20 }", null, null, null);
        }
    }
}
