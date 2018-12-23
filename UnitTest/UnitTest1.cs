using CsTxt;
using CsTxt.Block;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using UnitTest.Properties;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var text = File.ReadAllText(@"C:\Users\PC user\Source\Repos\CsTxt\UnitTest\Resource\Test.txt");

            CSharpText cst = new CSharpText(text);
            string result = cst.RunAsync().Result;

            var result2 = BlockFactory.Parse(text).ToCSharpAsync().Result;
        }
    }
}
