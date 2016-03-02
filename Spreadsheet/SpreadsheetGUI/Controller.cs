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
            FileName = "Spreadsheet1.ss";

            // Register event handlers
            view.NewFileEvent += HandleNewEvent;
            view.FileChosenEvent += HandleOpenEvent;
            view.SaveFileEvent += HandleSaveEvent;
            view.CloseEvent += HandleCloseEvent;
            view.HelpContentsEvent += HandleHelpContentsEvent;
            view.CellSelectionChangedEvent += HandleSelectionChangedEvent;
            view.SetContentsEvent += HandleSetContentsEvent;
            
            InitializeView();
        }

        public Controller(ISpreadsheetView window, string path)
            : this(window)
        {
            try
            {
                model = new Spreadsheet(new StreamReader(path));
            }
            catch (Exception e)
            {
                // TODO better error message?
                view.ShowErrorMessage(e.Message, "Error");
            }

            // Set values in view for all non-empty cells
            foreach (string cellName in model.GetNamesOfAllNonemptyCells())
            {
                int col, row;
                GetCellIndicesFromName(cellName, out col, out row);

                string cellValue = model.GetCellValue(cellName).ToString();
                view.SetCellValue(col, row, cellValue);
            }

            FileName = Path.GetFileName(path);

            InitializeView();
        }

        private void InitializeView()
        {
            UpdateSelectedCellView();
            SetTitle();
            view.DefaultOpenSaveFileName = Path.GetFileName(FileName);
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
            view.DoOpen(path);
        }

        /// <summary>
        /// Sets the title of the window according to the FileName of this spreadsheet
        /// </summary>
        private void SetTitle()
        {
            view.Title = FileName + " - Spreadsheet";
        }

        /// <summary>
        /// Handles a save file event
        /// </summary>
        private void HandleSaveEvent(string path)
        {
            // TODO implement a Save in addition to save as?
            if (model.Changed || PathIsDifferent(path))
            {
                model.Save(new StreamWriter(path));
                FileName = Path.GetFileName(path);
                view.DefaultOpenSaveFileName = FileName;
                SetTitle();
            }
        }

        private bool PathIsDifferent(string path)
        {
            return path != Path.GetDirectoryName(path) + "\\" + FileName;
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
            catch (Exception e)
            {
                // TODO better error message?
                view.ShowErrorMessage(e.Message, "Error");
            }

            // Update any cell values in the view that need updating
            UpdateCellValuesInView(cellsToUpdate);
            UpdateSelectedCellView();
        }

        /// <summary>
        /// Returns the row and column of a cell given its name.
        /// </summary>
        private void GetCellIndicesFromName(string name, out int col, out int row)
        {
            col = name[0] - 65;
            int.TryParse(name.Substring(1), out row);
            row--;
        }

        /// <summary>
        /// Updates the cell values in the view from the model. Only updates
        /// cells specified in cellsToUpdate.
        /// </summary>
        private void UpdateCellValuesInView(ISet<string> cellsToUpdate)
        {
            foreach (string name in cellsToUpdate)
            {
                int col, row;
                GetCellIndicesFromName(name, out col, out row);

                string cellValue = model.GetCellValue(name).ToString();
                view.SetCellValue(col, row, cellValue);
            }
        }

        /// <summary>
        /// Sets the selected cell name, value, and contents in the view to
        /// those values for the cell at col and row.
        /// </summary>
        private void HandleSelectionChangedEvent(int col, int row)
        {
            var cellName = GetCellNameFromIndices(col, row);            

            UpdateSelectedCellView();
        }

        /// <summary>
        /// Returns the cell name corresponding to col and row
        /// </summary>
        private string GetCellNameFromIndices(int col, int row)
        {
            char columnChar = (char)(col + 65);
            return columnChar.ToString() + (row + 1);
        }

        /// <summary>
        /// Updates the selected cell contents in the view from the model given the cell's name.
        /// If the contents is a Formula, appends "=" to the beginning of the string.
        /// </summary>
        private void SetSelectedCellContents(string cellName)
        {
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
        /// Updates the selected cell name, value, and contents in the view from the model.
        /// </summary>
        private void UpdateSelectedCellView()
        {
            // Get the selected cell's name
            int col, row;
            view.GetSelectedCell(out col, out row);
            var cellName = GetCellNameFromIndices(col, row);

            // Update the boxes
            view.SelectedCellName = cellName;
            view.SelectedCellValue = model.GetCellValue(cellName).ToString();
            SetSelectedCellContents(cellName);
        }
    }
}
