#if !INSTANT_GENERATOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace async_spy
{
    static public class URLGenerator
    {
        // Constructor
        static URLGenerator()
        {
            m_isDone = false;
            m_maxGlobalSuffix = Int32.Parse(fetchMaxSuffix(Config.url_base).TrimEnd('/'));
            directories = new List<DirInfo>( m_maxGlobalSuffix );
            for ( int i = 0; i < m_maxGlobalSuffix; i++ )
            {
                DirInfo temp = new DirInfo();
                directories.Add( temp );
            }
        }

        // Incremental generator
        public static void generate()
        {
            Filename url;
            for( int i = 1; i <= m_maxGlobalSuffix; i++ )
            {
                if( !Directory.Exists( Config.local_xls_base + i.ToString() + "/") )
                {
                    Directory.CreateDirectory(Config.local_xls_base + i.ToString() + "/");
                }

                url.dirNum = i;
                String maxFilename = fetchMaxSuffix(Config.url_base + i.ToString() + "/");
                int maxLocalSuffix = Int32.Parse(maxFilename.Substring( maxFilename.Length - ("**.xls").Length, 2 ));
                directories[i - 1].setMaxLocalSuffix(maxLocalSuffix);
                for( int j = 1; j <= maxLocalSuffix; j++ )
                {
                    url.fileNum = j;
                    if (URLGenerator.directories[i - 1].getDownload())
                    {
                        Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                        lock (Shared.s_url_queue)
                        {
                            Shared.s_url_queue.Enqueue(url);
                        }
                    }
                }
            }
            m_isDone = true;
        }

        public static bool isDone()
        {
            return m_isDone;
        }

        // Known format htm parser
        private static String fetchMaxSuffix(String url)
        {
            WebRequest request = WebRequest.Create(url);
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());

            Match matchedLine = Regex.Match(reader.ReadToEnd(), "href=\"(.*)\"", RegexOptions.RightToLeft);
            return matchedLine.Groups[1].Value;
        }

        // Private members
        static private int m_maxGlobalSuffix;
        static private bool m_isDone;

        static public List<DirInfo> directories;
    }
}

#endif