// Created by Morgan Empey for CS 3500, University of Utah, Spring 2015

using System;
using System.Windows.Forms;

namespace SSGui
{
    // Provides a controllable interface for the SpreadsheetWindow
    public interface ISpreadsheetView
    {
        /// <summary>
        /// Represents the title of the window
        /// </summary>
        string Title { set; }

        /// <summary>
        /// Represents the displayed name
        /// </summary>
        string SelectedCellName { get; set; }

        /// <summary>
        /// Represents the displayed value
        /// </summary>
        string SelectedCellValue { set; }

        /// <summary>
        /// Represents the displayed contents
        /// </summary>
        string SelectedCellContents { get; set; }

        /// <summary>
        /// Represents the default file name that shows up in the Open and Save As file dialogs
        /// </summary>
        string DefaultOpenSaveFileName { set; }

        /// <summary>
        /// Fired when the user selects New
        /// </summary>
        event Action NewFileEvent;

        /// <summary>
        /// Fired when the user selects a file in the Open dialog.
        /// The parameter is the full file name.
        /// </summary>
        event Action<string> OpenEvent;

        /// <summary>
        /// Fired when the user selects Save
        /// </summary>
        event Action SaveEvent;

        /// <summary>
        /// Fired when the user selects a file in the Save As dialog.
        /// The parameter is the full file name
        /// </summary>
        event Action<string> SaveAsEvent;

        /// <summary>
        /// Fired when the window is closed. The parameter gives information on why
        /// the window is closing.
        /// </summary>
        event Action<FormClosingEventArgs> CloseEvent;

        /// <summary>
        /// Fired when the user selects the help contents
        /// </summary>
        event Action HelpContentsEvent;

        /// <summary>
        /// Fired when the user attempts to set the contents of the selected cell.
        /// The parameters are the column and row of the cell.
        /// </summary>
        event Action<int, int> SetContentsEvent;

        /// <summary>
        /// Fired when the user selects a different cell. The parameters are the column
        /// and row of the newly-selected cell.
        /// </summary>
        event Action<int, int> CellSelectionChangedEvent;

        /// <summary>
        /// Sets the value displayed in the given cell in the window
        /// </summary>
        void SetCellValue(int col, int row, string value);

        /// <summary>
        /// Opens a new window
        /// </summary>
        void DoNew();

        /// <summary>
        /// Opens a new window for the opened file
        /// </summary>
        void DoOpen(string path);

        /// <summary>
        /// Displays a Save As dialog
        /// </summary>
        void DoSaveAs();

        /// <summary>
        /// Displays a warning if closing the window would result in loss of unsaved data
        /// </summary>
        DialogResult ShowCloseWarning();

        /// <summary>
        /// Displays the help contents window
        /// </summary>
        void DoHelpContents();

        /// <summary>
        /// Displays warning message
        /// </summary>
        void ShowWarningMessage(string message);

        /// <summary>
        /// Returns the column and row of the currently selected cell.
        /// </summary>
        void GetSelectedCell(out int col, out int row);
        
    }
}
