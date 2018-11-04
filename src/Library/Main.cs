using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Library
{
    public static class Main    
    {
        [STAThread]
        public static void Run(string args)
        {
            var process = Process.GetCurrentProcess();
            MessageBox.Show($"Current Process: {process.Id} Args: {args}", "Library");
        }
    }
}
