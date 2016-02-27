﻿using SSGUI;
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
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SpreadsheetWindow());
        }
    }
}
