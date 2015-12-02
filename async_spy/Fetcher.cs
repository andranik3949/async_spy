using System;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace async_spy
{
    public static class Fetcher
    {
        static Fetcher()
        {
            m_isDone = false;
            m_threadFree = new Semaphore( Config.max_thread_num, Config.max_thread_num );
        }

        public static void fetch()
        {      
            while ( !m_isDone )
            {
                Filename current;
                lock( Shared.s_url_queue )
                {
                    if( Shared.s_url_queue.Count != 0 )
                    {
                        current = Shared.s_url_queue.Dequeue();
                    }
                    else if ( !URLGenerator.isDone() )
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    else
                    {
                        m_isDone = true;
                        break;
                    }
                }
                if (URLGenerator.directories[current.getDirNum() - 1].getDownload())
                {
                    String currXLS = current.toXLS();
                    String currRemoteXLS = current.toRemoteXLS();

                    WebClient client = new WebClient();

                    m_threadFree.WaitOne();
                    Console.WriteLine("Downloading " + Config.url_base + currRemoteXLS + " to " + Config.local_xls_base + currXLS);
                    client.DownloadFileTaskAsync(Config.url_base + currRemoteXLS, Config.local_xls_base + currXLS).ContinueWith( (prevTask) => 
                    {
                        m_threadFree.Release(1);
                        if (prevTask.Exception != null)
                        {
                           Console.WriteLine("Failed to download " + Config.url_base + currRemoteXLS);
                           Console.WriteLine(prevTask.Exception.Message);
                           //throw e.Error;
                        }
                        lock (Shared.s_xls_queue)
                        {
                           Shared.s_xls_queue.Enqueue(current);
                        }
                        Console.WriteLine("Downloaded " + currXLS);
                    });
                }
                else
                {
                    Console.WriteLine("Skipping " + Config.url_base + current.toRemoteXLS() );
                }
                URLGenerator.directories[current.getDirNum() - 1].advance();
            }
        }

        public static bool isDone()
        {
            return m_isDone;
        }

        private static Semaphore m_threadFree;
        private static bool m_isDone;
    }
}
