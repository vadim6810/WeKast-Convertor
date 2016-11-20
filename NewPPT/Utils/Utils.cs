using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeCastConvertor.Utils
{
    public static class Utils
    {
        public static string GetFileSize(long size)
        {
            var len = size;
            string[] sizes = { "", "K", "M", "G" };
            var order = 0;
            while (len >= 1024 && ++order < sizes.Length)
            {
                len = len / 1024;
            }
            return $"{len}{sizes[order]}";
        }

    }
}
