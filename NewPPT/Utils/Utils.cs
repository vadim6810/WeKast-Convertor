using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "GetDiskFreeSpaceW")]
        static extern bool GetDiskFreeSpace(string lpRootPathName, out int lpSectorsPerCluster, out int lpBytesPerSector, out int lpNumberOfFreeClusters, out int lpTotalNumberOfClusters);

        public static int GetClusterSize(string path)
        {

            int sectorsPerCluster;
            int bytesPerSector;
            int freeClusters;
            int totalClusters;
            int clusterSize = 0;
            if (GetDiskFreeSpace(Path.GetPathRoot(path), out sectorsPerCluster, out bytesPerSector, out freeClusters, out totalClusters))
                clusterSize = bytesPerSector * sectorsPerCluster;
            return clusterSize;
        }
        

    }
}
