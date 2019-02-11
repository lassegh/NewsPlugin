using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wox.Plugin;
using Wox.Infrastructure;

namespace NewsPlugin
{
    public class Class1 : IPlugin, ISettingProvider
    {
        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>(); // Opretter liste af resultater

            if (query.RawQuery.Length > 4)
            {
                string queryString = query.RawQuery.Substring(4);

                foreach (string dictionaryKey in Feeds.FeedsDictionary.Keys) // Gennemløber alle feeds i Feeds
                {
                    if (Feeds.FeedsDictionary[dictionaryKey]) // Tjekker om feeded skal bruges (bool)
                    {
                        RssManager reader = new RssManager(Feeds.FeedsUrl[dictionaryKey]); // Opretter reader med feeded

                        foreach (Rss.Items items in reader.GetFeed()) // Gennemløber de enkelte feeds
                        {
                            if (items.Title.ToLower().Contains(queryString.ToLower()) || items.Description.ToLower().Contains(queryString.ToLower())) // Tjekker om query passer med noget i historien
                            {
                                Result newStory = new Result(); // Opretter resultat til listen
                                newStory.Title = items.Title; // Sætter title
                                newStory.SubTitle = items.Date.ToShortDateString() + " " + dictionaryKey; // Sætter subtitle til dato + navn på feed

                                newStory.Action = context => // sætter action på hver story
                                {
                                    // Do something
                                    System.Diagnostics.Process.Start(items.Link); // open browser

                                    return true;// True bestemmer at wox skal lukke, når man trykker på story
                                };

                                results.Add(newStory); // tilføjer til listen
                            }
                        }
                    }
                }

            }
            else
            {
                foreach (string dictionaryKey in Feeds.FeedsDictionary.Keys) // Gennemløber alle feeds i Feeds
                {
                    if (Feeds.FeedsDictionary[dictionaryKey]) // Tjekker om feeded skal bruges (bool)
                    {
                        RssManager reader = new RssManager(Feeds.FeedsUrl[dictionaryKey]); // Opretter reader med feeded

                        foreach (Rss.Items items in reader.GetFeed()) // Gennemløber de enkelte feeds
                        {
                            Result newStory = new Result(); // Opretter resultat til listen
                            newStory.Title = items.Title; // Sætter title
                            newStory.SubTitle = items.Date.ToShortDateString() + " " + dictionaryKey; // Sætter subtitle til dato + navn på feed

                            newStory.Action = context => // sætter action på hver story
                            {
                                // Do something
                                System.Diagnostics.Process.Start(items.Link); // open browser

                                return true;// True bestemmer at wox skal lukke, når man trykker på story
                            };

                            results.Add(newStory); // tilføjer til listen
                        }
                    }
                }
            }


            return results;
        }

        public void Init(PluginInitContext context)
        {
            //throw new NotImplementedException();
        }

        public Control CreateSettingPanel()
        {
            var control = new Control();
            return control;
        }
    }
}
