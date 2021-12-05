using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using KRLab.Core;
using KRLab.Core.SNet;
using KRLab.Core.DataStructures.Lists;

namespace ITS.DomainModule
{
    public class EquationMaker
    {
        //public enum EquMode
        //{
        //    Math,
        //    Phys
        //}

        //private EquMode _mode; 
        ////网络中代表公式的中间结点，该中间
        ////结点左右两边连接的label是=号
        //private List<SNNode> _equNodes;
        //private List<OldEquation> _equations; 

        //public List<OldEquation> Equations
        //{
        //    get { return _equations; }
        //}
        //public List<SNNode> EquNodes
        //{ get { return _equNodes; } }
        //public int EquCount
        //{ get { return _equations.Count; } }

        //public EquationMaker(EquMode mode,SemanticNet net)
        //{
        //    _mode = mode; 
        //    _equNodes = SNetParser.GetEquNodes(net);
        //    if (_equNodes.Count == 0)
        //        throw new Exception(net.Topic+"不是一个公式语义网");

        //    _equations = new List<OldEquation>();

        //    foreach(var node in _equNodes)
        //    {
        //        SemanticNet equNet = net.CreateSubNet(node);
        //        OldEquation equ = new OldEquation(node,equNet) ; 
        //        _equations.Add(equ);
        //    }   
        //} 

        //public void SetVariables(List<string> symbols)
        //{
        //    if(symbols.Count!=EquCount)
        //    {
        //        throw new Exception("设定的变量数目应该等于方程数目！");
        //    }

        //}

        //public List<Formula> GetConceptFormulae()
        //{
        //    List<Formula> fs = new List<Formula>();
        //    foreach(OldEquation eq in Equations)
        //    {
        //        fs.AddRange(eq.ConceptFormulae);
        //    }
        //    return fs;
        //}

    }
}
