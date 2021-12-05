using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    //Intentions represent the deliberative state of the agent:
    //what the agent has chosen to do. Intentions are desires to
    //which the agent has to some extent committed. In implemented systems,
    //this means the agent has begun executing a plan.
    //Plans: Plans are sequences of actions(recipes or knowledge areas) that 
    //an agent can perform to achieve one or more of its intentions.Plans may 
    //include other plans: my plan to go for a drive may include a plan to 
    //find my car keys.This reflects that in Bratman's model, plans are initially 
    //only partially conceived, with details being filled in as they progress.
    public class Intention :KaTable
    { 
    }
}
