﻿// Skeleton written by Joe Zachary for CS 3500, January 2015
// Revised by Joe Zachary, January 2016
// JLZ Repaired pair of mistakes, January 23, 2016
// Modified by Morgan Empey (u0634576) for CS 3500, Spring 2016, University of Utah

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public struct Formula
    {
        /// <summary>
        /// Represents the Formula as a string
        /// </summary>
        private List<string> formulaTokens;

        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(String formula)
        {
            // Representation of the formula
            formulaTokens = GetTokens(formula).ToList();

            int tokenCount = 0;
            int openParenCount = 0;
            int closeParentCount = 0;
            string prevToken = "";
              
            // Check for invalid tokens or syntax
            foreach (string token in formulaTokens)
            {
                tokenCount++;

                if (token == "(")
                {
                    openParenCount++;
                }
                else if (token == ")")
                {
                    closeParentCount++;
                }

                if (!IsValidToken(token))
                {
                    throw new FormulaFormatException("Invalid token");
                }

                if (tokenCount == 1 && !IsNumberToken(token) && !IsVariableToken(token) && token != "(")
                {
                    throw new FormulaFormatException("First token must be a number, variable, or open parentheses");
                }

                if (closeParentCount > openParenCount)
                {
                    throw new FormulaFormatException("Excess close parentheses");
                }

                if (prevToken == "(" || IsOperatorToken(prevToken))
                {
                    if (!IsNumberToken(token) && !IsVariableToken(token) && token != "(")
                    {
                        throw new FormulaFormatException("Invalid formula syntax");
                    }
                }

                if (IsNumberToken(prevToken) || IsVariableToken(prevToken) || prevToken == ")")
                {
                    if (!IsOperatorToken(token) && token != ")")
                    {
                        throw new FormulaFormatException("Invalid formula syntax");
                    }
                }

                prevToken = token;
            }

            if (tokenCount == 0)
            {
                throw new FormulaFormatException("Empty formula");
            }

            if (openParenCount != closeParentCount)
            {
                throw new FormulaFormatException("Parentheses count unequal");
            }

            if (!IsNumberToken(prevToken) && !IsVariableToken(prevToken) && prevToken != ")")
            {
                throw new FormulaFormatException("Last token must be a number, variable, or close parentheses");
            }
            
        }

        /// <summary>
        /// From the specification by Joe Zachary: "The purpose of a Normalizer is to convert variables
        /// into a canonical form.  The purpose of a Validator is to impose extra restrictions on the
        /// validity of a variable, beyond the ones already built into the Formula definition."
        /// </summary>
        public Formula(string formula, Normalizer normalizer, Validator validator)
            : this(formula)
        {
            for (int i = 0; i < formulaTokens.Count; i++)                    
            {
                var token = formulaTokens[i];

                if (IsVariableToken(token))
                {
                    var normalizedToken = normalizer(token);

                    if (!IsVariableToken(normalizedToken))
                    {
                        throw new FormulaFormatException("Invalid variable");
                    }
                    else if (!validator(normalizedToken))
                    {
                        throw new FormulaFormatException("Invalid variable");
                    }
                    else
                    {
                        // Replace the variable token with its normalized form.
                        formulaTokens[i] = normalizedToken;
                    }
                }
            }
        }

        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            FixNullFormula();

            // Create value and operator stacks
            var operatorStack = new Stack<string>();
            var valueStack = new Stack<double>();

            foreach (string token in formulaTokens)
            {
                double tokenValue = -1; // Need to initialize here, but it will be changed to the variable or token value.

                if (IsVariableToken(token))
                {
                    try
                    {
                        tokenValue = lookup(token);
                    }
                    catch (UndefinedVariableException)
                    {
                        throw new FormulaEvaluationException("Undefined variable in formula");
                    }
                }

                if (IsVariableToken(token) || double.TryParse(token, out tokenValue))
                {
                    valueStack.Push(tokenValue);

                    if (operatorStack.Count > 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                    {
                        PushNewValue(valueStack, operatorStack);
                    }
                }
                else if (token == "+" || token == "-")
                {
                    if (operatorStack.Count > 0 && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
                    {
                        PushNewValue(valueStack, operatorStack);
                    }

                    operatorStack.Push(token);
                }
                else if (token == "*" || token == "/" || token == "(")
                {
                    operatorStack.Push(token);
                }
                else if (token == ")")
                {
                    if (operatorStack.Count > 0 && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
                    {
                        PushNewValue(valueStack, operatorStack);
                    }

                    operatorStack.Pop(); // value should be "("

                    if (operatorStack.Count > 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                    {
                        PushNewValue(valueStack, operatorStack);
                    }
                }
            }

            if (operatorStack.Count != 0)
            {
                PushNewValue(valueStack, operatorStack);
            }

            return valueStack.Pop();
        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }

        /// <summary>
        /// Returns an ISet of every distinct variable in this formula, in normalized form.
        /// </summary>
        public ISet<string> GetVariables()
        {
            FixNullFormula();

            var result = new HashSet<string>();
            
            foreach (string token in formulaTokens)
            {
                if (IsVariableToken(token))
                {
                    result.Add(token.ToString());
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a string representation of this formula
        /// </summary>
        public override string ToString()
        {
            FixNullFormula();

            var result = "";

            foreach (string token in formulaTokens)
            {
                result += token;
            }

            return result;
        }

        /// <summary>
        /// Returns true if the token is a valid formula token.  Valid tokens are:
        ///     -Open or close parentheses
        ///     -+, -, *, or /
        ///     -Floating-point numbers
        ///     -Variables, which consist of one or more letters followed by zero or more letters/digits
        /// </summary>
        private Boolean IsValidToken(string token)
        {
            if (token == "(" ||
                token == ")" ||
                IsOperatorToken(token) ||
                IsNumberToken(token) ||
                IsVariableToken(token))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the token is a valid floating-point number.
        /// </summary>
        private Boolean IsNumberToken(string token)
        {
            double d;
            return Double.TryParse(token, out d);
        }

        /// <summary>
        /// Returns true if the token is one or more letters followed by zero or more letters/digits.
        /// </summary>
        private Boolean IsVariableToken(string token)
        {
            if (token == "" || !IsLetter(token[0]))
            {
                return false;
            }
            
            foreach (char c in token)
            {
                if (!IsLetter(c) && !IsNumeral(c))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if the token is a valid operator (+, -, *, or /)
        /// </summary>
        private Boolean IsOperatorToken(string s)
        {
            if (s == "+" || s == "-" || s == "*" || s == "/")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the character is an upper- or lower-case letter.
        /// </summary>
        private Boolean IsLetter(char c)
        {
            if ((c > 64 && c < 91) || (c > 96 && c < 123))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the character is a numeral 0-9
        /// </summary>
        private Boolean IsNumeral(char c)
        {
            if (c > 47 && c < 58)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Performs the given binary operation of the form (lhs)(oper)(rhs).
        /// oper is expected to be +, -, *, or /
        /// </summary>
        private double PerformOperation(double lhs, double rhs, string oper)
        {
            switch (oper)
            {
                case "+":
                    return lhs + rhs;
                case "-":
                    return lhs - rhs;
                case "*":
                    return lhs * rhs;
                case "/":
                    if (rhs == 0)
                    {
                        throw new FormulaEvaluationException("Cannot divide by zero");
                    }
                    return lhs / rhs;
                default:
                    throw new FormulaEvaluationException("Unsupported operation");
            }
        }

        /// <summary>
        /// Pops two values from the values stack, one operation from the opers stack,
        /// performs the operation on the values, and pushes the result onto the values
        /// stack.
        /// </summary>
        private void PushNewValue(Stack<double> values, Stack<string> opers)
        {
            string poppedOperator = opers.Pop();
            double rhs = values.Pop();

            values.Push(PerformOperation(values.Pop(), rhs, poppedOperator));
        }

        /// <summary>
        /// A Formula created with the zero-argument constructor will have a null formulaTokens.
        /// This should be treated the same as a "0" formula.
        /// </summary>
        private void FixNullFormula()
        {
            if (formulaTokens == null)
            {
                formulaTokens = new List<string>();
                formulaTokens.Add("0");
            }
        }
        
    }

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string s);

    /// <summary>
    /// A normalizer converts variables into a canonical form.
    /// </summary>
    public delegate string Normalizer(string s);

    /// <summary>
    /// A Validator imposes extra restrictions on the validity of a variable,
    /// beyond the ones already built into the Formula definition.
    /// </summary>
    public delegate bool Validator(string s);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}
