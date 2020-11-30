using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace get_audio
{
    class Program
    {
        // "https://www.yt-download.org/download/E-C22VCWB-U/mp3/320/1606705182/63a9a1d2fdf778f9ed5ca2cd556abfec5c96ac634fef2ff893e44003dbf5f94f/0"

        static void Main(string[] args)
        {
            Console.WriteLine("Calling API...");

            GetAudio("https://www.youtube.com/watch?v=3nINDTxu5eY");

            while (true) ;
        }

        static async Task GetAudio(string url)
        {
            HttpClient client = new HttpClient();

            Console.WriteLine("Posting");

            var key = GetKey(url);

            HttpResponseMessage response = await client.GetAsync("https://www.yt-download.org/api/button/mp3/" + key);

            var message = await response.Content.ReadAsStringAsync();

            var linkParser = new Regex(@"\b(?:https?://www.yt-download.org/download/" + @key + @"/mp3/320/" + @")\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            string link = linkParser.Match(message).Value;

            Console.WriteLine("Link used = " + link);


            // Get file after link is generated with key

            WebClient wc = new WebClient();

            wc.DownloadFile(link, "C:\\Users\\Owner\\Downloads\\song.mp3");
            
            

        }

        static string GetKey(string url)
        {
            string pattern = "https://www.youtube.com/watch?v=";

            int pos = url.IndexOf(pattern);
            string key = url.Substring(pos + pattern.Length);
            //int index = keyhalf.IndexOf('&');
            //string key = keyhalf.Substring(0, index);

            Console.WriteLine(key);

            return key;

        }
    }
}
