using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;

using ITS.DomainModule;

namespace ITS.MaterialModule
{
    /// <summary>
    /// 用于物理计算问题的学习
    /// </summary>
    public class PhysCalcuQAMaker 
    {
        //protected StoryMaker _storyMaker;
        //protected EquationMaker _equationMaker; 

        //public StoryMaker StoryMaker
        //{ get { return _storyMaker; } }
        //public EquationMaker EquationMaker
        //{ get { return _equationMaker; } }
 
        //public PhysCalcuQAMaker(SemanticNet net):base(net)
        //{
        //    //_storyMaker = new StoryMaker(net);
        //    _equationMaker = new EquationMaker(EquationMaker.EquMode.Phys, net);

        //    ProduceLevel0();
        //    //ProduceLevel1();
        //    //ProduceLevel2();
        //    //ProduceLevel3();
        //    //ProduceLevel4();
        //}   

        ///// <summary>
        ///// 产生等级0的学习问题
        ///// (1)有哪些物理量；（2）物理量的单位是什么；
        ///// </summary>
        //private void ProduceLevel0()
        //{
        //    PQAPair pqa = new PQAPair(0);

        //    pqa.ProblemText = "学习《" + SNet.Topic + "》章节中主要的物理量。" + System.Environment.NewLine
        //        +"请按如下格式写出物理量名称和它的单位：" + System.Environment.NewLine;
        //    List<PhysicalQuantity> phys = GetAllPQs();
        //    pqa.ProblemText += phys[0].ConceptName + "：" + phys[0].Units[0];

        //    string quest = "列出其它相关的物理量名称及其单位：";
        //    string ans = string.Empty;
        //    foreach (var p in phys)
        //        ans += p.ConceptName + "："+p.Units[0]+System.Environment.NewLine;
        //    pqa.QAs.Add(Tuple.Create<string, string>(quest, ans));
        //    _pqaDict.Add(0, new List<PQAPair>() { pqa });
        //}

        ///// <summary>
        ///// 产生等级1的学习问题
        ///// </summary>
        //private void ProduceLevel1()
        //{
        //    List<PQAPair> pqs = new List<PQAPair>();
        //    List<Formula> conceptFormulae = _equationMaker.GetConceptFormulae();
        //    for (int i = 0; i < conceptFormulae.Count; i++)
        //    {
        //        PQAPair pqa = new PQAPair(0);
        //        pqa.ProblemText = "学习《" + SNet.Topic + "》章节中有关物理量的定义式。"; 
        //        string quest = "请给出" + conceptFormulae[i].Name + "的定义式：";
        //        string ans = conceptFormulae[i].ToString();
        //        pqa.QAs.Add(Tuple.Create<string, string>(quest, ans));
        //        pqs.Add(pqa);
        //    }

        //    _pqaDict.Add(1, pqs);
        //}
        ///// <summary>
        ///// 产生等级2的学习问题
        ///// </summary>
        //private void ProduceLevel2()
        //{
        //    List<PQAPair> pqs = new List<PQAPair>();
        //    foreach (var equ in _equationMaker.Equations)
        //    {
        //        List<PhysicalQuantity> vPQs = GetVariablePQs();
        //        foreach(var pq in vPQs)
        //        {
        //            PQAPair pqa = new PQAPair(0);
        //            pqa.ProblemText = "学习《" + SNet.Topic + "》章节中有关物理量的计算方法。";

        //            List<PhysicalQuantity> tmp = vPQs;
        //            tmp.Remove(pq);

        //            string quest = "已知";
        //            for(int i=0;i<tmp.Count-1;i++)
        //            {
        //                quest += tmp[i].ConceptName + "，";
        //            }
        //            quest += tmp[tmp.Count - 1].ConceptName+"，请给出"+pq.ConceptName+"的计算公式？";

        //            string ans = equ.CreateAFormula(pq.ConceptName).ToString();
        //            pqa.QAs.Add(Tuple.Create<string, string>(quest, ans));
        //            pqs.Add(pqa);
        //        }
        //    }
        //    _pqaDict.Add(2, pqs);
        //}
        ///// <summary>
        ///// 产生等级3的学习问题
        ///// </summary>
        //private void ProduceLevel3()
        //{
        //    _pqaDict.Add(3, _pqaDict[2]);
        //}
        //private void ProduceLevel4()
        //{
        //    if (_equationMaker.Equations.Count ==1)
        //        _pqaDict.Add(3, new List<PQAPair>());
        //    else
        //    {
        //        foreach(var equ in _equationMaker.Equations)
        //        {

        //        }
        //    }

        //}

        //public List<PhysicalQuantity> GetAllPQs()
        //{
        //    List<PhysicalQuantity> pqs = new List<PhysicalQuantity>();
        //    pqs.AddRange(GetVariablePQs());
        //    pqs.AddRange(GetConstantPQs());
        //    return pqs;
        //}


        ///// <summary>
        ///// 获取所有相关的物理量总称
        ///// </summary>
        ///// <returns></returns>
        //public List<string> GetAllConcepts()
        //{
        //    List<string> names = new List<string>();
        //    foreach (var x in GetAllPQs())
        //    {
        //        names.Add(x.ConceptName);
        //    }

        //    return names;
        //}

        ///// <summary>
        ///// 获取变量总称为name，变量名称为pqName的物理量的字符
        ///// 比如总称为角度，变量名称为角度1和角度2
        ///// 比如总称为速度，变量名称为初始速度和末了速度
        ///// </summary>
        ///// <param name="pqName"></param>
        ///// <returns></returns>
        //public string GetSymbol(string name, string pqName)
        //{
        //    List<string> names = GetQuantities(name);
        //    foreach (var e in names)
        //    {
        //        return GetSymbol(e);
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// 返回某个物理量name的符号
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public string GetSymbol(string name)
        //{
        //    foreach (var e in GetAllPQs())
        //    {
        //        if (e.pqDicts.Keys.Contains(name))
        //            return e.pqDicts[name];
        //    }
        //    return null;
        //}

        ////获取某个物理概念的物理量，比如速度下面的各个物理量
        ////v0,v1等等
        //public List<string> GetQuantities(string name)
        //{
        //    foreach (var e in GetAllPQs())
        //    {
        //        if (e.ConceptName == name)
        //            return e.pqDicts.Keys.ToList();
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// 查询网络中可变的物理量
        ///// </summary> 
        ///// <returns></returns>
        //private List<PhysicalQuantity> GetVariablePQs()
        //{
        //    SNNode node = SNet.FindNode("变量");
        //    if (node == null)
        //        throw new Exception("没有找到变量结点，公式语义网制作不正确！"); 
        //    return FindPQs(node); 
        //}

        //private List<PhysicalQuantity> GetConstantPQs()
        //{
        //    SNNode node = SNet.FindNode("常变量");
        //    if (node == null)
        //        return new List<PhysicalQuantity>();
        //    else
        //        return FindPQs(node);
        //}

        //private class MyComparer:IEqualityComparer<PhysicalQuantity>
        //{
        //    public bool Equals(PhysicalQuantity x,PhysicalQuantity y)
        //    {
        //        return x.ConceptName == y.ConceptName;
        //    }
        //    public int GetHashCode(PhysicalQuantity obj)
        //    {
        //        return 0;
        //    }
        //}
        
        //private List<PhysicalQuantity> FindPQs(SNNode node)
        //{
        //    List<PhysicalQuantity> pqs = new List<PhysicalQuantity>();

        //    //找到变量结点下的所有子节点，这些子节点构成了公式中的变化物理量
        //    List<SNNode> nodes = SNetParser.GetIncomingISNodes(SNet, node);
        //    if (nodes.Count == 0)
        //        throw new Exception("公式语义网中应该包含有变量");

        //    foreach (var nd in nodes)
        //    {
        //        List<SNNode> unitNodes = SNetParser.GetATTNodes(SNet, nd, "单位");
        //        List<string> units = new List<string>();
        //        foreach (var un in unitNodes)
        //        {
        //            units.Add(un.Name);
        //        }
        //        List<SNNode> pqNodes = SNetParser.GetIncomingISNodes(SNet, nd);
        //        Dictionary<string, string> dict = new Dictionary<string, string>();
        //        PhysicalQuantity pq = new PhysicalQuantity(nd.Name, units, true);
        //        foreach (var nd1 in pqNodes)
        //        {
        //            //每个物理量都一定对应一个唯一的符号
        //            string symb = SNetParser.GetATTNodes(SNet, nd1)[0].Name;
        //            pq.AddPQ(nd1.Name, symb);
        //        }
        //        pqs.Add(pq);
        //    }

        //    return pqs;
        //}

        //public string QuestionCalculationStory(StoryMaker storyMaker, EquationMaker formulaMaker)
        //{
        //    ///标有"故事"的结点，其所在的网络为故事网络，用来产生学习问题
        //    string txt = StoryMaker.Story.GetStorylineRandomly().GetContent();

        //    List<PhysicalQuantity> pqs = this.GetVariablePQs();

        //    //必须已知的物理量数目
        //    int knownNb = pqs.Count - formulaMaker.EquCount;
        //    List<PhysicalQuantity> knownPQs = new List<PhysicalQuantity>();
        //    //从pqs中任意选取knownNb个物理量作为已知量
        //    for (int i = 0; i < knownNb; i++)
        //    {
        //        int j = new Random().Next(0, pqs.Count);
        //        knownPQs.Add(pqs[i]);
        //    }

        //    txt += "已知其";
        //    for (int i = 0; i < knownPQs.Count - 1; i++)
        //    {
        //        txt += knownPQs[i].ConceptName + "、";
        //        pqs.Remove(knownPQs[i]);
        //    }
        //    txt += knownPQs[knownPQs.Count - 1].ConceptName + "。";
        //    pqs.Remove(knownPQs[knownPQs.Count - 1]);

        //    txt += "如何求" + pqs[0].ConceptName + "？";
        //    return txt;
        //}

    }
}
