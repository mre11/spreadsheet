// Created by Morgan Empey for CS 3500, University of Utah, Spring 2015

using System.Text.RegularExpressions;
using SS;
using System.IO;
using Formulas;
using System.Collections.Generic;
using System;

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
            model = new Spreadsheet(new Regex(@"^[a-zA-z]+[1-9]+\d*$"));
            view = window;

            // Register event handlers
            view.NewFileEvent += HandleNewEvent;
            view.FileChosenEvent += HandleOpenEvent;
            view.SaveFileEvent += HandleSaveEvent;
            view.CloseEvent += HandleCloseEvent;
            view.HelpContentsEvent += HandleHelpContentsEvent;
            view.CellSelectionChangedEvent += HandleSelectionChangedEvent;
            view.SetContentsEvent += HandleSetContentsEvent;
            
            // TODO initialize boxes here?            
        }

        /// <summary>
        /// Handles a new file event
        /// </summary>
        private void HandleNewEvent()
        {
            view.DoNew();
        }

        /// <summary>
        /// Handles an open file event
        /// </summary>
        private void HandleOpenEvent(string path)
        {
            // TODO handle open event (incomplete) also need try catch
            model = new Spreadsheet(new StreamReader(path));

            foreach(string cellName in model.GetNamesOfAllNonemptyCells())
            {
                int col, row;
                GetCellIndicesFromName(cellName, out col, out row);
                view.SetCellValue(col, row, model.GetCellValue(cellName).ToString());
            }

            FileName = Path.GetFileName(path);
            SetTitle(path);

            // When the file is opened, set the name, value, and contents boxes
            HandleSelectionChangedEvent(0, 0);
        }

        /// <summary>
        /// Sets the title of the window given a full file path
        /// </summary>
        private void SetTitle(string path)
        {
            view.Title = Path.GetFileName(path) + " - Spreadsheet";
        }

        /// <summary>
        /// Handles a save file event
        /// </summary>
        private void HandleSaveEvent(string fileName)
        {
            // TODO handle save event (incomplete, need logic)
            model.Save(new StreamWriter(fileName));
        }

        /// <summary>
        /// Handles a close window event
        /// </summary>
        private void HandleCloseEvent()
        {
            view.DoClose();
        }

        /// <summary>
        /// Handles a request for help contents
        /// </summary>
        private void HandleHelpContentsEvent()
        {
            view.DoHelpContents();
        }

        /// <summary>
        /// Handles the event of setting the contents of a cell.
        /// Parameters are the column and row of the cell being set.
        /// </summary>
        private void HandleSetContentsEvent(int col, int row)
        {
            var cellName = GetCellNameFromIndices(col, row);
            ISet<string> cellsToUpdate = new HashSet<string>();

            try
            {
                cellsToUpdate = model.SetContentsOfCell(cellName, view.SelectedCellContents);
            }
            catch
            {
                // TODO catch stuff in set contents event
            }

            // Update any cell values in the view that need updating
            foreach (string name in cellsToUpdate)
            {
                int cellCol, cellRow;
                GetCellIndicesFromName(name, out cellCol, out cellRow);

                string cellValue = model.GetCellValue(name).ToString();

                view.SetCellValue(cellCol, cellRow, cellValue);
            }
        }

        private void GetCellIndicesFromName(string name, out int col, out int row)
        {
            col = name[0] - 65;
            int.TryParse(name.Substring(1), out row);
            row--;
        }

        /// <summary>
        /// Handles the event of changing the selected cell.
        /// Parameters are the column and row of the newly-selected cell.
        /// </summary>
        private void HandleSelectionChangedEvent(int col, int row)
        {
            var cellName = GetCellNameFromIndices(col, row);

            view.SelectedCellName = cellName;
            view.SelectedCellValue = model.GetCellValue(cellName).ToString();

            object cellContents = model.GetCellContents(cellName);
            string stringCellContents;
            
            // Append "=" if the contents is a formula
            if (cellContents.GetType() == typeof(Formula))
            {
                Formula formulaCellContents = (Formula)cellContents;
                stringCellContents = "=" + formulaCellContents.ToString();
            }
            else
            {
                stringCellContents = cellContents.ToString();
            }

            view.SelectedCellContents = stringCellContents;
        }

        /// <summary>
        /// Returns the cell name corresponding to col and row
        /// </summary>
        private string GetCellNameFromIndices(int col, int row)
        {
            char columnChar = (char)(col + 65);
            return columnChar.ToString() + (row + 1);
        }
    }
}
