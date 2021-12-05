using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KRLab.Core 
{
    public abstract class NodeRelationship:Relationship
    {
        NodeBase first;
        NodeBase second;

        protected NodeRelationship(NodeBase first, NodeBase second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            this.first = first;
            this.second = second;
        }

        public sealed override IEntity First
        {
            get
            {
                return first;
            }
            protected set
            {
                first = (NodeBase)value;
            }
        }

        public sealed override IEntity Second
        {
            get
            {
                return second;
            }
            protected set
            {
                second = (NodeBase)value;
            }
        }
    }
}
