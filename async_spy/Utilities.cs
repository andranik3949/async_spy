using System;

namespace async_spy
{
    public struct Filename
    {
        public int dirNum;
        public int fileNum;

        public String toXLS()
        {
            return toPath() + ".xls";
        }

        public String toRemoteXLS()
        {
            return toXLS().Replace('\\', '/');
        }

        public String toXLSX()
        {
            return toPath() + ".xlsx";
        }

        private String toPath()
        {
            String temp = fileNum.ToString();
            if( dirNum != 39 )   // Because fuck logic, that's why
            {
                temp = temp.PadLeft(2, '0');
            }
            return dirNum.ToString() + "\\" + dirNum.ToString() + "_" + temp;
        }
    }

    public class DirInfo
    {
        public DirInfo()
        {
            m_downloadFlag = true;
            m_maxLocalSuffix = -1;
            m_progress = 0;
        }

        public void setMaxLocalSuffix(int max)
        {
            m_maxLocalSuffix = max;
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
