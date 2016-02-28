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
        [TestMethod]
        public void TestMethod1()
        {
        }

        private Controller InitializeTest()
        {
            return new Controller( new SSView());
        }
    }    
}
