using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NewsPlugin
{
    class RssManager : IDisposable
    {
        private string _url;
        private string _feedTitle;
        private string _feedDescription;
        private Collection<Rss.Items> _rssItems = new Collection<Rss.Items>();
        private bool _IsDisposed;

        #region Constructors

        public RssManager()
        {
            Url = string.Empty;
        }

        public RssManager(string feedUrl)
        {
            Url = feedUrl;
        }

        #endregion

        public Collection<Rss.Items> GetFeed()
        {
            // Check to see if the feed URL is empty
            if (string.IsNullOrEmpty(Url))
            {
                // Throw an exception if not provided
                throw new ArgumentException("You must provide a feed url");
            }

            //Start parsing process
            using (XmlReader reader = XmlReader.Create(Url))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(reader);
                // Parse the items of the feed
                ParseDocElements(xmlDoc.SelectSingleNode("//channel"), "title", ref _feedTitle);
                ParseDocElements(xmlDoc.SelectSingleNode("//channel"), "description", ref _feedDescription);
                ParseRssItems(xmlDoc);

                return _rssItems;
            }
        }

        private void ParseRssItems(XmlDocument xmlDoc)
        {
            _rssItems.Clear();
            XmlNodeList nodes = xmlDoc.SelectNodes("rss/channel/item");

            foreach (XmlNode node in nodes)
            {
                Rss.Items item = new Rss.Items();
                ParseDocElements(node, "title", ref item.Title);
                ParseDocElements(node, "description", ref item.Description);
                ParseDocElements(node, "link", ref item.Link);

                string date = null;
                ParseDocElements(node, "pubDate", ref date);
                DateTime.TryParse(date, out item.Date);

                _rssItems.Add(item);
            }
        }

        private void ParseDocElements(XmlNode parent, string xPath, ref string property)
        {
            XmlNode node = parent.SelectSingleNode(xPath);
            if (node != null)
            {
                property = node.InnerText;
            }
            else property = "Unresolvable";
        }



        #region Properties

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public string FeedTitle
        {
            get { return _feedTitle; }
        }

        public string FeedDescription
        {
            get { return _feedDescription; }
        }

        public Collection<Rss.Items> RssItems
        {
            get { return _rssItems; }
        }

        #endregion

        private void Dispose(bool disposing)
        {
            if (disposing && !_IsDisposed)
            {
                _rssItems.Clear();
                _url = null;
                _feedTitle = null;
                _feedDescription = null;
            }

            _IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
