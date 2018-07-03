using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Zdd.Utility
{
    /// <summary>
    /// 性能计数器助手
    /// </summary>
    public class PerformanceCounterHelper:MarshalByRefObject,IDisposable
    {
        /// <summary>
        /// 系统健康度
        /// </summary>
        /// <returns>健康</returns>
        public static bool IsPerformanceHealth()
        {
            using (PerformanceCounterHelper helper = new PerformanceCounterHelper())
            {
                return helper.IsHealth();
            }
        }

        /// <summary>
        /// 系统健康度
        /// </summary>
        /// <returns>健康</returns>
        public bool IsHealth()
        {
            if (this.performanceCounter_disk.NextValue() < 5.0f)
            {
                return false;
            }
            if (this.performanceCounter_memory.NextValue() > 95.0f)
            {
                return false;
            }
            return true;
        }

        private PerformanceCounter performanceCounter_cpu;
        private PerformanceCounter performanceCounter_memory;
        private PerformanceCounter performanceCounter_memory_value;
        private PerformanceCounter performanceCounter_disk;
        private PerformanceCounter performanceCounter_disk_value;
        private PerformanceCounter performanceCounter_disk_read;
        private PerformanceCounter performanceCounter_disk_write;
        private List<PerformanceCounter> performanceCounter_net_send_list = new List<PerformanceCounter>();
        private List<PerformanceCounter> performanceCounter_net_read_list = new List<PerformanceCounter>();


        public PerformanceCounterHelper()
        {
            performanceCounter_cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            performanceCounter_memory = new PerformanceCounter("Memory", "% Committed Bytes In Use");
            performanceCounter_memory_value = new PerformanceCounter("Memory", "Committed Bytes");
            performanceCounter_disk = new PerformanceCounter("LogicalDisk", "% Free Space", "_Total");
            performanceCounter_disk_value = new PerformanceCounter("LogicalDisk", "Free Megabytes", "_Total");
            performanceCounter_disk_read = new PerformanceCounter("LogicalDisk", "Disk Read Bytes/sec", "_Total");
            performanceCounter_disk_write = new PerformanceCounter("LogicalDisk", "Disk Write Bytes/sec", "_Total");

            try
            {
                PerformanceCounterCategory pc = new PerformanceCounterCategory("Network Interface");
                string[] instanceNames = pc.GetInstanceNames();
                foreach (string instanceName in instanceNames)
                {
                    if (instanceName != "MS TCP Loopback interface")
                    {
                        performanceCounter_net_send_list.Add(new PerformanceCounter("Network Interface", "Bytes Sent/sec", instanceName));
                        performanceCounter_net_read_list.Add(new PerformanceCounter("Network Interface", "Bytes Received/sec", instanceName));
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 静态的系统信息
        /// </summary>
        /// <returns></returns>
        public SystemInfo GetSystemInfo()
        {
            SystemInfo info = new SystemInfo();
            info.FreediskPercent = this.performanceCounter_disk.NextValue();
            info.FreediskValue = performanceCounter_disk_value.NextValue();
            return info;
        }

        /// <summary>
        /// 获取动态的性能信息
        /// </summary>
        /// <returns></returns>
        public PerformanceInfo GetPerformanceInfo()
        {
            PerformanceInfo info = new PerformanceInfo();
            info.CpuPercent = performanceCounter_cpu.NextValue();
            info.MemoryPercent = performanceCounter_memory.NextValue();
            info.MemoryValue = performanceCounter_memory_value.NextValue();
            info.DiskRead = performanceCounter_disk_read.NextValue();
            info.DiskWrite = performanceCounter_disk_write.NextValue();

            foreach (PerformanceCounter var in performanceCounter_net_send_list)
            {
                info.NetworkWrite += var.NextValue();
            }

            foreach (PerformanceCounter var in performanceCounter_net_read_list)
            {
                info.NetworkRead += var.NextValue();
            }
            return info;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            performanceCounter_cpu.Dispose();
            performanceCounter_memory.Dispose();
            performanceCounter_memory_value.Dispose();
            performanceCounter_disk.Dispose();
            performanceCounter_disk_value.Dispose();
            foreach (PerformanceCounter var in performanceCounter_net_send_list)
            {
                var.Dispose();
            }
            performanceCounter_net_send_list.Clear();
            foreach (PerformanceCounter var in performanceCounter_net_read_list)
            {
                var.Dispose();
            }
            performanceCounter_net_read_list.Clear();
            performanceCounter_disk_read.Dispose();
            performanceCounter_disk_write.Dispose();
        }

        #endregion
    }

    /// <summary>
    /// 静态的系统信息
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemInfo
    {
        private float freediskPercent;
        /// <summary>
        /// 磁盘剩余空间比率(%)
        /// </summary>
        public float FreediskPercent
        {
            get { return freediskPercent; }
            internal set { freediskPercent = value; }
        }
        private float freediskValue;
        /// <summary>
        /// 磁盘剩余空间大小(MByte)
        /// </summary>
        public float FreediskValue
        {
            get { return freediskValue; }
            internal set { freediskValue = value; }
        }
        /// <summary>
        /// 磁盘空间大小(MByte)
        /// </summary>
        public float DiskValue
        {
            get
            {
                if (freediskPercent != 0)
                {
                    return freediskValue / freediskPercent * 100;
                }
                return 0;
            }
        }
    }

    /// <summary>
    /// 动态的性能信息
    /// </summary>
    [Serializable]
    public struct PerformanceInfo
    {
        private float cpuPercent;
        /// <summary>
        /// CPU占用率(%)
        /// </summary>
        public float CpuPercent
        {
            get
            {
                return this.cpuPercent;
            }
            internal set
            {
                this.cpuPercent = value;
            }
        }
        private float memoryPercent;
        /// <summary>
        /// 内存占用率(%)
        /// </summary>
        public float MemoryPercent
        {
            get { return memoryPercent; }
            internal set { memoryPercent = value; }
        }


        private float memoryValue;
        /// <summary>
        /// 内存使用量(Byte)
        /// </summary>
        public float MemoryValue
        {
            get { return memoryValue; }
            internal set { memoryValue = value; }
        }

        private float networkRead;
        /// <summary>
        /// 网络接收量(Byte)
        /// </summary>
        public float NetworkRead
        {
            get { return networkRead; }
            internal set { networkRead = value; }
        }
        private float networkWrite;
        /// <summary>
        /// 网络发送量(Byte)
        /// </summary>
        public float NetworkWrite
        {
            get { return networkWrite; }
            internal set { networkWrite = value; }
        }

        private float diskRead;
        /// <summary>
        /// 磁盘读取量(Byte)
        /// </summary>
        public float DiskRead
        {
            get { return diskRead; }
            internal set { diskRead = value; }
        }
        private float diskWrite;
        /// <summary>
        /// 磁盘写入量(Byte)
        /// </summary>
        public float DiskWrite
        {
            get { return diskWrite; }
            internal set { diskWrite = value; }
        }
    }
}
