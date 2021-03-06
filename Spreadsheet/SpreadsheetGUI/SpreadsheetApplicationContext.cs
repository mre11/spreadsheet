﻿// Created by Morgan Empey for CS 3500, University of Utah, Spring 2015

using System;
using System.Windows.Forms;

namespace SSGui
{
    /// <summary>
    /// Provides a context for the Spreadsheet application so multiple windows are supported
    /// </summary>
    class SpreadsheetApplicationContext : ApplicationContext
    {
        /// <summary>
        /// Keeps track of how many windows are open in this context
        /// </summary>
        private int windowCount = 0;

        /// <summary>
        /// A singleton application context
        /// </summary>
        private static SpreadsheetApplicationContext context;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private SpreadsheetApplicationContext()
        {
        }

        /// <summary>
        /// Returns the application context
        /// </summary>
        public static SpreadsheetApplicationContext GetContext()
        {
            if (context == null)
            {
                context = new SpreadsheetApplicationContext();
            }

            return context;
        }

        /// <summary>
        /// Creates a new Spreadsheet window
        /// </summary>
        public void RunNew()
        {
            var window = new SpreadsheetWindow();

            new Controller(window);

            windowCount++;

            window.FormClosed += HandleFormClosed;

            window.Show();
        }

        /// <summary>
        /// Creates a new Spreadsheet window from the existing file at path
        /// </summary>
        public void RunExisting(string path)
        {
            try
            {
                var window = new SpreadsheetWindow();

                new Controller(window, path);

                windowCount++;

                window.FormClosed += HandleFormClosed;

                window.Show();
            }
            catch (Exception) { }            
        }

        /// <summary>
        /// Handles the event of a Spreadsheet window being closed
        /// </summary>
        private void HandleFormClosed(object sender, FormClosedEventArgs e)
        {
            if (--windowCount < 1) ExitThread();
        }
    }
}
