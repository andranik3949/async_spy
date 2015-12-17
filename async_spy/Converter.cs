using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace async_spy
{
    public static class Converter
    {
        static Converter()
        {
            m_threadFree = new Semaphore(Config.max_thread_num / 2, Config.max_thread_num / 2);
        }

        public static void convert()
        {
            List<Task> taskList = new List<Task>();
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
                        Task.WaitAll(taskList.ToArray());
                        break;
                    }
                }
                if (URLGenerator.directories[current.getDirNum() - 1].getDownload())
                {
                    m_threadFree.WaitOne();
                    taskList.Add ( Task.Run(() =>
                    {
                        convertFile(current);
                    }).ContinueWith((lastTask) => { m_threadFree.Release(); }) );
                }
                else
                {
                    Console.WriteLine("Skipping " + Config.local_xls_base + current.toXLS());
                }
            }
        }

        private static void convertFile( Filename current )
        {
            String currXLS = current.toXLS();
            String currXLSX = current.toXLSX();
            Console.WriteLine("Converting " + Config.local_xls_base + currXLS + " to " + Config.local_xlsx_base + currXLSX);
            try
            {
               //Thread.Sleep(333);
               if( !Directory.Exists(Config.local_xlsx_base + current.getDirNum().ToString()) )
               {
                  Directory.CreateDirectory(Config.local_xlsx_base + current.getDirNum().ToString());
               }

               Process conversion = new Process();
               conversion.StartInfo.FileName = @"C:\Program Files (x86)\Microsoft Office\Office12\excelcnv.exe";
               conversion.StartInfo.Arguments = string.Format(@" -nme -oice {0} {1}", Config.local_xls_base + currXLS, Config.local_xlsx_base + currXLSX);
               conversion.Start();

               if( !conversion.WaitForExit(1000 * 10) )
               {
                  conversion.Kill();
                  lock( Shared.s_url_queue )
                  {
                     Shared.s_url_queue.Enqueue(current);
                  }
               }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to convert " + Config.local_xls_base + currXLS);
                Console.WriteLine(ex.Message);
                //throw ex;
            }
            Console.WriteLine("Converted " + currXLS);
         }

         private static SemaphoreSlim m_threadFree;
    }
}
