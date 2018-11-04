using System.Runtime.InteropServices;

namespace Host.Utils
{
    class Native
    {
        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(int vKey);
    }
}
