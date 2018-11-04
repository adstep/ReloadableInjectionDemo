using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Host
{
    class AssemblyLoader : MarshalByRefObject
    {
        #region Public Methods

        public void LoadAndRun(string assemblyPath, string args)
        {
            var asm = Assembly.LoadFrom(assemblyPath);

            var entryMethods = asm.GetTypes()
                                  .SelectMany(t => t.GetMethods())
                                  .Where(m => m.GetCustomAttributes(typeof(STAThreadAttribute), false).Any())
                                  .ToArray();

            if (!entryMethods.Any())
            {
                MessageBox.Show("Unable to find entry function with [STAThread] Attribute");
                return;
            }

            if (entryMethods.Length > 1)
            {
                MessageBox.Show("More than one function with an [STAThread] Attribute was found; injected assemblies must only have one");
                return;
            }

            var entry = entryMethods[0];

            entry.Invoke(null, string.IsNullOrEmpty(args) ? new object[] {string.Empty} : new object[] {args});
        }

        #endregion
    }
}
