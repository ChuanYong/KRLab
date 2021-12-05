using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    //Ka information related to execution
    public abstract class KaRuntimeFrame
    {
        protected KaBodyElement _this_element;
        protected KaRuntimeFrame _subframe;
        protected BDI _bdi;

        public abstract KaBodyElement.KaReturnType execute(Binding b, Goal goal);
        public abstract void intend(SoakElement s);
        public KaRuntimeFrame subframe
        {
            get { return _subframe; }
            set { _subframe = value; }
        }
        public KaBodyElement this_element
        {
            get { return _this_element; }
            set { _this_element = value; }
        } 

    }

    public class KaSimpleRuntimeFrame:KaRuntimeFrame
    { 
        public KaSimpleRuntimeFrame(BDI bdi,KaBodySimpleElement se)
        {
            _bdi = bdi;
            _this_element = se;
            _subframe = null; 
        }
        public override void intend(SoakElement s)
        { 
        }
        public override KaBodyElement.KaReturnType execute(Binding b, Goal goal)
        {
             
            KaBodySimpleElement kbse = (KaBodySimpleElement)this.this_element;
            Action action = kbse.action;
            Action.ActStatus return_val = action.execute(_bdi,b);
            if (return_val == Action.ActStatus.ACT_FAILED)
            {
                return KaBodyElement.KaReturnType.KA_ELEMENT_FAILED;
            }
            else
            { 
                return KaBodyElement.KaReturnType.KA_ELEMENT_COMPLETE;
            }
        }
    }

    public class KaGoalRuntimeFrame:KaRuntimeFrame
    {
        protected Goal _sub_goal;
        public KaGoalRuntimeFrame(BDI bdi,KaBodySimpleElement se)
        {
            _this_element = se; 
            _subframe = null;
            _sub_goal = null;
            _bdi = bdi;
        }
        public Goal sub_goal
        {
            get { return _sub_goal; }
            set { _sub_goal = value; }
        }

        public override KaBodyElement.KaReturnType execute(Binding b,Goal g)
        {
            //keep track of subgoal success and failure,Pretty much
            //just a modified version of the intention structure's
            //activate function
            Goal new_goal;
            KaRuntimeFrame active_frame;
            KaBodyElement.KaReturnType return_val; 

            if(g==null)
            {
                utils.print("warning! detected goal action within execution of CYCLE body");
                return KaBodyElement.KaReturnType.KA_ELEMENT_COMPLETE;
            }
            if(_sub_goal!=null)
            {
                if(_sub_goal.get_intention()!=null)
                {
                    active_frame = _sub_goal.runtime_frame;
                    if(active_frame!=null)
                    {
                        bool ok = _sub_goal.get_intention().ka.confirm_context(_bdi,_sub_goal.get_intention_binding());
                        if(!ok || _sub_goal.status== StatusType.IS_ABANDONED)
                        {
                            _sub_goal.remove_intention(true);
                            _bdi.desires.remove(_sub_goal);
                            _sub_goal.status = StatusType.IS_FAILURE;
                            _bdi.ins.set_current_goal(g);
                            return KaBodyElement.KaReturnType.KA_ELEMENT_FAILED;
                        }
                        return_val = active_frame.execute(_sub_goal.get_intention_binding(), _sub_goal);
                        if(return_val==KaBodyElement.KaReturnType.KA_ELEMENT_FAILED)
                        {
                            _sub_goal.remove_intention(true);
                            _bdi.desires.remove(_sub_goal);
                            g.sub_goal = null; 
                            _sub_goal = null;
                            _bdi.ins.set_current_goal(g); 
                            return KaBodyElement.KaReturnType.KA_ELEMENT_FAILED;                            
                        }
                        else if (return_val==KaBodyElement.KaReturnType.KA_ELEMENT_INCOMP)
                        {
                            return KaBodyElement.KaReturnType.KA_ELEMENT_INCOMP;
                        }
                        else//complete
                        {
                            if(active_frame.this_element.is_final_element())
                            {
                                _sub_goal.runtime_frame=null;  
                                return KaBodyElement.KaReturnType.KA_ELEMENT_COMPLETE;//KA_ELEMENT_INCOMP?????                              
                            }
                            else
                            {
                                _sub_goal.runtime_frame = active_frame.this_element.next_element.new_runtime_frame(_bdi);
                                return KaBodyElement.KaReturnType.KA_ELEMENT_INCOMP;//KA_ELEMENT_INCOMP?????
                            }
                        }
                    }
                    else
                    {
                        _sub_goal.remove_intention(false);
                        _bdi.desires.remove(sub_goal);
                        _sub_goal = null;
                        g.sub_goal = null;
                        _bdi.ins.set_current_goal(g);
                        return KaBodyElement.KaReturnType.KA_ELEMENT_COMPLETE;
                    }
                }
                else
                {
                    g.status = StatusType.IS_BLOCKED;
                    return KaBodyElement.KaReturnType.KA_ELEMENT_INCOMP;
                }
            }
            else//otherwise we need to post the subgoal to the system
            {
                KaBodySimpleElement kse = (KaBodySimpleElement)this_element;
                new_goal = _bdi.desires.add((GoalAction)kse.action, g.get_current_goal()); 
                _sub_goal = new_goal;
                _sub_goal.status = StatusType.IS_ACTIVE;
                return KaBodyElement.KaReturnType.KA_ELEMENT_INCOMP;
            }

        }
        public override void intend(SoakElement s)
        {
            //Add the subgoal to the bottom of the intention stack
            //We also need to check if we are supposed to run through
            //the normal or the EFFECTS section to set up correctly
            //for execution
            SoakElement new_se = new SoakElement();
            if (_sub_goal == null) _sub_goal = s.from_goal;

            new_se = s;
            _sub_goal.set_intention(new_se);
            _sub_goal.runtime_frame = new_se.ka.body.GetStartElement().new_runtime_frame(_bdi);
            if(new_se.from_goal.prev_goal!=null)
            {
                _sub_goal.prev_goal = new_se.from_goal.prev_goal;
            }

            if (new_se.from_goal.prev_goal != null)
                _sub_goal.prev_goal = new_se.from_goal.prev_goal;
        }
         
    }

    public class KaAtomicRuntimeFrame:KaRuntimeFrame
    {
        KaBodyElement _current_element;
        public KaAtomicRuntimeFrame(BDI bdi,KaBodyAtomicElement we)
        {
            _this_element = we;
            _subframe = we.get_start_element().new_runtime_frame(bdi);
            _current_element = we.get_start_element();
            _bdi = bdi;
             

        }
        public KaBodyElement current_element
        {
            get { return _current_element; }
            set { _current_element = value; }
        }
        public override KaBodyElement.KaReturnType execute(Binding b, Goal g)
        {
            KaBodyElement.KaReturnType body_return_val;
            KaBodyElement current;

            current = ((KaBodyAtomicElement)this_element).get_start_element();
            if(_subframe==null)
            {
                subframe = current.new_runtime_frame(_bdi);
            }
            while(current!=null)
            {
                body_return_val = _subframe.execute(b, g);
                if (body_return_val == KaBodyElement.KaReturnType.KA_ELEMENT_FAILED)
                {
                    subframe = null;
                    return KaBodyElement.KaReturnType.KA_ELEMENT_FAILED;
                }
                else if (body_return_val == KaBodyElement.KaReturnType.KA_ELEMENT_COMPLETE)
                {
                    if (!current.is_final_element())
                    {
                        subframe = current.next_element.new_runtime_frame(_bdi);
                    } 
                }
                else
                {
                    continue;
                } 
                current = current.next_element;
            }
            return KaBodyElement.KaReturnType.KA_ELEMENT_COMPLETE;

        }
        public override void intend(SoakElement s)
        {
            if(_subframe!=null)
            {
                _subframe.intend(s);
            }
        }
    }
}
