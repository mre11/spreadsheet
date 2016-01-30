// Written by Morgan Empey for CS 3500, University of Utah, Spring 2016

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;
using System.Linq;

namespace DependencyGraphTestCases
{
    /// <summary>
    /// Provides unit test cases for the DependencyGraph class
    /// </summary>
    [TestClass]
    public class DependencyGraphTests
    {
        /// <summary>
        /// Tests Size when dg is empty.
        /// </summary>
        [TestMethod]
        public void TestSize1()
        {
            var dg = new DependencyGraph();
            Assert.AreEqual(0, dg.Size);
        }

        /// <summary>
        /// Tests Size when dg is small.
        /// </summary>
        [TestMethod]
        public void TestSize2()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("a1", "a2");
            dg.AddDependency("a1", "a3");
            Assert.AreEqual(2, dg.Size);
        }

        /// <summary>
        /// Tests Size when dg is moderately sized.
        /// </summary>
        [TestMethod]
        public void TestSize3()
        {
            var dg = new DependencyGraph();

            for (var i = 0; i < 1000; i++)
            {
                dg.AddDependency("a" + i, "b" + i);
            }

            Assert.AreEqual(1000, dg.Size);
        }

        /// <summary>
        /// Tests Size when dg is large.
        /// </summary>
        [TestMethod]
        public void TestSize4()
        {
            var dg = new DependencyGraph();

            for (var i = 0; i < 100000; i++)
            {
                dg.AddDependency("a" + i, "b" + i);
            }

            Assert.AreEqual(1000, dg.Size);
        }

        /// <summary>
        /// Tests HasDependents
        /// </summary>
        [TestMethod]
        public void TestHasDependents1()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            Assert.IsTrue(dg.HasDependents("a"));
            Assert.IsFalse(dg.HasDependents("b"));            
        }

        /// <summary>
        /// Tests HasDependees
        /// </summary>
        [TestMethod]
        public void TestHasDependees1()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            Assert.IsTrue(dg.HasDependees("b"));
            Assert.IsFalse(dg.HasDependees("a"));
        }

        /// <summary>
        /// Tests GetDependents when there are none.
        /// </summary>
        [TestMethod]
        public void TestGetDependents1()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            Assert.AreEqual(0, dg.GetDependents("b").ToList().Count);
        }

        /// <summary>
        /// Tests GetDependents when there are few.
        /// </summary>
        [TestMethod]
        public void TestGetDependents2()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("1", "2");
            dg.AddDependency("1", "3");
            dg.AddDependency("1", "4");

            var count = 2;
            foreach (string s in dg.GetDependents("1"))
            {
                Assert.AreEqual(count++ + "", s);
            }
        }

        /// <summary>
        /// Tests GetDependents when there are many.
        /// </summary>
        [TestMethod]
        public void TestGetDependents3()
        {
            var dg = new DependencyGraph();

            for (var i = 0; i < 10000; i++)
            {
                dg.AddDependency("1", i + 1 + "");
            }

            var num = 2;
            foreach (string s in dg.GetDependents("1"))
            {
                Assert.AreEqual(num++ + "", s);
            }
        }

        /// <summary>
        /// Tests GetDependees when there are none.
        /// </summary>
        [TestMethod]
        public void TestGetDependees1()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            Assert.AreEqual(0, dg.GetDependees("a").ToList().Count);
        }

        /// <summary>
        /// Tests GetDependees when there are few.
        /// </summary>
        [TestMethod]
        public void TestGetDependees2()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("2", "1");
            dg.AddDependency("3", "1");
            dg.AddDependency("4", "1");

            var count = 2;
            foreach (string s in dg.GetDependees("1"))
            {
                Assert.AreEqual(count++ + "", s);
            }
        }

        /// <summary>
        /// Tests GetDependents when there are many.
        /// </summary>
        [TestMethod]
        public void TestGetDependees3()
        {
            var dg = new DependencyGraph();

            for (var i = 0; i < 10000; i++)
            {
                dg.AddDependency( i + 1 + "", "1");
            }

            var num = 2;
            foreach (string s in dg.GetDependees("1"))
            {
                Assert.AreEqual(num++ + "", s);
            }
        }
    }
}
