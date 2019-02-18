using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPlugin
{
    public class Feeds
    {
        public Feeds(bool beSeen, string name, string feedUrl, string imagePath)
        {
            ToBeSeen = beSeen;
            Name = name;
            FeedUrl = feedUrl;
            ImagePath = imagePath;

            FeedsList.Add(this);
        }

        public static List<Feeds> FeedsList = new List<Feeds>();

        public static void HardcodedFeeds()
        {
            new Feeds(true,"DR1", "https://www.dr.dk/nyheder/service/feeds/allenyheder", "Image\\dr.png");
            new Feeds(true,"TV2", "http://feeds.tv2.dk/nyhederne_seneste/rss", "Image\\tv2.png");
            new Feeds(true,"BT", "https://www.bt.dk/bt/seneste/rss", "Image\\bt.png");
            new Feeds(true,"EB", "https://ekstrabladet.dk/rssfeed/all/", "Image\\eb.png");
        }

        public bool ToBeSeen { get; set; }
        public string Name { get; set; }
        public string FeedUrl { get; set; }
        public string ImagePath { get; set; }
    }
}
