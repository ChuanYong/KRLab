using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    //Beliefs represent the informational state of the agent
    /// belief is a database of facts which are represented
    /// as relations. A relation has a name and a variable number of 
    /// fields.
    /// tree 20 "maple" "red"
    public class Belief :WmTable
    { 
        public Belief( ):base()
        { 
        }
    }

    public class BeliefSet:LinkedList<Belief>
    { 
    }
}
