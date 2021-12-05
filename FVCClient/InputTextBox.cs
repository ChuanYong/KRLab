using System;
using System.Collections.Generic;
using System.Media;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using System.Text.RegularExpressions;

namespace FVCClient
{
	public class InputTextBox : RichTextBox
    { 
		public bool IsPromptInsertedAtLaunch { get; set; }
		public bool IsSystemBeepEnabled { get; set; }
		public string Prompt { get; set; }

		public List<Answer> AnswerLog { get; private set; } 
		public bool IsInputEnabled { get; private set; } 

        public int LastPomptIndex { get; private set; }
        public int CaretIndex { get; set; } 

        private int indexInLog = 0;
        private TextRange _currentRange;

        private bool _isSelectAll = false;

        private string _helpTxt;

        public string Text
        {
            get
            {
                TextRange textRange = new TextRange(Document.ContentStart, Document.ContentEnd);
                return textRange.Text;
            }
            set
            {

                TextRange textRange = new TextRange(Document.ContentStart, Document.ContentEnd);
                textRange.Text = value;

                CaretPosition = Document.ContentEnd;
            }
        }

        public InputTextBox() {
			IsUndoEnabled = false;
			AcceptsReturn = false;
			AcceptsTab = false;

            AnswerLog = new List<Answer>();

			IsPromptInsertedAtLaunch = true;
			IsSystemBeepEnabled = true;
            LastPomptIndex = -1;

			Prompt = "> ";
			IsInputEnabled = true;   

            Loaded += (s, e) => {
				if (IsPromptInsertedAtLaunch)//提示提出新问题
					InsertNewPrompt();
			};

			TextChanged += (s, e) => {
                Focus();
				ScrollToEnd(); 
                CaretIndex = Text.Length;
            };

            _helpTxt = "欢迎来到<智学课堂>!\n\n";            
            _helpTxt += "如果需要提问或查询，请在问题后面加‘？’号。";
            _helpTxt += "比如，如果需要查询整个课程的章节，输入‘课程?'，\n";
            _helpTxt += "输入‘第1章？’、‘第1节?’可以查询章节内容。\n\n";
            _helpTxt += "按<左Shift+Return>键，智学系统为你提出学习课题。\n";
            _helpTxt += "按<左Ctrl+Return>键提交你的回答，并进行评估和检查。\n";
            _helpTxt += "按<左Ctrl+A>键，再按<delete>键清除当前输入的文档。\n\n";
            _helpTxt += "F1键，或输入'帮助?'，寻求使用说明。\n";
            _helpTxt += "F2键，或输入'答案?'，查看问题的正确答案。\n";
            _helpTxt += "F3键选择学习课程。\n"; 

            Text = _helpTxt;

        } 

        public void InsertNewPrompt()
        {
            AddText(Prompt);
            CaretIndex = Text.Length;
            LastPomptIndex = Text.Length;

            _currentRange = new TextRange(Document.ContentStart, CaretPosition.GetLineStartPosition(0));
            IsInputEnabled = true;
        } 

        // --------------------------------------------------------------------
        // EVENT HANDLER
        // --------------------------------------------------------------------
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if(e.Key==Key.F1)
            {
                AddHelpText();
            }

            if (_currentRange.Contains(CaretPosition))
            {
                if(e.Key==Key.Return )
                {
                    InsertNewPrompt();
                    e.Handled = true;
                    return;
                }
            } 

            if(Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
            {
                _isSelectAll = true;
            }

            if(e.Key==Key.Delete && _isSelectAll)
            {               
                Clear();
                InsertNewPrompt();
                _isSelectAll = false;
            }


            // If Ctrl+C is entered, raise an abort requested event !
            if (e.Key == Key.C) {
				if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) {
					RaiseAbortRequested();
					e.Handled = true;
					return;
				}
			}
            if(e.Key==Key.Return)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    if (ObtainAndSubmitAnswerHandler != null)
                        ObtainAndSubmitAnswerHandler.Invoke(); 
                    e.Handled = true;
                    return;
                }
                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    if (PresentQuestionHandler != null)
                    {
                         PresentQuestionHandler.Invoke();//主窗口的PresentAQuestion来处理
                    }
                    e.Handled = true;
                    return;
                }
            }

            if (e.Key==Key.F2)
            { 
                if (PresentCorrectHandler != null)
                {
                    PresentCorrectHandler();
                }
            }

            // Store the length of Text before any input processing.
            int initialLength = Text.Length;

            // If input is not allowed, warn the user and discard its input.
            if (!IsInputEnabled)
            {
                if (IsSystemBeepEnabled)
                    SystemSounds.Beep.Play();
                e.Handled = true;
            }

            if (CaretIndex < LastPomptIndex) {
				if (IsSystemBeepEnabled)
					SystemSounds.Beep.Play(); 
                CaretIndex = Text.Length;
                e.Handled = false;
                if (e.Key == Key.Back || e.Key == Key.Delete)
                    e.Handled = true;
            }
            else if (CaretIndex == LastPomptIndex && e.Key == Key.Back)
            {
				if (IsSystemBeepEnabled)
					SystemSounds.Beep.Play();
				e.Handled = true;
			}
            else if (CaretIndex == LastPomptIndex && e.Key == Key.Left)
            {
                if (IsSystemBeepEnabled)
                    SystemSounds.Beep.Play();
                e.Handled = true;
            }
            else if(CaretIndex>=LastPomptIndex && e.Key==Key.Up )
            {
                e.Handled = true;

            }

            // If input has not yet been discarded, test the key for special inputs.
            // ENTER   => validates the input
            // TAB     => launches command completion with registered commands
            // CTRL+C  => raises an abort request event
            if (!e.Handled)
            {
				switch (e.Key)
                {
                    //case Key.Enter:
				}
			}

            base.OnPreviewKeyDown(e);
		}

        protected void Clear()
        {
            Document.Blocks.Clear();
        }

        /// <summary>
        /// 获取用户输入的回答，在按下提交键或ctrl+return时调用。
        /// </summary>
        /// <param name="callback"></param>
        public void ObtainAnswer(Action<string> callback)
        {
            ///如果不是在输入提示符下输入的字符无效
            if (_currentRange.Contains(CaretPosition))
            {
                InsertNewPrompt();
                return; 
            }
            int idx = Text.LastIndexOf(Prompt);
            string answer = Text.Substring(idx+1); 

            IsInputEnabled = false;
            LastPomptIndex = int.MaxValue; 
             
            callback(answer);
        }
         
        /// <summary>
        /// 从ITS系统中获取
        /// </summary>
        /// <param name="info"></param>
        /// <param name="score"></param>
        public void AddLearningFeedback(string info,double score)
        { 
            AddText("得分"+score.ToString()+"。"+info); 
            InsertNewPrompt();
        }

        public void AddHint(string info)
        {
            AddText(info);
            InsertNewPrompt();
        }

        public void AddHelpText()
        {
            Paragraph p = new Paragraph(new Run(_helpTxt));
            p.FontSize = 20;
            p.LineHeight = 5;
            p.Foreground = Brushes.BlueViolet;
            Document.Blocks.Add(p);

            InsertNewPrompt();
        }

        public void AddCorrectText(string correct)
        { 
            Paragraph p = new Paragraph(new Run(correct));
            p.FontSize = 14;
            p.LineHeight = 3;
            p.Foreground = Brushes.Beige;
            Document.Blocks.Add(p);

            InsertNewPrompt();
        }

        public void AddWarningText(string txt)
        {
            string warning = System.Environment.NewLine + "---警示---" + System.Environment.NewLine;
            warning += txt;

            Paragraph p = new Paragraph(new Run(warning));
            p.FontSize = 14;
            p.LineHeight = 5;
            p.Foreground = Brushes.BlueViolet;
            Document.Blocks.Add(p);

        }

        public void ChooseCourse()
        {
            if (ChooseCourseHandler != null)
                ChooseCourseHandler.Invoke(); 
        }

        protected void AddText(string txt)
        {

            //Span sp = new Span();
            //sp.Inlines.Add(new Run(txt));

            //TextBlock tb = new TextBlock();
            //tb.Inlines.Add(sp);

            //Paragraph p = new Paragraph();
            //p.LineHeight = 4;
            //p.Inlines.Add(tb);
            Text += txt;
            
            //Document.Blocks.Add(new Paragraph(new Run(txt)));
            CaretPosition = Document.ContentEnd;
        }

        // --------------------------------------------------------------------
        // CUSTOM EVENTS
        // --------------------------------------------------------------------

        public event EventHandler<EventArgs> AbortRequested;

        public event Action PresentQuestionHandler;
        public event Action ObtainAndSubmitAnswerHandler;
        public event Action PresentCorrectHandler;
        public event Action ChooseCourseHandler;

		private void RaiseAbortRequested() {
			if (AbortRequested != null)
				AbortRequested(this, new EventArgs());
		} 
         

        public string CurrentInput
        {
            get
            {
                TextRange all = new TextRange(Document.ContentStart, Document.ContentEnd);
                return all.Text;
            }
            set
            {
                Document.Blocks.Clear();
                // TODO: re-parse and syntax highlight
                Document.ContentStart.InsertTextInRun(value.TrimEnd('\n', '\r'));
                CaretPosition = Document.ContentEnd;
            }
        }
        protected string GetTextWithPromptSuffix(string suffix)
        {
            string ret = Text.Substring(0, LastPomptIndex);
            return ret + suffix;
        }
        
        private int CurrentAnswerLineCountPreCursor
        {
            get
            {
                var lineCount = Document.Blocks.Count;
                if (lineCount > 0)
                {
                    CaretPosition.GetLineStartPosition(int.MinValue, out lineCount);
                    lineCount--;
                }
                else { lineCount = 1; }
                return Math.Abs(lineCount);
            }
        }

        private int CurrentAnswerLineCountPostCursor
        {
            get
            {
                var lineCount = Document.Blocks.Count;
                if (lineCount > 0)
                {
                    CaretPosition.GetLineStartPosition(int.MaxValue, out lineCount);
                    lineCount++; // because we're about to be on the next line ...
                }
                else { lineCount = 1; }
                return Math.Abs(lineCount);
            }
        }

        private int CurrentAnswerLineCount
        {
            get
            {
                var lineCount = Document.Blocks.Count;
                if (lineCount > 0)
                {
                    Document.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward).GetLineStartPosition(int.MaxValue, out lineCount);
                    lineCount++;
                }
                else { lineCount = 1; }
                return lineCount;
            }
        }

        private int GetLineIndex(TextPointer caretPosition)
        {
            var lineCount = Document.Blocks.Count;
            if (lineCount > 0)
            {
                caretPosition.GetLineStartPosition(int.MaxValue, out lineCount);
            }
            else
                lineCount = 1;
            return lineCount;
        }
    }
}
