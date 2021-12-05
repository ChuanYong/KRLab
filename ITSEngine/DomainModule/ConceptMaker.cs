using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ITS.DomainModule
{
    public class ConceptMaker
    {
        //private SemanticNet _net;
        //private SNNode _rootNode;
        //private Dictionary<string, SNNode> _conceptNodes;
        //private List<string> _vectorConcepts;
        //private List<string> _scaleConcepts;
        //private List<string> _compundUnits;
        //private List<string> _basicUnits;

        //public SemanticNet Net
        //{ get { return _net; } }
        //public SNNode RootNode
        //{ get { return _rootNode; } }
        //public Dictionary<string,SNNode> ConceptNodes
        //{ get { return _conceptNodes; } }
        //public List<string> Concepts
        //{ get { return _conceptNodes.Keys.ToList(); } }

        //public ConceptMaker(SemanticNet net)
        //{
        //    _net = net;
        //    _rootNode = _net.GetNode("概念");
        //    if (_rootNode == null) throw new Exception("没有发现概念结点");

        //    _conceptNodes = new Dictionary<string, SNNode>();
        //    _vectorConcepts = new List<string>();
        //    _scaleConcepts = new List<string>();
        //    _compundUnits = new List<string>();
        //    _basicUnits = new List<string>();

        //    List<SNNode> nodes = _net.GetIncomingISNodes(_rootNode); 
        //    foreach(var node in nodes)
        //    {
        //        _conceptNodes.Add(node.Name, node);
        //        if (IsAVector(node))
        //            _vectorConcepts.Add(node.Name);
        //        else//如果没有标注，默认为标量
        //            _scaleConcepts.Add(node.Name);
        //    }
        //    nodes = SNetParser.GetISASourceNodes(_net, _net.FindNode("复合单位"));
        //    foreach (var node in nodes)
        //        _compundUnits.Add(node.Name);
        //    nodes = SNetParser.GetISASourceNodes(_net, _net.FindNode("基本单位"));
        //    foreach (var node in nodes)
        //        _basicUnits.Add(node.Name);

        //}

        //private bool IsAVector(SNNode node)
        //{ 
        //    List<SNNode> nodes = SNetParser.GetOutgoingISNodes(_net, node);
        //    foreach(var nd in nodes)
        //    {
        //        if (nd.Name == "矢量")
        //            return true;
        //    }
        //    return false;
        //}
        //private bool IsAScale(SNNode node)
        //{ 
        //    List<SNNode> nodes = SNetParser.GetOutgoingISNodes(_net, node);
        //    foreach (var nd in nodes)
        //    {
        //        if (nd.Name == "标量")
        //            return true;
        //    }
        //    return false;
        //}

        //public bool IsVector(string concept)
        //{
        //    if (!Concepts.Contains(concept))
        //        throw new ArgumentNullException("没有发现这个概念：" + concept);
        //    if (_vectorConcepts.Contains(concept))
        //        return true;
        //    else
        //        return false;
        //}
        //public bool IsScale(string concept)
        //{
        //    if (!Concepts.Contains(concept))
        //        throw new ArgumentNullException("没有发现这个概念：" + concept);
        //    if (_scaleConcepts.Contains(concept))
        //        return true;
        //    else
        //        return false;
        //}

        //public bool IsCompundUnit(string concept)
        //{
        //    if (!Concepts.Contains(concept))
        //        throw new ArgumentNullException("没有发现这个概念：" + concept);
        //    if (_compundUnits.Contains(concept))
        //        return true;
        //    else
        //        return false;
        //}
        //public bool IsBasicUnit(string concept)
        //{
        //    if (!Concepts.Contains(concept))
        //        throw new ArgumentNullException("没有发现这个概念：" + concept);
        //    if (_basicUnits.Contains(concept))
        //        return true;
        //    else
        //        return false;
        //}

        //public string GetUnit(string concept)
        //{
        //    if (!Concepts.Contains(concept))
        //        throw new ArgumentNullException("没有发现这个概念：" + concept);

        //    return GetUnitNode(concept).Name;

        //}

        //private SNNode GetUnitNode(string concept)
        //{
        //    List<SNEdge> edges = _net.GetOutgoingEdges(_conceptNodes[concept]);
        //    foreach(var edge in edges)
        //    {
        //        if(edge.Rational.Rational==SNRational.ATT &&
        //            (edge.Rational.Label=="单位" || edge.Rational.Label=="unit"))
        //        {
        //            return edge.Destination;
        //        }
        //    }
        //    return null;
        //}
    }
}
