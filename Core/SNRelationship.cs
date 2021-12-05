using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

using KRLab.Translations;
using KRLab.Core.SNet;
using Utilities;
using MathNet.Numerics.Distributions;

namespace KRLab.Core
{
    public sealed class SNRelationship:NodeRelationship
    {
        SNRelationshipType _relationType;
        Direction _direction = Direction.Unidirectional;
        string startRole, endRole;
        string startMultiplicity, endMultiplicity;

        public event EventHandler Reversed;

        public SNRelationship(BasicSemanticNode first, BasicSemanticNode second) :
            base(first, second)
        {
            _relationType = SNRelationshipType.ASSOC;
            Label = SNRelationshipType.ASSOC.ToString();
            Attach();
        } 

        public override RelationshipType RelationshipType
        {
            get { return RelationshipType.SN_REL; }
        }

        public override bool SupportsLabel
        {
            get { return true;}
        }
        public override bool SupportsEndStartRole
        {
            get { return true; }
        }
        public Direction Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                if (_direction != value)
                {
                    _direction = value;
                    Changed();
                }
            }
        }
        public SNRelationshipType SNRelationshipType
        {
            get
            {
                return _relationType;
            }
            set
            {
                if (_relationType != value)
                {
                    _relationType = value;
                    Changed();
                }
            }
        }
        public string StartRole
        {
            get
            {
                return startRole;
            }
            set
            {
                if (value == "")
                    value = null;

                if (startRole != value)
                {
                    startRole = value;
                    Changed();
                }
            }
        }

        public string EndRole
        {
            get
            {
                return endRole;
            }
            set
            {
                if (value == "")
                    value = null;

                if (endRole != value)
                {
                    endRole = value;
                    Changed();
                }
            }
        }

        public string StartMultiplicity
        {
            get
            {
                return startMultiplicity;
            }
            set
            {
                if (value == "")
                    value = null;

                if (startMultiplicity != value)
                {
                    startMultiplicity = value;
                    Changed();
                }
            }
        }

        public string EndMultiplicity
        {
            get
            {
                return endMultiplicity;
            }
            set
            {
                if (value == "")
                    value = null;

                if (endMultiplicity != value)
                {
                    endMultiplicity = value;
                    Changed();
                }
            }
        }

        public void Reverse()
        { 
            IEntity first = First;
            First = Second;
            Second = first;

            OnReversed(EventArgs.Empty);
            Changed();
        }
        protected override void CopyFrom(Relationship relationship)
        {
            base.CopyFrom(relationship);

            SNRelationship relation = (SNRelationship)relationship;
            SNRelationshipType = relation.SNRelationshipType;
            _direction = relation._direction;
            startRole = relation.startRole;
            endRole = relation.endRole;
            startMultiplicity = relation.startMultiplicity;
            endMultiplicity = relation.endMultiplicity;
        }
        public SNRelationship Clone(BasicSemanticNode first, BasicSemanticNode second)
        {
            SNRelationship rel = new SNRelationship(first, second);
            rel.CopyFrom(this);
            return rel;
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="node"/> is null.
        /// </exception>
        public override void Serialize(XmlElement node)
        {
            base.Serialize(node);

            XmlElement directionNode = node.OwnerDocument.CreateElement("Direction");
            directionNode.InnerText = Direction.ToString();
            node.AppendChild(directionNode);

            XmlElement relationNode = node.OwnerDocument.CreateElement("SNRelationshipType");
            relationNode.InnerText = SNRelationshipType.ToString();
            node.AppendChild(relationNode);

            if (StartRole != null)
            {
                XmlElement roleNode = node.OwnerDocument.CreateElement("StartRole");
                roleNode.InnerText = StartRole.ToString();
                node.AppendChild(roleNode);
            }
            if (EndRole != null)
            {
                XmlElement roleNode = node.OwnerDocument.CreateElement("EndRole");
                roleNode.InnerText = EndRole.ToString();
                node.AppendChild(roleNode);
            }
            if (StartMultiplicity != null)
            {
                XmlElement multiplicityNode = node.OwnerDocument.CreateElement("StartMultiplicity");
                multiplicityNode.InnerText = StartMultiplicity.ToString();
                node.AppendChild(multiplicityNode);
            }
            if (EndMultiplicity != null)
            {
                XmlElement multiplicityNode = node.OwnerDocument.CreateElement("EndMultiplicity");
                multiplicityNode.InnerText = EndMultiplicity.ToString();
                node.AppendChild(multiplicityNode);
            }
        } 

        /// <exception cref="ArgumentNullException">
        /// <paramref name="node"/> is null.
        /// </exception>
        public override void Deserialize(XmlElement node)
        {
            base.Deserialize(node);

            XmlElement child = node["Direction"];

            RaiseChangedEvent = false;
            if (child != null)
            {                                              // Old file format
                if (child.InnerText == "Unidirectional" || child.InnerText == "SourceDestination")
                    Direction = Direction.Unidirectional;
                else
                    Direction = Direction.Bidirectional;
            }

            try
            { 

                child = node["SNRelationshipType"];
                XmlElement labelNode = node["Label"];

                if (child != null)
                {
                    //更新连接
                    if (child.InnerText == SNRational.IS && (KCNames.Names.Contains(Second.Name)
                        || Second.Name == "算法"))
                        SNRelationshipType = SNRelationshipType.KTYPE;

                    //IS类型
                    else if (child.InnerText == SNRational.IS)
                        SNRelationshipType = SNRelationshipType.IS;
                    else if (child.InnerText == SNRational.ISA)
                        SNRelationshipType = SNRelationshipType.ISA;
                    else if (child.InnerText == SNRational.ISO)
                        SNRelationshipType = SNRelationshipType.ISO;
                    else if (child.InnerText == SNRational.ISP)
                        SNRelationshipType = SNRelationshipType.ISP;
                    else if (child.InnerText == SNRational.ISPG)
                        SNRelationshipType = SNRelationshipType.ISPG;
                    else if (child.InnerText == SNRational.ISC)
                        SNRelationshipType = SNRelationshipType.ISC;
                    else if (child.InnerText == SNRational.AREC)
                        SNRelationshipType = SNRelationshipType.AREC;
                    else if (child.InnerText == SNRational.ATT)
                        SNRelationshipType = SNRelationshipType.ATT;
                    else if (child.InnerText == SNRational.VAL)
                        SNRelationshipType = SNRelationshipType.VAL;
                    else if (child.InnerText == SNRational.VALR)
                        SNRelationshipType = SNRelationshipType.VALR;
                    else if (child.InnerText == SNRational.NAME)
                        SNRelationshipType = SNRelationshipType.NAME;
                    else if (child.InnerText == SNRational.SYMB)
                        SNRelationshipType = SNRelationshipType.SYMB;
                    else if (child.InnerText == SNRational.ROLE)
                        SNRelationshipType = SNRelationshipType.ROLE;
                    else if (child.InnerText == SNRational.KTYPE)
                        SNRelationshipType = SNRelationshipType.KTYPE;
                    else if (child.InnerText == SNRational.DEF)
                        SNRelationshipType = SNRelationshipType.DEF;
                    else if (child.InnerText == SNRational.PROP)
                        SNRelationshipType = SNRelationshipType.PROP;
                    else if (child.InnerText == SNRational.PROPR)
                        SNRelationshipType = SNRelationshipType.PROPR;

                    //执行类型
                    else if (child.InnerText == SNRational.ACT)
                        SNRelationshipType = SNRelationshipType.ACTR;
                    else if (child.InnerText == SNRational.ACTR)
                        SNRelationshipType = SNRelationshipType.ACTR;
                    else if (child.InnerText == SNRational.MACTR)
                        SNRelationshipType = SNRelationshipType.MACTR;
                    else if (child.InnerText == SNRational.ACTED)
                        SNRelationshipType = SNRelationshipType.ACTED;
                    else if (child.InnerText == SNRational.RESULT)
                        SNRelationshipType = SNRelationshipType.RESULT;
                    else if (child.InnerText == SNRational.AFFED)
                        SNRelationshipType = SNRelationshipType.AFFED;
                    else if (child.InnerText == SNRational.CSTR)
                        SNRelationshipType = SNRelationshipType.CSTR;
                    else if (child.InnerText == SNRational.EXE)//更新
                        SNRelationshipType = SNRelationshipType.EXECR;
                    else if (child.InnerText == SNRational.EXECR)
                        SNRelationshipType = SNRelationshipType.EXECR;
                    else if (child.InnerText == SNRational.AVRT)
                        SNRelationshipType = SNRelationshipType.AVRT;
                    else if (child.InnerText == SNRational.CHPE)
                        SNRelationshipType = SNRelationshipType.CHPE;
                    else if (child.InnerText == SNRational.ENVIR)///更新
                        SNRelationshipType = SNRelationshipType.CIRCU;
                    else if (child.InnerText == SNRational.CIRCU)
                        SNRelationshipType = SNRelationshipType.CIRCU;
                    else if (child.InnerText == SNRational.ORNT)
                        SNRelationshipType = SNRelationshipType.ORNT;
                    else if (child.InnerText == SNRational.SUPPL)
                        SNRelationshipType = SNRelationshipType.SUPPL;
                    else if (child.InnerText == SNRational.TOOL)
                        SNRelationshipType = SNRelationshipType.TOOL;
                    else if (child.InnerText == SNRational.GOAL)
                        SNRelationshipType = SNRelationshipType.GOAL;
                    else if (child.InnerText == SNRational.MANNR)
                        SNRelationshipType = SNRelationshipType.MANNR;
                    else if (child.InnerText == SNRational.MODAL)
                        SNRelationshipType = SNRelationshipType.MODAL;

                    //关联类型
                    else if (child.InnerText == SNRational.ARGU)
                        SNRelationshipType = SNRelationshipType.ARGU;
                    else if (child.InnerText == SNRational.ARGV)
                        SNRelationshipType = SNRelationshipType.ARGV;
                    else if (child.InnerText == SNRational.ARGV2)
                        SNRelationshipType = SNRelationshipType.ARGV2;
                    else if (child.InnerText == SNRational.ARGV3)
                        SNRelationshipType = SNRelationshipType.ARGV3;
                    else if (child.InnerText == SNRational.FARGV)
                        SNRelationshipType = SNRelationshipType.FARGV;
                    else if (child.InnerText == SNRational.ASSOC)
                        SNRelationshipType = SNRelationshipType.ASSOC;
                    else if (child.InnerText == SNRational.DEPT)
                        SNRelationshipType = SNRelationshipType.DEPT;
                    else if (child.InnerText == SNRational.CAUSAL)
                        SNRelationshipType = SNRelationshipType.CAUSAL;
                    else if (child.InnerText == SNRational.CONFM)
                        SNRelationshipType = SNRelationshipType.CONFM;
                    else if (child.InnerText == SNRational.GRANU)
                        SNRelationshipType = SNRelationshipType.GRANU;
                    else if (child.InnerText == SNRational.ATTCH)
                        SNRelationshipType = SNRelationshipType.ATTCH;
                    else if (child.InnerText == SNRational.IMPL)
                        SNRelationshipType = SNRelationshipType.IMPL;
                    else if (child.InnerText == SNRational.JUST)
                        SNRelationshipType = SNRelationshipType.JUST;
                    else if (child.InnerText == SNRational.MCONT)
                        SNRelationshipType = SNRelationshipType.MCONT;
                    else if (child.InnerText == SNRational.METH)
                        SNRelationshipType = SNRelationshipType.METH;
                    else if (child.InnerText == SNRational.ORIG)
                        SNRelationshipType = SNRelationshipType.ORIG;

                    ///数学关系
                    else if (child.InnerText == SNRational.COMP)
                        SNRelationshipType = SNRelationshipType.COMP;
                    else if (child.InnerText == SNRational.ASSGN)
                        SNRelationshipType = SNRelationshipType.ASSGN;
                    else if (child.InnerText == SNRational.EXPR)
                        SNRelationshipType = SNRelationshipType.EXPR;
                    else if (child.InnerText == SNRational.MAJ)
                        SNRelationshipType = SNRelationshipType.MAJ;
                    else if (child.InnerText == SNRational.MAJE)
                        SNRelationshipType = SNRelationshipType.MAJE;
                    else if (child.InnerText == SNRational.MIN)
                        SNRelationshipType = SNRelationshipType.MIN;
                    else if (child.InnerText == SNRational.MINE)
                        SNRelationshipType = SNRelationshipType.MINE;
                    else if (child.InnerText == SNRational.OPPS)
                        SNRelationshipType = SNRelationshipType.OPPS;
                    else if (child.InnerText == SNRational.OPRED)
                        SNRelationshipType = SNRelationshipType.OPRED;
                    else if (child.InnerText == SNRational.OPRND)
                        SNRelationshipType = SNRelationshipType.OPRND;
                    else if (child.InnerText == SNRational.INVR)
                        SNRelationshipType = SNRelationshipType.INVR;
                    else if (child.InnerText == SNRational.MRESULT)
                        SNRelationshipType = SNRelationshipType.MRESULT;
                    else if (child.InnerText == SNRational.SUM)
                        SNRelationshipType = SNRelationshipType.SUM;
                    else if (child.InnerText == SNRational.MULTI)
                        SNRelationshipType = SNRelationshipType.MULTI;
                    else if (child.InnerText == SNRational.SUBM)
                        SNRelationshipType = SNRelationshipType.SUBM;
                    else if (child.InnerText == SNRational.SUBME)
                        SNRelationshipType = SNRelationshipType.SUBME;

                    //比较类型
                    else if (child.InnerText == SNRational.CORR)
                        SNRelationshipType = SNRelationshipType.CORR;
                    else if (child.InnerText == SNRational.CNVRS)
                        SNRelationshipType = SNRelationshipType.CNVRS;
                    else if (child.InnerText == SNRational.CONTR)
                        SNRelationshipType = SNRelationshipType.CONTR;
                    else if (child.InnerText == SNRational.COMPL)
                        SNRelationshipType = SNRelationshipType.COMPL;
                    else if (child.InnerText == SNRational.CONC)
                        SNRelationshipType = SNRelationshipType.CONC;
                    else if (child.InnerText == SNRational.ANLG)
                        SNRelationshipType = SNRelationshipType.ANLG;
                    else if (child.InnerText == SNRational.ANLG2)
                        SNRelationshipType = SNRelationshipType.ANLG2;
                    else if (child.InnerText == SNRational.ANLG3)
                        SNRelationshipType = SNRelationshipType.ANLG3;
                    else if (child.InnerText == SNRational.DIFF2)
                        SNRelationshipType = SNRelationshipType.DIFF2;
                    else if (child.InnerText == SNRational.DIFF3)
                        SNRelationshipType = SNRelationshipType.DIFF3;
                    else if (child.InnerText == SNRational.ANTO)
                        SNRelationshipType = SNRelationshipType.ANTO;
                    else if (child.InnerText == SNRational.SYNO)
                        SNRelationshipType = SNRelationshipType.SYNO;
                    else if (child.InnerText == SNRational.EQUL)
                        SNRelationshipType = SNRelationshipType.EQUL;
                    else if (child.InnerText == SNRational.EQUL2)
                        SNRelationshipType = SNRelationshipType.EQUL2;
                    else if (child.InnerText == SNRational.EQUL3)
                        SNRelationshipType = SNRelationshipType.EQUL3;
                    else if (child.InnerText == SNRational.SYMM)
                        SNRelationshipType = SNRelationshipType.SYMM;

                    //时间和空间
                    else if (child.InnerText == SNRational.SPACE)
                        SNRelationshipType = SNRelationshipType.SPACE;
                    else if (child.InnerText == SNRational.GEOM)
                        SNRelationshipType = SNRelationshipType.GEOM;
                    else if (child.InnerText == SNRational.LRANG)
                        SNRelationshipType = SNRelationshipType.LRANG;
                    else if (child.InnerText == SNRational.LOC)
                        SNRelationshipType = SNRelationshipType.LOC;
                    else if (child.InnerText == SNRational.LOCA)
                        SNRelationshipType = SNRelationshipType.LOCA;
                    else if (child.InnerText == SNRational.ORIGL)
                        SNRelationshipType = SNRelationshipType.ORIGL;
                    else if (child.InnerText == SNRational.DIR)
                        SNRelationshipType = SNRelationshipType.DIR;
                    else if (child.InnerText == SNRational.TIME)
                        SNRelationshipType = SNRelationshipType.ANTE;
                    else if (child.InnerText == SNRational.STRT)
                        SNRelationshipType = SNRelationshipType.STRT;
                    else if (child.InnerText == SNRational.ANTE)
                        SNRelationshipType = SNRelationshipType.ANTE;
                    else if (child.InnerText == SNRational.DUR)
                        SNRelationshipType = SNRelationshipType.DUR;
                    else if (child.InnerText == SNRational.DEST)
                        SNRelationshipType = SNRelationshipType.DEST;
                    else if (child.InnerText == SNRational.TDEST)
                        SNRelationshipType = SNRelationshipType.TDEST;
                    else if (child.InnerText == SNRational.PATH)
                        SNRelationshipType = SNRelationshipType.PATH;

                    //限制类型
                    else if (child.InnerText == SNRational.REF)
                        SNRelationshipType = SNRelationshipType.REF;
                    else if (child.InnerText == SNRational.RANGE)
                        SNRelationshipType = SNRelationshipType.RANGE;
                    else if (child.InnerText == SNRational.AMONG)
                        SNRelationshipType = SNRelationshipType.AMONG;
                    else if (child.InnerText == SNRational.IFTHEN)//更新
                        SNRelationshipType = SNRelationshipType.COND;
                    else if (child.InnerText == SNRational.COND)
                        SNRelationshipType = SNRelationshipType.COND;
                    else if (child.InnerText == SNRational.CONTXT)
                        SNRelationshipType = SNRelationshipType.CONTXT;
                    else if (child.InnerText == SNRational.INIT)
                        SNRelationshipType = SNRelationshipType.INIT;
                    else if (child.InnerText == SNRational.FINAL)
                        SNRelationshipType = SNRelationshipType.FINAL;
                    else if (child.InnerText == SNRational.ORIGM)
                        SNRelationshipType = SNRelationshipType.ORIGM;
                    else if (child.InnerText == SNRational.QMOD)
                        SNRelationshipType = SNRelationshipType.QMOD;
                    else if (child.InnerText == SNRational.NUM)
                        SNRelationshipType = SNRelationshipType.NUM;
                    else if (child.InnerText == SNRational.NON)
                        SNRelationshipType = SNRelationshipType.NON;
                    else if (child.InnerText == SNRational.UNIT)
                        SNRelationshipType = SNRelationshipType.UNIT;

                    //使用外部数据
                    else if (child.InnerText == SNRational.ANNIM)
                        SNRelationshipType = SNRelationshipType.ANNIM;
                    else if (child.InnerText == SNRational.DRAW)
                        SNRelationshipType = SNRelationshipType.DRAW;
                    else if (child.InnerText == SNRational.IMAGE)
                        SNRelationshipType = SNRelationshipType.IMAGE;
                    else if (child.InnerText == SNRational.SOUND)
                        SNRelationshipType = SNRelationshipType.SOUND;


                    //其它类型 
                    else if (child.InnerText == SNRational.AND)
                        SNRelationshipType = SNRelationshipType.AND;
                    else if (child.InnerText == SNRational.OR)
                        SNRelationshipType = SNRelationshipType.OR;
                    else if (child.InnerText == SNRational.HAS)
                        SNRelationshipType = SNRelationshipType.HAS;
                    else if (child.InnerText == SNRational.BENF)
                        SNRelationshipType = SNRelationshipType.BENF;
                    else if (child.InnerText == SNRational.MODE)
                        SNRelationshipType = SNRelationshipType.MODE;
                    else if (child.InnerText == SNRational.FORM)
                        SNRelationshipType = SNRelationshipType.FORM;
                    else if (child.InnerText == SNRational.SUBST)
                        SNRelationshipType = SNRelationshipType.SUBST;
                    else if (child.InnerText == SNRational.CHPA)
                        SNRelationshipType = SNRelationshipType.CHPA;
                    else if (child.InnerText == SNRational.CHPS)
                        SNRelationshipType = SNRelationshipType.CHPS;
                    else if (child.InnerText == SNRational.CHSP1)
                        SNRelationshipType = SNRelationshipType.CHSP1;
                    else if (child.InnerText == SNRational.CHSP2)
                        SNRelationshipType = SNRelationshipType.CHSP2;
                    else if (child.InnerText == SNRational.SSPE)
                        SNRelationshipType = SNRelationshipType.SSPE;

                    else
                        throw new NetException("没有找到<" + child.InnerText + ">连接类型");
                }

                child = node["StartRole"];
                if (child != null)
                    startRole = child.InnerText;

                child = node["EndRole"];
                if (child != null)
                    endRole = child.InnerText;

                child = node["StartMultiplicity"];
                if (child != null)
                    startMultiplicity = child.InnerText;

                child = node["EndMultiplicity"];
                if (child != null)
                    endMultiplicity = child.InnerText;
            }
            catch (ArgumentException)
            {
                // Wrong format
            }
            RaiseChangedEvent = true;
        }

        private void OnReversed(EventArgs e)
        {
            if (Reversed != null)
                Reversed(this, e);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(50);

            builder.Append(Strings.SNRelationship);
            builder.Append(": ");
            builder.Append(First.Name);

            switch (Direction)
            { 
                case Direction.Unidirectional:
                    if (SNRelationshipType == SNRelationshipType.ASSOC)
                        builder.Append(" --> ");
                    else
                        builder.Append(" <>-> ");
                    break;
                default:
                    builder.Append(", ");
                    break;
            }
            builder.Append(Second.Name);

            return builder.ToString();
        }
    }

}
