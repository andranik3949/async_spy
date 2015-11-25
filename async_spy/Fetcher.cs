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
        }

        public static void fetch()
        {
            WebClient client = new WebClient();
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
                String currXLS = current.toXLS();
                if (URLGenerator.directories[current.dirNum - 1].downloadFlag)
                {        
                    Console.WriteLine("Downloading " + Config.url_base + currXLS + " to " + Config.local_xls_base + currXLS);
                    try
                    {
                        client.DownloadFile(Config.url_base + currXLS, Config.local_xls_base + currXLS);
                    }
                    catch (WebException ex)
                    {
                        Console.WriteLine("Failed to download " + Config.url_base + currXLS);
                        Console.WriteLine(ex.Message);
                        continue;
                        //throw ex;
                    }
                    Console.WriteLine("Downloaded " + currXLS);

                    lock ( Shared.s_xls_queue )
                    {
                        Shared.s_xls_queue.Enqueue(current);
                    }
                }
                else
                {
                    Console.WriteLine("Skipping " + Config.url_base + currXLS);
                }
                URLGenerator.directories[current.dirNum - 1].advance();
            }
        }

        public static bool isDone()
        {
            return m_isDone;
        }

        private static bool m_isDone;
    }
}
