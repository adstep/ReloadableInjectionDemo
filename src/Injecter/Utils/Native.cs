using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Injecter.Utils
{
    class Native
    {
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern ulong GetProcAddress(ulong hModule, string procName);

        [DllImport("kernel32", SetLastError = true)]
        public static extern ulong GetProcAddress(ulong hModule, ulong procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern ulong GetModuleHandle(string lpModuleName);

        [DllImport("psapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int EnumProcessModules(IntPtr hProcess, [Out] ulong lphModule, uint cb, out uint lpcbNeeded);

        [DllImport("psapi.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern uint GetModuleBaseName(IntPtr hProcess, ulong hModule, [Out] StringBuilder lpBaseName, uint nSize);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern ulong LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary(ulong hModule);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern ulong CreateRemoteThread(IntPtr hProcess, ulong lpThreadAttributes, uint dwStackSize, ulong lpStartAddress, ulong lpParameter, uint dwCreationFlags, out ulong lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern ulong VirtualAllocEx(IntPtr hProcess, ulong lpAddress, uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, ulong lpBaseAddress, byte[] lpBuffer, int dwSize, int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, ulong lpBaseAddress, byte[] lpBuffer, uint nSize, ulong lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint WaitForSingleObject(ulong hHandle, uint dwMilliseconds);

        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }
    }
}
