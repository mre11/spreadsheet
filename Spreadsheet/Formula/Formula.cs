// Skeleton written by Joe Zachary for CS 3500, January 2015
// Revised by Joe Zachary, January 2016
// JLZ Repaired pair of mistakes, January 23, 2016
// Modified by Morgan Empey (u0634576) for CS 3500, Spring 2016, University of Utah

using System;
using System.Collections.Generic;
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
    public class Formula
    {
        // Represents the formula as a string
        private string formulaString;

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
            // Store representation
            formulaString = formula;

            // Store formula in container as tokens
            //List<string> formulaTokens = GetTokens(formula).ToList<string>();

            // Check that there is at least one token
            //if (formulaTokens.Count == 0)
            //{
            //    throw new FormulaFormatException("Empty formula");
            //}

            int tokenCount = 0;
            int openParenCount = 0;
            int closeParentCount = 0;
            string prevToken = "";
              
            // Check for invalid tokens or syntax
            foreach (string token in GetTokens(formula))
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
                        throw new FormulaFormatException("Invalid syntax"); // TODO better message here?
                    }
                }

                if (IsNumberToken(prevToken) || IsVariableToken(prevToken) || prevToken == ")")
                {
                    if (!IsOperatorToken(token) && token != ")")
                    {
                        throw new FormulaFormatException("Invalid syntax"); // TODO better message here?
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
            // Create value and operator stacks
            var operatorStack = new Stack<string>();
            var valueStack = new Stack<double>();

            foreach (string token in GetTokens(formulaString))
            {
                double currentValue = -1; // TODO hack...

                if (IsVariableToken(token))
                {
                    try
                    {
                        currentValue = lookup(token);
                    }
                    catch (UndefinedVariableException)
                    {
                        throw new FormulaEvaluationException("Undefined variable in formula");
                    }
                }

                if (IsVariableToken(token) || double.TryParse(token, out currentValue))
                {
                    if (operatorStack.Count > 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                    {
                        string poppedOperator = operatorStack.Pop();
                        double poppedValue = valueStack.Pop();

                        valueStack.Push(PerformOperation(poppedValue, currentValue, poppedOperator));
                    }
                    else
                    {
                        valueStack.Push(currentValue);
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
        /// Returns true if the token is a valid formula token.  Valid tokens are:
        ///     -Open or close parentheses
        ///     -+, -, *, or /
        ///     -Floating-point numbers
        ///     -Variables, which consist of one or more letters followed by zero or more letters/digits
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private Boolean IsValidToken(string token)
        {
            if (token.Equals("(") ||
                token.Equals(")") ||
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
        /// <param name="token"></param>
        /// <returns></returns>
        private Boolean IsNumberToken(string token)
        {
            double d;
            return Double.TryParse(token, out d);
        }

        /// <summary>
        /// Returns true if the token is one or more letters followed by zero or more letters/digits.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
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
        /// <param name="s"></param>
        /// <returns></returns>
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
        /// <param name="c"></param>
        /// <returns></returns>
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
        /// <param name="c"></param>
        /// <returns></returns>
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
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
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
                        throw new FormulaEvaluationException("Divide by zero");
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
        /// <param name="values"></param>
        /// <param name="opers"></param>
        private void PushNewValue(Stack<double> values, Stack<string> opers)
        {
            string poppedOperator = opers.Pop();
            double rhs = values.Pop();

            values.Push(PerformOperation(values.Pop(), rhs, poppedOperator));
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
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
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
