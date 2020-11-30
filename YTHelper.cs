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
            // this should take care of the videos you have watched part of and contain a timestamp in the url. 
            // Of course it wouldnt matter if you didn't grab the links from your own account :)
            if(url.Contains('&'))
            {
                int index = url.IndexOf('&');
                url = url.Substring(0, index);
                Console.WriteLine("Url Cropped");
            }

            var key = GetKey(url);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://www.yt-download.org/api/button/mp3/" + key);
            var message = await response.Content.ReadAsStringAsync();

            return GetLink(message, url);

        }

        public static async Task DownloadFromLink(string downloadLink, string path)
        {
            try
            {
                WebClient wc = new WebClient();
                string name = await GetName(downloadLink);
                string fullname = path + name;
                wc.DownloadFileAsync(new Uri(downloadLink), fullname);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                Console.WriteLine(
                    "\nHelpLink ---\n{0}", ex.HelpLink);
                Console.WriteLine("\nSource ---\n{0}", ex.Source);
                Console.WriteLine(
                    "\nStackTrace ---\n{0}", ex.StackTrace);
                Console.WriteLine(
                    "\nTargetSite ---\n{0}", ex.TargetSite);
            }

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

            return key;
        }

        private static async Task<string> GetName(string downloadLink)
        {
            string name = "defaultName";
            try
            {
                WebResponse nameResponse = await WebRequest.Create(new Uri(downloadLink)).GetResponseAsync();
                name = nameResponse.Headers["Content-Disposition"].Substring(nameResponse.Headers["Content-Disposition"].IndexOf("filename") + 9).Replace("\"", "");
                Console.WriteLine(name);
                nameResponse.Dispose();

            }
            catch
            {
                Console.WriteLine("Error in GetName");
            }
            return name;
        }
    }
}
