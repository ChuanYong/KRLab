

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;
using KRLab.Core;
using KRLab.Core.SNet;
using KRLab.Translations; 

namespace KRLab.DiagramEditor.NetworkDiagram.Dialogs
{
	public partial class SNConnectionDialog : Form
	{
        const int ArrowWidth = 18;
        const int ArrowHeight = 10;
        const int DiamondWidth = 20;
        const int DiamondHeight = 10;

        SNRelationship relation = null;
		Direction modifiedDirection;
        SNRelationshipType modifiedType;


        public SNConnectionDialog()
		{
			InitializeComponent();
			UpdateTexts();
		}

		public SNRelationship SNRelationship
		{
			get
			{
                return relation;
			}
			set
			{
				if (value != null)
				{
                    relation = value;
					UpdateFields();
                    UpdateTitle();
				}
			}
		}

		private void UpdateTexts()
		{
			this.Text = Strings.EditRelation;
			btnOK.Text = Strings.ButtonOK;
			btnCancel.Text = Strings.ButtonCancel;
		}

        private void UpdateTitle()
        {
            this.Text = "编辑关系类型---选中的类型：" + SNRelationship.SNRelationshipType.ToString();
        }

        private void UpdateFields()
        {
            modifiedDirection = SNRelationship.Direction;
            modifiedType = SNRelationship.SNRelationshipType;

            labelName.Text = SNRelationship.Label;
            txtStartRole.Text = SNRelationship.StartRole;
            txtEndRole.Text = SNRelationship.EndRole;
            txtStartMultiplicity.Text = SNRelationship.StartMultiplicity;
            txtEndMultiplicity.Text = SNRelationship.EndMultiplicity;
            
        }


		private void ModifyRelationship()
		{ 
            SNRelationship.SNRelationshipType = modifiedType;
            SNRelationship.Direction = modifiedDirection;
            SNRelationship.Label = labelName.Text;
            SNRelationship.StartMultiplicity = txtStartMultiplicity.Text;
            SNRelationship.EndMultiplicity = txtEndMultiplicity.Text;
            SNRelationship.StartRole = txtStartRole.Text;
            SNRelationship.EndRole = txtEndRole.Text;
        }

		private void picArrow_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.X <= DiamondWidth)
			{
				ChangeType();
				picArrow.Invalidate();
			}
			else if (e.X >= picArrow.Width - ArrowWidth)
			{
				ChangeHead();
				picArrow.Invalidate();
			}
		}

        private void NodeMouseHover(object sender,TreeNodeMouseHoverEventArgs e)
        {
            e.Node.ToolTipText = e.Node.ToolTipText;
        }

        private void ChangeSNRelationType(object sender,TreeViewEventArgs e)
        {
            if(e.Node!=null)
            {
                string text = e.Node.Text;

                //IS关系
                if (text == SNRational.IS)
                    modifiedType = SNRelationshipType.IS;
                else if (text == SNRational.ISA)
                    modifiedType = SNRelationshipType.ISA;
                else if (text == SNRational.ISO)
                    modifiedType = SNRelationshipType.ISO;
                else if (text == SNRational.ISP)
                    modifiedType = SNRelationshipType.ISP;
                else if (text == SNRational.ISPG)
                    modifiedType = SNRelationshipType.ISPG;
                else if (text == SNRational.ISC)
                    modifiedType = SNRelationshipType.ISC;
                else if (text == SNRational.AREC)
                    modifiedType = SNRelationshipType.AREC;
                else if (text == SNRational.ATT)
                    modifiedType = SNRelationshipType.ATT;
                else if (text == SNRational.VAL)
                    modifiedType = SNRelationshipType.VAL;
                else if (text == SNRational.VALR)
                    modifiedType = SNRelationshipType.VALR;
                else if (text == SNRational.NAME)
                    modifiedType = SNRelationshipType.NAME;
                else if (text == SNRational.SYMB)
                    modifiedType = SNRelationshipType.SYMB;
                else if (text == SNRational.ROLE)
                    modifiedType = SNRelationshipType.ROLE;
                else if (text == SNRational.KTYPE)
                    modifiedType = SNRelationshipType.KTYPE;
                else if (text == SNRational.DEF)
                    modifiedType = SNRelationshipType.DEF;
                else if (text == SNRational.PROP)
                    modifiedType = SNRelationshipType.PROP;
                else if (text == SNRational.PROPR)
                    modifiedType = SNRelationshipType.PROPR;

                //执行关系
                else if (text == SNRational.ACTR)
                    modifiedType = SNRelationshipType.ACTR;
                else if (text == SNRational.MACTR)
                    modifiedType = SNRelationshipType.MACTR;
                else if (text == SNRational.ACTED)
                    modifiedType = SNRelationshipType.ACTED;
                else if (text == SNRational.RESULT)
                    modifiedType = SNRelationshipType.RESULT;
                else if (text == SNRational.AFFED)
                    modifiedType = SNRelationshipType.AFFED;
                else if (text == SNRational.CSTR)
                    modifiedType = SNRelationshipType.CSTR;
                else if (text == SNRational.EXECR)
                    modifiedType = SNRelationshipType.EXECR;
                else if (text == SNRational.AVRT)
                    modifiedType = SNRelationshipType.AVRT;
                else if (text == SNRational.CHPE)
                    modifiedType = SNRelationshipType.CHPE;
                else if (text == SNRational.CIRCU)
                    modifiedType = SNRelationshipType.CIRCU;
                else if (text == SNRational.ORNT)
                    modifiedType = SNRelationshipType.ORNT;
                else if (text == SNRational.SUPPL)
                    modifiedType = SNRelationshipType.SUPPL;
                else if (text == SNRational.TOOL)
                    modifiedType = SNRelationshipType.TOOL;
                else if (text == SNRational.GOAL)
                    modifiedType = SNRelationshipType.GOAL;
                else if (text == SNRational.MANNR)
                    modifiedType = SNRelationshipType.MANNR;
                else if (text == SNRational.MODAL)
                    modifiedType = SNRelationshipType.MODAL;

                ///数学关系
                else if (text == SNRational.COMP)
                    modifiedType = SNRelationshipType.COMP;
                else if (text == SNRational.OPRED)
                    modifiedType = SNRelationshipType.OPRED;
                else if (text == SNRational.OPRND)
                    modifiedType = SNRelationshipType.OPRND;
                else if (text == SNRational.INVR)
                    modifiedType = SNRelationshipType.INVR;
                else if (text == SNRational.MAJ)
                    modifiedType = SNRelationshipType.MAJ;
                else if (text == SNRational.MAJE)
                    modifiedType = SNRelationshipType.MAJE;
                else if (text == SNRational.MIN)
                    modifiedType = SNRelationshipType.MIN;
                else if (text == SNRational.MINE)
                    modifiedType = SNRelationshipType.MINE;
                else if (text == SNRational.OPPS)
                    modifiedType = SNRelationshipType.OPPS;
                else if (text == SNRational.ASSGN)
                    modifiedType = SNRelationshipType.ASSGN;
                else if (text == SNRational.EXPR)
                    modifiedType = SNRelationshipType.EXPR;
                else if (text == SNRational.MRESULT)
                    modifiedType = SNRelationshipType.MRESULT;
                else if (text == SNRational.SUM)
                    modifiedType = SNRelationshipType.SUM;
                else if (text == SNRational.MULTI)
                    modifiedType = SNRelationshipType.MULTI;
                else if (text == SNRational.SUBM)
                    modifiedType = SNRelationshipType.SUBM;
                else if (text == SNRational.SUBME)
                    modifiedType = SNRelationshipType.SUBME;

                //比较关系
                else if (text == SNRational.CORR)
                    modifiedType = SNRelationshipType.CORR;
                else if (text == SNRational.CNVRS)
                    modifiedType = SNRelationshipType.CNVRS;
                else if (text == SNRational.CONTR)
                    modifiedType = SNRelationshipType.CONTR;
                else if (text == SNRational.COMPL)
                    modifiedType = SNRelationshipType.COMPL;
                else if (text == SNRational.CONC)
                    modifiedType = SNRelationshipType.CONC;
                else if (text == SNRational.ANLG)
                    modifiedType = SNRelationshipType.ANLG;
                else if (text == SNRational.ANLG2)
                    modifiedType = SNRelationshipType.ANLG2;
                else if (text == SNRational.ANLG3)
                    modifiedType = SNRelationshipType.ANLG3;
                else if (text == SNRational.DIFF2)
                    modifiedType = SNRelationshipType.DIFF2;
                else if (text == SNRational.DIFF3)
                    modifiedType = SNRelationshipType.DIFF3;
                else if (text == SNRational.ANTO)
                    modifiedType = SNRelationshipType.ANTO;
                else if (text == SNRational.SYNO)
                    modifiedType = SNRelationshipType.SYNO;
                else if (text == SNRational.SYMM)
                    modifiedType = SNRelationshipType.SYMM;
                else if (text == SNRational.EQUL)
                    modifiedType = SNRelationshipType.EQUL;
                else if (text == SNRational.EQUL2)
                    modifiedType = SNRelationshipType.EQUL2;
                else if (text == SNRational.EQUL3)
                    modifiedType = SNRelationshipType.EQUL3;

                //关联关系
                else if (text == SNRational.ARGU)
                    modifiedType = SNRelationshipType.ARGU;
                else if (text == SNRational.ARGV)
                    modifiedType = SNRelationshipType.ARGV;
                else if (text == SNRational.ARGV2)
                    modifiedType = SNRelationshipType.ARGV2;
                else if (text == SNRational.ARGV3)
                    modifiedType = SNRelationshipType.ARGV3;
                else if (text == SNRational.FARGV)
                    modifiedType = SNRelationshipType.FARGV;
                else if (text == SNRational.ASSOC)
                    modifiedType = SNRelationshipType.ASSOC;
                else if (text == SNRational.DEPT)
                    modifiedType = SNRelationshipType.DEPT;
                else if (text == SNRational.CAUSAL)
                    modifiedType = SNRelationshipType.CAUSAL;
                else if (text == SNRational.CONFM)
                    modifiedType = SNRelationshipType.CONFM;
                else if (text == SNRational.ATTCH)
                    modifiedType = SNRelationshipType.ATTCH;
                else if (text == SNRational.IMPL)
                    modifiedType = SNRelationshipType.IMPL;
                else if (text == SNRational.MCONT)
                    modifiedType = SNRelationshipType.MCONT;
                else if (text == SNRational.METH)
                    modifiedType = SNRelationshipType.METH;
                else if (text == SNRational.ORIG)
                    modifiedType = SNRelationshipType.ORIG;
                else if (text == SNRational.GRANU)
                    modifiedType = SNRelationshipType.GRANU;
                else if (text == SNRational.JUST)
                    modifiedType = SNRelationshipType.JUST;

                //时空关系
                else if (text == SNRational.SPACE)
                    modifiedType = SNRelationshipType.SPACE;
                else if (text == SNRational.GEOM)
                    modifiedType = SNRelationshipType.GEOM;
                else if (text == SNRational.LRANG)
                    modifiedType = SNRelationshipType.LRANG;
                else if (text == SNRational.LOC)
                    modifiedType = SNRelationshipType.LOC;
                else if (text == SNRational.LOCA)
                    modifiedType = SNRelationshipType.LOCA;
                else if (text == SNRational.ORIGL)
                    modifiedType = SNRelationshipType.ORIGL;
                else if (text == SNRational.DIR)
                    modifiedType = SNRelationshipType.DIR;
                else if (text == SNRational.TIME)
                    modifiedType = SNRelationshipType.TIME;
                else if (text == SNRational.STRT)
                    modifiedType = SNRelationshipType.STRT;
                else if (text == SNRational.DUR)
                    modifiedType = SNRelationshipType.DUR;
                else if (text == SNRational.ANTE)
                    modifiedType = SNRelationshipType.ANTE;
                else if (text == SNRational.DEST)
                    modifiedType = SNRelationshipType.DEST;
                else if (text == SNRational.TDEST)
                    modifiedType = SNRelationshipType.TDEST;
                else if (text == SNRational.PATH)
                    modifiedType = SNRelationshipType.PATH;

                //限制关系
                else if (text == SNRational.REF)
                    modifiedType = SNRelationshipType.REF;
                else if (text == SNRational.AMONG)
                    modifiedType = SNRelationshipType.AMONG;
                else if (text == SNRational.RANGE)
                    modifiedType = SNRelationshipType.RANGE;
                else if (text == SNRational.COND)
                    modifiedType = SNRelationshipType.COND;
                else if (text == SNRational.CONTXT)
                    modifiedType = SNRelationshipType.CONTXT;
                else if (text == SNRational.INIT)
                    modifiedType = SNRelationshipType.INIT;
                else if (text == SNRational.FINAL)
                    modifiedType = SNRelationshipType.FINAL;
                else if (text == SNRational.ORIGM)
                    modifiedType = SNRelationshipType.ORIGM;
                else if (text == SNRational.QMOD)
                    modifiedType = SNRelationshipType.QMOD;
                else if (text == SNRational.NON)
                    modifiedType = SNRelationshipType.NON;
                else if (text == SNRational.NUM)
                    modifiedType = SNRelationshipType.NUM;
                else if (text == SNRational.UNIT)
                    modifiedType = SNRelationshipType.UNIT;

                //使用外部数据
                else if (text == SNRational.ANNIM)
                    modifiedType = SNRelationshipType.ANNIM;
                else if (text == SNRational.DRAW)
                    modifiedType = SNRelationshipType.DRAW;
                else if (text == SNRational.IMAGE)
                    modifiedType = SNRelationshipType.IMAGE;
                else if (text == SNRational.SOUND)
                    modifiedType = SNRelationshipType.SOUND;


                //其它关系 
                else if (text == SNRational.AND)
                    modifiedType = SNRelationshipType.AND;
                else if (text == SNRational.OR)
                    modifiedType = SNRelationshipType.OR;
                else if (text == SNRational.HAS)
                    modifiedType = SNRelationshipType.HAS;
                else if (text == SNRational.BENF)
                    modifiedType = SNRelationshipType.BENF;
                else if (text == SNRational.MODE)
                    modifiedType = SNRelationshipType.MODE;
                else if (text == SNRational.FORM)
                    modifiedType = SNRelationshipType.FORM;
                else if (text == SNRational.SUBST)
                    modifiedType = SNRelationshipType.SUBST;
                else if (text == SNRational.CHPA)
                    modifiedType = SNRelationshipType.CHPA;
                else if (text == SNRational.CHPS)
                    modifiedType = SNRelationshipType.CHPS;
                else if (text == SNRational.CHSP1)
                    modifiedType = SNRelationshipType.CHSP1;
                else if (text == SNRational.CHSP2)
                    modifiedType = SNRelationshipType.CHSP2;
                else if (text == SNRational.SSPE)
                    modifiedType = SNRelationshipType.SSPE;

                labelName.Text = modifiedType.ToString();
                this.Text = "编辑关系类型---选中的类型：" + modifiedType.ToString();
            }
        }

        private void ChangeType()
        {

        }
         

		private void ChangeHead()
		{
			if (modifiedDirection == Direction.Bidirectional)
			{
				modifiedDirection = Direction.Unidirectional;
			}
			else
			{
				modifiedDirection = Direction.Bidirectional;
			}
		}

		private void picArrow_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			int center = picArrow.Height / 2;
			int width = picArrow.Width;

			// Draw line
			g.DrawLine(Pens.Black, 0, center, width, center);

			// Draw arrow head
			if (modifiedDirection == Direction.Unidirectional)
			{
				g.DrawLine(Pens.Black, width - ArrowWidth, center - ArrowHeight / 2, width, center);
				g.DrawLine(Pens.Black, width - ArrowWidth, center + ArrowHeight / 2, width, center);
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
            if (relation != null)
			{ 
                ModifyRelationship();
			}
		} 
         
	}
}
