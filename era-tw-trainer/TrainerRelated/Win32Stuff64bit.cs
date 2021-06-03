using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace era_tw_trainer
{
    class Win32Stuff64bit
    {
        // ATTENTION!!!! to target 64 bit app, remember to configure this project's target to x64 platform!!!!

        public const int PROCESS_WM_READ = 0x0010;
        public const int PROCESS_VM_WRITE = 0x0020;
        public const int PROCESS_VM_OPERATION = 0x0008;
        public const int PROCESS_QUERY_INFORMATION = 0x0400;
        public const int MEM_COMMIT = 0x00001000;
        public const int PAGE_READWRITE = 0x04;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int dwNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int dwNumberOfBytesWritten);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        //public struct SYSTEM_INFO
        //{
        //    public ushort processorArchitecture;
        //    ushort reserved;
        //    public uint pageSize;
        //    public IntPtr minimumApplicationAddress;  // minimum address
        //    public IntPtr maximumApplicationAddress;  // maximum address
        //    public IntPtr activeProcessorMask;
        //    public uint numberOfProcessors;
        //    public uint processorType;
        //    public uint allocationGranularity;
        //    public ushort processorLevel;
        //    public ushort processorRevision;
        //}

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO
        {
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }

        [DllImport("kernel32.dll")]
        public static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

        //public struct MEMORY_BASIC_INFORMATION
        //{
        //    public int BaseAddress;
        //    public int AllocationBase;
        //    public int AllocationProtect;
        //    public int RegionSize;   // size of the region allocated by the program
        //    public int State;   // check if allocated (MEM_COMMIT)
        //    public int Protect; // page protection (must be PAGE_READWRITE)
        //    public int lType;
        //}

        //// 32 bit app
        //[StructLayout(LayoutKind.Sequential)]
        //public struct MEMORY_BASIC_INFORMATION
        //{
        //    public IntPtr BaseAddress;
        //    public IntPtr AllocationBase;
        //    public uint AllocationProtect;
        //    public IntPtr RegionSize;
        //    public uint State;
        //    public uint Protect;
        //    public uint Type;
        //}

        //// 64 bit app
        //[StructLayout(LayoutKind.Sequential)]
        //public struct MEMORY_BASIC_INFORMATION64
        //{
        //    public ulong BaseAddress;
        //    public ulong AllocationBase;
        //    public int AllocationProtect;
        //    public int __alignment1;
        //    public ulong RegionSize;
        //    public int State;
        //    public int Protect;
        //    public int Type;
        //    public int __alignment2;
        //}

        public enum AllocationProtectEnum : uint
        {
            PAGE_EXECUTE = 0x00000010,
            PAGE_EXECUTE_READ = 0x00000020,
            PAGE_EXECUTE_READWRITE = 0x00000040,
            PAGE_EXECUTE_WRITECOPY = 0x00000080,
            PAGE_NOACCESS = 0x00000001,
            PAGE_READONLY = 0x00000002,
            PAGE_READWRITE = 0x00000004,
            PAGE_WRITECOPY = 0x00000008,
            PAGE_GUARD = 0x00000100,
            PAGE_NOCACHE = 0x00000200,
            PAGE_WRITECOMBINE = 0x00000400
        }

        public enum StateEnum : uint
        {
            MEM_COMMIT = 0x1000,
            MEM_FREE = 0x10000,
            MEM_RESERVE = 0x2000
        }

        public enum TypeEnum : uint
        {
            MEM_IMAGE = 0x1000000,
            MEM_MAPPED = 0x40000,
            MEM_PRIVATE = 0x20000
        }

        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public AllocationProtectEnum AllocationProtect;
            public IntPtr RegionSize;
            public StateEnum State;
            public AllocationProtectEnum Protect;
            public TypeEnum Type;
        }

        // for this program setting bad value flag to -100 instead
        //public static int BAD_RESULT_FLAG = -1;
        public static int BAD_RESULT_FLAG = -100;

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        public static byte readByte(IntPtr hProcess, IntPtr addr)
        {
            byte[] buffer = new byte[1];
            int bytesRead = 0;
            bool r = ReadProcessMemory(hProcess, addr, buffer, 1, ref bytesRead);
            if (!r)
            {
                return 0xFF;
            }
            return buffer[0];
        }

        public static bool writeByte(IntPtr hProcess, IntPtr addr, byte value)
        {
            byte[] buffer = new byte[] { value };
            int bytesWrote = 0;
            bool r = WriteProcessMemory(hProcess, addr, buffer, 1, ref bytesWrote);
            return r;
        }

        public static Int16 readInt16(IntPtr hProcess, IntPtr addr)
        {
            byte[] buffer = new byte[2];
            int bytesRead = 0;
            bool r = ReadProcessMemory(hProcess, addr, buffer, 2, ref bytesRead);
            if (!r)
            {
                return (Int16)BAD_RESULT_FLAG;
            }
            Int16 value = BitConverter.ToInt16(buffer, 0);
            return value;
        }

        public static bool writeInt16(IntPtr hProcess, IntPtr addr, Int16 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            int bytesWrote = 0;
            bool r = WriteProcessMemory(hProcess, addr, buffer, 2, ref bytesWrote);
            return r;
        }

        public static Int32 readInt32(IntPtr hProcess, IntPtr addr)
        {
            byte[] buffer = new byte[4];
            int bytesRead = 0;
            bool r = ReadProcessMemory(hProcess, addr, buffer, 4, ref bytesRead);
            if (!r)
            {
                return BAD_RESULT_FLAG;
            }
            Int32 value = BitConverter.ToInt32(buffer, 0);
            return value;
        }

        public static bool writeInt32(IntPtr hProcess, IntPtr addr, Int32 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            int bytesWrote = 0;
            bool r = WriteProcessMemory(hProcess, addr, buffer, 4, ref bytesWrote);
            return r;
        }

        public static Int64 readInt64(IntPtr hProcess, IntPtr addr)
        {
            byte[] buffer = new byte[8];
            int bytesRead = 0;
            bool r = ReadProcessMemory(hProcess, addr, buffer, 8, ref bytesRead);
            if (!r)
            {
                return BAD_RESULT_FLAG;
            }
            Int64 value = BitConverter.ToInt64(buffer, 0);
            return value;
        }

        public static bool writeInt64(IntPtr hProcess, IntPtr addr, Int64 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            int bytesWrote = 0;
            bool r = WriteProcessMemory(hProcess, addr, buffer, 8, ref bytesWrote);
            return r;
        }
    }
}
