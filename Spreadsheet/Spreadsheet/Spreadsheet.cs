using System;
using System.Collections.Generic;
using Formulas;
using System.Text.RegularExpressions;
using Dependencies;
using System.Linq;
using System.IO;

namespace SS
{
    /// <summary>
    /// A Spreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string is a cell name if and only if it consists of one or more letters, 
    /// followed by a non-zero digit, followed by zero or more digits.  Cell names
    /// are not case sensitive. @"^[a-zA-z]+[1-9]+\d*$"
    /// 
    /// For example, "A15", "a15", "XY32", and "BC7" are cell names.  (Note that 
    /// "A15" and "a15" name the same cell.)  On the other hand, "Z", "X07", and 
    /// "hello" are not cell names."
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  
    /// In addition to a name, each cell has a contents and a value.  The distinction is
    /// important, and it is important that you understand the distinction and use
    /// the right term when writing code, writing comments, and asking questions.
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// A Spreadsheet is set of Cells that can be looked up by name
        /// </summary>
        private Dictionary<string, Cell> cells;

        /// <summary>
        /// Tracks the dependencies among cells in the spreadsheet
        /// </summary>
        private DependencyGraph dg;

        private Regex IsValid { get; set; }

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get; protected set; }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression accepts every string.
        /// </summary>
        public Spreadsheet()
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            IsValid = new Regex(@".*");
            Changed = false;
        }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter
        /// </summary>
        public Spreadsheet(Regex isValid)
            : this()
        {
            IsValid = isValid;
        }

        /// <summary>
        /// Creates a Spreadsheet that is a duplicate of the spreadsheet saved in source.
        /// See the AbstractSpreadsheet.Save method and Spreadsheet.xsd for the file format 
        /// specification.  If there's a problem reading source, throws an IOException
        /// If the contents of source are not consistent with the schema in Spreadsheet.xsd, 
        /// throws a SpreadsheetReadException.  If there is an invalid cell name, or a 
        /// duplicate cell name, or an invalid formula in the source, throws a SpreadsheetReadException.
        /// If there's a Formula that causes a circular dependency, throws a SpreadsheetReadException.
        /// </summary>
        public Spreadsheet(TextReader source)
        {
            // TODO Spreadsheet TextReader constructor (tests and implementation)
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            foreach (KeyValuePair<string, Cell> kvp in cells)
            {
                if (!kvp.Value.IsEmpty())
                {
                yield return kvp.Key;
            }
        }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if (name == null || !IsValid.IsMatch(name))
            {
                throw new InvalidNameException();
            }

            var normalizedName = name.ToUpper();

            Cell c;
            if (cells.TryGetValue(normalizedName, out c))
            {
                return c.Contents;
            }

            return ""; // if name isn't in cells, the Cell's contents are the empty string
        }

        /// <summary>
        /// If formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            return SetCellContents(name, (object) formula);
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, string text)
        {
            return SetCellContents(name, (object) text);
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            return SetCellContents(name, (object) number);            
        }

        /// <summary>
        /// Helper method for SetCellContents protected methods. Method assumes that
        /// the caller has already checked for null and invalid parameters.
        /// </summary>
        private ISet<string> SetCellContents(string name, object contents)
        {
            var normalizedName = name.ToUpper();

            ISet<string> result = TryUpdateDependencies(normalizedName, contents);

            // Update the cell contents, or create a new cell if it doesn't already exist
            Cell c;
            if (cells.TryGetValue(normalizedName, out c))
            {
                c.Contents = contents;
            }
            else
            {
                cells.Add(normalizedName, new Cell(normalizedName, contents));
            }

            RecalculateCellValues(normalizedName);

            this.Changed = true;

            return result;
        }

        /// <summary>
        /// If the contents is a Formula that results in a circular reference, throws a CircularException
        /// Otherwise, replaces the dependents of the named cell with the new dependents in Formula.
        /// For all contents, returns a set consisting of name plus the names of all other cells
        /// whose value depends, directly or indirectly, on the named cell.
        /// </summary>
        private ISet<string> TryUpdateDependencies(string name, object contents)
        {
            ISet<string> result = new HashSet<string>();

            // Save dependencies so they can be restored later if a circular reference is found
            List<string> previousDependees = dg.GetDependents(name).ToList();

            try {
                // If contents is a Formula, add any dependencies to dg
                if (contents.GetType() == typeof(Formula))
                {
                    Formula formulaContents = (Formula) contents;
                    dg.ReplaceDependees(name, formulaContents.GetVariables());
                }                
                result.UnionWith(GetCellsToRecalculate(name)); // throws exception if a circular reference is found
            }
            catch (CircularException)
            {
                // Restore the previous dependents, then re-throw the exception
                dg.ReplaceDependees(name, previousDependees);
                throw;
            }

            return result;
        }

        /// <summary>
        /// Recalculates the value of the cell specified by name, as well as all cells that
        /// depend directly or indirectly on the cell.
        /// </summary>
        private void RecalculateCellValues(string name)
        {
            foreach (string cellName in GetCellsToRecalculate(name))
            {
                Cell c;
                if (cells.TryGetValue(cellName, out c))
                {
                    if (c.Contents.GetType() == typeof(string) || c.Contents.GetType() == typeof(double))
                    {
                        c.Value = c.Contents;
                    }
                    else
                    {
                        try
                        {
                            c.Value = ((Formula)c.Contents).Evaluate(CellValueLookup);
                        }
                        catch(Exception e)
                        {
                            c.Value = new FormulaError(e.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the value of a cell given its name
        /// </summary>
        private double CellValueLookup(string cellName)
        {
            Cell c;
            if (cells.TryGetValue(cellName, out c))
            {
                return (double) c.Value;
            }

            throw new FormulaEvaluationException("Cell reference not found");
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            else if (!IsValid.IsMatch(name))
            {
                throw new InvalidNameException();
            }

            var normalizedName = name.ToUpper();

            foreach (string dependentName in dg.GetDependents(normalizedName))
            {               
                yield return dependentName.ToUpper();
            }
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to dest using an XML format.
        /// The XML elements should be structured as follows:
        ///
        /// <spreadsheet IsValid="IsValid regex goes here">
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        /// </spreadsheet>
        ///
        /// The value of the isvalid attribute should be IsValid.ToString()
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.
        /// If the cell contains a string, the string (without surrounding double quotes) should be written as the contents.
        /// If the cell contains a double d, d.ToString() should be written as the contents.
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        ///
        /// If there are any problems writing to dest, the method should throw an IOException.
        /// </summary>
        public override void Save(TextWriter dest)
        {
            // TODO write tests and implement Save
            Changed = false;
            throw new NotImplementedException();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            if (name == null || !IsValid.IsMatch(name))
            {
                throw new InvalidNameException();
            }

            Cell c;
            if (cells.TryGetValue(name, out c))
            {
                return c.Value;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        ///
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor with s => s.ToUpper() as the normalizer and a validator that
        /// checks that s is a valid cell name as defined in the AbstractSpreadsheet
        /// class comment.  There are then three possibilities:
        ///
        ///   (1) If the remainder of content cannot be parsed into a Formula, a
        ///       Formulas.FormulaFormatException is thrown.
        ///
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///
        ///   (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes content.
        ///
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            else if (name == null || !IsValid.IsMatch(name))
            {
                throw new InvalidNameException();
            }
            
            double doubleContent;
            if (double.TryParse(content, out doubleContent))
            {
                return SetCellContents(name, doubleContent);
            }
            else if (content.Length > 0 && content[0] == '=')
            {
                return SetCellContents(name, new Formula(content.Substring(1), s => s.ToUpper(), IsValid.IsMatch));                
            }

            return SetCellContents(name, content);            
        }

        /// <summary>
        /// A spreadsheet contains a cell corresponding to every possible cell name.  
        /// In addition to a name, each cell has a contents and a value.
        /// </summary>
        private class Cell
        {
            /// <summary>
            /// A string is a cell name if and only if it consists of one or more letters, 
            /// followed by a non-zero digit, followed by zero or more digits.  Cell names
            /// are not case sensitive.
            /// </summary>
            private string name;

            /// <summary>
            /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
            /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
            /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
            /// 
            /// In an empty spreadsheet, the contents of every cell is the empty string.
            ///  
            /// If a cell's contents is a string, its value is that string.
            /// 
            /// If a cell's contents is a double, its value is that double.
            /// 
            /// If a cell's contents is a Formula, its value is either a double or a FormulaError.
            /// The value of a Formula, of course, can depend on the values of variables.  The value 
            /// of a Formula variable is the value of the spreadsheet cell it names (if that cell's 
            /// value is a double) or is undefined (otherwise).  If a Formula depends on an undefined
            /// variable or on a division by zero, its value is a FormulaError.  Otherwise, its value
            /// is a double, as specified in Formula.Evaluate.
            /// </summary>
            internal object Contents { get; set; }

            /// <summary>
            /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
            /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
            /// in the grid.)
            /// </summary>
            internal object Value { get; set; }

            /// <summary>
            /// Creates an empty cell with the given name. Does not check the name for validity.
            /// </summary>
            internal Cell(string name)
            {
                this.name = name;
                Contents = "";
            }

            /// <summary>
            /// Creates a new cell with the given name and contents
            /// </summary>
            internal Cell(string name, object contents)
                : this(name)
            {
                Contents = contents;
            }

            /// <summary>
            /// Returns true if the Contents of this Cell is the empty string.
            /// </summary>
            internal bool IsEmpty()
            {
                return this.Contents.GetType() == typeof(string) && (string)this.Contents == "";
            }
        }
    }
}
