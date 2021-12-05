using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    //Desires represent the motivational state of the agent 
    //Goals: A goal is a desire that has been adopted for active
    //pursuit by the agent. Usage of the term goals adds the further
    //restriction that the set of active desires must be consistent.
    //For example, one should not have concurrent goals to go to a 
    //party and to stay at home – even though they could both be desirable. 
    /*
     The initial, top-level goals that UM-PRS is to satisfy are specified 
     in a text format in a syntax identical to KA goal actions. System goals,
     however, may only be either achieve or maintain goals, and must contain
     only constant values (integer numbers, floating point numbers, or strings).
     A simple example of a system goal specification might look something like:
     ACHIEVE cone_demo :PRIORITY 10;
     MAINTAIN obstacles_avoided;
     */
    public class Desire :GoalSet
    {
        public Desire(BDI bdi):base(bdi) 
        {

        }
    } 
}
