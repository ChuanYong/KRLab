using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls; 
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes; 
using System.Reflection;
using System.Threading; 

using ScottPlot;
 
using Utilities;

using ITS;
using ITS.TutorModule;

namespace FVCClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// MainWindow is the student's GUI, the gateway of the 
    /// system for comunicating with the student. All the learning
    /// materials and test sets are presented to the student through
    /// this interface and test results are fed back to the system
    /// for asssesing the student.
    /// </summary>
    public partial class MainWindow : Window 
    { 
        private FormulaEditorDialog _dialog;
        private ClientSystem _client;         
        protected OutputTextBox _problemTextBox;

        protected ScottPlot.FormsPlot Chart;
        protected System.Windows.Forms.Timer timer1;
        
        public MainWindow()
        {
            InitializeComponent();
            AnswerInputBox.Prompt = ">";

            Loaded += (s, e) =>
            { 

                AnswerInputBox.AbortRequested += (ss, ee) =>
                {
                    System.Windows.MessageBox.Show("Abort!");

                };
                AnswerInputBox.PresentQuestionHandler += () =>
                  {
                      this.PresentAQuestion();
                  };
                AnswerInputBox.ObtainAndSubmitAnswerHandler += () =>
                  {
                      this.ObtainAndSubmitAnswer();
                  };

                AnswerInputBox.PresentCorrectHandler += () =>
                  {
                      this.PresentCorrects();
                  };

                AnswerInputBox.ChooseCourseHandler += () =>
                  {
                      this.ChooseCourse();
                  };

                AnswerInputBox.InsertNewPrompt();
            };
        }

        /// <summary>
        /// 通过ClientSystem启动ITS
        /// </summary>
        private void StartITSClient()
        {
            _client = new ClientSystem();
            _problemTextBox = new OutputTextBox(QuestionTextBox);

            try
            {
                _client.OnStart((isOk,question) =>
                {
                    string info = question.Content;
                    if(!isOk)
                    {
                        _problemTextBox.AddWarningText(info);
                    }
                    else 
                    {
                        _problemTextBox.AddQuestion(question); 
                    }
            
                });
                Title = "智学课堂---您正在学习的课程：" + _client.Course;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void ChooseButtonClick(object sender,RoutedEventArgs e)
        {
            ChooseCourse();
        }

        private void ChooseCourse()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = FileManager.KnowledgePath;
                dialog.Filter = "课程文件(*.topticsn)|*.topicsn";
                dialog.RestoreDirectory = true;
                dialog.FilterIndex = 1;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string course = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName);

                    Title = "智学课堂---您正在学习的课程：" + course;

                    _client.OnChooseCourse(course, question =>
                    {
                        _problemTextBox.OnChooseCourse(question.Content);
                    });
                }
            }
        }


        /// <summary>
        /// 开始学习，ITS系统自动给出学习材料和测试题目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            //PresentAQuestion();
        }

        private void PresentAQuestion()
        {
            _client.OnStartAQuestion((isOk, question) =>
            {
                if (!isOk)
                {
                    AnswerInputBox.AddHint(question.Content); 
                }
                else
                {
                    _problemTextBox.AddQuestion(question);
                }
            });
        }

        private void PresentCorrects()
        {
            _client.OnObtainCorrects(correct =>
            {
                AnswerInputBox.AddCorrectText(correct);
            });
        }

        /// <summary>
        /// 测试一个问题之后，点击提交按钮，该函数响应。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButtonClick(object sender, RoutedEventArgs e)
        {
            ObtainAndSubmitAnswer();
        }

        private void ObtainAndSubmitAnswer()
        {
            AnswerInputBox.ObtainAnswer(answer =>
            {
                if (answer.Contains("答案？") || answer.Contains("答案?"))
                {
                    PresentCorrects();
                }
                else if (answer.Contains("帮助？") || answer.Contains("帮助?"))
                {
                    AnswerInputBox.AddHelpText();
                }
                else if (answer.Contains("?") || answer.Contains("？"))
                {
                    int i = answer.IndexOf("？");
                    int j = answer.IndexOf("?");

                    if (i != -1)
                        answer = answer.Remove(i);
                    if (j != -1)
                        answer = answer.Remove(j);

                    _client.OnSearch(answer, (isOk, result) =>
                    {
                        AnswerInputBox.AddCorrectText(result);
                    });
                }
                else
                {
                    _client.OnAnswerSubmitted(answer, (info, topicResult) =>
                    {
                        AnswerInputBox.AddLearningFeedback(info, topicResult);
                    });
                }

            });
        }

        private void OnObtainCorrect()
        {
            _client.OnObtainCorrects(correct=>
            {
                AnswerInputBox.AddCorrectText(correct);
            }
                );
        }

        private void FormulaInputClick(object sender, RoutedEventArgs e)
        {
            _dialog = new FormulaEditorDialog();
            _dialog.onConfirm += OnFormulaConfirmed;
            _dialog.ShowDialog();
        }


        private void OnFormulaConfirmed(string fs)
        {
            try
            {
                //byte[] bs = QAEngine.StrToBytes(fs);
                //Image img = this.ExpressionToImage(bs);
                //this.AddImageToTextBox(img);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void LoadExtenstions(string[] exts)
        {
            int n = exts.Length;
            for (int i = 0; i < n; i++)
            {
                string sname = exts[i];
                try
                {
                    Assembly.Load(sname);
                }
                catch (Exception)
                { }
            }
        }

        private void InitializeAnalytics()
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartITSClient();
            InitializeAnalytics();

            ///动态画图
            //this.Chart = new ScottPlot.FormsPlot();

            //this.Chart.Dock = System.Windows.Forms.DockStyle.Top;
            //this.Chart.Location = new System.Drawing.Point(0, 0);
            //this.Chart.Name = "Chart";
            //this.Chart.Size = new System.Drawing.Size(861, 489);
            //this.Chart.TabIndex = 0;       
            
            //// 
            //// timer1
            //// 
            //this.timer1.Enabled = true;
            //this.timer1.Interval = 10;
            //this.timer1.Tick += new System.EventHandler(this.EveryFrame);

            //var A = MathS.Var("A");
            //var B = MathS.Var("B");
            //var expr1 = MathS.Cos(B) * MathS.Sin(B) * MathS.Pow(MathS.e, MathS.i * B * MathS.Cos(A));
            //var expr2 = B * MathS.Sin(A + B) * MathS.Pow(MathS.e, MathS.i * B * MathS.Cos(A));
            //niceFunc1 = expr1.Compile(A, B);
            //niceFunc2 = expr2.Compile(A, B);
        }


        //FastExpression niceFunc1;
        //FastExpression niceFunc2;
        double t = 120; 


        /// <summary>
        /// 动画
        /// </summary>
        readonly List<double> X1 = new List<double>();
        readonly List<double> Y1 = new List<double>();
        readonly List<double> X2 = new List<double>();
        readonly List<double> Y2 = new List<double>();
        private void EveryFrame(object sender, EventArgs e)
        {
            X1.Clear(); Y1.Clear();
            X2.Clear(); Y2.Clear();
            var A = t;
            for (double B = 0; B < A; B += 0.1)
            {
                //var res = niceFunc1.Call(A, B);
                //X1.Add(res.Re * 150);
                //Y1.Add(res.Im * 150);

                //res = niceFunc2.Call(A, B);
                //X2.Add(res.Re + 160);
                //Y2.Add(res.Im);
            }
            Chart.plt.Clear();
            Chart.plt.PlotScatter(X1.ToArray(), Y1.ToArray());
            Chart.plt.PlotScatter(X2.ToArray(), Y2.ToArray());
            Chart.Render();
            t += 0.0005;
        }

        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if(Keyboard.IsKeyDown(Key.F3))
            {
                AnswerInputBox.ChooseCourse();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _client.Exit(); 
        } 

    }
}
