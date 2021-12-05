using System;
using System.Collections.Generic;

namespace KRLab.Core.BDI
{

    //Beliefs of an agent
    public class WmTable:SymbolTable
    {
        public WmTable(int wmtab_sz=4096,int wmtab_inc_sz=1024,int hashtab_sz=1009):
            base(wmtab_sz,wmtab_inc_sz,hashtab_sz) 
        {

        }

        public new WmRelation lookup(int id)
        {
            return (WmRelation)base.lookup(id);
        }
        public new WmRelation lookup(string name)
        {
            return (WmRelation)base.lookup(name);
        }
        public bool match(Relation r,Binding b)
        { 
            WmTableBucketIterator next_wr=new WmTableBucketIterator(this,r);
            return next_wr.get_wm_relation(b)!=null?true:false;

        } 
    

        public void bdi_assert(Relation r,Binding b=null)
        {
            if (!match(r, b))
                add(new WmRelation(new Relation(r, b)));
            else
            {
                Console.WriteLine("already have this realtion: "+r.name); 
            }

        }

        public void add(params Relation[] rs)
        {
            foreach(Relation r in rs)
            {
                bdi_assert(r);
            }

        }
        public void add(Relation[] rs,Binding[] bs=null)
        {
            if(bs!=null)
            {
                if (rs.Length != bs.Length)
                    return;
                for(int i=0;i<rs.Length;i++)
                {
                    bdi_assert(rs[i], bs[i]);
                }
            }
            else
            {
                for(int i=0;i<rs.Length;i++)
                {
                    bdi_assert(rs[i]);
                }
            }
            
        }
        public void retract(Relation r,Binding b=null)
        {

        }
        public void update(Relation old_r,Relation new_r,Binding b=null)
        {

        }
        public bool is_new(int id)
        {
            return lookup(id).is_new();
        }
        public bool any_new()
        {
            Console.WriteLine("WmTable::any_new---TODO");
            System.Environment.Exit(0);
            return false;
        }
        public void clear_new_all()
        {

        }
        public override void print()
        {
            Relation r;
            WmRelation w;
            Console.WriteLine("Beliefs");
            for(int i=0;i<_nextid;i++)
            {
                w = lookup(i);
                r = w.get_relation();
                Console.WriteLine(i + ":");
                r.print();
            }

        }
    }

    public class WmTableBucketIterator
    {
        WmTable _table;
        Relation _relation;
        SymbolList _sl;
        SymbolList.Enumerator _itor;

        public WmTableBucketIterator(WmTable wt,Relation relation)
        {
            _table = wt;
            _relation = relation;
            _sl = wt.get_bucket(relation.name);
            _itor = _sl.GetEnumerator(); 
        }

        public WmRelation get_wm_relation(Binding binding)
        {
            bool b = _itor.MoveNext();
            if (!b) return null;
            WmRelation wr = (WmRelation)_itor.Current;
            if (wr.match_relation(_relation, binding))
                return wr;
            else
                return get_wm_relation(binding);
        }
 
    }
}
