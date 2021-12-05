using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KRLab.Core;

namespace KRLab.Core
{
    public class BasicBayesianNode:CompositeNode
    {
        public string VariableName { get; set; } 
        public bool Stable { get; set; }    

        //These variables are for state tracking:
        public Dictionary<string, BayesianRelation> influences { get; set; }   //for effeciency.
        public double timeLeft { get; set; }    //stores the time left till the variable is set to change. Don't depend on this being up to date.
        public double[] nextVarProbs { get; set; }  //stores the probability for the next variables. This will be updated as necessary. 

        protected List<string> States;
        protected Dictionary<string,float> CPs;
        
        public override EntityType EntityType
        {
            get { return Core.EntityType.BayesianNode; }
        }


        public override int MemberCount
        {
            get { return StateCount+CPCount; }
        }

        public int StateCount
        {
            get { return States.Count; }
        }

        public int CPCount
        {
            get { return 0; }
        }

        public BasicBayesianNode(string name)
        {
            VariableName = name;
        }

        public override NodeBase Clone()
        {
            BasicBayesianNode newNode = new BasicBayesianNode(VariableName);
            newNode.CopyFrom(this);
            return newNode;
        }

        public override Member GetMember(MemberType type, int idx)
        {
            if (type == MemberType.State)
            {
                return new StateMember(States[idx],this);
            }
            else
            {
                return new CPMember("", this);
            }
        }

        protected string GetState(int idx)
        {
            return States[idx];
        }

        public override void AddMember(MemberType type,out Member m)
        {
            switch (type)
            {
                case MemberType.State:
                    m=new StateMember(this);
                    AddState(m);
                    break;
                case MemberType.CP:
                    m = new CPMember(this);
                    AddCP(m);
                    break;

                default :
                    m = null;
                    break;

            }
        }

        protected void AddState(Member m)
        {
        }

        protected void AddCP(Member m)
        {
        }

    }
}
