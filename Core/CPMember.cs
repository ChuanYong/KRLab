using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace KRLab.Core
{
    public class CPMember:Member
    {
        public CPMember(string name, CompositeNode parent):
            base(name,parent)
        {
        }

        public CPMember(CompositeNode parent)
            : this("NewCPMember", parent)
        {
        }

        public sealed override MemberType Type
        {
            get { return MemberType.CP; }
        }

        protected override Member Clone(CompositeNode newParent)
        {
            CPMember cp = new CPMember(newParent);
            cp.CopyFrom(this);
            return cp;
        }

    }
}
