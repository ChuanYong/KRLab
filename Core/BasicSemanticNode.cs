using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace KRLab.Core
{
    /// <summary>
    /// 一个SN节点表示一个概念Concept，概念可以是自然语言中的任何单个字、词组、或完整的
    /// 句子所表示的东西。比如对象、事件、想法、动作、数字，等等
    /// </summary>
    public class BasicSemanticNode:NodeBase
    {

        public BasicSemanticNode(string name)
            : base(name)
        {
        }
        
        public override EntityType EntityType
        {
            get { return EntityType.SemanticNode; }
        }        
        
        public override NodeBase Clone()
        { 
            BasicSemanticNode newNode = new BasicSemanticNode("NewSNode");
            newNode.CopyFrom(this);
            return newNode;
        }
    }
}
