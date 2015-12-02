using System;

namespace async_spy
{
   public class Filename
   {  
      public Filename( int dirNum, String fileName )
      {
         m_dirNum = dirNum;
         m_fileName = fileName;
      }

      public int getDirNum()
      {
         return m_dirNum;
      }

      public String toXLS()
      {
         return m_dirNum.ToString() + "\\" + m_fileName;
      }
      
      public String toRemoteXLS()
      {
         return toXLS().Replace('\\', '/');
      }
      
      public String toXLSX()
      {
         return toXLS() + "x";
      }

      private int m_dirNum;
      private string m_fileName;
   }

   public class DirInfo
   {
      public DirInfo( int maxLocalSuffix )
      {
          m_downloadFlag = true;
          m_maxLocalSuffix = maxLocalSuffix;
          m_progress = 0;
      }
      
      public int getMaxLocalSuffix()
      {
         return m_maxLocalSuffix;
      }
      
      public bool getDownload()
      {
         return m_downloadFlag;
      }
      
      public void setDownload()
      {
         m_downloadFlag = true;
      }
      
      public void unsetDownload()
      {
         m_downloadFlag = false;
      }
      
      public void advance()
      {
         m_progress++;
      }
      
      private bool m_downloadFlag;
      private int m_maxLocalSuffix;
      private int m_progress;
   }
}
