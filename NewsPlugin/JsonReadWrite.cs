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
using ServiceStack.Text;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace NewsPlugin
{
    static class JsonReadWrite
    {
        public static async Task DocModeCreateFile(string jsonString)
        {
            string folderName = @"c:\WoxNews";

            string pathString = System.IO.Path.Combine(folderName, "Feeds");

            System.IO.Directory.CreateDirectory(pathString);
 
            string fileName = @"newsfeeds.json";

            pathString = System.IO.Path.Combine(pathString, fileName);

            UnicodeEncoding ue = new UnicodeEncoding();
            char[] charsToAdd = ue.GetChars(ue.GetBytes(jsonString));

            using (StreamWriter writer = File.CreateText(pathString))
            {
                await writer.WriteAsync(charsToAdd, 0, charsToAdd.Length);
            }
            
        } 

        private static string destination = @"c:\WoxNews\Feeds\newsfeeds.json";

        /// <summary>
        /// Gemmer projektets data
        /// </summary>
        /// <param name="feedList"></param>
        public static async Task SaveFeedsAsJsonAsync(List<Feeds> feedList)
        {
            string notesJsonString = JsonConvert.SerializeObject(feedList);
            await DocModeCreateFile(notesJsonString);
        }

        /// <summary>
        /// Loader projektets data
        /// </summary>
        /// <returns></returns>
        public static List<Feeds> LoadFeedsFromJsonAsync()
        {
            string notesJsonString = DeserializeFeedsFileAsync(destination);
            return (List<Feeds>)JsonConvert.DeserializeObject(notesJsonString, typeof(List<Feeds>));
        }

       // metode der kaldes i LoadNotesFromJsonAsync()
        private static string DeserializeFeedsFileAsync(string path)
        {
            // Open the file to read from.
            return File.ReadAllText(path);
        }

    }
}
