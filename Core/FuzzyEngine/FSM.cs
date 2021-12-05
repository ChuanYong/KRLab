using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.FuzzyEngine
{
    //fuzzy state machine
    public class FSM
    {
        protected List<State> _states;
        protected State _current;


        public List<State> States
        { get { return _states; } }
        public State CurrentState
        { get { return _current; } }

        public FSM()
        {
            _states = new List<State>();
        }

        public void SetInitialState(string stateName)
        {
            _current = States.Find(s => { return s.Name == stateName; });
            _current.OnEnter();
        }
        public void AddState(State state)
        {
            int index = -1;
            index = States.FindIndex(s => { return s.Name == state.Name; });
            if (index == -1) States.Add(state);
        }

        public void AddTransition(State from,State to)
        {
            int idxFrom = States.FindIndex(s => { return s.Name == from.Name; });
            int idxTo = States.FindIndex(s => { return s.Name == to.Name; });
            if (idxFrom >= 0 && idxTo >= 0)
                States[idxFrom].AddTransition(from, to);
        }

        public void Check()
        {
            bool changed;
        }


    }
}
