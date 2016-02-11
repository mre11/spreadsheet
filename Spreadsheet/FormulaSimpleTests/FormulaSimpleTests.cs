// Written by Joe Zachary for CS 3500, January 2016.
// Repaired error in Evaluate5.  Added TestMethod Attribute
//    for Evaluate4 and Evaluate5 - JLZ January 25, 2016
// Corrected comment for Evaluate3 - JLZ January 29, 2016
// Modified by Morgan Empey (u0634576) for CS 3500, Spring 2016, University of Utah

using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;

namespace FormulaTestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended to show you how
    /// client code can make use of the Formula class, and to show you how to create your
    /// own (which we strongly recommend).  To run them, pull down the Test menu and do
    /// Run > All Tests.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// This tests that a syntactically incorrect parameter to Formula results
        /// in a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct1()
        {
            Formula f = new Formula("_");
        }

        /// <summary>
        /// This is another syntax error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }

        /// <summary>
        /// First token is wrong.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct4()
        {
            Formula f = new Formula(")11+11)");
        }

        /// <summary>
        /// Excess close parentheses.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct5()
        {
            Formula f = new Formula("(11 + 11)) * (2 + 3)");
        }

        /// <summary>
        /// Bad token after "("
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct6()
        {
            Formula f = new Formula("()5 + 2)");
        }

        /// <summary>
        /// Bad token after "("
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct7()
        {
            Formula f = new Formula("(5)(2)");
        }

        /// <summary>
        /// Empty formula
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct8()
        {
            Formula f = new Formula("");
        }

        /// <summary>
        /// Another empty formula
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct9()
        {
            Formula f = new Formula("     ");
        }

        /// <summary>
        /// Unsupported operation
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct10()
        {
            Formula f = new Formula("(2 + 9)^2");
        }

        /// <summary>
        /// Tests the behavior of the zero-argument constructor.
        /// The formula format should be valid, and it should evaluate to 0.
        /// </summary>
        [TestMethod]
        public void ZeroArgumentConstructor1()
        {
            Formula f = new Formula();
            Assert.AreEqual(0, f.Evaluate(v => 1));
        }

        /// <summary>
        /// Tests the behavior of the zero-argument constructor.
        /// The formula format should behave the same as new Formula("0")
        /// </summary>
        [TestMethod]
        public void ZeroArgumentConstructor2()
        {
            Formula f1 = new Formula();
            Formula f2 = new Formula("0");

            Assert.IsTrue(f1.ToString() == f2.ToString());
            Assert.IsTrue(f1.Evaluate(v => 1) == f2.Evaluate(v => 1));
            Assert.IsTrue(f1.GetVariables().Count == f2.GetVariables().Count);
        }

        /// <summary>
        /// Invalid variable in formula (exception thrown by single-argument constructor)
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorStringNormVal1()
        {
            Formula f = new Formula("1 + 2a", Normalizer2, Validator1);
        }

        /// <summary>
        /// Normalizer creates bad variable
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorStringNormVal2()
        {
            Formula f = new Formula("a2", Normalizer2, Validator1);
        }

        /// <summary>
        /// Validator rejects variable
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorStringNormVal3()
        {
            Formula f = new Formula("a22", Normalizer1, Validator1);
        }

        /// <summary>
        /// Validator rejects variable
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorStringNormVal4()
        {
            Formula f = new Formula("bears", Normalizer1, Validator1);
        }

        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2+1+1+2-1");
            Assert.AreEqual(f.Evaluate(v => 0), 5.0, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5).  The value of
        /// the Formula depends on the value of x5, which is determined by
        /// the delegate passed to Evaluate.  Since this delegate maps all
        /// variables to 22.5, the return value should be 22.5.
        /// </summary>
        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5");
            Assert.AreEqual(f.Evaluate(v => 22.5), 22.5, 1e-6);
        }

        /// <summary>
        /// Here, the delegate passed to Evaluate always throws a
        /// UndefinedVariableException (meaning that no variables have
        /// values).  The test case checks that the result of
        /// evaluating the Formula is a FormulaEvaluationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(v => { throw new UndefinedVariableException(v); });
        }

        /// <summary>
        /// The delegate passed to Evaluate is defined below.  We check
        /// that evaluating the formula returns in 10.
        /// </summary>
        [TestMethod]
        public void Evaluate4()
        {
            Formula f = new Formula("x + y");
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate5 ()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }

        /// <summary>
        /// Test for FormulaEvaluationException if formula contains an undefined
        /// variable.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate6()
        {
            Formula f = new Formula("x / a");
            f.Evaluate(Lookup4);
        }

        /// <summary>
        /// Trivial formula
        /// </summary>
        [TestMethod]
        public void Evaluate7()
        {
            Formula f = new Formula("11");
            Assert.AreEqual(f.Evaluate(Lookup4), 11.0, 1e-6);
        }

        /// <summary>
        /// Trivial variable formula
        /// </summary>
        [TestMethod]
        public void Evaluate8()
        {
            Formula f = new Formula("x");
            Assert.AreEqual(f.Evaluate(Lookup4), 4.0, 1e-6);
        }

        /// <summary>
        /// Test for FormulaEvaluationException if divide by zero.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate9()
        {
            Formula f = new Formula("(2+1)/0");
            f.Evaluate(Lookup4);
        }

        /// <summary>
        /// Test the ToString method.
        /// </summary>
        [TestMethod]
        public void ToString1()
        {
            Formula f = new Formula("(x+5)*7/(2-3*y)");
            Assert.AreEqual("(x+5)*7/(2-3*y)", f.ToString());
        }

        /// <summary>
        /// Test GetVariables when there are no variables
        /// </summary>
        [TestMethod]
        public void GetVariables1()
        {
            Formula f = new Formula("42");
            Assert.AreEqual(0, f.GetVariables().Count);
        }

        /// <summary>
        /// Test GetVariables when there are no repeat variables
        /// </summary>
        [TestMethod]
        public void GetVariables2()
        {
            Formula f = new Formula("x + y + z");
            Assert.AreEqual(3, f.GetVariables().Count);
            Assert.IsTrue(f.GetVariables().Contains("x"));
            Assert.IsTrue(f.GetVariables().Contains("y"));
            Assert.IsTrue(f.GetVariables().Contains("z"));
        }

        /// <summary>
        /// Test GetVariables when there are repeat variables
        /// </summary>
        [TestMethod]
        public void GetVariables3()
        {
            Formula f = new Formula("a2 - x + 3*y/y + 2*x + z*z");
            Assert.AreEqual(4, f.GetVariables().Count);
            Assert.IsTrue(f.GetVariables().Contains("x"));
            Assert.IsTrue(f.GetVariables().Contains("y"));
            Assert.IsTrue(f.GetVariables().Contains("z"));
            Assert.IsTrue(f.GetVariables().Contains("a2"));
        }

        /// <summary>
        /// Test GetVariables when variables are normalized
        /// </summary>
        [TestMethod]
        public void GetVariables4()
        {
            Formula f = new Formula("a2 - x + 3*y/y + 2*x + z*z", Normalizer1, s => true);
            Assert.AreEqual(4, f.GetVariables().Count);
            Assert.IsTrue(f.GetVariables().Contains("X"));
            Assert.IsTrue(f.GetVariables().Contains("Y"));
            Assert.IsTrue(f.GetVariables().Contains("Z"));
            Assert.IsTrue(f.GetVariables().Contains("A2"));
        }

        /// <summary>
        /// A Lookup method that maps x to 4.0, y to 6.0, and z to 8.0.
        /// All other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "y": return 6.0;
                case "z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }

        /// <summary>
        /// Returns a s with all letters changed to upper-case.
        /// </summary>
        public string Normalizer1(string s)
        {
            return s.ToUpper();
        }

        /// <summary>
        /// Appends the string "1" to the beginning of s.
        /// This creates an invalid variable by definition.
        /// </summary>
        public string Normalizer2(string s)
        {
            return "1" + s;
        }

        /// <summary>
        /// Returns true if the string is a single letter followed by a single number
        /// </summary>
        public bool Validator1(string variable)
        {
            var pattern = @"^[a-zA-Z][0-9]{1}$";

            return Regex.IsMatch(variable, pattern);
        }
    }
}
