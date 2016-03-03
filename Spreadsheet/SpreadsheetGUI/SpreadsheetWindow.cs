﻿// Created by Morgan Empey for CS 3500, University of Utah, Spring 2015

using System;
using System.Windows.Forms;

namespace SSGui
{
    /// <summary>
    /// Provides a window to display and manipulate a spreadsheet.
    /// </summary>
    public partial class SpreadsheetWindow : Form, ISpreadsheetView
    {
        /// <summary>
        /// The name in the title bar of the spreadsheet window
        /// </summary>
        public string Title
        {
            set { this.Text = value; }
        }

        /// <summary>
        /// The currently selected cell name, displayed in nameBox
        /// </summary>
        public string SelectedCellName
        {
            get { return nameBox.Text; }

            set { nameBox.Text = value; }
        }

        /// <summary>
        /// The currently selected cell's value, displayed in valueBox
        /// </summary>
        public string SelectedCellValue
        {
            set { valueBox.Text = value; }
        }

        /// <summary>
        /// The currently selected cell's contents, displayed in contentsBox
        /// </summary>
        public string SelectedCellContents
        {
            get { return contentsBox.Text; }

            set { this.contentsBox.Text = value; }
        }

        /// <summary>
        /// The default file name when the open or save as dialogs are shown.
        /// </summary>
        public string DefaultOpenSaveFileName
        {
            set
            {
                openFileDialog.FileName = value;
                saveAsFileDialog.FileName = value;
            }
        }

        /// <summary>
        /// Creates a new spreadsheet window
        /// </summary>
        public SpreadsheetWindow()
        {
            InitializeComponent();

            spreadsheetPanel.SelectionChanged += CellSelectionHandler;
        }

        /// <summary>
        /// Handles the event of a cell being selected
        /// </summary>
        private void CellSelectionHandler(SpreadsheetPanel ssPanel)
        {
            int col, row;
            ssPanel.GetSelection(out col, out row);

            if (CellSelectionChangedEvent != null)
            {
                CellSelectionChangedEvent(col, row);
            }
        }
        
        public event Action NewFileEvent;
        public event Action<string> FileChosenEvent;
        public event Action SaveEvent;
        public event Action<string> SaveAsEvent;
        public event Action<FormClosingEventArgs> CloseEvent;
        public event Action HelpContentsEvent;
        public event Action<int, int> SetContentsEvent;
        public event Action<int, int> CellSelectionChangedEvent;        

        /// <summary>
        /// Event handler for New
        /// </summary>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NewFileEvent != null)
            {
                NewFileEvent();
            }
        }

        /// <summary>
        /// Event handler for Open
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (FileChosenEvent != null)
                {
                    FileChosenEvent(openFileDialog.FileName);
                }
            }
        }

        /// <summary>
        /// Event handler for Save
        /// </summary>
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveEvent();
        }

        /// <summary>
        /// Event handler for Save As
        /// </summary>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoSaveAs();
        }

        /// <summary>
        /// Event handler for Close
        /// </summary>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Event handler for Help >> Contents
        /// </summary>
        private void helpContentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (HelpContentsEvent != null)
            {
                HelpContentsEvent();
            }
        }

        /// <summary>
        /// Sets the cell in the spreadsheet panel at col and row to the specified value
        /// </summary>
        public void SetCellValue(int col, int row, string value)
        {
            spreadsheetPanel.SetValue(col, row, value);
        }

        /// <summary>
        /// Event handler for the enter key being pressed while contentsBox has focus
        /// </summary>
        private void contentsBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return) // ENTER key is pressed
            {
                FireSetContentsEvent();
                
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event handler for set button being clicked
        /// </summary>
        private void setButton_Click(object sender, EventArgs e)
        {
            FireSetContentsEvent();
        }

        /// <summary>
        /// Fires the set contents event for the selected cell
        /// </summary>
        private void FireSetContentsEvent()
        {
            int col, row;
            spreadsheetPanel.GetSelection(out col, out row);

            if (SetContentsEvent != null)
            {
                SetContentsEvent(col, row);
            }
        }

        /// <summary>
        /// Opens a new spreadsheet window
        /// </summary>
        public void DoNew()
        {
            SpreadsheetApplicationContext.GetContext().RunNew();
        }

        /// <summary>
        /// Opens a new spreadsheet window for the file at path
        /// </summary>
        public void DoOpen(string path)
        {
            SpreadsheetApplicationContext.GetContext().RunOpen(path);
        }

        /// <summary>
        /// Brings up a SaveAs dialog
        /// </summary>
        public void DoSaveAs()
        {
            DialogResult result = saveAsFileDialog.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (SaveAsEvent != null)
                {
                    SaveAsEvent(saveAsFileDialog.FileName);
                }
            }
        }

        /// <summary>
        /// Displays the help contents
        /// </summary>
        public void DoHelpContents()
        {
            new HelpWindow().Show();
        }

        /// <summary>
        /// Shows an error message
        /// </summary>
        public void ShowErrorMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Sets col and row to the currently selected cell's coordinates.
        /// </summary>
        public void GetSelectedCell(out int col, out int row)
        {
            spreadsheetPanel.GetSelection(out col, out row);            
        }

        private void SpreadsheetWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CloseEvent != null)
            {
                CloseEvent(e);
            }
        }

        public DialogResult ShowCloseWarning()
        {
            return MessageBox.Show("Unsaved changes. Are you sure you want to close?", "Spreadsheet", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }
    }
}
