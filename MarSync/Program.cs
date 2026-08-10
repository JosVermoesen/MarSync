using System;
using System.Threading;
using System.Windows.Forms;

namespace MarSync
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
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
