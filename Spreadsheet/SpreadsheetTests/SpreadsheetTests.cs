using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;

namespace SS
{
    /// <summary>
    /// Provides test cases for the Spreadsheet class and Cell inner class.
    /// </summary>
    [TestClass]
    public class SpreadsheetTests
    {
        /// <summary>
        /// Tests method for an empty spreadsheet
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells1()
        {
            var ss = new Spreadsheet();
            var list = ss.GetNamesOfAllNonemptyCells().ToList();

            Assert.AreEqual(0, list.Count);
        }

        /// <summary>
        /// Tests method for a non-empty spreadsheet
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells2()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("a1", "hello");
            ss.SetContentsOfCell("a2", "what");
            ss.SetContentsOfCell("a3", "42");
            var list = ss.GetNamesOfAllNonemptyCells().ToList();

            Assert.AreEqual(3, list.Count);
            Assert.IsTrue(list.Contains("A1"));
            Assert.IsTrue(list.Contains("A2"));
            Assert.IsTrue(list.Contains("A3"));
        }

        /// <summary>
        /// Tests method when parameter is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContents1()
        {
            var ss = new Spreadsheet();
            ss.GetCellContents(null);
        }

        /// <summary>
        /// Tests method when parameter is not a valid cell name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContents2()
        {
            var ss = new Spreadsheet();
            ss.GetCellContents("a01");
        }

        /// <summary>
        /// Tests method for an empty cell
        /// </summary>
        [TestMethod]
        public void TestGetCellContents3()
        {
            var ss = new Spreadsheet();
            Assert.AreEqual("", ss.GetCellContents("a1"));
        }

        /// <summary>
        /// Tests method when the cell contains a string, double, or Formula.
        /// Test ensures that cell name is not case-sensitive.
        /// </summary>
        [TestMethod]
        public void TestGetCellContents4()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("a1", "hello");
            ss.SetContentsOfCell("a2", "0.55");
            ss.SetContentsOfCell("a3", "=a2");
            ss.SetContentsOfCell("a4", "=3 + 4");

            Assert.AreEqual("hello", ss.GetCellContents("a1"));
            Assert.AreEqual(0.55, ss.GetCellContents("A2"));
            Assert.AreEqual("A2", ss.GetCellContents("a3").ToString());
            Assert.AreEqual("3+4", ss.GetCellContents("A4").ToString());
        }

        /// <summary>
        /// Tests method when text is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetCellContents1()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("a1", null);
        }

        /// <summary>
        /// Tests method when name is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContents2()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell(null, "hello");
        }

        /// <summary>
        /// Tests method when name is invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContents3()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("a01", "hello");
        }

        /// <summary>
        /// Tests method for a circular reference
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSetCellContents4()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("a3", "=a4 + 11");
            ss.SetContentsOfCell("A4", "=a3 + 5");
        }

        /// <summary>
        /// Tests method for a circular reference exception
        /// </summary>
        [TestMethod]
        public void TestSetCellContents5()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("a1", "hello");

            Assert.AreEqual("hello", ss.GetCellContents("a1"));

            ss.SetContentsOfCell("A2", "11");

            Assert.AreEqual(11d, ss.GetCellContents("a2"));

            var set1 = ss.SetContentsOfCell("a3", "=a4 + 11");
            var set2 = ss.SetContentsOfCell("A4", "=a2 + 5");
            var set3 = ss.SetContentsOfCell("a5", "=a2 + a3 + a4");

            Assert.AreEqual(1, set1.Count);
            Assert.IsTrue(set1.Contains("A3"));

            Assert.AreEqual(2, set2.Count);
            Assert.IsTrue(set2.Contains("A3"));
            Assert.IsTrue(set2.Contains("A4"));

            Assert.AreEqual(1, set3.Count);
            Assert.IsTrue(set3.Contains("A5"));
        }

        /// <summary>
        /// Tests method for changing existing cell contents
        /// </summary>
        [TestMethod]
        public void TestSetCellContents6()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("a3", "=a4 + 11");
            ss.SetContentsOfCell("a3", "hello");

            Assert.AreEqual("hello", ss.GetCellContents("A3"));
        }        

        /// <summary>
        /// Tests method for null parameter
        /// </summary>
        [TestMethod]
        public void TestGetDirectDependents3()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("a1", "=a2");
            ss.SetContentsOfCell("a2", "hello world");

            PrivateObject ssAccessor = new PrivateObject(ss);

            object[] parameters = { "a2" };
            List<string> result = ((IEnumerable<string>) ssAccessor.Invoke("GetDirectDependents", parameters)).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Contains("A1"));
        }

        /// <summary>
        /// Returns the upper-case version of s
        /// </summary>
        public string UpperCaseNormalizer(string s)
        {
            return s.ToUpper();
        }
    }
}
