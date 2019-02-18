using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Wox.Plugin;
using CheckBox = System.Windows.Controls.CheckBox;
using Control = System.Windows.Controls.Control;
using Label = System.Windows.Controls.Label;

namespace NewsPlugin
{
    public class Class1 : IPlugin
    {
        private static async void LoadFeeds()
        {
            Feeds.FeedsList = await JsonReadWrite.LoadFeedsFromJsonAsync();
        }

        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>(); // Opretter liste af resultater

            if (Feeds.FeedsList.Count == 0)
            {
                
                Feeds.HardcodedFeeds();
                // TODO Else load hardcoded feeds og gem til fil

            }

            //Settings window 
            results.Add(SettingsWindow()); // tilføjer til listen


            string queryString = ""; // Opretter string til søgeresultat

            if (query.RawQuery.Length > 4)
            {
                queryString = query.RawQuery.Substring(4); // Tilføjer søgning til string
            }

            foreach (Feeds feed in Feeds.FeedsList) // Gennemløber alle feeds i Feeds
            {
                if (feed.ToBeSeen) // Tjekker om feeded skal bruges (bool)
                {
                    RssManager reader = new RssManager(feed.FeedUrl); // Opretter reader med feeded

                    foreach (Rss.Items items in reader.GetFeed()) // Gennemløber de enkelte feeds
                    {
                        if (query.RawQuery.Length > 4) // Hvis der søges
                        {
                            if (items.Title.ToLower().Contains(queryString.ToLower()) ||
                                items.Description.ToLower().Contains(queryString.ToLower())
                            ) // Tjekker om query passer med noget i historien
                            {
                                results.Add(NewStory(items, feed.ImagePath)); // tilføjer til listen
                            }
                        }
                        else results.Add(NewStory(items, feed.ImagePath)); // tilføjer alle stories til listen
                    }
                }
            }

            results = results.OrderBy(o => o.SubTitle).ToList();
            results.Reverse();

            return results;
        }

        public Result NewStory(Rss.Items items, string imagePath)
        {
            Result newStory = new Result(); // Opretter resultat til listen
            newStory.Title = items.Title; // Sætter title
            newStory.SubTitle = items.Date.ToString();
            newStory.IcoPath = imagePath;

            newStory.Action = context => // sætter action på hver story
            {
                // Do something
                System.Diagnostics.Process.Start(items.Link); // open browser

                return true;// True bestemmer at wox skal lukke, når man trykker på story
            };
            return newStory;
        }

        public static Result SettingsWindow()
        {
            Result newSetting = new Result(); // Opretter resultat til listen
            newSetting.Title = "Settings";
            newSetting.SubTitle = DateTime.Now.ToString();
            newSetting.Action = context => // sætter action på hver story
            {

                var thread = new Thread(() =>
                {
                    var bw = new Window();
                    bw.MaxHeight = 400;
                    bw.MaxWidth = 300;

                    StackPanel stackPanel = new StackPanel();
                    Label label = new Label();
                    label.Content = "Settings";
                    label.Height = 100;
                    label.Width = 100;
                    stackPanel.Children.Add(label);

                    foreach (Feeds feed in Feeds.FeedsList)
                    {
                        CheckBox checkbox = new CheckBox();
                        checkbox.Name = feed.Name;
                        checkbox.Content = feed.Name;
                        checkbox.IsChecked = feed.ToBeSeen;
                        checkbox.Width = 50;
                        checkbox.Height = 50;
                        checkbox.Checked += CheckboxChecked;
                        stackPanel.Children.Add(checkbox);
                    }

                    bw.Content = stackPanel;
                    bw.Show();
                    bw.Closed += (s, e) => bw.Dispatcher.InvokeShutdown();
                    Dispatcher.Run();
                });
                thread.SetApartmentState(ApartmentState.STA);
                //thread.IsBackground = true;
                thread.Start();

                return false;// True bestemmer at wox skal lukke, når man trykker på story
            };

            return newSetting;
        }

        public static void CheckboxChecked(Object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox checkBox = (CheckBox)sender;

                if (checkBox.IsChecked == false)
                {
                    foreach (Feeds feed in Feeds.FeedsList)
                    {
                        if (feed.Name.Equals(checkBox.Name))
                        {
                            feed.ToBeSeen = false;
                            JsonReadWrite.SaveFeedsAsJsonAsync(Feeds.FeedsList);
                        }
                    }

                }
                else
                {

                    foreach (Feeds feed in Feeds.FeedsList)
                    {
                        if (feed.Name.Equals(checkBox.Name))
                        {
                            feed.ToBeSeen = true;
                            JsonReadWrite.SaveFeedsAsJsonAsync(Feeds.FeedsList);
                        }
                    }
                }
            }
        }

        public void Init(PluginInitContext context)
        {

        }
    }
}
