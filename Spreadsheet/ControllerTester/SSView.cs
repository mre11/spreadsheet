// Created by Morgan Empey for CS 3500, University of Utah, Spring 2015

using System;
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
        /// Records whether DoClose was called
        /// </summary>
        public bool CalledDoClose { get; private set; }

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

            SelectedCellName = "A1";
            SelectedCellContents = "";
            SelectedCellValue = "";
        }

        public event Action NewFileEvent;
        public event Action<string> FileChosenEvent;
        public event Action<string> SaveFileEvent;
        public event Action CloseEvent;
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
        public void FireFileChosenEvent(string fileName)
        {
            if (FileChosenEvent != null)
            {
                FileChosenEvent(fileName);
            }
        }

        /// <summary>
        /// Fires the event
        /// </summary>
        public void FireSaveFileEvent(string fileName)
        {
            if (SaveFileEvent != null)
            {
                SaveFileEvent(fileName);
            }
        }

        /// <summary>
        /// Fires the event
        /// </summary>
        public void FireCloseEvent()
        {
            if (CloseEvent != null)
            {
                CloseEvent();
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
                CellSelectionChangedEvent(col, row);
            }
        }

        public void DoNew()
        {
            CalledDoNew = true;
        }

        public void DoClose()
        {
            CalledDoClose = true;
        }

        public void DoHelpContents()
        {
            CalledHelpContents = true;
        }

        public void ShowErrorMessage(string message, string title)
        {
            CalledShowErrorMessage = true;
        }

        public void DoOpen(string path)
        {
            throw new NotImplementedException();
        }

        public void GetSelectedCell(out int col, out int row)
        {
            throw new NotImplementedException();
        }
    }
}
