using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SSGui
{
    [TestClass]
    public class ControllerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }

    /// <summary>
    /// Provides a stub view to test the controller against.
    /// </summary>
    class SSView : ISpreadsheetView
    {
        public string SelectedCellContents
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string SelectedCellName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string SelectedCellValue
        {
            set
            {
                throw new NotImplementedException();
            }
        }

        public string WindowName
        {
            set
            {
                throw new NotImplementedException();
            }
        }

        public event Action<int, int> CellSelectionChangedEvent;
        public event Action CloseEvent;
        public event Action<string> FileChosenEvent;
        public event Action HelpContentsEvent;
        public event Action NewFileEvent;
        public event Action<string> SaveFileEvent;
        public event Action<int, int> SetContentsEvent;

        public void SetCellValue(int col, int row, string value)
        {
            throw new NotImplementedException();
        }
    }
}
