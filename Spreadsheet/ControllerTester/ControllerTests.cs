// Created by Morgan Empey for CS 3500, University of Utah, Spring 2015

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        /// Tests the close event
        /// </summary>
        [TestMethod]
        public void TestClose1()
        {
            var view = new SSView();
            var controller = new Controller(view);

            view.FireCloseEvent();

            Assert.IsTrue(view.CalledDoClose);
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
        /// Tests the close event
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

        // TODO add tests to increase code coverage!



        /// <summary>
        /// Mimics how the real view opens a file in a new window.
        /// </summary>
        private void OpenFile(string path, SSView oldView, out SSView newView)
        {
            oldView.FireFileChosenEvent(path);

            newView = new SSView();
            var newController = new Controller(newView, path);
        }
    }    
}
