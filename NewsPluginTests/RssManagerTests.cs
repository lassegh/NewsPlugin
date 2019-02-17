using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPlugin.Tests
{
    [TestClass()]
    public class RssManagerTests
    {
        [TestMethod()]
        public void GetFeedTest()
        {
            NewsPlugin.RssManager reader = new NewsPlugin.RssManager();
            Assert.AreEqual(reader.GetFeed()[0].Title, "Kænguru på springtur er hjemme i god behold igen");
        }
    }
}