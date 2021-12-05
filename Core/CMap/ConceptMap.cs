using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KRLab.Core.DataStructures.Common;
using KRLab.Core.DataStructures.Lists;
using KRLab.Core.DataStructures.Graphs;

using KRLab.Core.Algorithms.Graphs;

namespace KRLab.CMap
{

    /// <summary>
    /// //////////////////////////////////////
    /// </summary>
    public class ConceptMap  
    {
        protected string _topic;
        protected DirectedWeightedSparseGraph<ConceptVertex> _graph=new DirectedWeightedSparseGraph<ConceptVertex>();

        public DirectedWeightedSparseGraph<ConceptVertex> Graph
        {
            get { return _graph; }
        }

        public ConceptMap(string topic)
        {
            _topic = topic;
        }

        public string topic
        {
            get { return _topic; }
        }

        public void add_concepts(ConceptVertex[] cs)
        {
            _graph.AddVertices(cs);
        }

        public void add_edge(ConceptVertex fr, ConceptVertex to, CWeight rational)
        {
            _graph.AddEdge(fr, to, rational);
        }

        public IEnumerable<ConceptVertex> topological_sort()
        {
            return TopologicalSorter.Sort<ConceptVertex>(_graph);
        }

        public void print()
        {
            Console.WriteLine("Concept Map: " + _topic);
            IEnumerable<ConceptVertex> nodes=_graph.Vertices;
            foreach(ConceptVertex node in nodes)
            {
                Console.WriteLine(node.content);
            }
        }

    }
}
