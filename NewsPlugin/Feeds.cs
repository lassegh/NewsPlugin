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
            new Feeds(true, "DR1", "https://www.dr.dk/nyheder/service/feeds/allenyheder", "Image\\dr.png");
            new Feeds(true, "TV2", "http://feeds.tv2.dk/nyhederne_seneste/rss", "Image\\tv2.png");
            new Feeds(true, "BT", "https://www.bt.dk/bt/seneste/rss", "Image\\bt.png");
            new Feeds(false, "EB", "https://ekstrabladet.dk/rssfeed/all/", "Image\\eb.png");
            new Feeds(false, "BBC", "http://feeds.bbci.co.uk/news/rss.xml","Image\\bbc.png");
            new Feeds(false, "Der Spiegel", "http://www.spiegel.de/international/index.rss", "Image\\spiegel.png");
            new Feeds(false, "France24", "https://www.france24.com/en/rss/", "Image\\france.png");
            new Feeds(false, "NHK", "https://www3.nhk.or.jp/rj/podcast/rss/english.xml", "Image\\nhk.png");
            new Feeds(false, "Al Jazeera", "https://www.aljazeera.com/xml/rss/all.xml", "Image\\aj.png");
            new Feeds(false, "El País", "https://economia.elpais.com/rss/elpais/inenglish.xml", "Image\\ep.png");
            new Feeds(false, "New York Times (World Feed)", "http://rss.nytimes.com/services/xml/rss/nyt/World.xml","Image\\nytwf.png");
            new Feeds(false, "New York US", "http://rss.nytimes.com/services/xml/rss/nyt/US.xml", "Image\\nytwf.png");
            new Feeds(true, "Reuters", "http://feeds.reuters.com/reuters/topNews?format=xml","Image\\reuters.png");
            new Feeds(false, "All Africa", "https://allafrica.com/tools/headlines/rdf/latest/headlines.rdf","Image\\aa.png");
            new Feeds(false, "The Economist", "https://www.economist.com/business/rss.xml", "Image\\te.png");
            //new Feeds(true, "Fox News", "https://www.foxnews.com/about/rss/", "Image\\fox.png");

        }

        public bool ToBeSeen { get; set; }
        public string Name { get; set; }
        public string FeedUrl { get; set; }
        public string ImagePath { get; set; }
    }
}
