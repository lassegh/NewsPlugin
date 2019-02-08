using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wox.Plugin;

namespace NewsPlugin
{
    public class Class1 : IPlugin
    {
        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();

            RssManager reader = new RssManager("http://feeds.tv2.dk/nyhederne_seneste/rss");

            foreach (Rss.Items items in reader.GetFeed())
            {
                Result newStory = new Result();
                newStory.Title = items.Title;
                newStory.SubTitle = items.Date.ToShortDateString();

                newStory.Action = context =>
                {
                    // Do something
                    System.Diagnostics.Process.Start(items.Link); // open browser

                    return false;// True false bestemmer hvorvidt vox skal lukkes eller forblive åbent
                };

                results.Add(newStory);
            }

            return results;
        }

        public void Init(PluginInitContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
