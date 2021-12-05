using System;
using System.Xml;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;

using KRLab.Core;
using KRLab.DiagramEditor.NetworkDiagram.Editors;
using KRLab.DiagramEditor.NetworkDiagram.Dialogs;

namespace KRLab.DiagramEditor.NetworkDiagram.Shapes
{
    public abstract class CompositeNodeShape : NodeShape
    {        
        const int AccessSpacing = 12;

        static CompositeNodeEditor typeEditor = new CompositeNodeEditor();
        static MemberEditor memberEditor = new MemberEditor();
        static MembersDialog membersDialog = new MembersDialog();

        static SolidBrush memberBrush = new SolidBrush(Color.Black);
        static StringFormat accessFormat = new StringFormat(StringFormat.GenericTypographic);
        static Pen selectionPen = new Pen(Color.Black);

        public override NodeBase Node
        {
            get { return CompositeNode; }
        }

        protected abstract CompositeNode CompositeNode { get; }

        protected TypeEditor HeaderEditor
        {
            get { return typeEditor; }
        }

        protected EditorWindow ContentEditor
        {
            get { return memberEditor; }
        }

        static CompositeNodeShape()
        {
            accessFormat.Alignment = StringAlignment.Center;
            accessFormat.LineAlignment = StringAlignment.Center;
            selectionPen.DashPattern = new float[] { 2, 4 };
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="typeBase"/> is null.
        /// </exception>
        protected CompositeNodeShape(NodeBase node)
            : base(node)
        {
            MinimumSize = defaultMinSize;
            node.Modified += delegate { UpdateMinSize(); };

            UpdateMinSize();
        }
        
        public override Size Size
        {
            get
            {
                if (Collapsed)
                    return new Size(Width, HeaderHeight);
                else
                    return base.Size;
            }
            set
            {
                base.Size = value;
            }
        } 

        public override int Height
        {
            get
            {
                if (Collapsed)
                    return HeaderHeight;
                else
                    return base.Height;
            }
            set
            {
                base.Height = value;
            }
        }

        private bool CanDrawChevron
        {
            get
            {
                return (
                    Settings.Default.ShowChevron == ChevronMode.Always
                );
            }
        }

        private RectangleF CaptionRegion
        {
            get
            {
                return new RectangleF(
                    Left + MarginSize, Top + MarginSize,
                    Width - MarginSize * 2, HeaderHeight - MarginSize * 2
                );
            }
        }

        public sealed override IEntity Entity
        {
            get { return Node; }
        }

        protected internal Member ActiveMember
        {
            get
            {
                return CompositeNode.GetMember(MemberType.CP,ActiveMemberIndex);
            }
        }

        //private bool HasIdentifier(Style style)
        //{
        //    return (
        //        style.ShowSignature ||
        //        style.ShowStereotype && NodeBase.Stereotype != null
        //    );
        //}

        public override void MoveUp()
        {
            if (ActiveMember != null && CompositeNode.MoveUpItem(ActiveMember))
            {
                ActiveMemberIndex--;
            }
        }

        public override void MoveDown()
        {
            if (ActiveMember != null && CompositeNode.MoveDownItem(ActiveMember))
            {
                ActiveMemberIndex++;
            }
        }

        protected internal void EditMembers()
        {
            membersDialog.ShowDialog(CompositeNode);
        }

        protected override EditorWindow GetEditorWindow()
        {
            if (IsActive)
            {
                if (ActiveMember == null)
                    return HeaderEditor;
                else
                    return memberEditor;
            }
            else
            {
                return null;
            }
        }

        private static string GetMemberString(Member member)
        {
            return member.ToString();
        }

        internal Rectangle GetMemberRectangle(int memberIndex)
        {
            Rectangle record = new Rectangle(
                Left + MarginSize, Top + HeaderHeight + MarginSize,
                Width - MarginSize * 2, MemberHeight);

            record.Y += memberIndex * MemberHeight;
            if ( memberIndex >= CompositeNode.MemberCount)
            {
                record.Y += MarginSize * 2;
            }
            return record;
        }
        private void SelectMember(PointF location)
        {
            if (Contains(location))
            {
                //int index;
                int y = (int)location.Y;
                int top = Top + HeaderHeight + MarginSize;

                if (top <= y)
                {
                    //if (CompositeType.SupportsFields)
                    //{
                    //    index = (y - top) / MemberHeight;
                    //    if (index < CompositeType.FieldCount)
                    //    {
                    //        ActiveMemberIndex = index;
                    //        return;
                    //    }
                    //    top += MarginSize * 2;
                    //}

                    //int operationTop = top + CompositeType.FieldCount * MemberHeight;
                    //if (operationTop <= y)
                    //{
                    //    index = (y - top) / MemberHeight;
                    //    if (index < CompositeType.MemberCount)
                    //    {
                    //        ActiveMemberIndex = index;
                    //        return;
                    //    }
                    //}
                }
                ActiveMemberIndex = -1;
            }
        }

        private Font GetMemberFont(Member member, Style style)
        {
            Font memberFont = style.StaticMemberFont;
            //if (member.IsStatic)
            //{
            //    memberFont = style.StaticMemberFont;
            //}
            //else if (member is Operation &&
            //    (((Operation)member).IsAbstract || member.Parent is InterfaceType))
            //{
            //    memberFont = style.AbstractMemberFont;
            //}
            //else
            //{
            //    memberFont = GetFont(style);
            //}

            return memberFont;
        }

        protected internal override void ShowEditor()
        {
            EditorWindow editor = GetEditorWindow();
            if (editor != null)
            {
               // ShowEditor(editor);
            }
        }

        protected internal override void HideEditor()
        {
            //if (showedEditor != null)
            //{
            //    HideWindow(showedEditor);
            //    showedEditor = null;
            //}
        }

        protected override void OnMove(MoveEventArgs e)
        {
            base.OnMove(e);
            EditorWindow window = GetEditorWindow();
            if (window != null)
                window.Relocate(this);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            EditorWindow window = GetEditorWindow();
            if (window != null)
                window.Relocate(this);
        }

        protected override void OnMouseDown(AbsoluteMouseEventArgs e)
        {
            base.OnMouseDown(e);
            SelectMember(e.Location);
        }

        protected override void OnMouseUp(AbsoluteMouseEventArgs e)
        {
            base.OnMouseUp(e);

            //if (showedEditor != null)
            //{
            //    showedEditor.Focus();
            //}
        }

        protected override void OnDoubleClick(AbsoluteMouseEventArgs e)
        {
            base.OnDoubleClick(e);

            //if (!IsChevronPressed(e.Location) && Contains(e.Location) &&
            //    e.Button == MouseButtons.Left)
            //{
            //    ShowEditor();
            //}
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            //showChevron = true;
            if (Settings.Default.ShowChevron == ChevronMode.AsNeeded)
                NeedsRedraw = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            //showChevron = false;
            if (Settings.Default.ShowChevron == ChevronMode.AsNeeded)
                NeedsRedraw = true;
        }

        protected override Shape.ResizeMode GetResizeMode(AbsoluteMouseEventArgs e)
        {
            ResizeMode resizeMode = base.GetResizeMode(e);

            if (Collapsed)
            {
                return (resizeMode & ~ResizeMode.Bottom);
            }
            else
            {
                return resizeMode;
            }
        }
        
        private static StringAlignment GetHorizontalAlignment(ContentAlignment alignment)
        {
            switch (alignment)
            {
                case ContentAlignment.BottomLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.TopLeft:
                    return StringAlignment.Near;

                case ContentAlignment.BottomCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.TopCenter:
                default:
                    return StringAlignment.Center;

                case ContentAlignment.BottomRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.TopRight:
                    return StringAlignment.Far;
            }
        }

        private static StringAlignment GetVerticalAlignment(ContentAlignment alignment)
        {
            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    return StringAlignment.Near;

                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                default:
                    return StringAlignment.Center;

                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    return StringAlignment.Far;
            }
        }

        private static float GetHeaderTextTop(RectangleF textRegion, float textHeight,
            ContentAlignment alignment)
        {
            float top = textRegion.Top;

            switch (alignment)
            {
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    top += textRegion.Height - textHeight;
                    break;

                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    top += (textRegion.Height - textHeight) / 2;
                    break;
            }

            return top;
        }

        internal void DeleteActiveMember()
        {
            if (ActiveMemberIndex >= 0)
            {
                //int newIndex;
                //int fieldCount = CompositeNode.FieldCount;
                //int memberCount = CompositeNode.MemberCount;

                //if (ActiveMemberIndex == fieldCount - 1 && fieldCount >= 2) // Last field
                //{
                //    newIndex = fieldCount - 2;
                //}
                //else if (ActiveMemberIndex == memberCount - 1) // Last member
                //{
                //    newIndex = ActiveMemberIndex - 1;
                //}
                //else
                //{
                //    newIndex = ActiveMemberIndex;
                //}

                //CompositeNode.RemoveMember(ActiveMember);
                //ActiveMemberIndex = newIndex;
                //OnActiveMemberChanged(EventArgs.Empty);
            }
        }

        protected internal sealed override bool DeleteSelectedMember(bool showConfirmation)
        {
            if (IsActive && ActiveMember != null)
            {
                if (!showConfirmation || ConfirmMemberDelete())
                    DeleteActiveMember();
                return true;
            }
            else
            {
                return false;
            }
        }
        
        protected void DrawContent(IGraphics g, Style style)
        {
            Rectangle record = new Rectangle(
                Left + MarginSize, Top + HeaderHeight + MarginSize,
                Width - MarginSize * 2, MemberHeight);

            // Draw fields
            //foreach (Field field in CompositeNode.Fields)
            //{
            //    DrawMember(g, field, record, style);
            //    record.Y += MemberHeight;
            //}

            ////Draw separator line 
            //if (CompositeNode.SupportsFields)
            //{
            //    DrawSeparatorLine(g, record.Top + MarginSize);
            //    record.Y += MarginSize * 2;
            //} 
        }  

        protected override float GetRequiredWidth(Graphics g, Style style)
        {
            //float nameWidth;
            float identifierWidth = 0;

            //nameWidth = g.MeasureString(NodeBase.Name, GetNameFont(style),
            //    PointF.Empty, headerFormat).Width;

            //if (HasIdentifier(style))
            //{
            //    string identifier =
            //        (style.ShowSignature) ? NodeBase.Signature : NodeBase.Stereotype;
            //    identifierWidth = g.MeasureString(identifier, style.IdentifierFont,
            //        PointF.Empty, headerFormat).Width;
            //}

            //float requiredWidth = Math.Max(nameWidth, identifierWidth) + MarginSize * 2;
            //return Math.Max(requiredWidth, base.GetRequiredWidth(g, style));
            return identifierWidth;
        }

        protected override int GetRequiredHeight()
        {
            int memberCount = 0;
            int spacingHeight = 0;

            //if (CompositeType.SupportsFields)
            //{
            //    memberCount += CompositeType.FieldCount;
            //    spacingHeight += MarginSize * 2;
            //}
            //if (CompositeType.SupportsOperations)
            //{
            //    memberCount += CompositeType.OperationCount;
            //    spacingHeight += MarginSize * 2;
            //}

            return (HeaderHeight + spacingHeight + (memberCount * MemberHeight));
        }

        protected internal override void MoveWindow()
        {
            EditorWindow editor = GetEditorWindow();
            if (editor != null)
                editor.Relocate(this);
        }

        protected override void OnSerializing(SerializeEventArgs e)
        {
            //if (collapsed)
            //{
            //    collapsed = false;
            //    base.OnSerializing(e);
            //    collapsed = true;
            //}
            //else
            //{
            //    base.OnSerializing(e);
            //}

            XmlElement collapsedNode = e.Node.OwnerDocument.CreateElement("Collapsed");
            collapsedNode.InnerText = Collapsed.ToString();
            e.Node.AppendChild(collapsedNode);
        }

        protected override void OnDeserializing(SerializeEventArgs e)
        {
            base.OnDeserializing(e);

            XmlElement collapsedNode = e.Node["Collapsed"];
            if (collapsedNode != null)
            {
                bool collapsed;
                if (bool.TryParse(collapsedNode.InnerText, out collapsed))
                    this.Collapsed = collapsed;
            }
            UpdateMinSize();
        }

        public override string ToString()
        {
            return Node.ToString();
        }
    }
}
