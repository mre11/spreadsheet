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
    }    
}
