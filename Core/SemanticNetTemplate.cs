using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core
{
    public sealed class SemanticNetTemplate:KnowledgeNet
    {
        private static SemanticNetTemplate _Instance = new SemanticNetTemplate();
        private RelationshipType _relationType;

        public sealed override string Name
        {
            get { return "SemanticNet"; }
        }

        public sealed override NetGraphType Type
        {
            get { return NetGraphType.SemanticNet; }
        }

        public static SemanticNetTemplate Instance
        {
            get { return _Instance; }
        }

        static SemanticNetTemplate()
        {
        }

        private SemanticNetTemplate()
        {
        }

        public override NodeBase CreateNode()
        {
            BasicSemanticNode node=new BasicSemanticNode("NewSNode");
            return node;
        }

        public override NodeRelationship AddRelationship(NodeBase first, NodeBase second, RelationshipType type)
        {
            BasicSemanticNode fNode = (BasicSemanticNode)first;
            BasicSemanticNode sNode = (BasicSemanticNode)second;
            _relationType = type;

            return new SNRelationship(fNode, sNode); 

        }
    }
}
