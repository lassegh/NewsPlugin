using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPlugin
{
    static class Feeds
    {
        public static Dictionary<string,bool> FeedsDictionary = new Dictionary<string, bool>()
        {
            {"Dr1",true},
            {"Tv2",true },
            {"Bt",true },
            {"Eb",true }
        };
        //
        public static Dictionary<string, string> FeedsUrl = new Dictionary<string, string>()
        {
            {"Dr1","https://www.dr.dk/nyheder/service/feeds/allenyheder"},
            {"Tv2","http://feeds.tv2.dk/nyhederne_seneste/rss" },
            {"Bt","https://www.bt.dk/bt/seneste/rss" },
            {"Eb","https://ekstrabladet.dk/rssfeed/all/" }
        };
        public static Dictionary<string, string> FeedsIcon = new Dictionary<string, string>()
        {
            {"Dr1","Images\\dr.png"},
            {"Tv2","Images\\tv2.png" },
            {"Bt","Images\\bt.png" },
            {"Eb","Images\\eb.png" }
        };
    }
}
