using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core;
using KRLab.Translations;

namespace KRLab.Core.SNet
{
    public class SNRational:KRLab.Core.DataStructures.Graphs.CWeight
    {
        public static readonly string NULLRational = "无关系"; 
        ///IS类型
        public static readonly string ISA = SNRelationshipType.ISA.ToString();
        public static readonly string IS = SNRelationshipType.IS.ToString();
        public static readonly string ISO = SNRelationshipType.ISO.ToString();
        public static readonly string ISP = SNRelationshipType.ISP.ToString();
        public static readonly string ISPG = SNRelationshipType.ISPG.ToString();
        public static readonly string ISC = SNRelationshipType.ISC.ToString();
        public static readonly string AREC = SNRelationshipType.AREC.ToString();
        public static readonly string ATT = SNRelationshipType.ATT.ToString();
        public static readonly string ROLE = SNRelationshipType.ROLE.ToString();
        public static readonly string VAL = SNRelationshipType.VAL.ToString();
        public static readonly string VALR = SNRelationshipType.VALR.ToString();
        public static readonly string NAME = SNRelationshipType.NAME.ToString();
        public static readonly string SYMB = SNRelationshipType.SYMB.ToString();
        public static readonly string KTYPE = SNRelationshipType.KTYPE.ToString();
        public static readonly string DEF = SNRelationshipType.DEF.ToString();
        public static readonly string PROP = SNRelationshipType.PROP.ToString();
        public static readonly string PROPR = SNRelationshipType.PROPR.ToString();

        //时空类型
        public static readonly string TIME = SNRelationshipType.TIME.ToString();
        public static readonly string STRT = SNRelationshipType.STRT.ToString();
        public static readonly string ANTE = SNRelationshipType.ANTE.ToString();
        public static readonly string DUR = SNRelationshipType.DUR.ToString();
        public static readonly string DEST = SNRelationshipType.DEST.ToString();
        public static readonly string TDEST = SNRelationshipType.TDEST.ToString();
        public static readonly string SPACE = SNRelationshipType.SPACE.ToString();
        public static readonly string GEOM = SNRelationshipType.GEOM.ToString();
        public static readonly string LRANG = SNRelationshipType.LRANG.ToString();
        public static readonly string LOC = SNRelationshipType.LOC.ToString();
        public static readonly string LOCA = SNRelationshipType.LOCA.ToString();
        public static readonly string ORIGL = SNRelationshipType.ORIGL.ToString();
        public static readonly string DIR = SNRelationshipType.DIR.ToString();
        public static readonly string PATH = SNRelationshipType.PATH.ToString();


        //关联类型
        public static readonly string ARGU = SNRelationshipType.ARGU.ToString();
        public static readonly string ARGV = SNRelationshipType.ARGV.ToString();
        public static readonly string ARGV2 = SNRelationshipType.ARGV2.ToString();
        public static readonly string ARGV3 = SNRelationshipType.ARGV3.ToString();
        public static readonly string FARGV = SNRelationshipType.FARGV.ToString();
        public static readonly string ASSOC = SNRelationshipType.ASSOC.ToString();
        public static readonly string DEPT = SNRelationshipType.DEPT.ToString();
        public static readonly string GRANU = SNRelationshipType.GRANU.ToString();
        public static readonly string CAUSAL = SNRelationshipType.CAUSAL.ToString();
        public static readonly string CONFM = SNRelationshipType.CONFM.ToString();
        public static readonly string ATTCH = SNRelationshipType.ATTCH.ToString();
        public static readonly string IMPL = SNRelationshipType.IMPL.ToString();
        public static readonly string JUST = SNRelationshipType.JUST.ToString();
        public static readonly string MCONT = SNRelationshipType.MCONT.ToString();
        public static readonly string METH = SNRelationshipType.METH.ToString();
        public static readonly string ORIG = SNRelationshipType.ORIG.ToString();

        //执行类型
        public static readonly string ACT = SNRelationshipType.ACT.ToString();
        public static readonly string ACTR = SNRelationshipType.ACTR.ToString();
        public static readonly string MACTR = SNRelationshipType.MACTR.ToString();
        public static readonly string ACTED = SNRelationshipType.ACTED.ToString();
        public static readonly string RESULT = SNRelationshipType.RESULT.ToString();
        public static readonly string AFFED = SNRelationshipType.AFFED.ToString();
        public static readonly string CSTR = SNRelationshipType.CSTR.ToString();
        public static readonly string EXE = SNRelationshipType.EXE.ToString();
        public static readonly string EXECR = SNRelationshipType.EXECR.ToString();
        public static readonly string CHPE = SNRelationshipType.CHPE.ToString();
        public static readonly string ENVIR = SNRelationshipType.ENVIR.ToString();
        public static readonly string CIRCU = SNRelationshipType.CIRCU.ToString();
        public static readonly string ORNT = SNRelationshipType.ORNT.ToString();
        public static readonly string SUPPL = SNRelationshipType.SUPPL.ToString();
        public static readonly string MANNR = SNRelationshipType.MANNR.ToString();
        public static readonly string MODAL = SNRelationshipType.MODAL.ToString();
        public static readonly string TOOL = SNRelationshipType.TOOL.ToString();
        public static readonly string GOAL = SNRelationshipType.GOAL.ToString();

        //比较类型
        public static readonly string CORR = SNRelationshipType.CORR.ToString();
        public static readonly string CNVRS = SNRelationshipType.CNVRS.ToString();
        public static readonly string CONTR = SNRelationshipType.CONTR.ToString();
        public static readonly string COMPL = SNRelationshipType.COMPL.ToString();
        public static readonly string CONC = SNRelationshipType.CONC.ToString();
        public static readonly string ANLG = SNRelationshipType.ANLG.ToString();
        public static readonly string ANLG2 = SNRelationshipType.ANLG2.ToString();
        public static readonly string ANLG3 = SNRelationshipType.ANLG3.ToString();
        public static readonly string DIFF2 = SNRelationshipType.DIFF2.ToString();
        public static readonly string DIFF3 = SNRelationshipType.DIFF3.ToString();
        public static readonly string EQUL = SNRelationshipType.EQUL.ToString();
        public static readonly string EQUL2 = SNRelationshipType.EQUL2.ToString();
        public static readonly string EQUL3 = SNRelationshipType.EQUL3.ToString();
        public static readonly string ANTO = SNRelationshipType.ANTO.ToString();
        public static readonly string SYNO = SNRelationshipType.SYNO.ToString();
        public static readonly string SYMM = SNRelationshipType.SYMM.ToString();

        //限制关系类型
        public static readonly string AVRT = SNRelationshipType.AVRT.ToString();
        public static readonly string AMONG = SNRelationshipType.AMONG.ToString();
        public static readonly string REF = SNRelationshipType.REF.ToString();
        public static readonly string RANGE = SNRelationshipType.RANGE.ToString();
        public static readonly string IFTHEN = SNRelationshipType.IFTHEN.ToString();
        public static readonly string COND = SNRelationshipType.COND.ToString();
        public static readonly string CONTXT = SNRelationshipType.CONTXT.ToString();
        public static readonly string INIT = SNRelationshipType.INIT.ToString();
        public static readonly string FINAL = SNRelationshipType.FINAL.ToString();
        public static readonly string ORIGM = SNRelationshipType.ORIGM.ToString();
        public static readonly string QMOD = SNRelationshipType.QMOD.ToString();
        public static readonly string NUM = SNRelationshipType.NUM.ToString();
        public static readonly string NON = SNRelationshipType.NON.ToString();
        public static readonly string UNIT = SNRelationshipType.UNIT.ToString();

        //调用外部文件
        public static readonly string ANNIM = SNRelationshipType.ANNIM.ToString();
        public static readonly string DRAW = SNRelationshipType.DRAW.ToString();
        public static readonly string IMAGE = SNRelationshipType.IMAGE.ToString();
        public static readonly string SOUND = SNRelationshipType.SOUND.ToString();

        //数学类型
        public static readonly string COMP = SNRelationshipType.COMP.ToString();
        public static readonly string OPRND = SNRelationshipType.OPRND.ToString();
        public static readonly string OPRED = SNRelationshipType.OPRED.ToString();
        public static readonly string INVR = SNRelationshipType.INVR.ToString();
        public static readonly string ASSGN = SNRelationshipType.ASSGN.ToString();
        public static readonly string EXPR = SNRelationshipType.EXPR.ToString();
        public static readonly string MAJ = SNRelationshipType.MAJ.ToString();
        public static readonly string MAJE = SNRelationshipType.MAJE.ToString();
        public static readonly string MIN = SNRelationshipType.MIN.ToString();
        public static readonly string MINE = SNRelationshipType.MINE.ToString();
        public static readonly string MRESULT = SNRelationshipType.MRESULT.ToString();
        public static readonly string OPPS = SNRelationshipType.OPPS.ToString();
        public static readonly string SUM = SNRelationshipType.SUM.ToString();
        public static readonly string MULTI = SNRelationshipType.MULTI.ToString();
        public static readonly string SUBM = SNRelationshipType.SUBM.ToString();
        public static readonly string SUBME = SNRelationshipType.SUBME.ToString();


        //其它类型 
        public static readonly string AND = SNRelationshipType.AND.ToString();
        public static readonly string OR = SNRelationshipType.OR.ToString();
        public static readonly string HAS = SNRelationshipType.HAS.ToString();
        public static readonly string BENF = SNRelationshipType.BENF.ToString();
        public static readonly string MODE = SNRelationshipType.MODE.ToString();
        public static readonly string FORM = SNRelationshipType.FORM.ToString();
        public static readonly string SUBST = SNRelationshipType.SUBST.ToString();
        public static readonly string CHPA = SNRelationshipType.CHPA.ToString();
        public static readonly string CHPS = SNRelationshipType.CHPS.ToString();
        public static readonly string CHSP1 = SNRelationshipType.CHSP1.ToString();
        public static readonly string CHSP2 = SNRelationshipType.CHSP2.ToString();
        public static readonly string SSPE = SNRelationshipType.SSPE.ToString();

        /// <summary>
        /// 每个连接默认的标签
        /// </summary>
        public static readonly Dictionary<string, string> CHN = new Dictionary<string, string>()
        {
            {IS,"是一种"},
            {ISA,"是一个" },
            {ISO,"其中之一" },
            {ISP,"其中之一" },
            {ISC,"的" },
            {AREC,"的" },
            {ISPG,"部分" },
            {ATT,"的" },
            {VAL,"取值为" },
            {VALR,"取值范围为" },
            {NAME,"名字为" },
            {SYMB,"符号为" },
            {KTYPE,"知识类型为" },
            {ROLE,"作用是" },
            {DEF,"定义是" },
            {PROP,"" },
            {PROPR,"" },

            {ARGU,"参数为"},
            {ARGV,"参数为" },
            {ARGV2,"参数为" },
            {ARGV3,"参数为" },
            {ASSOC,"相关联" },
            {CAUSAL,"造成" },
            {CONFM,"" },
            {ATTCH,"附着" },
            {FARGV,"" },
            {DEPT,"" },
            {IMPL,"" },
            {MCONT,"" },
            {METH,"" },
            {ORIG,"" },
            {GRANU,""},

            {TIME,"" },
            {STRT,"" },
            {ANTE,"" },
            {DUR,"" },
            {DEST,"" },
            {TDEST,"" },
            {SPACE,"" },
            {GEOM,"" },
            {LRANG,"" },
            {LOC,"" },
            {LOCA,"" },
            {ORIGL,"" },
            {DIR,"" },
            {PATH,"" },
 
            {ACT,"" },
            {ACTR,"" },
            {MACTR,"" },
            {ACTED,"" },
            {RESULT,""},
            {AFFED,"" },
            {CSTR,"" },
            {EXE,"" },
            {EXECR,"" },
            {CHPE,"" },
            {ENVIR,"" },
            {CIRCU,"" },
            {ORNT,"" },
            {SUPPL,"" },
            {MANNR,"" },
            {MODAL,"" },
            {TOOL,"" },
            {GOAL,"" }, 

            {ANLG,"相似" },
            {ANLG2,"" },
            {ANLG3,"" },
            {CORR,"" },
            {CNVRS,"" },
            {CONTR,"" },
            {COMPL,"" },
            {CONC,"" },
            {DIFF2,"" },
            {DIFF3,"" },
            {EQUL,"" },
            {EQUL2,"" },
            {EQUL3,"" },
            {ANTO,"" },
            {SYNO,"" },
            {SYMM,"对称" },

            {AVRT,"" },
            {AMONG,"" },
            {REF,"" },
            {RANGE,"" },
            {IFTHEN,"" },
            {COND,"" },
            {CONTXT,"" },
            {INIT,"" },
            {FINAL,"" },
            {ORIGM,"" },
            {QMOD,"" },
            {NUM,"" },
            {NON,"" },
            {UNIT,"" },     

            {ANNIM,"" },
            {DRAW,"" },
            {IMAGE,"" },
            {SOUND,"" },

            {COMP,"" },
            {OPRND,"" },
            {OPRED,"" },
            {INVR,"" },
            {ASSGN,"" },
            {EXPR,"" },
            {MAJ,"" },
            {MAJE,"" },
            {MIN,"" },
            {MINE,"" },
            {OPPS,"" },
            {MRESULT,"" },
            {SUM,"" },
            {MULTI,"" },
            {SUBM,"" },
            {SUBME,"" },

            {AND,"" },
            {OR,"" },
            {HAS,"" },
            {BENF,"" },
            {MODE,"" },
            {FORM,"" },
            {SUBST,"" },
            {CHPA,"" },
            {CHPS,"" },
            {CHSP1,"" },
            {CHSP2,"" },
            {SSPE,"" },

    };

        protected string _label;
        protected string _startMulti;
        protected string _endMulti;
        protected string _startRole;
        protected string _endRole;
 
        public string Label
        {
            get { return _label; }
        }

        public string StartMulti
        {
            get { return _startMulti; }
        }
        public string EndMulti
        {
            get { return _endMulti; }
        }

        public string StartRole
        {
            get { return _startRole; }
        }
        public string EndRole
        {
            get { return _endRole; }
        }


        public SNRational(string rational,string label,
            string startMulti,string endMulti,
            string startRole,string endRole) :
            base(rational,0)
        {
            _label = label;
            _startMulti = startMulti;
            _endMulti = endMulti;
            _startRole = startRole;
            _endRole = endRole;

        }
    }
}
