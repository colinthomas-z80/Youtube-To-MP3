using System;
using System.IO;
using System.Threading.Tasks;

namespace get_audio
{
    class Program
    {
        public static string[] links = File.ReadAllLines("C:\\Users\\Owner\\Programming\\audio_scraper\\src\\doowoplinks.txt");
        public static string path = "C:\\Users\\Owner\\Music\\Scraper\\DooWop\\";

        static void Main(string[] args)
        {
            run();
            Console.Write("Enter 'q' to finish");
            while (Console.ReadLine() != "q") ;

        }

        static async Task run()
        {

            for (int i = 25; i < 36; i++)
            {
                Console.WriteLine("Calling API...");
                string link = await YTHelper.ConvertToLink(links[i]);
                Console.WriteLine(links[i]);
                await YTHelper.DownloadFromLink(link, Program.path);
            }
        }

        
    }
}
