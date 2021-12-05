using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{

    public class KaContext
    {
        protected ConditionList _conditions=new ConditionList();

        public KaContext(params Condition[] cs)
        { 
            foreach(Condition c in cs)
            { 
                add(c);
            }

        } 
        public KaContext(KaContext kc)
        {
            foreach (Condition c in kc.conditions)
            { 
                add(c);
            }

        }

        public KaContext(ConditionList conditions)
        {
            _conditions = conditions; 
        }
        public ConditionList add(ConditionList cl)
        {
            foreach(Condition c in cl)
            { 
                _conditions.AddLast(c);
            }
            return _conditions;
        }
        public Condition add(Condition c)
        { 
            _conditions.AddLast(c);
            return c;
        }
 
        public ConditionList conditions
        {
            get { return _conditions; }
        }
        public int check(BDI bdi, ref BindingList bl)
        { 
            LinkedList<Condition>.Enumerator itor = _conditions.GetEnumerator();
            while (itor.MoveNext()) 
            {
                Condition c = itor.Current;
                if (c.check(bdi,ref bl) == 0)
                { 
                    break;
                }
            } ;
            return bl.Count;
        }
        public bool confirm(BDI bdi, Binding b)
        {
            //LinkedListNode<Condition> node = _conditions.First;
            //while (node != null)
            //{
            //    if (!node.Value.confirm(b))
            //        return false;
            //    node = node.Next;
            //}
            //return true;

            LinkedList<Condition>.Enumerator itor = _conditions.GetEnumerator();
            while (itor.MoveNext())
            {
                if (!itor.Current.confirm(bdi,b))
                    return false;
            }
            return true;
        }

    }

    // KaBody describes the sequence of actions, a procedure, to be taken 
    //in order to accomplish a goal.the body may contain provided UM-PRS 
    //actions and user-defined primitive functions, and can be organized 
    //into branches which are conditional executed and loops which can be
    //conditionally repeated.
    public class KaBody
    {
        public KaBodyElementList _elements = new KaBodyElementList();  

        public enum StatusType
        {
            KA_UNTRIED,
            KA_SUCCESS,
            KA_FAILURE,
            KA_ACTIVE,
            KA_BLOCKED,
            KA_ABANDONED
        }; 
 
        public KaBody(KaBodyElement be = null)
        { 
        }
        public KaBody(params KaBodyElement[] elems)
        {
            foreach(KaBodyAtomicElement ke in elems)
            {
                _elements.AddLast(ke);
            } 
        }
        public KaBodyElementList elements
        {
            get { return _elements; }
        }
        public KaBodyElement GetStartElement()
        {
            if(_elements.First==null)
            {
                utils.print("ERROR:the start element in the body is null");
                return null;
            }
            return _elements.First.Value; 
        }
 
        public LinkedListNode<KaBodyElement> add_first(KaBodyElement ke)
        {
            return _elements.AddFirst(ke); 
        }
        public LinkedListNode<KaBodyElement> add_last(KaBodyElement ke)
        {
            return _elements.AddLast(ke);
        }
        public LinkedListNode<KaBodyElement> add_before(LinkedListNode<KaBodyElement> node,KaBodyElement ke)
        {
            return _elements.AddBefore(node,ke); 
        }
        public LinkedListNode<KaBodyElement> add_after(LinkedListNode<KaBodyElement>node,KaBodyElement ke)
        {
            return _elements.AddAfter(node, ke);
        }

    }
    //A Knowledge Area(KA) is a declarative procedure specification of how
    //to satisfy a system goal or query. It consists of the Goal, the Context
    //and the body. 
    //(1)Goal(a goal,query,test,or world model assertion or retraction) for executing the
    //KA. During execution of PRS, this Goal will be matched against 
    //top-level goals, as specified by the user before the
    //system is started, and against KA body actions that specify a goal
    //or query. If the KA’s goal matches, and the context is satisfied
    //(as explained below), it is considered an applicable approach to 
    //solving the goal and may be intended and executed. 
    //(2)Context: in which the KA is applicable, a graphic network called
    //the Body which specifies what is required to satisfy the purpose in terms
    //of primitive functions, subgoals,conditonal branches, etc,and a symbol
    //table which holds values for varibles when a KA is instantiated for a
    //specific situation.The Context consists of a mixed sequence of patterns
    //to be matched against the WM and expressions to be satisfied using the 
    //the variable bindings generated during the matching.
    //(3)Body: describes the procedure's steps,consisting of a network of 
    //actions. The body can be viewed as a plan schema. The schema is instantiated
    //with the bindings which are generated when purpose and context of the KA
    //are checked during SOAK generation
    public class Ka
    {
        private string _name;
        private string _doc;
        int _valid;

        private SymbolTable _symtab;
        private Expression _priority;
        private KaBody _effect;
        private KaBodyAtomicElement _failure;

        private Action _goal;
        private KaContext _context;
        private KaBody _body;        

        public Ka(string name,string doc="")
        {
            _name = name;
            _doc = doc;

            _symtab = new SymbolTable();
            _valid = -1;
            _goal = null;
            _context = null;
            _effect = null;
            _body = null;
            _failure = null;
        }
        public int check_context(BDI bdi, ref BindingList bs)
        {
            return _context != null ? _context.check(bdi,ref bs) : 1;
        }
        public bool confirm_context(BDI bdi, Binding b)
        {
            return _context != null ? _context.confirm(bdi,b) : true;
        }
        public double eval_priority(Binding b)
        {
            return (_priority != null) ? _priority.eval(b).get_real() : 0.0;
        }
       

        public SymbolTable get_symtab() { return _symtab; }
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string doc
        { 
            get { return _doc; }
            set { _doc = value; }
        }
        public int is_valid() { return _valid; }
        public void disable() { _valid = 0; }
        public KaContext context
        {
            get { return _context; }
            set { _context = value; }
        }
        public Action goal
        {
            get { return _goal; }
            set { _goal = value; }
        }
        public KaBody body
        {
            get { return _body; }
            set { _body = value; }
        }
        public KaBody effect
        {
            get { return _effect; }
            set { _effect = value; }
        }
        public KaBodyAtomicElement get_failure() { return _failure; }


    }
    public class KaEnt:Symbol
    {
        private Ka _ka;
        public KaEnt(Ka ka):base(ka.goal.name)
        {
            _ka = ka;
        }
        public Ka ka
        {
            get { return _ka; } 
        }
    }

    // KA is hashed on the 'goal-name' of the KA (not the KA name) 
    public class KaTable :SymbolTable
    {
        public KaTable(int size=7):base(size)
        {

        }

        public new KaEnt lookup(int id)
        {
            return (KaEnt)base.lookup(id);
        }
        public new KaEnt lookup(string name)
        {
            return (KaEnt)base.lookup(name);
        }
        public KaEnt add_ka(Ka ka)
        {
            KaEnt ke = new KaEnt(ka);
            return add(ke);
        }

        private KaEnt add(KaEnt ke)
        {
            return (KaEnt)base.add(ke);
        } 

        public void disable(string name)
        {
            //foreach(KaEnt ke in _name_id_table)
            //{
            //    if (ke.ka.name == name)
            //        ke.ka.disable();
            //}
        }

    }

    public class KaTableBucketIterator
    {
        KaTable _table;
        Goal _goal;
        SymbolList _sl;
        SymbolList.Enumerator _itor;

        public KaTableBucketIterator(KaTable kt,Goal goal)
        {
            _table = kt;
            _goal = goal;
            _sl = kt.get_bucket(goal.name);
            _itor = _sl.GetEnumerator();
        }

        public Ka get_ka(out Binding ka_binding)
        {
            bool b = _itor.MoveNext();
            if (!b)
            {
                ka_binding = null;
                return null;
            }

            KaEnt ke = (KaEnt)_itor.Current;
            Ka ka = ke.ka;
            ka_binding = new Binding(ka.get_symtab());

            if(!_goal.goal_action.is_eligible(ka.name,ka_binding))
            {
                return get_ka(out ka_binding);
            }

            Relation ka_relation = ka.goal.get_relation();
            if (_goal.match_relation(ka_relation, ka_binding))
                return ka;
            else
            {
                return get_ka(out ka_binding);
            }
        }

    }

}
