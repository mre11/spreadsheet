using System;

namespace SSGUI
{
    // Provides a controllable interface for the SpreadsheetWindow
    public interface ISpreadsheetView
    {
        event Action NewFileEvent;

        event Action<string> FileChosenEvent;

        event Action<string> SaveFileEvent;

        event Action CloseEvent;

        event Action HelpContentsEvent;

        event Action<string> SetContentsEvent;

        event Action<string> CellSelectionChangedEvent;

        void SetCellValue(int col, int row, string value);

        string WindowName { set; }

        string SelectedCellName { get; set; }

        string SelectedCellValue { set; }

        string SelectedCellContents { get; set; }
    }
}
