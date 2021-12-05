using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    //must be static 
 
    public class BDI
    {
        private Intention _intentions= new Intention();
        private Belief _beliefs = new Belief();
        private Desire _desires;
        private IntentionStructure _is;
        private Ka _current_ka;

        public BDI()
        {
            _desires = new Desire(this);
            _is = new IntentionStructure(this);
        }

        public Intention intentions
        {
            get { return _intentions; }
            set { _intentions = value; }
        }
        public Belief beliefs
        {
            get { return _beliefs; }
            set { _beliefs = value; }
        }
        public Desire desires
        {
            get { return _desires; }
            set { _desires = value; }
        }
        public IntentionStructure ins
        {
            get { return _is; }
            set { _is = value; }
        }
        public Ka current_ka
        {
            get { return _current_ka; }
            set { _current_ka = value; }
        }
    }

    public abstract class BDIGenerator 
    {
        protected string _name; 
        protected ActionList _acts;
         
        protected bool _is_config = false;
        protected BDI _bdi;

        public BDIGenerator(string name)
        { 
            _acts = new ActionList();
            _name = name;
            _bdi = new BDI();
        }

        public abstract void ConfigBelief();
        public abstract void ConfigDesire();
        public abstract void ConfigIntention();

        public virtual void Config()
        {
            ConfigBelief();
            ConfigDesire();
            ConfigIntention(); 
             
            _is_config = true;

        } 
 
        protected Ka create_ka(string name, string doc, Action goal, KaContext kc, params Action[] acts)
        {
            Ka ka = new Ka(name, doc);
            ka.name = name;
            ka.doc = doc;
            ka.goal = goal;

            if (kc == null)
            {
                ka.context = null;
            }
            else
            {
                ka.context = new KaContext(kc);

            }
            KaBody kb = new KaBody();
            foreach (Action a in acts)
            {
                if (a.elem_type() == Action.ELEMTYPE.SIMPLE)
                {
                    KaBodySimpleElement ke = new KaBodySimpleElement(a);
                    kb.add_last(ke);
                }
            }

            LinkedListNode<KaBodyElement> node = kb.elements.First;
            LinkedListNode<KaBodyElement> pre_node;
            while(node!=null)
            {
                pre_node = node;
                node = node.Next;
                if (node == null)
                {
                    pre_node.Value.next_element = null;
                }
                else
                    pre_node.Value.next_element = node.Value;
            } 
            
            ka.body = kb; 
            return ka;
        }
 
 
        public bool is_config
        {
            get { return _is_config; }
        }
        public string name
        {
            get { return _name; }
        }
        public BDI BDI
        {
            get { return _bdi; }
        }
    }
}
