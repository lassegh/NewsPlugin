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
            string queryString = "Kænguru";
            RssManager reader = new RssManager();
            List<string> titleList = new List<string>();
            foreach (Rss.Items items in reader.GetFeed()) // Gennemløber de enkelte feeds
            {
                if (items.Title.ToLower().Contains(queryString.ToLower())) // Tjekker om query passer med noget i historien
                {
                    titleList.Add(items.Title); // tilføjer til listen
                }
            }

            Assert.IsTrue(titleList[0].Contains(queryString));
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