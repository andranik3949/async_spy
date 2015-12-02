using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace async_spy
{
   static class Manager
    {
        public static void Main()
        {
            var timer = Stopwatch.StartNew();
            ///////

            // Generate URLs
            Task generation = Task.Run(() => 
            {
                URLGenerator.generate();
            });
            
            // Fetch files
            Task fetching = Task.Run(() => 
            {
                Fetcher.fetch();
            });
         
            // Convert files
            Task conversion = Task.Run(() =>
            {
                Converter.convert();
            });
            
            Task.WaitAll( conversion );
            ///////
            timer.Stop();
            Console.WriteLine(timer.Elapsed.Minutes.ToString() + "m" + timer.Elapsed.Seconds.ToString() + "s");
        }
    }

    public static class Config
    {
        public static int max_thread_num = Environment.ProcessorCount * 2;
        public static string local_xls_base = Environment.CurrentDirectory + "\\" + "XLS\\";
        public static string local_xlsx_base = Environment.CurrentDirectory + "\\" + "XLSX\\";
        public const string url_base = "http://www.police.am/Hanraqve/";
    }
}