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
        public static void WriteToTextFile(string textLog)
        {
            FileStream objFS = null;


            string strFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Exception Log\" + System.DateTime.Now.ToString("yyyy-MM-dd ") + "Exception.log";
            if (!File.Exists(strFilePath))
            {
                objFS = new FileStream(strFilePath, FileMode.Create);
            }
            else
                objFS = new FileStream(strFilePath, FileMode.Append);

            using (StreamWriter Sr = new StreamWriter(objFS))
            {
                Sr.WriteLine(System.DateTime.Now.ToShortTimeString() + "---" + textLog);
            }

        }

        static void CreateFileMsDoc()
        {
            string path = @"c:\woxnews\MyTest.txt";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Hello");
                    sw.WriteLine("And");
                    sw.WriteLine("Welcome");
                }
            }

            // Open the file to read from.
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
        }

        private static string destination = AppDomain.CurrentDomain.BaseDirectory + @"\feeds.json";

        /// <summary>
        /// Gemmer projektets data
        /// </summary>
        /// <param name="feed"></param>
        public static async void SaveFeedsAsJsonAsync(List<Feeds> feedList)
        {
            string notesJsonString = JsonConvert.SerializeObject(feedList);
            SerializeFeedsFileAsync(notesJsonString);
        }

        // motode der kaldes i SaveNotesAsJsonAsync()
        private static async void SerializeFeedsFileAsync(string notesJsonString)
        {

            var streamManager = new RecyclableMemoryStreamManager();

            using (var file = File.Open(destination, FileMode.Create))
            {
                using (var memoryStream = streamManager.GetStream()) // RecyclableMemoryStream will be returned, it inherits MemoryStream, however prevents data allocation into the LOH
                {
                    using (var writer = new StreamWriter(memoryStream))
                    {
                        var serializer = JsonSerializer.CreateDefault();

                        serializer.Serialize(writer, notesJsonString);

                        await writer.FlushAsync().ConfigureAwait(false);

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        await memoryStream.CopyToAsync(file).ConfigureAwait(false);
                    }
                }

                await file.FlushAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Loader projektets data
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Feeds>> LoadFeedsFromJsonAsync()
        {
            string notesJsonString = await DeserializeFeedsFileAsync(destination);
            return (List<Feeds>)JsonConvert.DeserializeObject(notesJsonString, typeof(List<Feeds>));
        }

       // metode der kaldes i LoadNotesFromJsonAsync()
        private static async Task<string> DeserializeFeedsFileAsync(string path)
        {
            // Open the file to read from.
            return File.ReadAllText(path);
        }
    }
}
