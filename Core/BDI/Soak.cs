using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{ 
    public class SoakElement
    {
        public Ka _ka;
        public Goal _from_goal;
        public Binding _binding;

        public SoakElement()
        {
            _ka = null;
            _from_goal = null;
            _binding = null;
        } 

        public double eval_priority()
        {
            double goal_priority = _from_goal.eval_priority();
            double ka_priority = _ka.eval_priority(_binding);
            return goal_priority + ka_priority;

        }
        public void print()
        {

        }

        public Ka ka
        {
            get { return _ka; }
            set { _ka = value; }
        }
        public Goal from_goal
        {
            get { return _from_goal; }
            set { _from_goal = value; }
        }

    }
    /**
     A SOAK(set of Applicable KAs) is a collection of KAs which have been 
     instantiated to achieve a goal(purpose) that has just been activated.
     Each KA in the SOAK is applicable to the specific situation, as one role
     of the context is to filter out KAs that are not relevant to a particular
     situation.
    */
    public class Soak
    {
        protected LinkedList<SoakElement> _elements;
        protected BDI _bdi;
        public Soak(BDI bdi)
        {
            _bdi = bdi;

            Ka ka;
            _elements = new LinkedList<SoakElement>();

            LinkedList<Goal> gs = _bdi.desires.get_goals();
            LinkedList<Goal>.Enumerator itor = gs.GetEnumerator();
            while (itor.MoveNext())
            {
                Goal g = itor.Current;
                if (!g.generate_soak()) continue;
                KaTableBucketIterator next_ka = new KaTableBucketIterator(_bdi.intentions, g);
                Binding ka_binding;
                 
                while((ka=next_ka.get_ka(out ka_binding))!=null)
                {
                    instantiate(ka, ka_binding, g);
                } 
            }
             
        }

        //check only the purpose(goal) and the context of the KA
        public void instantiate(Ka ka,Binding ka_binding,Goal goal)
        {
            BindingList bl=new BindingList(ka_binding);
            int nb = ka.check_context(_bdi,ref bl);
            if (nb!=0)
            {
                BindingList.Enumerator itor = bl.GetEnumerator();
                while(itor.MoveNext())
                {
                    Binding b = itor.Current;
                    if(goal.is_new() || b.is_new_wm_binding())
                    {
                        add(ka, goal, b);
                    }
                }

            }

        }

        public SoakElement add(Ka ka,Goal g,Binding b)
        {
            SoakElement se = new SoakElement();
            se.ka = ka;
            se.from_goal = g;
            se._binding = b;

            _elements.AddLast(se);
            return se;
        }
        public int get_size()
        {
            return _elements.Count;
        }
        public SoakElement get_elem(int nth)
        {
            if (nth >= _elements.Count)
                return null;
            else
            {
                return _elements.ElementAt(nth);
            }

        }
        public LinkedList<SoakElement> get_elems()
        {
            return _elements;
        }
        public SoakElement get_first()
        {
            return _elements.First();
        }
        public SoakElement get_priority_random()
        {
            int [] index= new int[_elements.Count];
            index[0] = 0;
            int max_count = 0;

            double p;
            double max_priority = -1;
            int i = 0;
            foreach(SoakElement se in _elements)
            {
                p = se.eval_priority();
                if(p>max_priority)
                {
                    max_priority = p;
                    max_count = 0;
                    index[0] = i; 
                }
                else if(p==max_priority)
                {
                    index[++max_count] = i;
                }
                i++;
            }
            Random r = new Random();
            int max_index = index[r.Next(0,max_count + 1)];
            return get_se(max_index);
        }

        public SoakElement get_se(int nth)
        {
            int nb = _elements.Count;
            LinkedListNode<SoakElement> node;

            if(nth<=nb-1)
            {
                node = _elements.First ;
                for(int i=1;i<=nth;i++)
                {
                    node = node.Next ;
                }
                return node.Value;
            }
            return null;
        }
 

    }
}
