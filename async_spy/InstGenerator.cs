#if INSTANT_GENERATOR

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
            m_maxGlobalSuffix = Int32.Parse( fetchMaxSuffix(Config.url_base).TrimEnd('/') );
        }

        public static int getLocalMax(int dirNum)
        {
            return Int32.Parse(fetchMaxSuffix(Config.url_base + dirNum.ToString() + "/").Substring(2, 2));
        }

        public static int getGlobalMax()
        {
            return m_maxGlobalSuffix;
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
    }
}

#endif