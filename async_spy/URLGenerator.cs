using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace async_spy
{
   static public class URLGenerator
   {
      // Constructor
      static URLGenerator()
      {
         m_isDone = false;
         directories = new List<DirInfo>();
      }
       
       // Incremental generator
      public static void generate()
      {
         HtmlWeb web = new HtmlWeb();
         HtmlNodeCollection dirs = web.Load(Config.url_base).DocumentNode.SelectNodes("//a[@href]");
         m_maxGlobalSuffix = dirs.Count - 5;   //known value
         foreach (HtmlNode dirLink in dirs)
         {
            String dirName = dirLink.Attributes["href"].Value;
            if (Char.IsDigit(dirName[0]))
            {
               int dirNum = Int32.Parse(dirName.TrimEnd('/'));
               if (!Directory.Exists(Config.local_xls_base + dirNum.ToString() + "\\"))
               {
                  Directory.CreateDirectory(Config.local_xls_base + dirNum.ToString() + "\\");
               }

               HtmlNodeCollection files = web.Load(Config.url_base + dirName).DocumentNode.SelectNodes("//a[@href]");
               directories.Add(new DirInfo(files.Count - 5));
               foreach (HtmlNode fileLink in files)
               {
                  String fileName = fileLink.Attributes["href"].Value;
                  if (Char.IsDigit(fileName[0]) && URLGenerator.directories[dirNum - 1].getDownload())
                  {
                     lock (Shared.s_url_queue)
                     {
                        Shared.s_url_queue.Enqueue(new Filename(dirNum, fileLink.Attributes["href"].Value));
                     }
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

      // Private members
      static private int m_maxGlobalSuffix;
      static private bool m_isDone;
      
      static public List<DirInfo> directories;
   }
}