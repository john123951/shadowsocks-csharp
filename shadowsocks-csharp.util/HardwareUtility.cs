using System;
using System.Management;
using System.Text;

namespace shadowsocks_csharp.util
{
    /// <summary>
    /// 得到硬件信息
    /// </summary>
    public static class HardwareUtility
    {
        /// <summary>
        /// 返回硬盘序列号标识
        /// </summary>
        /// <returns></returns>
        public static string GetHardDiskSerialNo()
        {
            ManagementClass searcher = new ManagementClass("WIN32_PhysicalMedia");
            ManagementObjectCollection moc = searcher.GetInstances();

            if (moc != null && moc.Count > 0)
            {
                var itor = moc.GetEnumerator();
                if (itor.MoveNext())
                {
                    //硬盘序列号
                    var serialNum = itor.Current["SerialNumber"];

                    if (serialNum != null)
                    {
                        return serialNum.ToString().Trim();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 返回CPU标识
        /// </summary>
        /// <returns></returns>
        public static string GetCpuId()
        {
            ManagementClass searcher = new ManagementClass("WIN32_Processor");
            ManagementObjectCollection moc = searcher.GetInstances();
            var sbCpuIds = new StringBuilder();

            foreach (ManagementObject mo in moc)
            {
                var uniqueId = mo["UniqueId"];
                var processorId = mo["ProcessorId"];
                sbCpuIds.AppendFormat("{0}_{1}_", processorId, uniqueId);
            }
            return sbCpuIds.ToString(0, Math.Max(0, sbCpuIds.Length - 1));
        }
    }
}