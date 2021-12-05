using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Symbolics;
using Expr = MathNet.Symbolics.SymbolicExpression;



namespace ITS.DomainModule
{
    public class PhysicalQuantity 
    { 
        //物理量的概念名称，比如速度
        protected string _conceptName; 
        //单位，一个物理量有可能有多个单位
        protected List<string> _units;
        //物理变量名称及符号表示，比如初始速度v0,末了速度v1等
        protected Dictionary<string,string> _pqDict; 
        //是否可变
        protected bool _variable;

        public string ConceptName
        { get { return _conceptName; } } 
        public Dictionary<string,string> pqDicts
        { get { return _pqDict; } }
        public List<string> Units
        { get { return _units; } } 
        public bool Variable
        { get { return _variable; } }
         
         
        public PhysicalQuantity(
            string conceptName, 
            List<string> units, 
            bool variable)
        { 
            _conceptName = conceptName; 
            _units = units; 
            _variable = variable;
            _pqDict = new Dictionary<string, string>();
        }  

        public void AddPQ(string name,string symbol)
        {
            _pqDict.Add(name, symbol);
        }

    }
}
