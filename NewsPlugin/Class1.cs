using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using Wox.Plugin;
using CheckBox = System.Windows.Controls.CheckBox;
using Control = System.Windows.Controls.Control;
using Label = System.Windows.Controls.Label;

namespace NewsPlugin
{
    public class Class1 : IPlugin
    {
        private static List<CheckBox> checkboxList;
        private static Window bw;

        private static async void LoadFeeds()
        {
            Feeds.FeedsList = JsonReadWrite.LoadFeedsFromJsonAsync();
        }

        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>(); // Opretter liste af resultater

            if (Feeds.FeedsList.Count == 0)
            {
                try
                {
                    Feeds.FeedsList = JsonReadWrite.LoadFeedsFromJsonAsync();
                }
                catch (Exception e)
                {
                     Feeds.HardcodedFeeds();
                }
                //Feeds.HardcodedFeeds();
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

        public Result SettingsWindow()
        {
            Result newSetting = new Result(); // Opretter resultat til listen
            newSetting.Title = "Settings";
            newSetting.SubTitle = DateTime.Now.ToString();
            newSetting.Action = context => // sætter action på hver story
            {

                var thread = new Thread(() =>
                {
                    bw = new Window();
                    bw.Height = 800;
                    bw.Width = 500;
                    
                    StackPanel stackPanel = new StackPanel();
                    Label label = new Label();
                    label.Content = "Settings";
                    label.Height = 100;
                    label.Width = 100;
                    stackPanel.Children.Add(label);

                    Grid grid = new Grid();
                    grid.ColumnDefinitions.Add(new ColumnDefinition(){Width = new GridLength(250)});
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(250) });
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.RowDefinitions.Add(new RowDefinition());
                    stackPanel.Children.Add(grid);

                    checkboxList = new List<CheckBox>();
                    int rowDefinition = 0;
                    int columnDefinition = 0;

                    foreach (Feeds feed in Feeds.FeedsList)
                    {
                        CheckBox checkbox = new CheckBox();
                        checkbox.Content = feed.Name;
                        checkbox.IsChecked = feed.ToBeSeen;
                        checkbox.Width = 200;
                        checkbox.Height = 50;
                        checkbox.HorizontalAlignment = HorizontalAlignment.Left;
                        checkbox.Margin = new Thickness(10,0,0,0);
                        grid.Children.Add(checkbox);
                        Grid.SetColumn(checkbox,columnDefinition);
                        Grid.SetRow(checkbox, rowDefinition);
                        checkboxList.Add(checkbox);

                        rowDefinition++;
                        if (rowDefinition == 8)
                        {
                            rowDefinition = 0;
                            columnDefinition = 1;
                        }
                    }

                    Button saveSettings = new Button();
                    saveSettings.FontSize = 16;
                    saveSettings.Content = "Save settings";
                    saveSettings.Height = 100;
                    saveSettings.Width = 500;
                    saveSettings.Click += SaveSettings_Click;
                    stackPanel.Children.Add(saveSettings);

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

        private static void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < checkboxList.Count; i++)
            {
                if (checkboxList[i].IsChecked.HasValue && checkboxList[i].IsChecked.Value)
                {
                    Feeds.FeedsList[i].ToBeSeen = true;
                }
                else Feeds.FeedsList[i].ToBeSeen = false;
            }
            bw.Close();
            JsonReadWrite.SaveFeedsAsJsonAsync(Feeds.FeedsList);
            
        }

        public void Init(PluginInitContext context)
        {

        }
    }
}
