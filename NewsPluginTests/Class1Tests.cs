using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Wox.Plugin;

namespace NewsPlugin.Tests
{
    [TestClass()]
    public class Class1Tests
    {


        [TestMethod()]
        public void QueryTest()
        {
            /*
            var query = new Mock<Query>();
            query.SetupProperty(x => x.RawQuery, "brøndby");
            Class1 obj = new Class1();
            
            Assert.IsTrue(obj.Query(query.Object).Count>0);*/
        }

        [TestMethod()]
        public void NewStoryTest()
        {
            NewsPlugin.RssManager reader = new NewsPlugin.RssManager();
            List<string> titleList = new List<string>();
            foreach (Rss.Items items in reader.GetFeed()) // Gennemløber de enkelte feeds
            {
                titleList.Add(items.Title); // Sætter title
            }

            Assert.AreEqual(titleList[0], "Kænguru på springtur er hjemme i god behold igen");
        }
    }
}