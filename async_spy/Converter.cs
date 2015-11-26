using System;
using System.Threading;

namespace async_spy
{
    public static class Converter
    {
        public static void convert()
        {
            while (true)
            {
                Filename current;
                lock (Shared.s_xls_queue)
                {
                    if (Shared.s_xls_queue.Count != 0)
                    {
                        current = Shared.s_xls_queue.Dequeue();
                    }
                    else if (!Fetcher.isDone())
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                String currXLS = current.toXLS();
                if (URLGenerator.directories[current.dirNum - 1].getDownload())
                {
                    Console.WriteLine("Converting " + Config.local_xls_base + currXLS + " to " + Config.local_xlsx_base + current.toXLSX());
                    try
                    {
                        Thread.Sleep(100);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to convert " + Config.local_xls_base + currXLS);
                        Console.WriteLine(ex.Message);
                        continue;
                        //throw ex;
                    }
                    Console.WriteLine("Converted " + currXLS);
                }
                else
                {
                    Console.WriteLine("Skipping " + Config.local_xls_base + currXLS);
                }

                /*Console.WriteLine("Converting " + Config.local_xls_base + filename);
                try
                {
                    Process conversion = new Process();
                    conversion.StartInfo.FileName = @"C:\Program Files (x86)\Microsoft Office\Office12\excelcnv.exe";
                    conversion.StartInfo.Arguments = string.Format(@" -nme -oice {0} {1}", Config.local_xls_base + filename, Config.local_xlsx_base + filename);
                    conversion.Start();

                    conversion.WaitForExit(1000 * 60);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to convert " + filename);
                    Console.WriteLine(ex.Message);
                    throw ex;
                };
                Console.WriteLine(Config.local_xls_base + filename + " saved as " + Config.local_xlsx_base + filename);*/
            }
        }
    }
}
