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
        /// Represents the graph as a Dictionary of vertices, where the key is the name of the Vertex.
        /// Each Vertex maintains references to its dependents and dependees.
        /// </summary>
        private Dictionary<string, Vertex> vertices;

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            vertices = new Dictionary<string, Vertex>();
        }

        /// <summary>
        /// Creates a dependency graph that is an independent copy of dg.
        /// </summary>
        public DependencyGraph(DependencyGraph dg)
            : this()
        {
            foreach (KeyValuePair<string, Vertex> kvp in dg.vertices)
            {
                this.vertices.Add(kvp.Key, (Vertex) kvp.Value.Clone());
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
                foreach (KeyValuePair<string, Vertex> pair in vertices)
                {
                    count += pair.Value.Size;
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

            Vertex v;

            if (vertices.TryGetValue(s, out v))
            {
                if (v.GetAllDependents().Any())
                {
                    return true;
                }
            }

            return false;
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

            Vertex v;

            if (vertices.TryGetValue(s, out v))
            {
                if (v.GetAllDependees().Any())
                {
                    return true;
                }
            }

            return false;
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

            Vertex v;

            if (vertices.TryGetValue(s, out v))
            {
                foreach (string name in v.GetAllDependentsNames())
                {
                    yield return name;
                }
            }
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// Throws an exception if null arguments are provided.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }

            Vertex v;

            if (vertices.TryGetValue(s, out v))
            {
                foreach (string name in v.GetAllDependeesNames())
                {
                    yield return name;
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

            Vertex left;
            Vertex right;
            
            // If a vertex doesn't exist, add it.
            if (!vertices.TryGetValue(s, out left))
            {
                vertices.Add(s, left = new Vertex(s));
            }

            if (!vertices.TryGetValue(t, out right))
            {
                vertices.Add(t, right = new Vertex(t));
            }

            // Add the dependency
            left.AddDependent(right);
            right.AddDependee(left);
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

            Vertex left;
            Vertex right;

            // If either vertex doesn't exist, there's nothing to remove
            if (!vertices.TryGetValue(s, out left) || !vertices.TryGetValue(t, out right))
            {
                return;
            }

            // Remove the dependency
            left.RemoveDependent(right.Name);
            right.RemoveDependee(left.Name);
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

            Vertex left;

            if (!vertices.TryGetValue(s, out left))
            {
                vertices.Add(s, left = new Vertex(s));
            }

            left.RemoveAllDependents();

            // Remove s from the dependees list of each of its dependents
            foreach (KeyValuePair<string, Vertex> pair in vertices)
            {
                pair.Value.RemoveDependee(s);
            }

            // Add the new dependents
            foreach (string name in newDependents)
            {
                AddDependency(s, name);
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

            Vertex right;

            if (!vertices.TryGetValue(t, out right))
            {
                vertices.Add(t, right = new Vertex(t));
            }

            right.RemoveAllDependees();

            // Remove t from the dependents list of each of its dependees
            foreach (KeyValuePair<string, Vertex> pair in vertices)
            {
                pair.Value.RemoveDependent(t);
            }

            // Add the new dependees
            foreach (string name in newDependees)
            {
                AddDependency(name, t);
            }
        }

        /// <summary>
        /// Provides a representation of a single vertex of a DependencyGraph.
        /// A Vertex knows all its dependents and dependees.
        /// </summary>
        private class Vertex : ICloneable
        {
            /// <summary>
            /// The unique name of this Vertex.
            /// </summary>
            private string key;

            /// <summary>
            /// The size of a Vertex is the number of its dependents.
            /// </summary>
            private int size;

            /// <summary>
            /// Lists all dependents of this Vertex.
            /// </summary>
            private Dictionary<string, Vertex> dependents;

            /// <summary>
            /// Lists all dependees of this Vertex.
            /// </summary>
            private Dictionary<string, Vertex> dependees;

            /// <summary>
            /// Constructs a new Vertex object with no dependents or dependees.
            /// </summary>
            internal Vertex(string name)
            {
                key = name;
                size = 0;
                dependents = new Dictionary<string, Vertex>();
                dependees = new Dictionary<string, Vertex>();
            }

            /// <summary>
            /// Constructs a new Vertex object with the given name, dependents, and dependees.
            /// Dependents and dependees are allowed to be empty.
            /// </summary>
            internal Vertex(string name, IEnumerable<Vertex> dependents, IEnumerable<Vertex> dependees)
                : this(name)
            {
                this.AddDependents(dependents);
                this.AddDependees(dependees);             
            }

            /// <summary>
            /// Returns the name of this Vertex.
            /// </summary>
            internal string Name
            {
                get { return key; }
            }

            /// <summary>
            /// Returns the size of this Vertex.
            /// </summary>
            internal int Size
            {
                get { return size; }
            }

            /// <summary>
            /// Returns all dependents of this Vertex.
            /// </summary>
            internal IEnumerable<Vertex> GetAllDependents()
            {
                foreach (KeyValuePair<string, Vertex> pair in this.dependents)
                {
                    yield return pair.Value;
                }
            }

            /// <summary>
            /// Returns an enumeration of the names of all dependents of this Vertex.
            /// </summary>
            internal IEnumerable<string> GetAllDependentsNames()
            {
                foreach (KeyValuePair<string, Vertex> pair in this.dependents)
                {
                    yield return pair.Key;
                }
            }

            /// <summary>
            /// Returns all dependees of this Vertex.
            /// </summary>
            internal IEnumerable<Vertex> GetAllDependees()
            {
                foreach (KeyValuePair<string, Vertex> pair in this.dependees)
                {
                    yield return pair.Value;
                }
            }

            /// <summary>
            /// Returns an enumeration of the names of all dependees of this Vertex.
            /// </summary>
            internal IEnumerable<string> GetAllDependeesNames()
            {
                foreach (KeyValuePair<string, Vertex> pair in this.dependees)
                {
                    yield return pair.Key;
                }
            }

            /// <summary>
            /// Adds a single dependent to this Vertex. Does nothing if the dependent already exists.
            /// </summary>
            internal void AddDependent(Vertex v)
            {
                if (!dependents.ContainsKey(v.Name))
                {
                    dependents.Add(v.Name, v);
                    size++;
                }
            }

            /// <summary>
            /// Adds multiple dependents to this Vertex.
            /// </summary>
            internal void AddDependents(IEnumerable<Vertex> vertices)
            {
                foreach (Vertex v in vertices)
                {
                    this.AddDependent(v);
                }
            }

            /// <summary>
            /// Adds a single dependee to this Vertex. Does nothing if the dependee already exists.
            /// </summary>
            internal void AddDependee(Vertex v)
            {
                if (!dependees.ContainsKey(v.Name))
                {
                    dependees.Add(v.Name, v);
                }
            }

            /// <summary>
            /// Adds multiple dependees to this Vertex.
            /// </summary>
            internal void AddDependees(IEnumerable<Vertex> vertices)
            {
                foreach (Vertex v in vertices)
                {
                    this.AddDependee(v);
                }
            }

            /// <summary>
            /// Removes a single dependent from this Vertex.
            /// </summary>
            internal void RemoveDependent(string name)
            {
                if (dependents.Remove(name))
                {
                    size--;
                }
            }

            /// <summary>
            /// Removes all dependents from this vertex.
            /// </summary>
            internal void RemoveAllDependents()
            {
                dependents = new Dictionary<string, Vertex>();
                size = 0;
            }

            /// <summary>
            /// Removes a single dependee from this Vertex.
            /// </summary>
            internal void RemoveDependee(string name)
            {
                dependees.Remove(name);
            }

            /// <summary>
            /// Removes all dependees from this Vertex.
            /// </summary>
            internal void RemoveAllDependees()
            {
                dependees = new Dictionary<string, Vertex>();
            }

            /// <summary>
            /// The hash code of a Vertex is the hash code of its Name.
            /// </summary>
            public override int GetHashCode()
            {
                return key.GetHashCode();
            }

            /// <summary>
            /// Returns a clone (deep copy) of this Vertex
            /// </summary>
            public object Clone()
            {
                return new Vertex(this.key, this.GetAllDependents(), this.GetAllDependees());
            }
        }
    }
}
