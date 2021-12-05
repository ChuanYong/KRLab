using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core;
using KRLab.Core.DataStructures.Graphs;

namespace KRLab.Core.SNet
{ 

    //nodes represent concepts, objects, features, events, time,
    public class SNNode:IComparable<SNNode> 
    {

        public string Name { get; set; }

        protected List<SNEdge> _InEdges;
        protected List<SNEdge> _OutEdges;

        public List<SNEdge> InEdges { get { return _InEdges; } }
        public List<SNEdge> OutEdges { get { return _OutEdges; } }

        public int EdgeCount
        {
            get { return InEdges.Count + OutEdges.Count; }
        }

        public SNNode(string name)
        {
            Name = name; 

            _InEdges = new List<SNEdge>();
            _OutEdges = new List<SNEdge>();
        }

        public void AddInEdge(SNEdge edge)
        {
            _InEdges.Add(edge);
        }

        public void AddOutEdge(SNEdge edge)
        {
            _OutEdges.Add(edge);
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(SNNode other)
        {
            if (other == null) return 1;
            else
            {
                return this.Name.CompareTo(other.Name);
            }
        }
    }
}
