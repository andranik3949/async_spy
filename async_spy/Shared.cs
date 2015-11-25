using System.Collections.Generic;

namespace async_spy
{
    public static class Shared
    {
        public static Queue<Filename> s_url_queue = new Queue<Filename>();
        public static Queue<Filename> s_xls_queue = new Queue<Filename>();
    }
}