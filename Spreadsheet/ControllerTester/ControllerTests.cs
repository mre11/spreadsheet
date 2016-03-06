// Created by Morgan Empey for CS 3500, University of Utah, Spring 2015

using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using SSGui;

namespace SSControllerTester
{
    /// <summary>
    /// Provides test cases for the Controller class
    /// </summary>
    [TestClass]
    public class ControllerTests
    {
        // Folder containing XML test data for reading
        private const string DATA_FOLDER = @"..\..\..\TestData\XML\Data\";

        /// <summary>
        /// Tests the new event
        /// </summary>
        [TestMethod]
        public void TestNew1()
        {
            var view = new SSView();
            var controller = new Controller(view);

            view.FireNewFileEvent();

            Assert.IsTrue(view.CalledDoNew);
        }

        /// <summary>
        /// Tests the close event for an unchanged file
        /// </summary>
        [TestMethod]
        public void TestClose1()
        {
            var view = new SSView();
            var controller = new Controller(view);

            view.FireCloseEvent();

            Assert.IsFalse(view.ShowedCloseWarning);
        }

        /// <summary>
        /// Tests the close event for a changed file
        /// </summary>
        [TestMethod]
        public void TestClose2()
        {
            var view = new SSView();
            var controller = new Controller(view);

            view.SelectedCellContents = "hello";
            view.FireSetContentsEvent(0, 0);

            view.FireCloseEvent();

            Assert.IsTrue(view.ShowedCloseWarning);
        }

        /// <summary>
        /// Tests the close event
        /// </summary>
        [TestMethod]
        public void TestHelpContents1()
        {
            var view = new SSView();
            var controller = new Controller(view);

            view.FireHelpContentsEvent();

            Assert.IsTrue(view.CalledHelpContents);
        }

        /// <summary>
        /// Tests the open event for an empty spreadsheet
        /// </summary>
        [TestMethod]
        public void TestOpen1()
        {
            var initialView = new SSView();
            SSView openedFileView;
            var controller = new Controller(initialView);

            OpenFile(DATA_FOLDER + "open1.ss", initialView, out openedFileView); // this is an empty spreadsheet

            Assert.AreEqual("open1.ss - Spreadsheet", openedFileView.Title);
            Assert.AreEqual("", openedFileView.GetCellValue(0, 0));
        }

        /// <summary>
        /// Tests the open event for an un-empty spreadsheet file
        /// </summary>
        [TestMethod]
        public void TestOpen2()
        {
            var initialView = new SSView();
            SSView openedFileView;
            var controller = new Controller(initialView);
            
            OpenFile(DATA_FOLDER + "open2.ss", initialView, out openedFileView);

            Assert.AreEqual("open2.ss - Spreadsheet", openedFileView.Title);
            Assert.AreEqual("12", openedFileView.GetCellValue(0, 0));
            Assert.AreEqual("hello", openedFileView.GetCellValue(0, 1));
            Assert.AreEqual("2.54", openedFileView.GetCellValue(1, 21));
            Assert.AreEqual("2.54", openedFileView.GetCellValue(2, 98));
            Assert.AreEqual("25.4", openedFileView.GetCellValue(3, 4));
            Assert.AreEqual("42", openedFileView.GetCellValue(25, 16));
        }

        /// <summary>
        /// Tests the open event for a corrupt file
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestOpen3()
        {
            var initialView = new SSView();
            SSView openedFileView;
            var controller = new Controller(initialView);

            OpenFile(DATA_FOLDER + "read5.xml", initialView, out openedFileView);

            Assert.IsTrue(initialView.CalledDoOpen);
            Assert.IsTrue(openedFileView.CalledShowErrorMessage);
        }


        /// <summary>
        /// Tests SelectionChanged event
        /// </summary>
        [TestMethod]
        public void TestSelection1()
        {
            var initialView = new SSView();
            SSView openedFileView;
            var controller = new Controller(initialView);

            OpenFile(DATA_FOLDER + "open2.ss", initialView, out openedFileView);
            
            Assert.AreEqual("A1", openedFileView.SelectedCellName);
            Assert.AreEqual("12", openedFileView.SelectedCellValue);
            Assert.AreEqual("=3*4", openedFileView.SelectedCellContents);

            openedFileView.FireCellSelectionChangedEvent(1, 21);

            Assert.AreEqual("B22", openedFileView.SelectedCellName);
            Assert.AreEqual("2.54", openedFileView.SelectedCellValue);
            Assert.AreEqual("2.54", openedFileView.SelectedCellContents);

            openedFileView.FireCellSelectionChangedEvent(2, 98);

            Assert.AreEqual("C99", openedFileView.SelectedCellName);
            Assert.AreEqual("2.54", openedFileView.SelectedCellValue);
            Assert.AreEqual("=B22", openedFileView.SelectedCellContents);

            openedFileView.FireCellSelectionChangedEvent(3, 4);

            Assert.AreEqual("D5", openedFileView.SelectedCellName);
            Assert.AreEqual("25.4", openedFileView.SelectedCellValue);
            Assert.AreEqual("=B22*10", openedFileView.SelectedCellContents);

            openedFileView.FireCellSelectionChangedEvent(25, 16);

            Assert.AreEqual("Z17", openedFileView.SelectedCellName);
            Assert.AreEqual("42", openedFileView.SelectedCellValue);
            Assert.AreEqual("=40+2", openedFileView.SelectedCellContents);

            openedFileView.FireCellSelectionChangedEvent(0, 1);

            Assert.AreEqual("A2", openedFileView.SelectedCellName);
            Assert.AreEqual("hello", openedFileView.SelectedCellValue);
            Assert.AreEqual("hello", openedFileView.SelectedCellContents);
        }

        /// <summary>
        /// Tests the set contents event (simple)
        /// </summary>
        [TestMethod]
        public void TestSetContents1()
        {
            var view = new SSView();
            var controller = new Controller(view);

            view.SelectedCellContents = "hello";
            view.FireSetContentsEvent(0, 0);
  
            Assert.AreEqual("hello", view.SelectedCellValue);
            Assert.AreEqual("hello", view.GetCellValue(0, 0));
        }

        /// <summary>
        /// Tests the set contents event (update referenced cell)
        /// </summary>
        [TestMethod]
        public void TestSetContents2()
        {
            var view = new SSView();
            var controller = new Controller(view);

            view.FireCellSelectionChangedEvent(0, 0);
            view.SelectedCellContents = "11";
            view.FireSetContentsEvent(0, 0);

            Assert.AreEqual("11", view.SelectedCellValue);
            Assert.AreEqual("11", view.GetCellValue(0, 0));

            view.FireCellSelectionChangedEvent(2, 2);
            view.SelectedCellContents = "=a1*2";
            view.FireSetContentsEvent(2, 2);

            Assert.AreEqual("22", view.SelectedCellValue);
            Assert.AreEqual("22", view.GetCellValue(2, 2));

            view.FireCellSelectionChangedEvent(0, 0);
            view.SelectedCellContents = "10";
            view.FireSetContentsEvent(0, 0);

            Assert.AreEqual("10", view.SelectedCellValue);
            Assert.AreEqual("10", view.GetCellValue(0, 0));
            Assert.AreEqual("20", view.GetCellValue(2, 2));
        }

        /// <summary>
        /// Tests the set contents event (error message)
        /// </summary>
        [TestMethod]
        public void TestSetContents3()
        {
            var view = new SSView();
            var controller = new Controller(view);

            view.FireCellSelectionChangedEvent(0, 0);
            view.SelectedCellContents = "=A100";
            view.FireSetContentsEvent(0, 0);

            Assert.IsTrue(view.CalledShowErrorMessage);            
        }

        /// <summary>
        /// Tests the save feature on a new spreadsheet
        /// </summary>
        [TestMethod]
        public void TestControllerSave1()
        {
            var view = new SSView();
            var controller = new Controller(view);

            view.FireSaveEvent();

            Assert.IsTrue(view.CalledDoSaveAs);
        }

        /// <summary>
        /// Open file, then save without changing it
        /// </summary>
        [TestMethod]
        public void TestControllerSave2()
        {
            var initialView = new SSView();
            SSView openedFileView;
            var controller = new Controller(initialView);

            OpenFile(DATA_FOLDER + "open2.ss", initialView, out openedFileView);

            openedFileView.FireSaveEvent();

            Assert.IsFalse(openedFileView.CalledDoSaveAs);
        }

        /// <summary>
        /// Open file, then save after changing it
        /// </summary>
        [TestMethod]
        public void TestControllerSave3()
        {
            var initialView = new SSView();
            SSView openedFileView;
            var controller = new Controller(initialView);

            OpenFile(DATA_FOLDER + "open2.ss", initialView, out openedFileView);

            var previousSavedTime = File.GetLastWriteTime(DATA_FOLDER + "open2.ss");

            // Change a cell's contents
            openedFileView.FireCellSelectionChangedEvent(20, 80);
            openedFileView.SelectedCellContents = "55";
            openedFileView.FireSetContentsEvent(20, 80);

            // Change it back to preserve the original file
            openedFileView.FireCellSelectionChangedEvent(20, 80);
            openedFileView.SelectedCellContents = "";
            openedFileView.FireSetContentsEvent(20, 80);

            openedFileView.FireSaveEvent();

            var currentSavedTime = File.GetLastWriteTime(DATA_FOLDER + "open2.ss");

            Assert.IsFalse(openedFileView.CalledDoSaveAs);
            Assert.AreNotEqual(previousSavedTime, currentSavedTime); // the file should be newly saved
        }

        /// <summary>
        /// Open file, then save as after changing it
        /// </summary>
        [TestMethod]
        public void TestControllerSaveAs1()
        {
            var initialView = new SSView();
            SSView openedFileView;
            var controller = new Controller(initialView);
            var filePath = DATA_FOLDER + "open2.ss";

            OpenFile(filePath, initialView, out openedFileView);

            var previousSavedTime = File.GetLastWriteTime(filePath);

            // Change a cell's contents
            openedFileView.FireCellSelectionChangedEvent(20, 80);
            openedFileView.SelectedCellContents = "55";
            openedFileView.FireSetContentsEvent(20, 80);

            // Change it back to preserve the original file
            openedFileView.FireCellSelectionChangedEvent(20, 80);
            openedFileView.SelectedCellContents = "";
            openedFileView.FireSetContentsEvent(20, 80);

            openedFileView.FireSaveAsEvent(filePath);

            var currentSavedTime = File.GetLastWriteTime(filePath);

            Assert.IsFalse(openedFileView.CalledDoSaveAs);
            Assert.AreNotEqual(previousSavedTime, currentSavedTime); // the file should be newly saved
        }

        /// <summary>
        /// Test for an error on saving
        /// </summary>
        [TestMethod]
        public void TestControllerSaveAs2()
        {
            var initialView = new SSView();
            SSView openedFileView;
            var controller = new Controller(initialView);
            var filePath = DATA_FOLDER + "open2.ss";

            OpenFile(filePath, initialView, out openedFileView);

            // Change a cell's contents
            openedFileView.FireCellSelectionChangedEvent(20, 80);
            openedFileView.SelectedCellContents = "55";
            openedFileView.FireSetContentsEvent(20, 80);

            openedFileView.FireSaveAsEvent(@"Z:\doesNotExist\test.ss");
            
            Assert.IsTrue(openedFileView.CalledShowErrorMessage);            
        }

        // TODO test that exceptions are handled

        /// <summary>
        /// Mimics how the real view opens a file in a new window.
        /// </summary>
        private void OpenFile(string path, SSView oldView, out SSView newView)
        {
            oldView.FireOpenEvent(path);

            newView = new SSView();
            var newController = new Controller(newView, path);
        }
    }    
}
