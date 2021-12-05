using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.CMap
{
    /// <summary>
    /// /////////////////////////
    /// </summary>
    public class ConceptVertex : IComparable<ConceptVertex>
    {
        protected string _content;
        public ConceptVertex(string content)
        {
            _content = content;
        }

        public int CompareTo(ConceptVertex obj)
        {
            if (obj == null) return 1;
            else
                return this._content.CompareTo(obj._content); 
        }

        public string content
        {
            get { return _content; }
            set { _content = value; }
        }
    }
}
