using System;
using System.Text;

namespace Injecter.Utils
{
    class NativeExtensions
    {
        public static ulong GetModuleFunction(string moduleName, string functionName)
            => Native.GetProcAddress(Native.GetModuleHandle(moduleName), functionName);

        public static void WaitForThread(ulong threadHandle)
            => Native.WaitForSingleObject(threadHandle, uint.MaxValue);

        public static string GetModuleBaseName(IntPtr processHandle, ulong moduleHandle)
        {
            StringBuilder name = new StringBuilder(1024);

            Native.GetModuleBaseName(processHandle, moduleHandle, name, 1024);

            return name.ToString();
        }
    }
}
