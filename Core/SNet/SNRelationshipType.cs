using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.SNet
{
    public enum TypeType
    {
        IS,//
        ASS,//关联
        TS,//时空
        ACT,//行为
        COMP,//比较
        RES,//限制
        EXTN,//外部数据
        MATH,//数学
        OTH,//其它
        SYNO,
    }

    public enum SNRelationshipType
    { 
        //IS////////////////////////////////////////////////////////////////////////
        ISA,//是一个
        IS,//是一种
        ISO,//其中之一，
        ISP, //组成部分，主要表现实物或集合的整体-部分关系
        ISC,//是...特征关系，主要用于形容词与名词之间的连接
        AREC,//多个对象之间的特征，比如两个三角形全等。
        ISPG,//用于广义的组成关系，比如一个婚礼：接待客人，婚礼仪式，婚礼晚会组成
        ROLE,//作用，功能等
        ATT,//属性
        VAL,
        VALR,
        NAME,
        SYMB,//符号
        KTYPE,//知识类型
        DEF,//表示定义
        PROP,//表示对象的特性
        PROPR,//表示多个对象的特性

        //ASS////////////////////////////////////////////////////////////////////////////////////
        ARGU,//表示一个节点内容中的某个部分，这个部分可以用不同的节点内容进行替换。意义要比ARGV广
        ARGV,//表示一个参数，是一个数学量或物理量，为变量或常量
        ARGV2,
        ARGV3,
        FARGV,//函数或算法的参数
        ASSOC,//关联关系
        GRANU,//指向一个节点内容的细颗粒
        CAUSAL,//因果关系
        CONFM,//依据，根据
        DEPT,//依赖关系
        ATTCH,
        IMPL,
        JUST,///一个事情或状态被另外的事情所证实或证明
        MCONT,
        ORIG,

        //TS///////////////////////////////////////////////////////
        TIME,//已经删除！！！！
        STRT,
        ANTE,
        DUR,
        DEST,//空间上的目的
        TDEST,
        SPACE,//空间关系
        GEOM,
        LRANG,//空间范围
        LOC,
        LOCA,//抽象地址
        ORIGL,
        DIR,
        PATH,

        //ACT/////////////////////////////////////////////////////////////////////////
        ACT,//已经删除！！！！！！
        ACTR,//执行关系,指明动作或行为的执行者
        MACTR,//情感意识上的行为，比如，思考、认为，
        ACTED,//动作，事件的被执行者，
        RESULT,//推理关系，也可以表示一个事件的结果关系，也表示因果关系，
        AFFED,
        EXE,//已经删除！！！！！！
        EXECR,//一个事件、算法的执行
        CSTR,//一个事件的造成者
        CHPE,
        AVRT,
        ENVIR,//已经删除！！！！！！
        CIRCU,//事情发生时的环境，这里的环境可以是具体的环境，也可以是情形和情况
        ORNT,
        SUPPL,//动词组词
        MANNR,
        MODAL,//情态，表达一定、必须、能够等方面的信息
        METH,//实现某个过程或状态的方法
        TOOL,
        GOAL,//目的关系
         
        //COM////////////////////////////////////////////////////
        NON,//否定
        CORR,
        CNVRS,
        CONTR,
        COMPL,
        CONC,
        ANLG,//两个对象近似，类似
        ANLG2,
        ANLG3,//进行比较，表示近似、类似和相似等
        DIFF2,
        DIFF3,
        ANTO,
        SYNO,//等价,相同
        EQUL,
        EQUL2,
        EQUL3,
        SYMM,//对称

        //RES////////////////////////////////////////////////////////////
        RANGE,//范围
        REF,//作为参考，相对等关系，
        AMONG,//抽象的“之间”的关系，时间和空间之外的彼此之间
        IFTHEN,//已经删除！！！！！！
        COND,//条件关系
        CONTXT,
        INIT,
        FINAL,
        ORIGM,//物体的材料组成，
        QMOD,
        NUM,
        UNIT,

        //EXTN///////////////////////////////////////////////////
        ANNIM,//动画
        DRAW,///画图
        IMAGE,///调用图片文件
        SOUND,///播放声音

        ///MATH///////////////////////////////////////////////////////
        COMP,//大于，小于，等于等等
        OPRND,//操作数
        OPRED,//被操作数
        ASSGN,//赋值    
        EXPR,//表达式
        MAJ,
        MAJE,
        MIN,
        MINE,
        MRESULT,//一个运算、函数或算法的结果，与RESULT区分
        OPPS,//相反数
        INVR,//倒数
        SUM,
        MULTI,
        SUBM,//子集
        SUBME,//子集，包含相等

        //OTH/////////////////////////////////////////////////
        HAS,//有 
        AND,//与或关系
        OR,
        BENF,//益处
        MODE,
        FORM,
        SUBST,//替代
        CHPA, 
        CHPS,
        CHSP1,
        CHSP2,
        SSPE,
    }

    public class SNRelTypeType
    {
        public static Dictionary<string, TypeType> TopType = new Dictionary<string, TypeType>()
        {
            {SNRational.IS,TypeType.IS },
            {SNRational.ISA,TypeType.IS },
            {SNRational.ISO,TypeType.IS },
            {SNRational.ISP,TypeType.IS },
            {SNRational.ISC,TypeType.IS },
            {SNRational.AREC,TypeType.IS },
            {SNRational.ISPG,TypeType.IS },
            {SNRational.ATT,TypeType.IS },
            {SNRational.VAL,TypeType.IS },
            {SNRational.VALR,TypeType.IS },
            {SNRational.NAME,TypeType.IS },
            {SNRational.SYMB,TypeType.IS },
            {SNRational.ROLE,TypeType.IS },
            {SNRational.DEF,TypeType.IS },
            {SNRational.PROP,TypeType.IS },
            {SNRational.PROPR,TypeType.IS },

            {SNRational.ARGU,TypeType.ASS },
            {SNRational.ARGV,TypeType.ASS },
            {SNRational.ARGV2,TypeType.ASS },
            {SNRational.ARGV3,TypeType.ASS },
            {SNRational.FARGV,TypeType.ASS },
            {SNRational.ASSOC,TypeType.ASS },
            {SNRational.CAUSAL,TypeType.ASS },
            {SNRational.CONFM,TypeType.ASS },
            {SNRational.DEPT,TypeType.ASS },
            {SNRational.GRANU,TypeType.ASS },
            {SNRational.ATTCH,TypeType.ASS },
            {SNRational.IMPL,TypeType.ASS },
            {SNRational.JUST,TypeType.ASS },
            {SNRational.MCONT,TypeType.ASS },
            {SNRational.ORIG,TypeType.ASS },

            {SNRational.TIME,TypeType.TS },
            {SNRational.STRT,TypeType.TS },
            {SNRational.ANTE,TypeType.TS },
            {SNRational.DUR,TypeType.TS },
            {SNRational.DEST,TypeType.TS },
            {SNRational.TDEST,TypeType.TS },
            {SNRational.SPACE,TypeType.TS },
            {SNRational.GEOM,TypeType.TS },
            {SNRational.LRANG,TypeType.TS },
            {SNRational.LOC,TypeType.TS },
            {SNRational.ORIGL,TypeType.TS }, 
            {SNRational.DIR,TypeType.TS },
            {SNRational.PATH,TypeType.TS },
            {SNRational.LOCA,TypeType.TS  },

            {SNRational.ACTR,TypeType.ACT },
            {SNRational.MACTR,TypeType.ACT },
            {SNRational.ACTED,TypeType.ACT },
            {SNRational.RESULT,TypeType.ACT },
            {SNRational.AFFED,TypeType.ACT },
            {SNRational.EXECR,TypeType.ACT },
            {SNRational.CSTR,TypeType.ACT },
            {SNRational.CHPE,TypeType.ACT },
            {SNRational.AVRT,TypeType.ACT },
            {SNRational.ENVIR,TypeType.ACT },
            {SNRational.CIRCU,TypeType.ACT },
            {SNRational.ORNT,TypeType.ACT },
            {SNRational.SUPPL,TypeType.ACT },
            {SNRational.MANNR,TypeType.ACT },
            {SNRational.MODAL,TypeType.ACT },
            {SNRational.METH,TypeType.ACT },
            {SNRational.TOOL,TypeType.ACT },
            {SNRational.GOAL,TypeType.ACT },

            {SNRational.NON,TypeType.COMP },
            {SNRational.CORR,TypeType.COMP },
            {SNRational.CNVRS,TypeType.COMP },
            {SNRational.CONTR,TypeType.COMP },
            {SNRational.COMPL,TypeType.COMP },
            {SNRational.CONC,TypeType.COMP },
            {SNRational.ANLG,TypeType.COMP },
            {SNRational.ANLG2,TypeType.COMP },
            {SNRational.ANLG3,TypeType.COMP },
            {SNRational.DIFF2,TypeType.COMP },
            {SNRational.DIFF3,TypeType.COMP },
            {SNRational.ANTO,TypeType.COMP },
            {SNRational.SYNO,TypeType.COMP },
            {SNRational.EQUL,TypeType.COMP },
            {SNRational.EQUL2,TypeType.COMP },
            {SNRational.EQUL3,TypeType.COMP },
            {SNRational.SYMM,TypeType.COMP },

            {SNRational.RANGE,TypeType.RES },
            {SNRational.REF,TypeType.RES },
            {SNRational.AMONG,TypeType.RES },
            {SNRational.COND,TypeType.RES },
            {SNRational.CONTXT,TypeType.RES },
            {SNRational.INIT,TypeType.RES },
            {SNRational.FINAL,TypeType.RES },
            {SNRational.ORIGM,TypeType.RES },
            {SNRational.QMOD,TypeType.RES },
            {SNRational.NUM,TypeType.RES },
            {SNRational.UNIT,TypeType.RES },

            {SNRational.ANNIM,TypeType.EXTN },
            {SNRational.DRAW,TypeType.EXTN },
            {SNRational.IMAGE,TypeType.EXTN },
            {SNRational.SOUND,TypeType.EXTN },

            {SNRational.COMP,TypeType.MATH },
            {SNRational.OPRED,TypeType.MATH },
            {SNRational.OPRND,TypeType.MATH },
            {SNRational.ASSGN,TypeType.MATH },
            {SNRational.EXPR,TypeType.MATH },
            {SNRational.MAJ,TypeType.MATH },
            {SNRational.MAJE,TypeType.MATH },
            {SNRational.MIN,TypeType.MATH },
            {SNRational.MINE,TypeType.MATH },
            {SNRational.OPPS,TypeType.MATH },
            {SNRational.INVR,TypeType.MATH },
            {SNRational.MRESULT,TypeType.MATH },
            {SNRational.SUM,TypeType.MATH },
            {SNRational.MULTI,TypeType.MATH },
            {SNRational.SUBM,TypeType.MATH },
            {SNRational.SUBME,TypeType.MATH },


            {SNRational.HAS,TypeType.OTH },
            {SNRational.AND,TypeType.OTH }, 
            {SNRational.OR,TypeType.OTH },
            {SNRational.BENF,TypeType.OTH },
            {SNRational.MODE,TypeType.OTH },
            {SNRational.FORM,TypeType.OTH },
            {SNRational.SUBST,TypeType.OTH },
            {SNRational.CHPA,TypeType.OTH },
            {SNRational.CHPS,TypeType.OTH },
            {SNRational.CHSP1,TypeType.OTH },
            {SNRational.CHSP2,TypeType.OTH },
            {SNRational.SSPE,TypeType.OTH },

        };
    }

}
