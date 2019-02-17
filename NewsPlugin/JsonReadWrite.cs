using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace NewsPlugin
{
    static class JsonReadWrite
    {
        private static string FeedsFileName = "Feeds.txt";


        /// <summary>
        /// Gemmer projektets data
        /// </summary>
        /// <param name="dict"></param>
        public static async void SaveFeedsAsJsonAsync(Dictionary<string, bool> dict)
        {
            string notesJsonString = JsonConvert.SerializeObject(dict);
            SerializeNotesFileAsync(notesJsonString, FeedsFileName);
        }



        /// <summary>
        /// Loader projektets data
        /// </summary>
        /// <returns></returns>
        public static async Task<Dictionary<string, bool>> LoadProjectsFromJsonAsync()
        {
            string notesJsonString = await DeserializeNotesFileAsync(FeedsFileName);
            return (Dictionary<string, bool>)JsonConvert.DeserializeObject(notesJsonString, typeof(Dictionary<string, bool>));
        }


        // motode der kaldes i SaveNotesAsJsonAsync()
        private static async void SerializeNotesFileAsync(string notesJsonString, string fileName)
        {
            string path = @"c:\woxnews\" + fileName;
            
            using (FileStream fs = File.Create(path))
            {
                File.WriteAllText(path, notesJsonString, Encoding.UTF8);
            }
        }

        // metode der kaldes i LoadNotesFromJsonAsync()
        private static async Task<string> DeserializeNotesFileAsync(string fileName)
        {
            try
            {
                string path = @"c:\wox.news\" + fileName;
                // Open the file to read from.
                return File.ReadAllText(path);

            }
            catch (FileNotFoundException e)
            {
                HardcodedFeeds();
                return null;
            }
        }

        // Hardcoded dimser
        private static Dictionary<string, bool> HardcodedFeeds()
        {
            Dictionary<string, bool> dictionaryFeeds = new Dictionary<string, bool>();


            return dictionaryFeeds;
        }
    }
}
