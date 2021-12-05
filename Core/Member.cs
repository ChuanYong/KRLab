using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KRLab.Translations;

namespace KRLab.Core
{
    public abstract class Member:Element
    {
        private string _Name;
        private MemberType _Type;
        private CompositeNode _Parent;

        protected Member(string name, CompositeNode parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            //if (parent.KnowledgeNet != this.KnowledgeNet)
            //    throw new ArgumentException(Strings.ErrorKRDoNotEqual);

            Parent = parent;
            Name = name;            
        }

        public CompositeNode Parent
        {
            get { return _Parent; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (_Parent != value)
                {
                    _Parent = value;
                    Changed();
                }
            }
        }

        public virtual string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (value != _Name)
                {
                    _Name = value;
                    Changed();
                }
            }
        }

        public virtual MemberType Type
        {
            get
            {
                return _Type;
            }
            set
            {
                if (value != _Type)
                {
                    _Type = value;
                    Changed();
                }
            }
        }
        
        protected abstract Member Clone(CompositeNode newParent);

        protected virtual void CopyFrom(Member member)
        {
            _Name = member._Name;
            _Type = member._Type;
            _Parent = member._Parent;
        }

    }
}
