// Skeleton implementation written by Joe Zachary for CS 3500, January 2015.
// Revised for CS 3500 by Joe Zachary, January 29, 2016
// Modified for CS 3500 by Morgan Empey, University of Utah, Spring 2016

using System;
using System.Collections.Generic;
using System.Linq;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// Each vertex of the graph is a string. The two dictionaries can be used to look up the
        /// dependents and dependees, respectively, of a given string.
        /// </summary>
        private Dictionary<string, HashSet<string>> dependents;
        private Dictionary<string, HashSet<string>> dependees;

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// Creates a dependency graph that is an independent copy of dg.
        /// </summary>
        public DependencyGraph(DependencyGraph dg)
            : this()
        {
            foreach (KeyValuePair<string, HashSet<string>> kvp in dg.dependents)
            {
                this.dependents.Add(kvp.Key, new HashSet<string>(kvp.Value));
            }

            foreach (KeyValuePair<string, HashSet<string>> kvp in dg.dependees)
            {
                this.dependees.Add(kvp.Key, new HashSet<string>(kvp.Value));
            }
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get
            {
                var count = 0;
                foreach (KeyValuePair<string, HashSet<string>> kvp in dependents)
                {
                    count += kvp.Value.Count;
                }
                return count;
            }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// Throws an exception if null arguments are provided.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }

            HashSet<string> tempSet;
            var hasValue = dependents.TryGetValue(s, out tempSet);

            return hasValue && tempSet.Count > 0;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// Throws an exception if null arguments are provided.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }

            HashSet<string> tempSet;
            var hasValue = dependees.TryGetValue(s, out tempSet);

            return hasValue && tempSet.Count > 0;
        }

        /// <summary>
        /// Enumerates dependents(s).
        /// Throws an exception if null arguments are provided.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }

            HashSet<string> tempSet;

            if (dependents.TryGetValue(s, out tempSet))
            {
                foreach (string t in tempSet)
                {
                    yield return t;
                }
            }
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// Throws an exception if null arguments are provided.
        /// </summary>
        public IEnumerable<string> GetDependees(string t)
        {
            if (t == null)
            {
                throw new ArgumentNullException();
            }

            HashSet<string> tempSet;

            if (dependees.TryGetValue(t, out tempSet))
            {
                foreach (string s in tempSet)
                {
                    yield return s;
                }
            }
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Throws an exception if null arguments are provided.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if (s == null || t == null)
            {
                throw new ArgumentNullException();
            }

            HashSet<string> tempSet;

            // Add the dependency to dependents
            if (dependents.TryGetValue(s, out tempSet))
            {
                tempSet.Add(t);
            }
            else
            {
                tempSet = new HashSet<string>();
                tempSet.Add(t);
                dependents.Add(s, tempSet);
            }

            // Add the dependency to dependees
            if (dependees.TryGetValue(t, out tempSet))
            {
                tempSet.Add(s);
            }
            else
            {
                tempSet = new HashSet<string>();
                tempSet.Add(s);
                dependees.Add(t, tempSet);
            }
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Throws an exception if null arguments are provided.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if (s == null || t == null)
            {
                throw new ArgumentNullException();
            }

            HashSet<string> tempSet;

            // Remove dependency from dependents
            if (dependents.TryGetValue(s, out tempSet))
            {
                tempSet.Remove(t);
            }

            // Remove dependency from dependees
            if (dependees.TryGetValue(t, out tempSet))
            {
                tempSet.Remove(s);
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Throws an exception if null arguments are provided.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (s == null || newDependents == null || newDependents.Contains(null))
            {
                throw new ArgumentNullException();
            }
            
            HashSet<string> tempSet;

            // Remove the dependencies from dependents
            if (dependents.TryGetValue(s, out tempSet))
            {
                dependents.Remove(s);
            }

            // Remove the dependencies from dependees
            foreach (KeyValuePair<string, HashSet<string>> kvp in dependees)
            {
                kvp.Value.Remove(s);
            }

            // Add the new dependencies to dependents and dependees
            foreach (string newString in newDependents)
            {
                this.AddDependency(s, newString);
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Throws an exception if null arguments are provided.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (t == null || newDependees == null || newDependees.Contains(null))
            {
                throw new ArgumentNullException();
            }
            
            HashSet<string> tempSet;

            // Remove the dependencies from dependees
            if (dependees.TryGetValue(t, out tempSet))
            {
                dependees.Remove(t);
            }

            // Remove the dependencies from dependents
            foreach (KeyValuePair<string, HashSet<string>> kvp in dependents)
            {
                kvp.Value.Remove(t);
            }

            // Add the new dependencies to dependents and dependees
            foreach (string newString in newDependees)
            {
                this.AddDependency(newString, t);
            }
        }
    }
}
