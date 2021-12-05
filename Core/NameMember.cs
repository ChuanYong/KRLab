using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KRLab.Core
{
    public class NameMember:Member
    {
        public NameMember(CompositeNode parent)
            : this("NewName", parent)
        { 
        }

        public NameMember(string name, CompositeNode parent):
            base(name,parent)
        {
            Type = MemberType.Name;
        }        

        protected override Member Clone(CompositeNode newParent)
        {
            NameMember name = new NameMember(newParent);
            name.CopyFrom(this);
            return name;
        }
    }
}
