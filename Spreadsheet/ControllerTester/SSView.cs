// Created by Morgan Empey for CS 3500, University of Utah, Spring 2015

using System;
using System.Windows.Forms;
using SSGui;

namespace SSControllerTester
{
    /// <summary>
    /// Provides a stub view to test the controller against.
    /// </summary>
    class SSView : ISpreadsheetView
    {
        /// <summary>
        /// Represents the displayed cell values in the spreadsheet.
        /// Indices are [col,row]
        /// </summary>
        private string[,] displayedValues;

        /// <summary>
        /// The name of the view
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The column and row of the selected cell
        /// </summary>
        private int[] SelectedCell { get; set; }

        /// <summary>
        /// The displayed name of the currently selected cell
        /// </summary>
        public string SelectedCellName { get; set; }

        /// <summary>
        /// The displayed value of the currently selected cell
        /// </summary>
        public string SelectedCellValue { get; set; }

        /// <summary>
        /// The displayed contents of the currently selected cell
        /// </summary>
        public string SelectedCellContents { get; set; }

        /// <summary>
        /// Records whether DoNew was called
        /// </summary>
        public bool CalledDoNew { get; private set; }

        /// <summary>
        /// Records whether DoOpen was called
        /// </summary>
        public bool CalledDoOpen { get; private set; }

        /// <summary>
        /// Records whether DoSaveAs was called
        /// </summary>
        public bool CalledDoSaveAs { get; private set; }

        /// <summary>
        /// Records whether DoHelpContents was called
        /// </summary>
        public bool CalledHelpContents { get; private set; }

        /// <summary>
        /// Records if an error message was shown.
        /// </summary>
        public bool CalledShowErrorMessage { get; private set; }

        /// <summary>
        /// Default file name that would be shown in open or save as file dialogs
        /// </summary>
        public string DefaultOpenSaveFileName { get; set; }

        /// <summary>
        /// Records if the warning for unsaved file is shown on close.
        /// </summary>
        public bool ShowedCloseWarning { get; private set; }

        /// <summary>
        /// Creates an empty SSView
        /// </summary>
        internal SSView()
        {
            int colCount = 26;
            int rowCount = 99;

            displayedValues = new string[colCount, rowCount];

            // Initialize all values to the empty string
            for (int i = 0; i < colCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    displayedValues[i, j] = "";
                }
            }

            SelectedCell = new int[] { 0, 0 };
            SelectedCellName = "A1";
            SelectedCellContents = "";
            SelectedCellValue = "";
        }

        public event Action NewFileEvent;
        public event Action<string> OpenEvent;
        public event Action SaveEvent;
        public event Action<string> SaveAsEvent;
        public event Action<FormClosingEventArgs> CloseEvent;
        public event Action HelpContentsEvent;
        public event Action<int, int> SetContentsEvent;
        public event Action<int, int> CellSelectionChangedEvent;        

        /// <summary>
        /// Sets the cell at col and row to the given value
        /// </summary>
        public void SetCellValue(int col, int row, string value)
        {
            displayedValues[col, row] = value;
        }

        /// <summary>
        /// Returns the value of the cell at col and row
        /// </summary>
        public string GetCellValue(int col, int row)
        {
            return displayedValues[col, row];
        }

        /// <summary>
        /// Fires the event
        /// </summary>
        public void FireNewFileEvent()
        {
            if (NewFileEvent != null)
            {
                NewFileEvent();
            }
        }

        /// <summary>
        /// Fires the event
        /// </summary>
        public void FireOpenEvent(string fileName)
        {
            if (OpenEvent != null)
            {
                OpenEvent(fileName);
            }
        }

        /// <summary>
        /// Fires the event
        /// </summary>
        public void FireSaveEvent()
        {
            if (SaveEvent != null)
            {
                SaveEvent();
            }
        }

        /// <summary>
        /// Fires the event
        /// </summary>
        public void FireSaveAsEvent(string fileName)
        {
            if (SaveAsEvent != null)
            {
                SaveAsEvent(fileName);
            }
        }

        /// <summary>
        /// Fires the event
        /// </summary>
        public void FireCloseEvent()
        {
            if (CloseEvent != null)
            {
                CloseEvent(new FormClosingEventArgs(CloseReason.UserClosing, false));
            }
        }

        /// <summary>
        /// Fires the event
        /// </summary>
        public void FireHelpContentsEvent()
        {
            if (HelpContentsEvent != null)
            {
                HelpContentsEvent();
            }
        }

        /// <summary>
        /// Fires the event
        /// </summary>
        public void FireSetContentsEvent(int col, int row)
        {
            if (SetContentsEvent != null)
            {
                SetContentsEvent(col, row);
            }
        }

        /// <summary>
        /// Fires the event
        /// </summary>
        public void FireCellSelectionChangedEvent(int col, int row)
        {
            if (CellSelectionChangedEvent != null)
            {
                SelectedCell[0] = col;
                SelectedCell[1] = row;
                CellSelectionChangedEvent(col, row);
            }
        }

        /// <summary>
        /// Sets property to true if called
        /// </summary>
        public void DoNew()
        {
            CalledDoNew = true;
        }

        /// <summary>
        /// Sets property to true if called
        /// </summary>
        public void DoOpen(string path)
        {
            CalledDoOpen = true;
        }

        /// <summary>
        /// Sets property to true if called, then fires a SaveAsEvent
        /// </summary>
        public void DoSaveAs()
        {
            CalledDoSaveAs = true;
            // save the file to the default since stub doesn't have a save as dialog
            FireSaveAsEvent("stub_" + DefaultOpenSaveFileName);
        }

        /// <summary>
        /// Sets property to true if called
        /// </summary>
        public void DoHelpContents()
        {
            CalledHelpContents = true;
        }

        /// <summary>
        /// Sets property to true if called
        /// </summary>
        public void ShowWarningMessage(string message)
        {
            CalledShowErrorMessage = true;
        }

        /// <summary>
        /// Returns the column and row of the currently-selected cell
        /// </summary>
        public void GetSelectedCell(out int col, out int row)
        {
            col = SelectedCell[0];
            row = SelectedCell[1];
        }

        /// <summary>
        /// Sets property to true if called. Simulates a No result.
        /// </summary>
        public DialogResult ShowCloseWarning()
        {
            ShowedCloseWarning = true;
            return DialogResult.No;
        }
    }
}
