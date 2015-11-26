﻿using System;
using System.Diagnostics;
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
            Console.WriteLine(timer.ElapsedMilliseconds / (60 * 1000.0));
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