using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.FuzzyEngine
{
    public class State
    {
        private string _name;
        protected List<Transition> _transitions;

        protected bool _finished = false;         

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public List<Transition> Transitions
        {
            get { return _transitions; }
        }

        public bool IsFinished
        { get { return _finished; } }

        public State(string name)
        {
            _name = name;
            _transitions = new List<Transition>();
        }  

        public void AddTransition(State from,State to)
        {
            Transitions.Add(new Transition(from, to));
        }

        public void OnEnter()
        {
            _finished = false;
        }
        public void OnExit()
        {
            _finished = true;
        } 
    }
}
