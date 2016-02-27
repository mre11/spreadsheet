using System;

namespace SSGui
{
    // Provides a controllable interface for the SpreadsheetWindow
    public interface ISpreadsheetView
    {
        event Action NewFileEvent;

        event Action<string> FileChosenEvent;

        event Action<string> SaveFileEvent;

        event Action CloseEvent;

        event Action HelpContentsEvent;

        event Action<int, int> SetContentsEvent;

        event Action<int, int> CellSelectionChangedEvent;

        void SetCellValue(int col, int row, string value);

        string WindowName { set; }

        string SelectedCellName { get; set; }

        string SelectedCellValue { set; }

        string SelectedCellContents { get; set; }
    }
}
