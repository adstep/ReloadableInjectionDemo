using System;
using System.Runtime.InteropServices;

namespace Injecter.Utils
{
    static unsafe class ProcessExtensions
    {
        public static ulong AllocateMemory(this System.Diagnostics.Process process, uint length, Native.AllocationType allocationType, Native.MemoryProtection memoryProtection) =>
            Native.VirtualAllocEx(process.Handle, 0, length, allocationType, memoryProtection);

        public static ulong AllocateAndWrite(this System.Diagnostics.Process process, byte[] buffer, Native.AllocationType allocationType, Native.MemoryProtection memoryProtection)
        {
            ulong allocatedMemory = process.AllocateMemory((uint)buffer.Length, allocationType, memoryProtection);

            process.WriteRawMemory(buffer, allocatedMemory);

            return allocatedMemory;
        }

        public static void WriteRawMemory(this System.Diagnostics.Process process, byte[] buffer, ulong memoryPointer)
        {
            if (!Native.WriteProcessMemory(process.Handle, memoryPointer, buffer, (uint)buffer.Length, 0))
                throw new Exception($"WriteBuffer - WriteProcessMemory() failed - {Marshal.GetLastWin32Error():x2}");
        }

        public static ulong GetModuleByName(this System.Diagnostics.Process process, string moduleName)
        {
            ulong[] moduleHandleArray = new ulong[1000];

            fixed (ulong* hMods = moduleHandleArray)
            {
                if (Native.EnumProcessModules(process.Handle, (ulong)hMods, (uint)(sizeof(ulong) * moduleHandleArray.Length), out uint cbNeeded) > 0)
                {
                    for (int moduleIndex = 0; moduleIndex < cbNeeded / sizeof(ulong); moduleIndex++)
                    {
                        string name = NativeExtensions.GetModuleBaseName(process.Handle, moduleHandleArray[moduleIndex]);

                        if (String.Equals(name, moduleName, StringComparison.InvariantCultureIgnoreCase))
                            return moduleHandleArray[moduleIndex];
                    }
                }
            }

            return 0;
        }
    }
}
