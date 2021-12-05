using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.FuzzyEngine
{
    public class Transition
    { 
        protected State _from;
        protected State _to;
         

        public State FromName
        {
            get { return _from; }
        }
        public State ToName
        {
            get { return _to; }
        }
        public Transition(State from, State to)
        {
            _from = from;
            _to = to; 
        }
    }
}
