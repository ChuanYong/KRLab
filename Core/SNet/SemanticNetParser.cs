using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 参见指南文档，在启用知识库时，要分类给出大致的解析模块，以提高解析效率，
    /// 通过最顶端的IS节点，进行分类，
    /// </summary>
    public static class SemanticNetParser
    {
        public static void EQULRelation(SemanticNet net,SNNode node)
        {
            
        }

        ///从IS和ISA关系解析一个定义 ,
        ///先检查基类节点是否有ATT和HAS节点，再检查派生节点对应的ATT和HAS节点，
        ///以此达到定义的目的
        public static void DefinitionParser(SemanticNet net ,SNEdge isisa)
        {

        }

        /// <summary>
        /// 从EQUL关系解析一个判定
        /// </summary>
        /// <param name="net"></param>
        /// <param name="equl"></param>
        public static void AdjustmentParser(SemanticNet net, SNEdge equl)
        {

        }

        public static void EQUL3Parser(SemanticNet net,SNEdge equal3)
        {

        }

        /// <summary>
        /// 解析GEOM关系
        /// </summary>
        /// <param name="net"></param>
        /// <param name="geom"></param>
        public static void GEOMParser(SemanticNet net ,SNEdge geom)
        {

        }

        /// <summary>
        /// 解析SPACE关系
        /// </summary>
        /// <param name="net"></param>
        /// <param name="space"></param>
        public static void SPACEParser(SemanticNet net ,SNEdge space)
        {

        }

        /// <summary>
        /// 解析GRANU关系，
        /// </summary>
        /// <param name="net"></param>
        /// <param name="granu"></param>
        public static void GRANUParser(SemanticNet net,SNEdge granu)
        {

        }

        /// <summary>
        ///解析一个数学函数
        /// </summary>
        /// <param name="net"></param>
        public static void FunctionParser(SemanticNet net)
        {

        }

        /// <summary>
        /// 解析DEF连接，此时要全面考虑DEF的起始节点和终止节点的相关
        /// 连接，特别是DEF中被定义节点所连接的CONTXT连接，
        /// 参见九年级数学上.结论--相交和相切--切线的判定定理，
        /// 一般来说，DEF的初始阶段所发出的连接都是修饰初始节点的，比如
        /// 对应正多边形每条边的圆心角是正多边形的中心角
        /// 
        /// </summary>
        /// <param name="net"></param>
        /// <param name="def"></param>
        public static void DefParser(SemanticNet net, SNEdge def)
        {

        }

        /// <summary>
        /// 解析EXPR连接，
        /// 参见九年级数学上.概念--弧长
        /// </summary>
        /// <param name="net"></param>
        /// <param name="expr"></param>
        public static void ExprParser(SemanticNet net,SNEdge expr)
        {

        }

    }
}
