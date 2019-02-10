using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPlugin
{
    class ViewModel
    {
        private bool _dr1;
        private bool _tv2;
        private bool _bt;
        private bool _eb;

        public ViewModel()
        {
            
        }

        public bool Dr1
        {
            get { return _dr1; }
            set
            {
                _dr1 = value;
                Feeds.FeedsDictionary["Dr1"] = value;
            }
        }

        public bool Tv2
        {
            get { return _tv2; }
            set
            {
                _tv2 = value;
                Feeds.FeedsDictionary["Tv2"] = value;
            }
        }

        public bool Bt
        {
            get { return _bt; }
            set
            {
                _bt = value;
                Feeds.FeedsDictionary["Bt"] = value;
            }
        }

        public bool Eb
        {
            get { return _eb; }
            set
            {
                _eb = value;
                Feeds.FeedsDictionary["Eb"] = value;
            }
        }
    }
}
