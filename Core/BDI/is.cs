using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{ 
    public enum StatusType
    {
        IS_UNTRIED,
        IS_SUCCESS,
        IS_FAILURE,
        IS_ACTIVE,
        IS_BLOCKED,
        IS_ABANDONED
    }
    
    public class IntentionStack
    {
        private Goal _top_level_goal;
        private Goal _current_goal;

        public IntentionStack()
        {
            _top_level_goal = null;
            _current_goal = null;
        }
        public IntentionStack(Goal g)
        {
            _top_level_goal = g;
            _current_goal = g;
        }
        //check to see if a goal in the stack is blocked
        public int is_stack_blocked()
        {
            Goal gl = _top_level_goal;
            while(gl!=null)
            {
                if (gl.status == StatusType.IS_BLOCKED)
                    return 1;
                gl = gl.sub_goal;
            }

            return 0;

        }

        public Goal top_level_goal
        {
            get { return _top_level_goal; }
            set { _top_level_goal = value; }
        }
        public Goal current_goal
        {
            get { return _current_goal; }
            set { _current_goal = value; }
        }
    }

    //maintains the runtime state of the set of the currently active goals.
    //The intention structure maintains information related to the runtime 
    //state of progress made toward the system's top-level or "system" goals.
    //The intention structure acts as the run-time stack for the system. It 
    //keeps track of the progress of each high-level goal and all of the subgoals
    //.The intention structure suspends, resumes,cancels, and proceeds with 
    //execution of goals in much the same way as an operation system. The 
    //intention structure maintains information about what KAs are currently
    //active, as well as what actions in each KA are to be executed next
    
    public class IntentionStructure
    {
        //the system goals being pursued
        private IntentionStackList _top_level_goals;
        //the active stack
        private IntentionStack _current_stack;

        private BDI _bdi;

        public IntentionStructure(BDI bdi)
        {
            _bdi = bdi;
            _top_level_goals = new IntentionStackList();
            _current_stack = null;
        }


        public KaBody.StatusType ExecuteCycle()
        {
            KaRuntimeFrame top_level_frame;
            KaBodyElement.KaReturnType return_value;

            Ka cycle = _bdi.current_ka;
            if(cycle!=null)
            {                
                Binding b = new Binding(cycle.get_symtab());
                KaBodyElement kbe = cycle.body.GetStartElement();
                top_level_frame = kbe.new_runtime_frame(_bdi);
                Goal gl = null;

                while (top_level_frame != null)
                {
                    //utils.print(name);
                    return_value = top_level_frame.execute(b, gl);
                    if (return_value == KaBodyElement.KaReturnType.KA_ELEMENT_FAILED)
                    {
                        Console.WriteLine("failed for this KA");
                        return KaBody.StatusType.KA_FAILURE;
                    }
                    else if (return_value == KaBodyElement.KaReturnType.KA_ELEMENT_COMPLETE)
                    {
                        if (top_level_frame.this_element.is_final_element())
                        {
                            return KaBody.StatusType.KA_SUCCESS;
                        }
                        else
                        {
                            top_level_frame = top_level_frame.this_element.next_element.new_runtime_frame(_bdi);

                        }
                    }
                    else if (return_value == KaBodyElement.KaReturnType.KA_ELEMENT_INCOMP)
                    {
                        continue;
                    } 
                }
            
            }

            return KaBody.StatusType.KA_FAILURE;
        }

        public SoakElement intend(SoakElement element)
        {
            SoakElement new_se;
            Goal old_current;
            Goal current_goal;

            int new_flag = 0;
            IntentionStack stack;
            IntentionStack current_stack;

            double element_priority;

            //see if the goal is already in the IS
            current_stack = find_goal_in_stacks(element.from_goal);
            refresh_stack_priorities();

            //if first top_level goal, then set pointers
            if(_top_level_goals.Count==0)
            {
                stack = new IntentionStack(element._from_goal);
                _top_level_goals.AddLast(stack);
                current_goal = element.from_goal;
                current_goal.status = StatusType.IS_ACTIVE;
                _current_stack = stack;
                new_flag = 1;
            }
            // If this is a top level goal (i.e. no parent goal), or if it is an
            // existing goal (it is a leaf goal in an existing intention stack),
            // we need to see if it has high enough priority to get executed.
            else if (find_goal_in_stacks(element.from_goal.prev_goal)==null ||
                find_current_goal_in_stacks(element.from_goal) != null)
            {
                current_stack = find_goal_in_stacks(element.from_goal);
                element_priority = element.eval_priority();

                if(current_stack==null && (utils.highest_stack_priority(_top_level_goals,element_priority)==1 ||
                    (_current_stack!=null && _current_stack.current_goal.status==StatusType.IS_BLOCKED)))
                {
                    //flag the intential goal as being active
                    element.from_goal.status = StatusType.IS_ACTIVE;
                    stack = new IntentionStack(element.from_goal);
                    _top_level_goals.AddLast(stack);
                    current_goal = element.from_goal;
                    _current_stack = stack;
                    new_flag = 1;
                }
                else
                {
                    _current_stack = get_highest_priority_stack();
                    _current_stack.current_goal.status = StatusType.IS_ACTIVE;

                    return element;
                }

            }
            //otherwise,this is a subgoal
            else
            {
                _current_stack = current_stack; 
                old_current = current_stack.current_goal;
                current_stack.current_goal = element.from_goal;
                current_goal = current_stack.current_goal;
            }

            new_se = new SoakElement();
            new_se = element;

            current_goal.Is = this;
            current_goal.set_intention(new_se);

            current_goal.runtime_frame = element.ka.body.GetStartElement().new_runtime_frame(_bdi);

 
            //element.from_goal.
            if(new_flag==0)
            {
                old_current = current_stack.current_goal;
                old_current.runtime_frame.intend(new_se);
            }
            return new_se;
        }
        public void clear_current_stack()
        {
            if(_current_stack!=null)
            {
                _current_stack.top_level_goal.remove_intention(false);
                remove_stack(_current_stack);
                _current_stack = get_highest_priority_stack();
            }

        }
        public StatusType activate()
        {
            Goal top_level_goal;
            KaRuntimeFrame top_level_frame;
            KaBodyElement.KaReturnType return_val;
            KaBodyAtomicElement failure_section;

            //check to see if we have anything intended on the IS
            if(_current_stack==null)
            {
                utils.print("Nothing currently active");
                return StatusType.IS_SUCCESS;
            }

            top_level_goal = _current_stack.top_level_goal;
            top_level_frame = top_level_goal.runtime_frame; 
            //at this point we will be executing an action from a
            //KA so we have to check the top goal to see if it is 
            //still valid
            bool confirm=top_level_goal.get_intention().ka.confirm_context(_bdi,top_level_goal.get_intention_binding());
            if(!confirm || top_level_goal.status==StatusType.IS_ABANDONED)
            {
                top_level_goal.remove_intention(true);
                if(top_level_goal.status==StatusType.IS_ABANDONED)
                {
                    _bdi.desires.remove(top_level_goal);
                }
                else
                {
                    top_level_goal.status = StatusType.IS_FAILURE;
                    top_level_goal.sub_goal = null;
                }
                _current_stack.current_goal = null;
                _bdi.desires.renew_leaf_goals();
                return StatusType.IS_FAILURE;
            }
            return_val = top_level_frame.execute(top_level_goal.get_intention_binding(),top_level_goal);
            if(return_val==KaBodyElement.KaReturnType.KA_ELEMENT_FAILED)
            {
                failure_section = top_level_goal.get_intention().ka.get_failure();
                if (failure_section!=null)
                {
                    top_level_frame = failure_section.new_runtime_frame(_bdi);
                    top_level_frame.execute(top_level_goal.get_intention_binding(),
                        top_level_goal);
                }
                top_level_goal.remove_intention(false);
                top_level_goal.status = StatusType.IS_FAILURE;
                top_level_goal.sub_goal = null;
                _bdi.desires.renew_leaf_goals();
                return StatusType.IS_FAILURE;
            }
            else if(return_val==KaBodyElement.KaReturnType.KA_ELEMENT_INCOMP)
            {
                return StatusType.IS_ACTIVE;
            }
            else//complete
            {
                //Done with the KA
                if(top_level_frame.this_element.is_final_element())
                {
                    top_level_goal.runtime_frame = null;
                    _bdi.desires.remove(top_level_goal);
                    remove_stack(_current_stack);
                    _current_stack = get_highest_priority_stack();
                    _bdi.desires.renew_leaf_goals();
                    return StatusType.IS_SUCCESS;

                }
                else
                {
                    top_level_goal.runtime_frame = top_level_frame.this_element.next_element.new_runtime_frame(_bdi);
                }
                return StatusType.IS_ACTIVE;
            }
        }

        
        public void print()
        {

        }
        //return the intention structure stack in which the goal is found
        public IntentionStack find_goal_in_stacks(Goal goal)
        {
            Goal stack_goal;
            foreach(IntentionStack ins in _top_level_goals)
            {
                stack_goal = ins.top_level_goal;
                while(stack_goal!=null)
                {
                    if (goal == stack_goal)
                        return ins;
                    stack_goal = stack_goal.sub_goal;
                }
            }

            return null;

        }
        public IntentionStack find_toplevel_goal_in_stacks(Goal goal)
        {
            Goal stack_goal;
            foreach (IntentionStack ins in _top_level_goals)
            {
                stack_goal = ins.top_level_goal;
                if (goal == stack_goal)
                    return ins;
            }

            return null;
        }
        public IntentionStack find_current_goal_in_stacks(Goal goal)
        {
            Goal stack_goal;
            foreach (IntentionStack ins in _top_level_goals)
            {
                stack_goal = ins.top_level_goal;
                if (goal == stack_goal)
                    return ins;
            }

            return null;
        }

        public Goal get_current_goal()
        {
            return _current_stack.current_goal;
        }
        public void set_current_goal(Goal goal)
        {
            _current_stack.current_goal = goal;
        }
        public Goal get_top_level_goal()
        {
            return (_current_stack != null) ? _current_stack.top_level_goal : null;
        }

        public IntentionStack current_stack
        {
            get { return _current_stack; }
            set { _current_stack = value; }
        }

        public void refresh_stack_priorities()
        { 
            Goal stack_goal;
            LinkedList<IntentionStack>.Enumerator itor = _top_level_goals.GetEnumerator();
            while (itor.MoveNext())
            {
                stack_goal = itor.Current.top_level_goal;
                while (stack_goal != null)
                {
                    SoakElement intent = stack_goal.get_intention();
                    if(intent!=null)
                    {
                        intent.ka.confirm_context(_bdi,stack_goal.get_intention_binding());
                    }
                    stack_goal = stack_goal.sub_goal;
                }
            }

        }
        public IntentionStack get_highest_priority_stack()
        {
            double highest_priority=-1;
            double highest_unblocked_priority=-1;
            IntentionStack highest_unblocked_stack = null;
            IntentionStack highest_stack = _current_stack;
            if (highest_stack != null)
            {
                highest_priority = highest_stack.current_goal.get_intention().eval_priority();
                if (highest_stack.current_goal.status != StatusType.IS_BLOCKED)
                {
                    highest_unblocked_priority = highest_priority;
                    highest_unblocked_stack = highest_stack;
                }
            }
            else
            {
                highest_priority = highest_unblocked_priority = -100000.0;
            }

            LinkedList<IntentionStack>.Enumerator itor = _top_level_goals.GetEnumerator();
            while (itor.MoveNext())
            {
                double stack_priority;
                stack_priority = itor.Current.current_goal.get_intention().eval_priority();
                if(stack_priority>highest_priority)
                {
                    highest_priority = stack_priority;
                    highest_stack = itor.Current;
                }
                if ((itor.Current.current_goal.status != StatusType.IS_BLOCKED) &&
                    (stack_priority>highest_unblocked_priority))
                {
                    highest_unblocked_priority = stack_priority;
                    highest_unblocked_stack = itor.Current;
                }
            }
 
            return (highest_unblocked_stack != null) ? highest_unblocked_stack : highest_stack;
        }

        /// <summary>
        /// search for and remove the indicated intention stack
        /// </summary>
        /// <param name="Is"></param>
        public void remove_stack(IntentionStack Is)
        { 
            LinkedListNode< IntentionStack > node = _top_level_goals.First;
            while(node!=null && node.Value!=Is)
            {
                node = node.Next; 
            }
            if(node!=null)
            {
                _top_level_goals.Remove(node.Value);
                if(node.Value==_current_stack)
                {
                    _current_stack = null;
                }
            }
            else
            {
                utils.print("IS::remove_stack-could not find stack");
            }

        }
        //public List<IntentionStack> get_top_level_goals()
        //{
        //    return _top_level_goals;
        //}
        

    }
}
