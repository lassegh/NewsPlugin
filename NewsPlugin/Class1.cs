using System;
using System.Collections.Generic;
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
        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>(); // Opretter liste af resultater

            /* Settings window has been commented out, as it is not fully implemented 
            results.Add(SettingsWindow()); // tilføjer til listen
            */

            string queryString = ""; // Opretter string til søgeresultat

            if (query.RawQuery.Length > 4)
            {
                queryString = query.RawQuery.Substring(4); // Tilføjer søgning til string
            }

            foreach (string dictionaryKey in Feeds.FeedsDictionary.Keys) // Gennemløber alle feeds i Feeds
            {
                if (Feeds.FeedsDictionary[dictionaryKey]) // Tjekker om feeded skal bruges (bool)
                {
                    RssManager reader = new RssManager(Feeds.FeedsUrl[dictionaryKey]); // Opretter reader med feeded

                    foreach (Rss.Items items in reader.GetFeed()) // Gennemløber de enkelte feeds
                    {
                        if (query.RawQuery.Length > 4) // Hvis der søges
                        {
                            if (items.Title.ToLower().Contains(queryString.ToLower()) ||
                                items.Description.ToLower().Contains(queryString.ToLower())
                            ) // Tjekker om query passer med noget i historien
                            {
                                results.Add(NewStory(items, dictionaryKey)); // tilføjer til listen
                            }
                        }
                        else results.Add(NewStory(items, dictionaryKey)); // tilføjer alle stories til listen
                    }
                }
            }

            results = results.OrderBy(o => o.SubTitle).ToList();
            results.Reverse();

            return results;
        }

        public Result NewStory(Rss.Items items, string dictionaryKey)
        {
            Result newStory = new Result(); // Opretter resultat til listen
            newStory.Title = items.Title; // Sætter title
            newStory.SubTitle = items.Date.ToString();
            newStory.IcoPath = Feeds.FeedsIcon[dictionaryKey];

            newStory.Action = context => // sætter action på hver story
            {
                // Do something
                System.Diagnostics.Process.Start(items.Link); // open browser

                return true;// True bestemmer at wox skal lukke, når man trykker på story
            };
            return newStory;
        }

        private Result SettingsWindow()
        {
            Result newSetting = new Result(); // Opretter resultat til listen
            newSetting.Title = "Settings";
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
                    label.Width = 50;

                    CheckBox dynamicCheckBox_DR = new CheckBox();
                    dynamicCheckBox_DR.Name = "DynamicCheckBox_DR";
                    dynamicCheckBox_DR.Content = "Dr1";
                    dynamicCheckBox_DR.IsChecked = Feeds.FeedsDictionary["Dr1"];
                    dynamicCheckBox_DR.Width = 50;
                    dynamicCheckBox_DR.Height = 50;
                    dynamicCheckBox_DR.Checked += CheckboxChecked;

                    CheckBox dynamicCheckBox_TV2 = new CheckBox();
                    dynamicCheckBox_TV2.Name = "DynamicCheckBox_TV2";
                    dynamicCheckBox_TV2.Content = "Tv2";
                    dynamicCheckBox_TV2.IsChecked = Feeds.FeedsDictionary["Tv2"];
                    dynamicCheckBox_TV2.Width = 50;
                    dynamicCheckBox_TV2.Height = 50;
                    dynamicCheckBox_TV2.Checked += CheckboxChecked;

                    CheckBox dynamicCheckBox_BT = new CheckBox();
                    dynamicCheckBox_BT.Name = "DynamicCheckBox_BT";
                    dynamicCheckBox_BT.Content = "Bt";
                    dynamicCheckBox_BT.IsChecked = Feeds.FeedsDictionary["Bt"];
                    dynamicCheckBox_BT.Width = 50;
                    dynamicCheckBox_BT.Height = 50;
                    dynamicCheckBox_BT.Checked += CheckboxChecked;

                    CheckBox dynamicCheckBox_EB = new CheckBox();
                    dynamicCheckBox_EB.Name = "DynamicCheckBox_EB";
                    dynamicCheckBox_EB.Content = "Eb";
                    dynamicCheckBox_BT.IsChecked = Feeds.FeedsDictionary["Eb"];
                    dynamicCheckBox_EB.Width = 50;
                    dynamicCheckBox_EB.Height = 50;
                    dynamicCheckBox_EB.Checked += CheckboxChecked;

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

                return false;// True bestemmer at wox skal lukke, når man trykker på story
            };

            return newSetting;
        }

        private void CheckboxChecked(Object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox.IsChecked == false)
            {
                Feeds.FeedsDictionary[checkBox.Name] = false;
            }
            else Feeds.FeedsDictionary[checkBox.Name] = true;

            JsonReadWrite.SaveFeedsAsJsonAsync(Feeds.FeedsDictionary);

        }

        public void Init(PluginInitContext context)
        {
            
        }
    }
}
