using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    // each Action in a KA, one line of KA's BODY, can specify a goal or condition to ACHIEVE,
    // WAIT for, MAINTAIN, or QUERY.In addition, a Ka action can be low-level function to EXECUTE
    // directly , an ASSERTION of a fact to the world model, a RETRACTION of a fact from the world 
    // model, an UPDATE of a fact in the world model, a FACT or a RETRIEVE statement that retrieves 
    // relation values from the world model, or an ASSIGN statement that assign variables the results
    // of run-time computations.Furthermore, iteration and branching are accomplished through WHILE,DO,
    // OR,and AND actions. For convenience when testing KA failure(or other reasons) there is also a FALL
    // action which is an action that always fails.
    /*
        Actions are the arcs in a KA body that constitute either primitive actions
    or subgoals to achieve. A "primitive" action is a behavior or activity that
    can be executed directly. Any other type of actions represents:
    (1)a goal that needs achievement, maintenance, or to be waited upon
    (2)a query,
    (3)a test,
    (4)or an assertion or retraction of world model information.
    Actions are represented by a base public class that holds information regarding
    the KA in which the action is found and the action name. Each of the types 
    of actions are represented by a derived public class that maintains such information
    as function pointers (for a primitive action),the expression to evaluate(for 
    a test), a goal or query expression, or a world model relation to assert or 
    retract.
    */

    public abstract class Action
    { 
        protected string _name;  

        /// <summary>
        /// used for action specification
        /// </summary>
        public enum ELEMTYPE
        {
            SIMPLE,
            WHEN,
            WHILE,
            DO,
            BRANCH,
            ATOMIC,
            NONE//有些Action不能作为KaBodyElement
        }
        public enum ActStatus
        {
            ACT_SUCCEEDED,      // Ran to completion successfully
            ACT_FAILED,         // Found impossible to finish 
            ACT_CANNOT_EXECUTE  // Not directly executable. eg. goal actions
        }

        //Act type
        public enum ActType
        {
            ACT_UNDEFINED,
            ACT_PRIMITIVE,//primitive action 原子行动
            ACT_LOAD,
            ACT_PARSE,
            ACT_ASSIGN,//
            ACT_FACT,
            ACT_RETRIEVE,//
            ACT_TEST,
            ACT_ASSERT, //
            ACT_FAIL,//
            ACT_RETRACT,
            ACT_UPDATE,//
            ACT_POST,
            ACT_UNPOST,
            ACT_GOAL_ACTION,
            ACT_ACHIEVE,//
            ACT_MAINTAIN,// 
            ACT_WAIT, //
            ACT_QUERY//
        };

        public Action(string name)
        {
            _name = name; 

        }

        public string name
        {
            get { return _name; }
        }
        public virtual Relation get_relation()
        {
            return null;
        }
        public abstract ELEMTYPE elem_type();
        public abstract bool is_executable_action();
        public abstract ActStatus execute(BDI bdi, Binding b);
 

        public virtual ActType type()
        {
            return ActType.ACT_UNDEFINED;
        }

    }

    public class GoalAction: Action
    {
        protected Relation _goal;
        protected Expression _priority;
        protected ExpList _by;
        protected ExpList _not_by;

        public bool is_eligible(string name,Binding binding)
        { 
            Value str;
            if(_by!=null)
            {
                foreach(Expression e in _by)
                {
                    str = e.eval(binding);
                    if (str != null && str.get_name() == _name) return true;
                }
                return false;
            }
            if(_not_by!=null)
            {
                foreach (Expression e in _not_by)
                {
                    str = e.eval(binding);
                    if (str != null && str.get_name() == _name) return false;
                }
                return true;
            }
            return true;
        }
        public GoalAction(string name,
            Relation goal,
            Expression priority=null,
            ExpList by=null,
            ExpList not_by=null):base(name)
        {
            _goal = goal;
            _priority = priority;
            _by = by;
            _not_by = not_by;
        }
        public GoalAction(GoalAction g,Binding binding):
            base(g.name)
        {
            _goal = new Relation(g._goal, binding);
            _priority = new Value(g.eval_priority(binding));
            _by = utils.explist_eval_new(g._by, binding);
            _not_by = utils.explist_eval_new(g._not_by, binding);
        }
        public override ActType type()
        {
            return ActType.ACT_GOAL_ACTION;
        }
        public override Relation get_relation()
        {
            return _goal;
        }
        public Expression get_priority()
        {
            return _priority;
        }
        public double eval_priority(Binding binding)
        {
            return (_priority != null) ? _priority.eval(binding).get_real() : 0.0;
        }
        public Expression set_priority(Expression priority)
        {
            return _priority = priority;
        }

        public override ELEMTYPE elem_type()
        {
            return ELEMTYPE.SIMPLE;
        }

        public override bool is_executable_action()
        {
            return false;
        }
        public override ActStatus execute(BDI bdi, Binding b)
        {
            return ActStatus.ACT_CANNOT_EXECUTE;
        }
        public ExpList set_by(ExpList by)
        {
            return _by = by;
        }
        public ExpList set_not_by(ExpList not_by)
        {
            return _not_by = not_by;
        }

    }

    /* 
     * An ACHIEVE action causes the system to subgoal from the currently
     * executing goal.This triggers the system for KAs in the KA library 
     * that can satisfy the goal goal_name given the current context.
     */
    public class Achieve :GoalAction
    {
        public Achieve(string name,Relation goal,Expression priority=null,
            ExpList by=null,ExpList not_by=null):base(name,goal,priority,by,not_by)
        {

        }
        public override ActType type()
        {
            return ActType.ACT_ACHIEVE;
        }
    }

    public class Post:Action
    {
        protected GoalAction _goal_action;

        public Post(GoalAction goal_action):base("POST")
        {
            _goal_action = goal_action;
        }
        public override ELEMTYPE elem_type()
        {
            return ELEMTYPE.SIMPLE;
        }
        public override ActType type()
        {
            return Action.ActType.ACT_POST;
        }
        public override bool is_executable_action()
        {
            return true;
        }
        public override ActStatus execute(BDI bdi,Binding b)
        {
            GoalAction ga = new GoalAction(_goal_action, b); 
            bdi.desires.add(ga); 
            return ActStatus.ACT_SUCCEEDED;

        }
    }

    public class Fact:Action
    {
        ///information from the world model can be accessed using this action
        ///
        private Relation _relation;
        public Fact(string name):base(name)
        {

        }
        public Fact(string name,params Expression[] args):base(name)
        {
            ExpList el = new ExpList();
            foreach(Expression e in args)
            {
                el.AddLast(e);
            }
            _relation = new Relation(name, el);
        }
        public Fact(Relation r):base(r.name)
        {
            _relation = r;
        }
        public override ELEMTYPE elem_type()
        {
            return ELEMTYPE.SIMPLE;
        }
        public override ActType type() { return ActType.ACT_FACT; }
        public override bool is_executable_action()
        {
            return true;
        }
        public override Relation get_relation()
        {
            return _relation;
        }
        public override ActStatus execute(BDI bdi, Binding b)
        {
            return bdi.beliefs.match(_relation, b) ? ActStatus.ACT_SUCCEEDED : ActStatus.ACT_FAILED;
        }
    }

    public class Primitive : Action
    {

        public delegate Value FUNC(int arity, ExpList args, Binding binding=null);

        protected int _arity;
        protected ExpList _args;
        protected FUNC _func;

        public Primitive(string name):base(name)
        {
            _func = null;
            _arity = 0;
            _args = null;

        }
        public Primitive(string name, FUNC f,ExpList el):
            base(name)
        {
            _func = f;
            _args = el;
            _arity = el!=null ? el.Count : 0;
        }
        public int arity
        {
            get { return _arity; }
        }

        public override ELEMTYPE elem_type()
        {
            return ELEMTYPE.SIMPLE;
        }
        public override ActType type()
        { return ActType.ACT_PRIMITIVE; }

        public override bool is_executable_action()
        {
            return true;
        }
        public override ActStatus execute(BDI bdi, Binding b)
        {
            return _func(_arity, _args, b).eval(b).is_true() ? ActStatus.ACT_SUCCEEDED : ActStatus.ACT_FAILED;
        }
    }

    public class Print:Primitive
    {
        public static Value print(int arity,ExpList args,Binding b=null)
        {
            if (arity < 0) return Value.False; 
            foreach(Expression e in args)
            {
                e.eval(b).print(b);
            }
            Console.WriteLine("");
            //This primitive function has successfully completed.
            return Value.True;
        }
        public Print(int arity,ExpList args):base("Print")
        {
            _arity = arity;
            _args = args;
            _func = print;
        }
        public Print(Relation r):base(r.name)
        {
            _arity = r.arity + 1; 
            _args = r.args;
            _args.AddFirst(new Value(r.name));
            _func = print;
        }
        public Print(Value v):base(v.get_name())
        {
            _arity = 1;
            _args = new ExpList(); 
            _args.AddFirst(v);
            _func = print;
        }
    }


}
