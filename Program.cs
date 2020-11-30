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
        static void Main(string[] args)
        {
            Console.WriteLine("Calling API...");

            string link = ConvertToLink("https://www.youtube.com/watch?v=3nINDTxu5eY").Result;
            DownloadFromLink(link);

            Console.Write("Enter 'q' to finish");
            while (Console.ReadLine() != "q") ;

        }

        
    }
}
