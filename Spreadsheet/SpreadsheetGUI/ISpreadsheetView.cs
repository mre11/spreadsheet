﻿// Created by Morgan Empey for CS 3500, University of Utah, Spring 2015

using System;

namespace SSGui
{
    // Provides a controllable interface for the SpreadsheetWindow
    public interface ISpreadsheetView
    {
        string Title { set; }

        string SelectedCellName { get; set; }

        string SelectedCellValue { set; }

        string SelectedCellContents { get; set; }

        string DefaultOpenSaveFileName { set; }

        event Action NewFileEvent;

        event Action<string> FileChosenEvent;

        event Action SaveEvent;

        event Action<string> SaveAsEvent;

        event Action CloseEvent;

        event Action HelpContentsEvent;

        event Action<int, int> SetContentsEvent;

        event Action<int, int> CellSelectionChangedEvent;

        void SetCellValue(int col, int row, string value);

        void DoNew();

        void DoOpen(string path);

        void DoSaveAs();

        void DoClose();

        void DoHelpContents();

        void ShowErrorMessage(string message, string title);

        void GetSelectedCell(out int col, out int row);        
    }
}
