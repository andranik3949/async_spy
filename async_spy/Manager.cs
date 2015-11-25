using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace async_spy
{
    static class Manager
    {
        public static void Main()
        {
            var timer = Stopwatch.StartNew();
            ///////

            URLGenerator.generate();
            //Fetcher.fetch();
            /*
            var tasks = new Task[Config.max_thread_num];
            for (int taskNumber = 0; taskNumber < Config.max_thread_num; taskNumber++)
            {
                tasks[taskNumber] = Task.Factory.StartNew(
                    () =>
                    {
                        Fetcher.fetch();
                    });
            }

            Task.WaitAll(tasks);
            */

            Task.Run(() => { Parallel.For(0, Config.max_thread_num, index => { Fetcher.fetch(); }); });
            Parallel.For(0, Config.max_thread_num, index => { Converter.convert(); });

            ///////
            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds / 1000.0);
        }
    }

    public static class Config
    {
        public static int max_thread_num = Environment.ProcessorCount;
        public const string url_base = "http://www.police.am/Hanraqve/";
        public const string local_xls_base = "XLS/";
        public const string local_xlsx_base = "XLSX/";
    }
}