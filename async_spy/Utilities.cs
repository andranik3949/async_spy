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
            return dirNum.ToString() + "/" + dirNum.ToString() + "_" + temp;
        }
    }

    public struct DirInfo
    {
        public bool downloadFlag;
        public int maxLocalSuffix;
        public int progress;

        public void setMaxLocalSuffix(int max)
        {
            maxLocalSuffix = max;
        }

        public void advance()
        {
            progress++;
        }
    }
}
