using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core
{
    public sealed class ConceptMapTemplate:KnowledgeNet
    {
        private static ConceptMapTemplate _Instance;

        public static ConceptMapTemplate Instance
        {
            get { return _Instance; }
        }

        public override string Name
        {
            get
            {
                return "ConceptMap";
            }
        }

        public override NetGraphType Type
        {
            get
            {
                return NetGraphType.ConceptMap;
            } 
        }

        static ConceptMapTemplate()
        {
        }

        public override NodeBase CreateNode()
        {
            return new BasicConceptualNode("NewCNode");
        }

        public override NodeRelationship AddRelationship(NodeBase first, NodeBase second, RelationshipType type)
        {
            BasicConceptualNode fNode = (BasicConceptualNode)first;
            BasicConceptualNode sNode = (BasicConceptualNode)second;

            CMRelationship relationship = new CMRelationship(fNode,sNode);
            return relationship;
            
        }
    }
}
