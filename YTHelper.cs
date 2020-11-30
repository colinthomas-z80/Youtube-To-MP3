using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace get_audio
{
    public class YTHelper
    {
        public static async Task<String> ConvertToLink(string url)
        {
            var key = GetKey(url);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://www.yt-download.org/api/button/mp3/" + key);
            var message = await response.Content.ReadAsStringAsync();

            return GetLink(message, url);

        }

        public static async Task DownloadFromLink(string downloadLink)
        {
            WebClient wc = new WebClient();
            string name = await GetName(downloadLink);
            await wc.DownloadFileTaskAsync(new Uri(downloadLink), "C:\\Users\\Owner\\Downloads\\" + name);

            Console.WriteLine("\nDownload Complete");
        }

        private static string GetLink(string message, string url)
        {
            var key = GetKey(url);

            var linkExpression = new Regex(@"\b(?:https?://www.yt-download.org/download/" + @key + @"/mp3/320/" + @")\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return linkExpression.Match(message).Value;
        }

        private static string GetKey(string url)
        {
            string pattern = "https://www.youtube.com/watch?v=";

            int pos = url.IndexOf(pattern);
            string key = url.Substring(pos + pattern.Length);

            // Currently, this will not work on videos that are part of a playlist. Probably due to the parsing on the API end
            //int index = keyhalf.IndexOf('&');
            //string key = keyhalf.Substring(0, index);

            return key;
        }

        private static async Task<string> GetName(string downloadLink)
        {
            WebResponse nameResponse = await WebRequest.Create(new Uri(downloadLink)).GetResponseAsync();
            string name = nameResponse.Headers["Content-Disposition"].Substring(nameResponse.Headers["Content-Disposition"].IndexOf("filename") + 9).Replace("\"", "");

            return name;
        }
    }
}
