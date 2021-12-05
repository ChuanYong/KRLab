using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core
{
    public class BayesianNetTemplate:KnowledgeNet
    {
        private static BayesianNetTemplate _Instance;

        public override string Name
        {
            get { return "BayesianNet"; }
        }

        public static BayesianNetTemplate Instance
        {
            get { return _Instance; }
        }

        public sealed override NetGraphType Type
        {
            get { return NetGraphType.BayesianNet; }
        }

        static BayesianNetTemplate()
        { }

        public override NodeBase CreateNode()
        {
            return new BasicBayesianNode("NewBNode"); 
        }

        public override NodeRelationship AddRelationship(NodeBase first, NodeBase second, RelationshipType type)
        {
            BasicBayesianNode fNode = (BasicBayesianNode)first;
            BasicBayesianNode sNode = (BasicBayesianNode)second;

            return new BayesianRelation(fNode, sNode);
        }
    }
}
