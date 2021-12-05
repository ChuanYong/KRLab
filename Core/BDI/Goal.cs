using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    //
    //Goals are the world states that the system is trying to bring
    //about(or maintain,etc.). A goal can be either a top-level goal,
    //which controls the system's highest order behavior, or a subgoal
    //activated by the execution of a KA arc.
    //
    public class Goal
    {
        private GoalAction _goal;
        private Goal _sub_goal;
        private Goal _prev_goal;
        // Flag of whether goal is new or not.
        //1:new,0:old
        private bool _new_goal;

        private StatusType _status;
        private SoakElement _intention;
        public KaRuntimeFrame _runtime_frame;

        private IntentionStructure _is;

        private BDI _bdi;

        public Goal(BDI bdi,GoalAction ga,Goal prev=null)
        {
            _goal = ga;
            _sub_goal = null;
            _prev_goal = prev;
            if (prev != null)
                prev.sub_goal=this;
            _new_goal = true;
            _intention = null;
            _is = null;
            _runtime_frame = null;

            _status = StatusType.IS_UNTRIED;
            _bdi = bdi;

        }

        public bool generate_soak()
        {
            if((_status!=StatusType.IS_SUCCESS && 
                _status !=StatusType.IS_ABANDONED) &&
                _sub_goal==null
                )
            {
                if (_new_goal)
                    return true;
                if(_intention==null)
                {
                    // If anything in the world model has changed then we need to
                    // consider the goal for SOAK generation
                    if (_bdi.beliefs.any_new())
                        return true; 
                }

            }
            return false;
        }
        public Binding get_goal_binding()
        {
            return (_prev_goal != null) ? _prev_goal.get_intention_binding() : null;
        }

        public Binding get_intention_binding()
        {
            return (_intention != null) ? _intention._binding : null;
        }

        public bool is_valid()
        {
            return (_status != StatusType.IS_SUCCESS) && 
                (_status != StatusType.IS_ABANDONED);
        }


        public bool is_new()
        {
            return _new_goal;
        }

        public bool is_toplevel_goal()
        {
            return _prev_goal == null;
        }
        public bool is_leaf_goal()
        {
            return _sub_goal == null;
        }

        public bool is_stack_blocked()
        {
            utils.print("TODO for Goal::is_stack_blocked ");
            System.Environment.Exit(0);
            return false;
        }

        public double eval_priority()
        {
            Expression goal_priority = _goal.get_priority();
            return (goal_priority != null) ? goal_priority.eval(get_goal_binding()).get_real() : 0.0;
        }
        public bool match_relation(Relation patt_relation, Binding patt_binding)
        {
            return utils.unify(patt_relation, patt_binding, get_relation(), get_goal_binding());
        }
        public bool match_goal(GoalAction goal_action, Binding goal_action_binding)
        {
            if (match_relation(goal_action.get_relation(), goal_action_binding))
            {
                if (goal_action.get_priority() == null || eval_priority() == goal_action.eval_priority(goal_action_binding))
                {
                    return true;
                }
            }
            return false;
        }


        public GoalAction goal_action
        {
            get { return _goal; }
        }
        public string name
        {
            get { return _goal != null ? _goal.name : null; }
        }
        public Relation get_relation()
        {
            return (_goal != null) ? _goal.get_relation() : null;
        }

        public void set_new()
        {
            _new_goal = true;
        }
        public void clear_new()
        {
            _new_goal = false;
        }

        public Goal sub_goal
        {
            get { return _sub_goal; }
            set { _sub_goal = value; }
        }

        public Goal prev_goal
        {
            get { return _prev_goal; }
            set { _prev_goal = value; }
        }

        public SoakElement get_intention() { return _intention; }
        public SoakElement set_intention(SoakElement e)
        {
            _intention = e;
            return _intention;
        }         

        public IntentionStructure Is
        {
            get { return _is; }
            set { _is = value; }
        } 

        public StatusType status
        {
            get { return _status; }
            set { _status = value; }
        }


        public Goal get_current_goal()
        {
            return _is.get_current_goal();
        } 

        public void remove_intention(bool failed)
        {
            //removes the entire subgoal stack,including removing subgoals
            //from the GoalSet and executing the FAILURE sections of each
            //subgoal
            Goal a_goal;
            KaBodyAtomicElement failure_section;
            a_goal = this;

            while(a_goal!=null && a_goal.sub_goal!=null)
            {
                a_goal = a_goal.sub_goal;
            }

            while(a_goal!=null && (a_goal!=this))
            {
                if(failed && (a_goal.get_intention()!=null))
                {
                    failure_section = a_goal.get_intention().ka.get_failure();
                    if(failure_section!=null)
                    {
                        a_goal.runtime_frame = failure_section.new_runtime_frame(_bdi);
                        a_goal.runtime_frame.execute(a_goal.get_intention_binding(), a_goal);
                    }
                }
                a_goal.set_intention(null);
                ///update the global goal_list
                _bdi.desires.remove(a_goal);
                
                a_goal = a_goal.prev_goal;
            }

            if(failed)
            {
                if(_intention!=null)
                {
                    failure_section = _intention.ka.get_failure();
                    if(failure_section!=null)
                    {
                        this.runtime_frame = failure_section.new_runtime_frame(_bdi);
                        Goal g=this;
                        this.runtime_frame.execute(a_goal.get_intention_binding(),g);
                        _update(g);
                    }
                }
            }
            _intention = null;
            _sub_goal = null;
            _runtime_frame = null;
        }

        public KaRuntimeFrame runtime_frame
        {
            get { return _runtime_frame; }
            set { _runtime_frame = value; }
        }

        private void _update(Goal g)
        {
            this._goal = g._goal;
            this._intention = g._intention;
            this._is = g._is;
            this._new_goal = g._new_goal;
            this._prev_goal = g._prev_goal;
            this._runtime_frame = g._runtime_frame;
            this._status = g.status;
            this._sub_goal = g._sub_goal;
            this._bdi = g._bdi;
        }

    }

    public class GoalSet 
    {
        private LinkedList<Goal> _goals = new LinkedList<Goal>();
        private BDI _bdi;
        public GoalSet(BDI bdi)
        {
            _bdi = bdi;
        }
        public bool all_goals_done()
        { 
            LinkedList<Goal>.Enumerator itor = _goals.GetEnumerator();
            while (itor.MoveNext())
            {
                if (itor.Current.is_toplevel_goal() && itor.Current.status != StatusType.IS_SUCCESS)
                    return false;
            }

            return true;
        }
        public void renew_leaf_goals()
        {
            //foreach(Goal g in _goals)
            //{
            //    if(g.is_leaf_goal())
            //    {
            //        g.set_new();
            //    }
            //}
            LinkedList<Goal>.Enumerator itor = _goals.GetEnumerator();
            while (itor.MoveNext())
            {
                if (itor.Current.is_leaf_goal())
                    itor.Current.set_new();
            } 

        }
        public void remove(Goal gl)
        {
            LinkedListNode<Goal> gl_node = _goals.Find(gl);
            if (gl_node != null)
            {
                if (gl_node.Value.prev_goal != null)
                {
                    gl_node.Value.prev_goal.sub_goal = null;
                }
                _goals.Remove(gl_node);
            }
            //_goals.Remove(gl);
        }

        public Goal add(GoalAction ga,Goal prev_ga=null)
        {
            Goal new_ga = new Goal(_bdi,ga, prev_ga);
            LinkedListNode<Goal> node = _goals.First;
            _goals.AddLast(new_ga);            
            return new_ga;
        }

        //public void add(params GoalAction[] gs)
        //{
        //    foreach(GoalAction g in gs)
        //    {
        //        add(g);
        //    }
        //}
        public LinkedList<Goal> get_goals()
        {
            return _goals;
        }
        public int size()
        {
            return _goals.Count;
        }
    }
}
