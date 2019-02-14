using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using Wox.Infrastructure.Storage;
using Wox.Plugin;
using CheckBox = System.Windows.Controls.CheckBox;
using Control = System.Windows.Controls.Control;
using Label = System.Windows.Controls.Label;

namespace NewsPlugin
{
    public class Class1 : IPlugin, ISettingProvider, IPluginI18n
    {
        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>(); // Opretter liste af resultater

            Result newSetting = new Result(); // Opretter resultat til listen
            newSetting.Title = "Settings";
            newSetting.Action = context => // sætter action på hver story
            {
                // Do something

                var thread = new Thread(() =>
                {
                    var bw = new Window();
                    StackPanel stackPanel = new StackPanel();
                    Label label = new Label();
                    label.Content = "Settings";
                    label.Height = 100;
                    label.Width = 50;

                    CheckBox dynamicCheckBox_DR = new CheckBox();
                    dynamicCheckBox_DR.Name = "DynamicCheckBox_DR";
                    dynamicCheckBox_DR.Content = "DR";
                    dynamicCheckBox_DR.IsChecked = true;
                    dynamicCheckBox_DR.Width = 50;
                    dynamicCheckBox_DR.Height = 50;

                    if (dynamicCheckBox_DR.IsChecked == false)
                    {
                        Feeds.FeedsDictionary["Dr1"] = false;
                    }

                    CheckBox dynamicCheckBox_TV2 = new CheckBox();
                    dynamicCheckBox_TV2.Name = "DynamicCheckBox_TV2";
                    dynamicCheckBox_TV2.Content = "TV2";
                    dynamicCheckBox_TV2.IsChecked = true;
                    dynamicCheckBox_TV2.Width = 50;
                    dynamicCheckBox_TV2.Height = 50;

                    if (dynamicCheckBox_TV2.IsChecked == false)
                    {
                        Feeds.FeedsDictionary["Tv2"] = false;
                    }

                    CheckBox dynamicCheckBox_BT = new CheckBox();
                    dynamicCheckBox_BT.Name = "DynamicCheckBox_BT";
                    dynamicCheckBox_BT.Content = "BT";
                    dynamicCheckBox_BT.IsChecked = true;
                    dynamicCheckBox_BT.Width = 50;
                    dynamicCheckBox_BT.Height = 50;

                    if (dynamicCheckBox_BT.IsChecked == false)
                    {
                        Feeds.FeedsDictionary["Bt"] = false;
                    }

                    CheckBox dynamicCheckBox_EB = new CheckBox();
                    dynamicCheckBox_EB.Name = "DynamicCheckBox_EB";
                    dynamicCheckBox_EB.Content = "Ekstra Bladet";
                    dynamicCheckBox_EB.Width = 50;
                    dynamicCheckBox_EB.Height = 50;

                    if (dynamicCheckBox_EB.IsChecked == false)
                    {
                        Feeds.FeedsDictionary["Eb"] = false;
                    }
                    else Feeds.FeedsDictionary["Eb"] = true;

                    stackPanel.Children.Add(label);
                    stackPanel.Children.Add(dynamicCheckBox_DR);
                    stackPanel.Children.Add(dynamicCheckBox_TV2);
                    stackPanel.Children.Add(dynamicCheckBox_BT);
                    stackPanel.Children.Add(dynamicCheckBox_EB);
                    bw.Content = stackPanel;
                    bw.Show();
                    bw.Closed += (s, e) => bw.Dispatcher.InvokeShutdown();
                    Dispatcher.Run();
                });
                thread.SetApartmentState(ApartmentState.STA);
                //thread.IsBackground = true;
                thread.Start();

                return true;// True bestemmer at wox skal lukke, når man trykker på story
            };
            
            results.Add(newSetting); // tilføjer til listen

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
                                newStory.IcoPath = Feeds.FeedsIcon[dictionaryKey];

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
                            newStory.IcoPath = Feeds.FeedsIcon[dictionaryKey];

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
            var control = new UserControl1();

            return control;
        }

        public string GetTranslatedPluginTitle()
        {
            return "News";
        }

        public string GetTranslatedPluginDescription()
        {
            return "Shows news";
        }
    }
}
