// Written by Morgan Empey for CS 3500, University of Utah, Spring 2016

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;
using System.Linq;
using System.Collections.Generic;
using System;

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
        public void TestConstructor1()
        {
            var dg1 = new DependencyGraph();
            var dg2 = new DependencyGraph(dg1);

            Assert.IsFalse(dg1 == dg2);
        }

        /// <summary>
        /// Tests Size when dg is empty.
        /// </summary>
        [TestMethod]
        public void TestConstructor2()
        {
            var dg1 = new DependencyGraph();

            dg1.AddDependency("a1", "a2");
            dg1.AddDependency("a1", "a3");
            dg1.AddDependency("b1", "a1");
            dg1.AddDependency("a2", "a3");
            dg1.AddDependency("b2", "a2");
            dg1.AddDependency("b2", "a3");
            dg1.AddDependency("a1", "b2");

            var dg2 = new DependencyGraph(dg1);

            Assert.AreEqual(7, dg1.Size);
            Assert.AreEqual(7, dg2.Size);

            // Check that modifying dg1 doesn't modify dg2
            dg1.RemoveDependency("a1", "a2");

            var list1 = dg1.GetDependents("a1").ToList();
            var list2 = dg2.GetDependents("a1").ToList();

            Assert.AreEqual(2, list1.Count);
            Assert.AreEqual(3, list2.Count);

            Assert.IsFalse(list1.Contains("a2"));
            Assert.IsTrue(list2.Contains("a2"));

            dg1.AddDependency("b1", "c1");

            list2 = dg2.GetDependents("b1").ToList();

            Assert.IsFalse(list2.Contains("c1"));
        }

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
        /// Also tests AddDependency.
        /// </summary>
        [TestMethod]
        public void TestSize2()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("a1", "a2");
            dg.AddDependency("a1", "a3");
            dg.AddDependency("b1", "a1");
            dg.AddDependency("a2", "a3");
            dg.AddDependency("b2", "a2");
            dg.AddDependency("b2", "a3");
            dg.AddDependency("a1", "b2");
            Assert.AreEqual(7, dg.Size);
        }

        /// <summary>
        /// Tests Size when dg is moderately sized.
        /// Also tests AddDependency.
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
        /// Also tests AddDependency.
        /// </summary>
        [TestMethod]
        public void TestSize4()
        {
            var dg = new DependencyGraph();

            for (var i = 0; i < 100000; i++)
            {
                dg.AddDependency("a" + i, "b" + i);
            }

            Assert.AreEqual(100000, dg.Size);
        }

        /// <summary>
        /// Tests Size after more complex dg operations.
        /// Also serves as an integration test.
        /// </summary>
        [TestMethod]
        public void TestSize5()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("a1", "a2");
            dg.AddDependency("a1", "a3");
            dg.AddDependency("b1", "a1");
            dg.AddDependency("a2", "a3");
            dg.AddDependency("b2", "a2");
            dg.AddDependency("b2", "a3");
            dg.AddDependency("a1", "b2");
            dg.ReplaceDependents("b2", new List<string>());
            dg.RemoveDependency("a2", "a3");
            dg.RemoveDependency("a1", "a3");
            Assert.AreEqual(3, dg.Size);
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
        /// Tests HasDependents for null argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestHasDependents2()
        {
            var dg = new DependencyGraph();
            dg.HasDependents(null);
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
        /// Tests HasDependees for null argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestHasDependees2()
        {
            var dg = new DependencyGraph();
            dg.HasDependees(null);
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

            var list = dg.GetDependents("1").ToList<string>();
            Assert.IsTrue(list.Contains("2"));
            Assert.IsTrue(list.Contains("3"));
            Assert.IsTrue(list.Contains("4"));
        }

        /// <summary>
        /// Tests GetDependents for null argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetDependents3()
        {
            var dg = new DependencyGraph();
            var list = dg.GetDependents(null).ToList<string>();
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

            var list = dg.GetDependees("1").ToList<string>();
            Assert.IsTrue(list.Contains("2"));
            Assert.IsTrue(list.Contains("3"));
            Assert.IsTrue(list.Contains("4"));
        }

        /// <summary>
        /// Tests GetDependees for null argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetDependees3()
        {
            var dg = new DependencyGraph();
            var list = dg.GetDependees(null).ToList<string>();
        }

        /// <summary>
        /// Tests AddDependency for multple adds of the same
        /// dependency.
        /// </summary>
        [TestMethod]
        public void TestAddDependency1()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("2", "1");
            dg.AddDependency("2", "1");
            dg.AddDependency("2", "1");

            Assert.AreEqual(1, dg.Size);
        }

        /// <summary>
        /// Tests AddDependency for null first argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddDependency2()
        {
            var dg = new DependencyGraph();
            dg.AddDependency(null, "1");
        }

        /// <summary>
        /// Tests AddDependency for null second argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddDependency3()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("1", null);
        }

        /// <summary>
        /// Tests AddDependency for null both arguments
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddDependency4()
        {
            var dg = new DependencyGraph();
            dg.AddDependency(null, null);
        }

        /// <summary>
        /// Tests RemoveDependency when the dependent is not found.
        /// </summary>
        [TestMethod]
        public void TestRemoveDependency1()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("2", "1");
            dg.AddDependency("3", "1");
            dg.AddDependency("4", "1");

            dg.RemoveDependency("5", "1");

            Assert.AreEqual(3, dg.Size);
        }

        /// <summary>
        /// Tests RemoveDependency when the dependee is not found.
        /// </summary>
        [TestMethod]
        public void TestRemoveDependency2()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("2", "1");
            dg.AddDependency("3", "1");
            dg.AddDependency("4", "1");

            dg.RemoveDependency("2", "3");

            Assert.AreEqual(3, dg.Size);
        }

        /// <summary>
        /// Tests RemoveDependency when the dependency exists.
        /// </summary>
        [TestMethod]
        public void TestRemoveDependency3()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("2", "1");
            dg.AddDependency("3", "1");
            dg.AddDependency("4", "1");

            dg.RemoveDependency("2", "1");

            Assert.AreEqual(2, dg.Size);
        }

        /// <summary>
        /// Tests multiple RemoveDependency calls.
        /// </summary>
        [TestMethod]
        public void TestRemoveDependency4()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("2", "1");
            dg.AddDependency("3", "1");
            dg.AddDependency("4", "1");
            dg.AddDependency("5", "1");

            dg.RemoveDependency("2", "1");
            dg.RemoveDependency("3", "1");
            dg.RemoveDependency("4", "1");

            Assert.AreEqual(1, dg.Size);
        }

        /// <summary>
        /// Tests RemoveDependency for null first argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRemoveDependency5()
        {
            var dg = new DependencyGraph();
            dg.RemoveDependency(null, "1");
        }

        /// <summary>
        /// Tests RemoveDependency for null second argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRemoveDependency6()
        {
            var dg = new DependencyGraph();
            dg.RemoveDependency("1", null);
        }

        /// <summary>
        /// Tests RemoveDependency for null both arguments
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRemoveDependency7()
        {
            var dg = new DependencyGraph();
            dg.RemoveDependency(null, null);
        }

        /// <summary>
        /// Test ReplaceDependents with no replacements.
        /// </summary>
        [TestMethod]
        public void TestReplaceDependents1()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("2", "1");
            dg.AddDependency("2", "3");
            dg.AddDependency("2", "4");
            dg.AddDependency("2", "5");

            dg.ReplaceDependents("2", new List<string>());

            Assert.AreEqual(0, dg.Size);
        }

        /// <summary>
        /// Test ReplaceDependents with replacements.
        /// </summary>
        [TestMethod]
        public void TestReplaceDependents2()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("2", "1");
            dg.AddDependency("2", "3");
            dg.AddDependency("2", "4");
            dg.AddDependency("2", "5");

            var list = new List<string>();
            list.Add("a");
            list.Add("b");
            list.Add("c");

            dg.ReplaceDependents("2", list);

            Assert.AreEqual(3, dg.Size);
        }

        /// <summary>
        /// Test ReplaceDependents without pre-existing dependents.
        /// </summary>
        [TestMethod]
        public void TestReplaceDependents3()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("2", "1");
            dg.AddDependency("2", "3");
            dg.AddDependency("2", "4");
            dg.AddDependency("2", "5");

            var list = new List<string>();
            list.Add("a");
            list.Add("b");
            list.Add("c");

            dg.ReplaceDependents("6", list);

            Assert.AreEqual(7, dg.Size);
        }

        /// <summary>
        /// Tests ReplaceDependents for null first argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependents4()
        {
            var dg = new DependencyGraph();

            var list = new List<string>();
            list.Add("a");
            list.Add("b");
            list.Add("c");

            dg.ReplaceDependents(null, list);
        }

        /// <summary>
        /// Tests ReplaceDependents for null second argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependents5()
        {
            var dg = new DependencyGraph();
            dg.ReplaceDependents("1", null);
        }

        /// <summary>
        /// Tests ReplaceDependents for null both arguments
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependents6()
        {
            var dg = new DependencyGraph();
            dg.ReplaceDependents(null, null);
        }

        /// <summary>
        /// Tests ReplaceDependents for null in list
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependents7()
        {
            var dg = new DependencyGraph();

            var list = new List<string>();
            list.Add("a");
            list.Add("b");
            list.Add(null);
            list.Add("c");

            dg.ReplaceDependents("1", list);
        }

        /// <summary>
        /// Test ReplaceDependees with no replacements.
        /// </summary>
        [TestMethod]
        public void TestReplaceDependees1()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("2", "1");
            dg.AddDependency("3", "1");
            dg.AddDependency("4", "1");
            dg.AddDependency("5", "1");

            dg.ReplaceDependees("1", new List<string>());

            Assert.AreEqual(0, dg.Size);
        }

        /// <summary>
        /// Test ReplaceDependees with replacements.
        /// </summary>
        [TestMethod]
        public void TestReplaceDependees2()
        {
            var dg = new DependencyGraph();
            dg.AddDependency("2", "1");
            dg.AddDependency("3", "1");
            dg.AddDependency("4", "1");
            dg.AddDependency("5", "1");

            var list = new List<string>();
            list.Add("a");
            list.Add("b");
            list.Add("c");

            dg.ReplaceDependees("1", list);

            Assert.AreEqual(3, dg.Size);
        }

        /// <summary>
        /// Test ReplaceDependees on an empty DependencyGraph.
        /// </summary>
        [TestMethod]
        public void TestReplaceDependees3()
        {
            var dg = new DependencyGraph();

            var list = new List<string>();
            list.Add("a");
            list.Add("b");
            list.Add("c");

            dg.ReplaceDependees("1", list);

            Assert.AreEqual(3, dg.Size);
        }

        /// <summary>
        /// Tests ReplaceDependees for null first argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependees4()
        {
            var dg = new DependencyGraph();

            var list = new List<string>();
            list.Add("a");
            list.Add("b");
            list.Add("c");

            dg.ReplaceDependees(null, list);
        }

        /// <summary>
        /// Tests ReplaceDependees for null second argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependees5()
        {
            var dg = new DependencyGraph();
            dg.ReplaceDependees("1", null);
        }

        /// <summary>
        /// Tests ReplaceDependees for null both arguments
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependees6()
        {
            var dg = new DependencyGraph();
            dg.ReplaceDependees(null, null);
        }

        /// <summary>
        /// Tests ReplaceDependees for null in list
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependees7()
        {
            var dg = new DependencyGraph();

            var list = new List<string>();
            list.Add("a");
            list.Add("b");
            list.Add(null);
            list.Add("c");

            dg.ReplaceDependees("1", list);
        }

        /// <summary>
        /// Tests DependencyGraph performance by adding one million randomly-generated dependencies and
        /// performing other operations.
        /// </summary>
        [TestMethod]
        public void TestPerformance1()
        {
            var dg = new DependencyGraph();
            var cellNumberUpperBound = 1000;
            var random = new Random();

            for (var i = 0; i < 1000000; i++)
            {
                dg.AddDependency(GenerateRandomIntString(cellNumberUpperBound, random), GenerateRandomIntString(cellNumberUpperBound, random));
            }

            Assert.IsTrue(dg.HasDependents("42"));
            Assert.IsTrue(dg.HasDependees("11"));
            dg.ReplaceDependees("42", new List<string>());
            dg.ReplaceDependents("555", new List<string>());
            dg.AddDependency("1", "1001");
            dg.AddDependency("2", "3");
            dg.RemoveDependency("1", "1001");
            dg.RemoveDependency("22", "17");
            Assert.IsTrue(dg.Size > 0);
        }

        /// <summary>
        /// Returns a random string representing an in between 0 and upperBound.
        /// </summary>
        private string GenerateRandomIntString(int upperBound, Random random)
        {
            return "" + random.Next(upperBound);
        }
    }
}
