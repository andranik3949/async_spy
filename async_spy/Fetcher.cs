using System;
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
                if (URLGenerator.directories[current.getDirNum()].getDownload())
                {
                    m_threadFree.WaitOne();
                    Task.Run(() =>
                    {
                        fetchFile(current);
                    }).ContinueWith((prevTask) => { Console.WriteLine( m_threadFree.Release(1) ); });
                }
                else
                {
                    Console.WriteLine("Skipping " + Config.url_base + current.toXLS() );
                }
                URLGenerator.directories[current.getDirNum()].advance();
            }
        }

        private static void fetchFile( Filename current )
        {
            WebClient client = new WebClient();
            String currXLS = current.toXLS();
            String currRemoteXLS = current.toRemoteXLS();

            Console.WriteLine("Downloading " + Config.url_base + currRemoteXLS + " to " + Config.local_xls_base + currXLS);
            try
            {
               client.DownloadFile(Config.url_base + currRemoteXLS, Config.local_xls_base + currXLS);
               //Thread.Sleep(1000);
            }
            catch (WebException ex)
            {
                Console.WriteLine("Failed to download " + Config.url_base + currRemoteXLS);
                Console.WriteLine(ex.Message);
                //throw ex;
            }

            lock (Shared.s_xls_queue)
            {
                Shared.s_xls_queue.Enqueue(current);
            }
            Console.WriteLine("Downloaded " + currXLS);
        }

        public static bool isDone()
        {
            return m_isDone;
        }

        private static Semaphore m_threadFree;
        private static bool m_isDone;
    }
}
