﻿using SSGui;
using System;
using System.Windows.Forms;

namespace SS
{
    static class Launch
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Run a single context that supports multiple windows
            var appContext = SpreadsheetApplicationContext.GetContext();

            if (args.Length > 0)
            {
                appContext.RunExisting(args[0]);
            }
            else
            {
                appContext.RunNew();
            }
            
            Application.Run(appContext);
        }
    }
}
