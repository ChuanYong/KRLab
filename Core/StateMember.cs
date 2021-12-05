using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace KRLab.Core
{
    public class StateMember:Member
    {
        public StateMember(string name, CompositeNode parent)
            : base(name, parent)
        {
        }

        public StateMember(CompositeNode parent)
            : this("NewStateMember",parent)
        {
        }

        public sealed override MemberType Type
        {
            get { return Core.MemberType.State; }
        }

        protected override Member Clone(CompositeNode newParent)
        {
            StateMember state = new StateMember(newParent);
            state.CopyFrom(this);
            return state;
        } 
   
    }
}
