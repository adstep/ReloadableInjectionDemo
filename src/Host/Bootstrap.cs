using Host.Utils;
using RGiesecke.DllExport;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Message = Host.Utils.Message;

namespace Host
{
    public static class Bootstrap
    {
        #region Constants

        private const Keys ReloadHotKey = Keys.F11;

        #endregion

        #region Fields

        private static readonly Random Rand = new Random();

        #endregion

        #region Public Methods

        [DllExport("Run", CallingConvention.StdCall)]
        public static void Run([MarshalAs(UnmanagedType.LPWStr)] string args)
        {
            var parts = args.Split(' ');

            var assemblyPath = parts[0];
            var assemblyArgs = string.Join(" ", parts.Skip(1));

            ReloadLoop(assemblyPath, assemblyArgs);
        }

        #endregion

        #region Internal Methods

        [STAThread]
        private static void ReloadLoop(string assemblyPath, string args)
        {
            Debugger.Launch();

            if (!File.Exists(assemblyPath))
            {
                Message.ShowError($"Could not find assembly at '{assemblyPath}.");
            }

            var extension = Path.GetExtension(assemblyPath);
            if (extension != ".exe" && extension != ".dll")
            {
                Message.ShowError($"Invalid file type for '{assemblyPath}', can only load exe/dll files");
            }

            HostDomain(assemblyPath, args);

            while (true)
            {
                if ((Native.GetAsyncKeyState((int)ReloadHotKey) & 1) == 1)
                {
                    HostDomain(assemblyPath, args);
                }

                Thread.Sleep(10);
            }
        }

        private static void HostDomain(string assemblyPath, string args)
        {
            AppDomain currentDomain = null;

            try
            {
                var appBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                var ads = new AppDomainSetup()
                {
                    ApplicationBase = appBase,
                    PrivateBinPath = appBase
                };

                var domainName = $"Host_Internal_{Rand.Next(0, 100000)}";

                currentDomain = AppDomain.CreateDomain(domainName, AppDomain.CurrentDomain.Evidence, ads);

                var loader = (AssemblyLoader)currentDomain.CreateInstanceAndUnwrap(
                    Assembly.GetExecutingAssembly().FullName, typeof(AssemblyLoader).FullName);

                loader.LoadAndRun(assemblyPath, args);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Host Error");
            }
            finally
            {
                if (currentDomain != null)
                    AppDomain.Unload(currentDomain);
            }
        }

        #endregion
    }
}
