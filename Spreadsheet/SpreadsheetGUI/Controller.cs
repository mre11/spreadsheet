// Created by Morgan Empey for CS 3500, University of Utah, Spring 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;

namespace SSGui
{
    /// <summary>
    /// Controls a Spreadsheet window
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// The GUI for a spreadsheet window
        /// </summary>
        private ISpreadsheetView view;

        /// <summary>
        /// The representation for a single spreadsheet
        /// </summary>
        private Spreadsheet model;

        /// <summary>
        /// The file name of this spreadsheet
        /// </summary>
        private string FileName { get; set; }

        /// <summary>
        /// Creates a new controller for the spreadsheet window
        /// </summary>
        public Controller(ISpreadsheetView window)
        {
            
        }

        /// <summary>
        /// Handles a new file event
        /// </summary>
        private void HandleNewEvent()
        {

        }

        /// <summary>
        /// Handles an open file event
        /// </summary>
        private void HandleOpenEvent(string fileName)
        {

        }

        /// <summary>
        /// Handles a save file event
        /// </summary>
        private void HandleSaveEvent(string fileName)
        {

        }

        /// <summary>
        /// Handles a close window event
        /// </summary>
        private void HandleCloseEvent()
        {

        }

        /// <summary>
        /// Handles a request for help contents
        /// </summary>
        private void HandleHelpContentsEvent()
        {

        }

        /// <summary>
        /// Handles the event of setting the contents of a cell.
        /// Parameters are the column and row of the cell.
        /// </summary>
        private void HandleSetContentsEvent(int col, int row)
        {

        }

        /// <summary>
        /// Handles the event of changing the selected cell.
        /// Parameters are the column and row of the newly-selected cell.
        /// </summary>
        private void HandleSelectionChangedEvent(int col, int row)
        {

        }
    }
}
