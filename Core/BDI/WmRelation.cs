using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{ 
    public class WmRelation : Symbol
    {
        //the content of the WM Relation Table
        protected Relation _relation;
        //set to 1 for new entry and cleared at every cycle
        protected bool _new_tag;
        protected void _init(Relation r, bool b = true)
        {
            _relation = r;
            _new_tag = b;
        }

        public WmRelation(string name, ExpList args = null) : base(name)
        {
            _init(new Relation(name, args));
        }
        public WmRelation(Relation rel) : base(rel.name)
        {
            _init(rel);
        }
        public WmRelation(WmRelation rel) : base(rel)
        {
            _init(new Relation(rel._relation));
        }
 
        public Relation get_relation()
        {
            return _relation;
        }
        public bool is_new() { return _new_tag; }
        public void clear_new() { _new_tag = false; }
        public bool match_relation(Relation patt_rel, Binding patt_bind)
        {
            Relation dst_rel = get_relation();
            if (utils.unify(patt_rel, patt_bind, dst_rel, null))
            { 
                if (patt_bind != null)
                    patt_bind.check_new_wm_binding(is_new());
                return true;
            }
            else
                return false;
        }
    }
}
