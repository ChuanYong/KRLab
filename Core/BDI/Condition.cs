using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    /// <summary>
    /// conditions are used for the context of a KA
    /// </summary>
    public class Condition
    {
        protected Condition _rep;
        protected int _active_value; 

        public enum ConditionType
        {
            COND_GOAL,
            COND_EXP,
            COND_FACT,
            COND_RETRIEVE,
            COND_UNDEFINED,
        }

        public Condition()
        {
            _rep = null;
            _active_value = 1; 
        } 
        public Condition set_positive()
        {
            _active_value = 1;
            return this;
        }
        public Condition set_negative()
        {
            _active_value = 0;
            return this;
        }
        public virtual string get_name()
        {
            return _rep.get_name();
        }
        public virtual ConditionType type()
        {
            return _rep.type();
        }
        public virtual int check(BDI bdi,ref BindingList bl)
        {
            return _rep.check(bdi,ref bl);
        }
        public virtual bool confirm(BDI bdi, Binding b)
        {
            return _rep.confirm(bdi,b);
        }
    }

    public class ExpCondition:Condition
    {
        Expression _expression;
        public ExpCondition(Expression e):base()
        {
            _expression = e;
        }
        public override string get_name()
        {
            return _expression.get_name();
        }
        public override ConditionType type()
        {
            return ConditionType.COND_EXP;
        }
        public override int check(BDI bdi, ref BindingList bl)
        { 
            foreach(Binding b in bl)
            {
                if(!_expression.eval(b).is_true())
                {
                    bl.Remove(b);
                }
            }
            return bl.Count;
        }
        public override bool confirm(BDI bdi, Binding b)
        {
            return _expression.eval(b).is_true();
        }
    }

    public class RelCondition:Condition
    {
        Relation _relation;
        public RelCondition(Relation r):base()
        {
            _relation = r;
        }
        public override string get_name()
        {
            return _relation.name;  
        }
        public Relation relation
        {
            get { return _relation; }
        }

        public override ConditionType type()
        {
            return ConditionType.COND_UNDEFINED;
        }
        public override int check(BDI bdi, ref BindingList bl)
        {
            return 0;
        }
        public override bool confirm(BDI bdi, Binding b)
        {
            return false;
        }

    }

    public class FactCondition:RelCondition
    { 
        public FactCondition(Relation r):base(r)
        {    
        }
        public override ConditionType type()
        {
            return ConditionType.COND_FACT;
        }
        public override int check(BDI bdi, ref BindingList bl)
        { 
            Binding new_b;
            LinkedListNode<Binding> node = bl.First;
            while (node!=null)
            {
                WmTableBucketIterator next_wr = new WmTableBucketIterator(bdi.beliefs, this.relation);
                for (new_b = new Binding(node.Value);
                    next_wr.get_wm_relation(new_b) != null;
                    new_b = new Binding(node.Value))
                {
                    bl.AddBefore(bl.Find(node.Value), new_b);
                }
                LinkedListNode<Binding> next = node.Next;
                bl.Remove(node.Value);
                node = next;
                
            }
            //BindingList.Enumerator itor = bl.GetEnumerator();
            //while(itor.MoveNext())
            //{
            //    WmTableBucketIterator next_wr = new WmTableBucketIterator(_bdi.beliefs, this.relation);
            //    for (new_b = new Binding(itor.Current);
            //        next_wr.get_wm_relation(new_b) != null;
            //        new_b = new Binding(itor.Current))
            //    { 
            //        bl.AddBefore(bl.Find(itor.Current), new_b); 
            //    }
            //    bl.Remove(itor.Current); 
            //    itor = bl.GetEnumerator();
            //}

            return bl.Count;
     
        }

        public override bool confirm(BDI bdi, Binding b)
        {
            return bdi.beliefs.match(this.relation, b);
        }
    }

    public class RetrieveCondition:RelCondition
    { 
        public RetrieveCondition(Relation r):base(r)
        { 
        }
        public override ConditionType type()
        {
            return ConditionType.COND_RETRIEVE;
        }
        public override int check(BDI bdi, ref BindingList bl)
        {
            utils.print("TODO:RetrieveCondition");
            return 0;
        }
        public override bool confirm(BDI bdi,Binding b)
        {
            b.unbind_variables(this.relation.args);
            return bdi.beliefs.match(this.relation, b);
        }
    }
}
