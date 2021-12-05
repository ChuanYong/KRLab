using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    abstract public class KaBodyElement
    { 
        public enum KaReturnType
        {
            KA_ELEMENT_INCOMP,//未完成
            KA_ELEMENT_COMPLETE,
            KA_ELEMENT_FAILED
        }
        public enum KaElementType
        {
            KA_UNDEFINED,
            KA_SIMPLE,
            KA_GOAL,
            KA_BRANCH,
            KA_WHEN,
            KA_WHILE,
            KA_DO,
            KA_ATOMIC
        }

        protected KaElementType _type;
        protected KaBodyElement _next_element;


        public KaBodyElement()
        {
            _type = KaElementType.KA_UNDEFINED;
            _next_element = null;
        }
        public KaBodyElement(KaBodyElement ne)
        {
            _next_element = ne;
        }
        public abstract KaRuntimeFrame new_runtime_frame(BDI bdi);
        public virtual bool is_final_element()
        {
            return _next_element != null ? false : true;
        }
        public KaBodyElement next_element
        {
            get { return _next_element; }
            set { _next_element = value; }
        }
        public KaElementType type
        {
            get { return _type; }
            set { _type = value; }
        }


    }
     
    /// <summary>
    /// for executable actions
    /// </summary>
    public class KaBodySimpleElement:KaBodyElement
    {
        protected Action _action;
        public KaBodySimpleElement(Action a):base()
        {
            _action = a;
            _type = KaElementType.KA_SIMPLE;
        }
        public override KaRuntimeFrame new_runtime_frame(BDI bdi)
        {
            if (_action.is_executable_action())
            {
                return new KaSimpleRuntimeFrame(bdi,this);
            }
            else
            {
                return new KaGoalRuntimeFrame(bdi,this);
            }
        }
        public Action action
        {
            get { return _action; }
            set { _action = value; }
        }

    } 
 

    public class KaBodyAtomicElement:KaBodyElement
    {
        KaBodyElement _start_element;

        public KaBodyAtomicElement(KaBodyElement body):base()
        {
            _start_element = body;
            _type = KaElementType.KA_ATOMIC;
        }
        public KaBodyElement get_start_element() { return _start_element; }
        public new KaElementType type() { return _type; }
        public override KaRuntimeFrame new_runtime_frame(BDI bdi)
        {
            KaAtomicRuntimeFrame tmp = new KaAtomicRuntimeFrame(bdi,this);
            return tmp;
        }
    }
}
