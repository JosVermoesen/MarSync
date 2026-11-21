using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MarSync
{
    internal static class Program
    {
        private sealed class ConsoleDebugTextWriter : TextWriter
        {
            private const string AppPrefix = "[APP] ";

            public override Encoding Encoding => Encoding.UTF8;

            public override void WriteLine(string value)
            {
                string prefixed = string.IsNullOrEmpty(value) || value.StartsWith(AppPrefix, StringComparison.Ordinal)
                    ? value
                    : AppPrefix + value;

                Debug.WriteLine(prefixed);
            }

            public override void Write(char value)
            {
                Debug.Write(value);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.SetOut(new ConsoleDebugTextWriter());
            Console.SetError(new ConsoleDebugTextWriter());

            using (var mutex = new Mutex(
                initiallyOwned: true,
                name: @"Global\MarSyncSingleInstance", // any unique name
                createdNew: out bool createdNew))
            {
                if (!createdNew)
                {
                    // MessageBox.Show(
                    //     "MarSync is already running.",
                    //     "Already running",
                    //     MessageBoxButtons.OK,
                    //     MessageBoxIcon.Information);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMarSync());
            }
        }
    }
}
