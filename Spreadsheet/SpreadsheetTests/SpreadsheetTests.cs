using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace SS
{
    /// <summary>
    /// Provides test cases for the Spreadsheet class and Cell inner class.
    /// </summary>
    [TestClass]
    public class SpreadsheetTests
    {
        // TODO these folders need to be relative paths!!!
        // Folder containing XML test baselines
        private const string BASELINE_FOLDER = @"E:\Morgan\Development\cs3500\spreadsheet\Spreadsheet\TestData\XML\Baselines\";

        // Folder for XML test output
        private const string OUTPUT_FOLDER = @"E:\Morgan\Development\cs3500\spreadsheet\Spreadsheet\TestData\XML\Output\";

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
            var ss = new Spreadsheet(new Regex(@"^[a-zA-z]+[1-9]+\d*$"));
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
            var ss = new Spreadsheet(new Regex(@"^[a-zA-z]+[1-9]+\d*$"));
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
        /// Tests method for a null name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellValue1()
        {
            var ss = new Spreadsheet(new Regex(@"^[a-zA-z]+[1-9]+\d*$"));
            ss.GetCellValue(null);
        }

        /// <summary>
        /// Tests method for an invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellValue2()
        {
            var ss = new Spreadsheet(new Regex(@"^[a-zA-z]+[1-9]+\d*$"));
            ss.GetCellValue("AA23B");
        }

        /// <summary>
        /// Tests method for an empty cell
        /// </summary>
        [TestMethod]
        public void TestGetCellValue3()
        {
            var ss = new Spreadsheet();
            Assert.AreEqual("", ss.GetCellValue("A42"));
        }

        /// <summary>
        /// Tests method for a cell containing a string
        /// </summary>
        [TestMethod]
        public void TestGetCellValue4()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "hello");
            Assert.AreEqual("hello", ss.GetCellValue("A1"));
        }

        /// <summary>
        /// Tests method for a cell containing a double
        /// </summary>
        [TestMethod]
        public void TestGetCellValue5()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "2.54");
            Assert.AreEqual(2.54, ss.GetCellValue("A1"));
        }

        /// <summary>
        /// Tests method for a cell containing a valid Formula
        /// </summary>
        [TestMethod]
        public void TestGetCellValue6()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "2.54");
            ss.SetContentsOfCell("B52", "=A1*3");
            Assert.AreEqual(2.54*3, ss.GetCellValue("B52"));
        }

        /// <summary>
        /// Tests method for a cell containing a bad Formula
        /// </summary>
        [TestMethod]
        public void TestGetCellValue7()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "hello, world");
            ss.SetContentsOfCell("B2", "=A1*3");
            Assert.IsTrue(ss.GetCellValue("B2").GetType() == typeof(FormulaError));
        }

        /// <summary>
        /// Tests method for a cell containing a bad Formula (reference to an empty cell)
        /// </summary>
        [TestMethod]
        public void TestGetCellValue8()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("B2", "=A1*3");
            Assert.IsTrue(ss.GetCellValue("B2").GetType() == typeof(FormulaError));
        }

        /// <summary>
        /// Tests method for a cell containing a bad Formula (reference to an empty cell)
        /// </summary>
        [TestMethod]
        public void TestGetCellValue9()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "");
            ss.SetContentsOfCell("B2", "=A1*3");
            Assert.IsTrue(ss.GetCellValue("B2").GetType() == typeof(FormulaError));
        }

        /// <summary>
        /// Tests method for a cell containing a bad Formula (divide by zero)
        /// </summary>
        [TestMethod]
        public void TestGetCellValue10()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "0");
            ss.SetContentsOfCell("B2", "=2.54/A1");
            Assert.IsTrue(ss.GetCellValue("B2").GetType() == typeof(FormulaError));
        }

        /// <summary>
        /// Save an empty spreadsheet with no IsValid
        /// </summary>
        [TestMethod]
        public void TestSave1()
        {
            var ss = new Spreadsheet();

            var baseFileName = "save1";
            var outputWriter = new StreamWriter(OUTPUT_FOLDER + baseFileName + ".txt");
            ss.Save(outputWriter);
            outputWriter.Close();

            CompareTestFiles(baseFileName);
        }

        /// <summary>
        /// Save an empty spreadsheet with IsValid
        /// </summary>
        [TestMethod]
        public void TestSave2()
        {
            var ss = new Spreadsheet(new Regex(@"^[a-zA-z]+[1-9]+\d*$"));

            var baseFileName = "save2";
            var outputWriter = new StreamWriter(OUTPUT_FOLDER + baseFileName + ".txt");
            ss.Save(outputWriter);
            outputWriter.Close();

            CompareTestFiles(baseFileName);
        }

        /// <summary>
        /// Save an empty spreadsheet with IsValid
        /// </summary>
        [TestMethod]
        public void TestSave3()
        {
            var ss = new Spreadsheet(new Regex(@"^[a-zA-z]+[1-9]+\d*$"));
            ss.SetContentsOfCell("A1", "hello");
            ss.SetContentsOfCell("B22", "2.54");
            ss.SetContentsOfCell("C100001", "=A1");
            ss.SetContentsOfCell("D555", "=B22*10");

            var baseFileName = "save3";
            var outputWriter = new StreamWriter(OUTPUT_FOLDER + baseFileName + ".txt");
            ss.Save(outputWriter);
            outputWriter.Close();

            CompareTestFiles(baseFileName);
        }

        /// <summary>
        /// Asserts that the test output is equal to the baseline.
        /// The test output is assumed to be "baseFileName".txt
        /// The test baseline is assumed to be "baseFileName"_base.txt
        /// </summary>
        private void CompareTestFiles(string baseFileName)
        {
            var baselineReader = new StreamReader(BASELINE_FOLDER + baseFileName + "_base.txt");
            var outputReader = new StreamReader(OUTPUT_FOLDER + baseFileName + ".txt");

            string baseline = baselineReader.ReadToEnd();
            string output = outputReader.ReadToEnd();

            Assert.AreEqual(baseline, output);
        }

        // TODO write tests for Spreadsheet TextReader constructor

        /// <summary>
        /// Returns the upper-case version of s
        /// </summary>
        public string UpperCaseNormalizer(string s)
        {
            return s.ToUpper();
        }
    }
}
