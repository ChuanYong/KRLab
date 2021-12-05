using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.DataStructures.Graphs;

namespace KRLab.CMap
{
    class ElectricityMagnetismConceptMap:ConceptMap
    {
        public ElectricityMagnetismConceptMap():base("electricity magnetism")
        {
            ConceptVertex c0 = new ConceptVertex("cs1");
            ConceptVertex c1 = new ConceptVertex("cs2");
            ConceptVertex c2 = new ConceptVertex("assembly language");
            ConceptVertex c3 = new ConceptVertex("data struture");
            ConceptVertex c4 = new ConceptVertex("operation system");
            ConceptVertex c5 = new ConceptVertex("algorithms");

            ConceptVertex[] concepts = { c0, c1, c2, c3, c4, c5 };
             
            add_concepts(concepts);

            add_edge(c0, c1, new CWeight("then", 0));
            add_edge(c1, c2, new CWeight("then", 0));
            add_edge(c2, c3, new CWeight("then", 0));
            add_edge(c3, c4, new CWeight("then", 0));
            add_edge(c3, c5, new CWeight("then", 0));

        }
    }
}
