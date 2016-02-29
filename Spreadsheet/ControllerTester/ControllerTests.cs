// Created by Morgan Empey for CS 3500, University of Utah, Spring 2015

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
        private const string DATA_FOLDER = @"..\..\..\..\TestData\XML\Data\";

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
            var view = new SSView();
            var controller = new Controller(view);

            view.FireFileChosenEvent(DATA_FOLDER + "open1.ss"); // this is an empty spreadsheet

            Assert.AreEqual("open1.ss - Spreadsheet", view.Title);
            Assert.AreEqual("", view.GetCellValue(0, 0));
        }

        /// <summary>
        /// Tests the close event
        /// </summary>
        [TestMethod]
        public void TestOpen2()
        {
            var view = new SSView();
            var controller = new Controller(view);

            view.FireFileChosenEvent(DATA_FOLDER + "open2.ss");

            Assert.AreEqual("open2.ss - Spreadsheet", view.Title);
            Assert.AreEqual("12", view.GetCellValue(0, 0));
            Assert.AreEqual("hello", view.GetCellValue(0, 1));
            Assert.AreEqual("2.54", view.GetCellValue(1, 21));
            Assert.AreEqual("2.54", view.GetCellValue(2, 98));
            Assert.AreEqual("25.4", view.GetCellValue(3, 4));
            Assert.AreEqual("42", view.GetCellValue(25, 16));
        }

        /// <summary>
        /// Tests SelectionChanged event
        /// </summary>
        [TestMethod]
        public void TestSelection1()
        {
            var view = new SSView();
            var controller = new Controller(view);

            view.FireFileChosenEvent(DATA_FOLDER + "open2.ss");
            
            Assert.AreEqual("A1", view.SelectedCellName);
            Assert.AreEqual("12", view.SelectedCellValue);
            Assert.AreEqual("=3*4", view.SelectedCellContents);

            view.FireCellSelectionChangedEvent(1, 21);

            Assert.AreEqual("B22", view.SelectedCellName);
            Assert.AreEqual("2.54", view.SelectedCellValue);
            Assert.AreEqual("2.54", view.SelectedCellContents);

            view.FireCellSelectionChangedEvent(2, 98);

            Assert.AreEqual("C99", view.SelectedCellName);
            Assert.AreEqual("2.54", view.SelectedCellValue);
            Assert.AreEqual("=B22", view.SelectedCellContents);

            view.FireCellSelectionChangedEvent(3, 4);

            Assert.AreEqual("D5", view.SelectedCellName);
            Assert.AreEqual("25.4", view.SelectedCellValue);
            Assert.AreEqual("=B22*10", view.SelectedCellContents);

            view.FireCellSelectionChangedEvent(25, 16);

            Assert.AreEqual("Z17", view.SelectedCellName);
            Assert.AreEqual("42", view.SelectedCellValue);
            Assert.AreEqual("=40+2", view.SelectedCellContents);

            view.FireCellSelectionChangedEvent(0, 1);

            Assert.AreEqual("A2", view.SelectedCellName);
            Assert.AreEqual("hello", view.SelectedCellValue);
            Assert.AreEqual("hello", view.SelectedCellContents);
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

            // TODO figure out how to set boxes when first new or opened
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
    }    
}
