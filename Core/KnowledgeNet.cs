using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KRLab.Core
{
    public abstract class KnowledgeNet
    { 
        public abstract string Name
        {
            get; 
        } 

        public abstract NetGraphType Type
        {
            get; 
        }         

        public abstract NodeBase CreateNode();
        public abstract NodeRelationship AddRelationship(NodeBase first,NodeBase second,RelationshipType type);

    }
}
