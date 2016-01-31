﻿// Skeleton implementation written by Joe Zachary for CS 3500, January 2015.
// Revised for CS 3500 by Joe Zachary, January 29, 2016
// Modified for CS 3500 by Morgan Empey, University of Utah, Spring 2016

using System;
using System.Collections.Generic;

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
        /// Represents the graph as a Dictionary of vertices, where the key is the name of the Vertex.
        /// Each Vertex maintains references to its dependents and dependees.
        /// </summary>
        private Dictionary<string, Vertex> cells;

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return 0; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            return false;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            return null;
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            return null;
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void AddDependency(string s, string t)
        {
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
        }

        /// <summary>
        /// Provides a representation of a single vertex of a DependencyGraph.
        /// A Vertex knows all its dependents and dependees.
        /// </summary>
        private class Vertex
        {
            /// <summary>
            /// The unique name of this Vertex.
            /// </summary>
            string key;

            /// <summary>
            /// The size of a Vertex is the number of its dependents.
            /// </summary>
            int size;

            /// <summary>
            /// Lists all dependents of this Vertex.
            /// </summary>
            HashSet<Vertex> dependents;

            /// <summary>
            /// Lists all dependees of this Vertex.
            /// </summary>
            HashSet<Vertex> dependees;

            /// <summary>
            /// Constructs a new Vertex object with no dependents or dependees.
            /// </summary>
            Vertex(string name)
            {
                key = name;
                size = 0;
                dependents = new HashSet<Vertex>();
                dependees = new HashSet<Vertex>();
            }

            /// <summary>
            /// Constructs a new Vertex object with the given name, dependents, and dependees.
            /// Dependents and dependees are allowed to be empty.
            /// </summary>
            Vertex(string name, IEnumerable<Vertex> dependents, IEnumerable<Vertex> dependees)
                : this(name)
            {
                this.AddDependents(dependents);
                this.AddDependees(dependees);             
            }

            /// <summary>
            /// Returns the name of this Vertex.
            /// </summary>
            string Name
            {
                get { return key; }
            }

            /// <summary>
            /// Returns the size of this Vertex.
            /// </summary>
            int Size
            {
                get { return size; }
            }

            /// <summary>
            /// Returns all dependents of this Vertex.
            /// </summary>
            IEnumerable<Vertex> GetAllDependents()
            {
                foreach (Vertex v in this.dependents)
                {
                    yield return v;
                }
            }

            /// <summary>
            /// Returns all dependees of this Vertex.
            /// </summary>
            IEnumerable<Vertex> GetAllDependees()
            {
                foreach (Vertex v in this.dependees)
                {
                    yield return v;
                }
            }

            /// <summary>
            /// Adds a single dependent to this Vertex.
            /// </summary>
            void AddDependent(Vertex vertex)
            {
                if (dependents.Add(vertex))
                {
                    size++;
                }
            }

            /// <summary>
            /// Adds multiple dependents to this Vertex.
            /// </summary>
            void AddDependents(IEnumerable<Vertex> vertices)
            {
                foreach (Vertex v in vertices)
                {
                    this.AddDependent(v);
                }
            }

            /// <summary>
            /// Adds a single dependee to this Vertex.
            /// </summary>
            void AddDependee(Vertex vertex)
            {
                dependees.Add(vertex);
            }

            /// <summary>
            /// Adds multiple dependees to this Vertex.
            /// </summary>
            void AddDependees(IEnumerable<Vertex> vertices)
            {
                foreach (Vertex v in vertices)
                {
                    this.AddDependee(v);
                }
            }

            /// <summary>
            /// Removes a single dependent from this Vertex.
            /// </summary>
            void RemoveDependent(Vertex vertex)
            {
                dependents.Remove(vertex);
                size--;
            }

            /// <summary>
            /// Removes all dependents from this vertex.
            /// </summary>
            void RemoveAllDependents()
            {
                dependents = new HashSet<Vertex>();
                size = 0;
            }

            /// <summary>
            /// Removes a single dependee from this Vertex.
            /// </summary>
            void RemoveDependee(Vertex vertex)
            {
                dependees.Remove(vertex);
            }

            /// <summary>
            /// Removes all dependees from this Vertex.
            /// </summary>
            void RemoveAllDependees()
            {
                dependees = new HashSet<Vertex>();
            }

            /// <summary>
            /// The hash code of a Vertex is the hash code of its Name.
            /// </summary>
            public override int GetHashCode()
            {
                return key.GetHashCode();
            }
        }
    }
}
