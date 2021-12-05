using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.FuzzyEngine;

namespace KRLabConsole
{
    using matrix = List<List<double>>;

    /// <summary>
    /// Response Accuracy
    /// </summary>
    class COR
    {
        protected string _name;
        protected List<List<double>> _data;
        public string Name
        { get { return _name; } }
        public List<List<double>> Data
        { get { return _data; } }

        public COR(string name)
        {
            _name = name;
            _data = new List<List<double>>();
        } 

        /// <summary>
        /// calcualte the membership grade of time
        /// seeing equation (3) in the literature.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="t">question solving time</param>
        /// <param name="apha">lower limit solving time</param>
        /// <param name="gama">upper limit solving time</param>
        public virtual void MembershipGrade(out double data,double t,double apha,double gama)
        {
            double beita = 0.5 * (apha + gama);
            if (t <= apha)
                data = 1;
            else if (t > apha && t <= beita)
            {
                double tmp = (t - apha) / (gama - apha);
                data = 1 - 2 * tmp * tmp;
            }
            else if (t >= beita && t < gama)
            {
                double tmp = (t - gama) / (gama - apha);
                data = 2 * tmp * tmp;
            }
            else if(t>=gama)
            {
                data = 0;
            }
            else
            {
                data = double.NaN;
                throw new Exception("error for membership grade of time!");
            }
        }
    }
    class ICOR:COR
    {
        private double _k;
        public ICOR(double weight):base("Importance")
        {
            _k = weight;
        }

        public override void MembershipGrade(out double data, double t, double apha, double gama)
        {
            base.MembershipGrade(out data, t, apha, gama);
            data = Math.Pow(data, _k);
        }
    }
    class CCOR:COR
    {
        private double _k;
        public CCOR(double weight):base("Complex")
        {
            _k = weight;
        }

        public override void MembershipGrade(out double data, double t, double apha, double gama)
        {
            double gama1 = gama + apha;
            double beita = 0.5 * (apha + gama1);
            if (t <= apha)
                data = 1;
            else if (t > apha && t <= beita)
            {
                double tmp = (t - apha) / (gama1 - apha);
                data = 1 - 2 * tmp * tmp;
            }
            else if (t >= beita && t < gama1)
            {
                double tmp = (t - gama1) / (gama1 - apha);
                data = 2 * tmp * tmp;
            }
            else if (t >= gama1)
            {
                data = 0;
            }
            else
            {
                data = double.NaN;
                throw new Exception("error for membership grade of time!");
            }

            data = Math.Pow(data, _k);
        }
    }
    class DCOR:COR
    {
        private double _h;
        public DCOR(double weight):base("Difficulty")
        {
            _h = weight;
        }

        public override void MembershipGrade(out double data, double t, double apha, double gama)
        {
            base.MembershipGrade(out data, t, apha, gama);
            data = Math.Pow(data, _h);
        }
    }
    /// <summary>
    /// see literature:
    /// S. Bai and S. Chen,
    /// "Evaluating students’ learning achievement using fuzzy
    ///membership functions and fuzzy rules".
    /// Expert Systems with Application, 34, 399-410,2008.
    /// </summary>
    class BaiChenEvaluationMethod
    {
        private LinguisticVariable _performanceFuzzyVar;

        private LinguisticVariable _fuzzyVar;
        private List<IMembershipFunction> _mfs;

        private IFuzzyEngine _fuzzyEngine;

        //data for accuracy rate matrix
        private matrix _arm;
        //data for time rate matrix
        private matrix _trm;
        //data for grades of question set
        private List<double> _g;
        //data for importance matrix
        private matrix _im;
        //data for complexity matrix
        private matrix _cm;

        private string[] _fuzzySet = new [] {"low","more or less low",
            "medium","more or less high","high"}; 

        public BaiChenEvaluationMethod()
        {
            InitARM();
            Utilities.Print("A", _arm);
            InitTRM();
            Utilities.Print("T", _trm);
            InitG();
            Utilities.Print("G", _g);
            InitIM();
            Utilities.Print("IM", _im);
            InitCM();
            Utilities.Print("C", _cm);
            InitMFs();
            InitFuzzyEngine();
        }

        public void Run()
        {
            //(1)the first step
            List<double> avrA, avrT;
            CalculateAverageAccuracy(out avrA);
            CalculateAverageTime(out avrT);
            Utilities.Print("avrA", avrA); Utilities.Print("avrT", avrT);

            matrix FA, FT;
            ProduceFuzzyMatrix(out FA, out FT,avrA,avrT);
            Utilities.Print("FA", FA); Utilities.Print("FT", FT);

            List<double> scores;
            CalculateStudentTotalScore(out scores);
            Utilities.Print("Score",scores);

            List<List<int>> equScores;
            FindEqualScoreInfo(out equScores,scores);


            //(2)the second step,based on the fuzzy matrices FA and FT,
            //, and fuzzy rules, perform the fuzzy reasoning to evaluate
            //the difficulty of each question.
            matrix D = new matrix();
            //for (int i = 0; i < 5; i++)
            //{
            //    List<double> tmp = new List<double>()
            //    {
            //        Di0(i,FA,FT),Di1(i,FA,FT),Di2(i,FA,FT),Di3(i,FA,FT),Di4(i,FA,FT)
            //    };

            //    D.Add(tmp);
            //}

            CalculateDifficultyMatrix(out D, _arm);
            //D = FT;
            Utilities.Print("D", D);

            List<double> avrD = new List<double>();
            CalculateAveragedDifficulty(out avrD, D);
            Utilities.Print("avrD", avrD);

            //(3)calculate answer-cost matrix CO
            matrix CO = new matrix();
            for (int i = 0; i < 5; i++)
            {
                List<double> tmp = new List<double>()
                {
                    ACi0(i,D,_cm),ACi1(i,D,_cm),ACi2(i,D,_cm),ACi3(i,D,_cm),ACi4(i,D,_cm)
                };

                CO.Add(tmp);
            }
            Utilities.Print("CO",CO);

            //(4)based on the answer-cost matrices and the importance matrics
            //performance the fuzzy reasoning to evaluate the adjustment value
            //of each question.
            matrix V = new matrix();
            for (int i = 0; i < 5; i++)
            {
                List<double> tmp = new List<double>()
                {
                    Vi0(i,_im,CO),Vi1(i,_im,CO),Vi2(i,_im,CO),Vi3(i,_im,CO),Vi4(i,_im,CO)
                };

                V.Add(tmp);
            }
            Utilities.Print("V",V);
            List<double> adv;
            CalculateADV(out adv, V);
            Utilities.Print("ADV",adv);

            //(5)assum there are k students having the same
            //total grade, we construct a new grade matrix EA
            // for these equal-grade students,
            matrix EA;

            equScores = new List<List<int>>() { new List<int> {0,1,2,3,4,5,6,7,8,9 } };
           
            CalculateEA(out EA, equScores[0]);
            Utilities.Print("EA",EA);
            //(6)
            List<double> SOD;
            CalculateSOD(out SOD, _arm, adv,_g,scores);

            Utilities.Print("SOD",SOD);

            List<int> sortedIndex0;
            SortSOD(out sortedIndex0, scores);
            Utilities.Print("index0", sortedIndex0);

            List<int> sortedIndex1;
            SortSOD(out sortedIndex1, SOD);
            Utilities.Print("index1", sortedIndex1);

        }

        /// <summary>
        /// 计算某个学生的总分数
        /// </summary>
        /// <param name="i">某个学生的序号</param>
        /// <returns></returns>
        public double CalcualteTScoreOfStudent(int i)
        {
            double sum = 0;
            List<double> tmp = new List<double>();
            for(int j=0;j<_arm.Count;j++)
            {
                tmp.Add(_arm[j][i]);
            }

            if (tmp.Count != _g.Count)
                throw new Exception("Error for the score calculation");

            for(int j=0;j<_g.Count;j++)
            {
                sum += tmp[j] * _g[j];
            }

            return sum;

        }

        /// <summary>
        /// 计算某个题目的评价准确率
        /// </summary>
        /// <param name="i">某个题目的序号</param>
        /// <returns></returns>
        private void CalculateAverageAccuracy(out List<double> av)
        {
            int nb = _arm.Count;
            av = new List<double>();
            for(int i=0;i<nb;i++)
            {
                av.Add(_arm[i].Average());
            }
        }
        /// <summary>
        /// 计算某个题目的平均时间
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public void CalculateAverageTime(out List<double> av)
        {
            int nb = _arm.Count;
            av = new List<double>();
            for (int i = 0; i < nb; i++)
            {
                av.Add(_trm[i].Average());
            } 
        }

        public void CalculateStudentTotalScore(out List<double> data)
        {
            data = new List<double>();
            int nb = _arm[0].Count;
            for(int i=0;i<nb;i++)
            {
                double sum = 0.0;
                for (int j = 0; j < _arm.Count; j++)
                {
                    sum += _arm[j][i]*_g[j];
                }
                data.Add(sum);
            }

        }

        /// <summary>
        /// 查到scores中分数相同的元素，序号和值。
        /// </summary>
        /// <param name="scores"></param>
        public void FindEqualScoreInfo(out List<List<int>> index,List<double> scores)
        {
            List<double> tmp = new List<double>();
            for(int i=0;i<scores.Count;i++)
            {
                if (!tmp.Contains(scores[i]))
                {
                    List<double> list = scores.FindAll(x => { return x == scores[i]; });

                    if (list.Count >= 2)
                    {
                        tmp.Add(scores[i]);
                    }
                }
            }

            index = new List<List<int>>();
            foreach(var e in tmp)
            {
                List<int> list = new List<int>();
                for(int i=0;i<scores.Count;i++)
                {
                    if (e == scores[i])
                        list.Add(i);
                }
                index.Add(list);
            } 

        }

        private void InitFuzzyEngine()
        {
            _fuzzyEngine = new FuzzyEngineFactory().Default();

            //为学习成绩(learning performance)创建模糊集的成员关系函数(MF)
            _performanceFuzzyVar = new LinguisticVariable("Performance");
            var excel = _performanceFuzzyVar.MembershipFunctions.AddTriangle("Excellent", 0.95, 1, 1);
            var veryGood = _performanceFuzzyVar.MembershipFunctions.AddTriangle("VeryGood", 0.8, 0.9, 0.9);
            var good = _performanceFuzzyVar.MembershipFunctions.AddTriangle("Good", 0.7, 0.8, 0.9);
            var average = _performanceFuzzyVar.MembershipFunctions.AddTriangle("Average", 0.5, 0.6, 0.75);
            var poor = _performanceFuzzyVar.MembershipFunctions.AddTriangle("Poor", 0, 0, 0.55);             
    
        }

        private void InitMFs()
        {
            _fuzzyVar = new LinguisticVariable("FuzzyVariable");
            var mf0 = _fuzzyVar.MembershipFunctions.AddTrapezoid(_fuzzySet[0], 0, 0, 0.1, 0.3);
            var mf1 = _fuzzyVar.MembershipFunctions.AddTriangle(_fuzzySet[1], 0.1, 0.3, 0.5);
            var mf2 = _fuzzyVar.MembershipFunctions.AddTriangle(_fuzzySet[2], 0.3, 0.5, 0.7);
            var mf3 = _fuzzyVar.MembershipFunctions.AddTriangle(_fuzzySet[3], 0.5, 0.7, 0.9);
            var mf4 = _fuzzyVar.MembershipFunctions.AddTrapezoid(_fuzzySet[4], 0.7, 0.9, 1.0, 1.0);

            _mfs=new List<IMembershipFunction>(){mf0,mf1,mf2,mf3,mf4 };
        }
         

        private void InitARM()
        {
            var row0 = new List<double>() {0.59,0.35,1,0.66,0.11,0.08,0.84,0.23,0.4,0.24 };
            var row1 = new List<double>() {0.01,0.27,0.14,0.04,0.88,0.16,0.04,0.22,0.81,0.53};
            var row2 = new List<double>() {0.77,0.69,0.97,0.71,0.17,0.86,0.87,0.42,0.91,0.74};
            var row3 = new List<double>() {0.73,0.72,0.18,0.16,0.5,0.02,0.32,0.92,0.9,0.25};
            var row4 = new List<double>() {0.93,0.49,0.08,0.81,0.65,0.93,0.39,0.51,0.97,0.61};

            _arm = new matrix() {row0,row1,row2,row3,row4 }; 
        }
        private void InitTRM()
        { 
            var row0 = new List<double>() {0.7,0.4,0.1,1,0.7,0.2,0.7,0.6,0.4,0.9 };
            var row1 = new List<double>() {1,0,0.9,0.3,1,0.3,0.2,0.8,0,0.3 };
            var row2 = new List<double>() {0,0.1,0,0.1,0.9,1,0.2,0.3,0.1,0.4 };
            var row3 = new List<double>() {0.2,0.1,0,1,1,0.3,0.4,0.8,0.7,0.5 };
            var row4 = new List<double>() {0,0.1,1,1,0.6,1,0.8,0.2,0.8,0.2 };

            _trm = new matrix() { row0, row1, row2, row3, row4 }; 
        }
        private void InitG()
        {
            _g = new List<double>() {10,15,20,25,30 };
        }
        private void InitIM()
        { 
            var row0 = new List<double>() {0,0,0,0,1 };
            var row1 = new List<double>() {0, 0.33, 0.67, 0, 0 };
            var row2 = new List<double>() {0,0,0,0.15,0.85 };
            var row3 = new List<double>() {1,0,0,0,0 };
            var row4 = new List<double>() {0,0.07,0.93,0,0 };

            _im = new matrix() {row0,row1,row2,row3,row4 }; 
        }

        private void InitCM()
        { 
            var row0 = new List<double>() { 0, 0.85, 0.15, 0, 0 };
            var row1 = new List<double>() { 0, 0,0.33, 0.67, 0 };
            var row2 = new List<double>() { 0, 0, 0, 0.69, 0.31 };
            var row3 = new List<double>() { 0.56, 0.44, 0, 0, 0 };
            var row4 = new List<double>() { 0, 0, 0.7,0.3, 0 };
            _cm = new matrix() { row0, row1, row2, row3, row4 }; 
        }

        /// <summary>
        /// calculate the fuzzy grade matrix fam for the average accuracy and 
        /// the fuzzy grade matrix tam for the average answer-time.
        /// </summary>
        /// <param name="fam"></param>
        private void ProduceFuzzyMatrix(out matrix fam,
            out matrix tam,List<double> avrA,List<double> avrT)
        {
            fam = new matrix();
            tam = new matrix();
            int nb = _arm.Count;//the number of questions
            for(int i=0;i<nb;i++)
            {
                double aa = avrA[i];  
                double at = avrT[i];  
                List<double> tmp0 = new List<double>();
                List<double> tmp1 = new List<double>();
                foreach(var e in _mfs)
                {
                    tmp0.Add(e.Fuzzify(aa));
                    tmp1.Add(e.Fuzzify(at));
                }
                fam.Add(tmp0);
                tam.Add(tmp1);
            }
        }

        /// <summary>
        /// equation (23)
        /// </summary>
        /// <param name="i"></param>
        /// <param name="fa"></param>
        /// <param name="ft"></param>
        /// <returns></returns>
        private double Di0(int i,matrix fa,matrix ft)
        {
            List<double> list = new List<double>()
            {
                0.6*fa[i][3]+0.4*ft[i][0],
                0.6*fa[i][4]+0.4*ft[i][0],
                0.6*fa[i][4]+0.4*ft[i][1]
            };
            return list.Max();
        }
        private double Di1(int i, matrix fa, matrix ft)
        {
            List<double> list = new List<double>()
            {
                0.6*fa[i][1]+0.4*ft[i][0],
                0.6*fa[i][2]+0.4*ft[i][0],
                0.6*fa[i][2]+0.4*ft[i][1],
                0.6*fa[i][3]+0.4*ft[i][1],
                0.6*fa[i][3]+0.4*ft[i][2],
                0.6*fa[i][4]+0.4*ft[i][2],
                0.6*fa[i][4]+0.4*ft[i][3]
            };
            return list.Max();
        }
        private double Di2(int i, matrix fa, matrix ft)
        {
            List<double> list = new List<double>()
            {
                0.6*fa[i][0]+0.4*ft[i][0],
                0.6*fa[i][1]+0.4*ft[i][1],
                0.6*fa[i][2]+0.4*ft[i][2],
                0.6*fa[i][3]+0.4*ft[i][3],
                0.6*fa[i][4]+0.4*ft[i][4], 
            };
            return list.Max();
        }
        private double Di3(int i, matrix fa, matrix ft)
        {
            List<double> list = new List<double>()
            {
                0.6*fa[i][0]+0.4*ft[i][1],
                0.6*fa[i][0]+0.4*ft[i][2],
                0.6*fa[i][1]+0.4*ft[i][2],
                0.6*fa[i][1]+0.4*ft[i][3],
                0.6*fa[i][2]+0.4*ft[i][3],
                0.6*fa[i][2]+0.4*ft[i][4],
                0.6*fa[i][3]+0.4*ft[i][4]
            };
            return list.Max();
        }
        private double Di4(int i, matrix fa, matrix ft)
        {
            List<double> list = new List<double>()
            {
                0.6*fa[i][0]+0.4*ft[i][3],
                0.6*fa[i][0]+0.4*ft[i][4],
                0.6*fa[i][1]+0.4*ft[i][4], 
            };
            return list.Max();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="d">difficulty matrix</param>
        /// <param name="c">complexity matrix</param>
        /// <returns></returns>
        private double ACi0(int i,matrix d,matrix c)
        {
            List<double> tmp = new List<double>()
            {
                0.7*d[i][0]+0.3*c[i][0],
                0.7*d[i][0]+0.3*c[i][1],
                0.7*d[i][1]+0.3*c[i][0]
            };
            return tmp.Max();
        }
        private double ACi1(int i,matrix d,matrix c)
        {
            List<double> tmp = new List<double>()
            {
                0.7*d[i][0]+0.3*c[i][2],
                0.7*d[i][0]+0.3*c[i][3],
                0.7*d[i][1]+0.3*c[i][1],
                0.7*d[i][1]+0.3*c[i][2],
                0.7*d[i][2]+0.3*c[i][0],
                0.7*d[i][2]+0.3*c[i][1],
                0.7*d[i][3]+0.3*c[i][0]

            };
            return tmp.Max();
        }
        private double ACi2(int i, matrix d, matrix c)
        {
            List<double> tmp = new List<double>()
            {
                0.7*d[i][0]+0.3*c[i][4],
                0.7*d[i][1]+0.3*c[i][3],
                0.7*d[i][2]+0.3*c[i][2],
                0.7*d[i][3]+0.3*c[i][1],
                0.7*d[i][4]+0.3*c[i][0] 
            };
            return tmp.Max();
        }
        private double ACi3(int i, matrix d, matrix c)
        {
            List<double> tmp = new List<double>()
            {
                0.7*d[i][1]+0.3*c[i][4],
                0.6*d[i][2]+0.4*c[i][3],
                0.6*d[i][2]+0.4*c[i][4],
                0.6*d[i][3]+0.4*c[i][2],
                0.6*d[i][3]+0.4*c[i][3],
                0.6*d[i][4]+0.4*c[i][1],
                0.6*d[i][4]+0.4*c[i][2]

            };
            return tmp.Max();
        }
        private double ACi4(int i, matrix d, matrix c)
        {
            List<double> tmp = new List<double>()
            {
                0.7*d[i][3]+0.3*c[i][4],
                0.7*d[i][4]+0.3*c[i][3],
                0.7*d[i][4]+0.3*c[i][4] 
            };
            return tmp.Max();
        }

        private double Vi0(int i,matrix im,matrix ac)
        {
            List<double> tmp = new List<double>()
            {
                0.5*(im[i][0]+ac[i][0]),
                0.5*(im[i][0]+ac[i][1]),
                0.5*(im[i][1]+ac[i][0])
            };

            return tmp.Max();
        }
        private double Vi1(int i, matrix im, matrix ac)
        {
            List<double> tmp = new List<double>()
            {
                0.5*(im[i][0]+ac[i][2]),
                0.5*(im[i][0]+ac[i][3]),
                0.5*(im[i][1]+ac[i][1]),
                0.5*(im[i][1]+ac[i][2]),
                0.5*(im[i][2]+ac[i][0]),
                0.5*(im[i][2]+ac[i][1]),
                0.5*(im[i][3]+ac[i][0]),
            };

            return tmp.Max();
        }
        private double Vi2(int i, matrix im, matrix ac)
        {
            List<double> tmp = new List<double>()
            {
                0.5*(im[i][0]+ac[i][4]),
                0.5*(im[i][1]+ac[i][3]),
                0.5*(im[i][2]+ac[i][2]),
                0.5*(im[i][3]+ac[i][1]),
                0.5*(im[i][4]+ac[i][0]), 
            };

            return tmp.Max();
        }
        private double Vi3(int i, matrix im, matrix ac)
        {
            List<double> tmp = new List<double>()
            {
                0.5*(im[i][1]+ac[i][4]),
                0.5*(im[i][2]+ac[i][3]),
                0.5*(im[i][2]+ac[i][4]),
                0.5*(im[i][3]+ac[i][2]),
                0.5*(im[i][3]+ac[i][3]),
                0.5*(im[i][4]+ac[i][1]),
                0.5*(im[i][4]+ac[i][2]),
            };

            return tmp.Max();
        }
        private double Vi4(int i, matrix im, matrix ac)
        {
            List<double> tmp = new List<double>()
            {
                0.5*(im[i][3]+ac[i][4]),
                0.5*(im[i][4]+ac[i][3]),
                0.5*(im[i][4]+ac[i][4]), 
            };

            return tmp.Max();
        }

        private void CalculateADV(out List<double> adv,matrix v)
        {
            adv = new List<double>();
            foreach(var list in v)
            {
                double a = 0.1 + 0.3 + 0.5 + 0.7 + 0.9;
                double x = (0.1 * list[0] + 0.3 * list[1] + 0.5 * list[2] + 0.7 * list[3] + 0.9 * list[4]) / a;
                adv.Add(x);
            }
        }

        private void CalculateEA(out matrix EA,List<int> equalIndex)
        {
            EA = new matrix();
            foreach(var list in _arm)
            {
                List<double> tmp = new List<double>();
                foreach(var i in equalIndex)
                {
                    tmp.Add(list[i]);
                }
                EA.Add(tmp);
            }
        }

        private void CalculateSOD(out List<double> sod,matrix ea,List<double> adv,List<double> g,
            List<double> scores)
        {
            double sum=scores.Sum();
            sod = new List<double>();
            int k = ea[0].Count;
            for(int j=0;j<k;j++)
            {
                double x = 0;
                for (int p = 0; p < k; p++)
                {
                    for(int i=0;i<ea.Count;i++)
                    {
                        x += (ea[i][j] - ea[i][p])*g[i] * (0.5 + adv[i])+scores[j];  
                    }
                }
                sod.Add(x);
            }
        }

        /// <summary>
        /// 查找sod中的排序序号，从小到大
        /// </summary>
        /// <param name="result"></param>
        /// <param name="sod"></param>
        private void SortSOD(out List<int> result,List<double> sod)
        {
            result = new List<int>();
            List<Tuple<int, double>> tuples = new List<Tuple<int, double>>();
            for(int i=0;i<sod.Count;i++)
            {
                tuples.Add(new Tuple<int, double>(i, sod[i]));
            }
            tuples.Sort((x, y) =>
            {
                if (x.Item2 < y.Item2)
                    return 1;
                else if (x.Item2 == y.Item2)
                    return 0;
                else
                    return -1;
            }
            );

            foreach(var e in tuples)
            {
                result.Add(e.Item1 + 1);
            }
        }

        private void CalculateDifficultyMatrix(out matrix D, matrix A)
        {
            D = new matrix(); 
            foreach (var list in A)
            { 
                List<double> nbList = new List<double>() { 0, 0, 0, 0, 0 };
                foreach(var e in list)
                {
                    if (e > 0.8 && e <= 1)
                        nbList[0] += 0.1;
                    else if (e > 0.6 && e <= 0.8)
                        nbList[1] += 0.1;
                    else if (e > 0.4 && e <= 0.6)
                        nbList[2] += 0.1;
                    else if (e > 0.2 && e <= 0.4)
                        nbList[3] += 0.1;
                    else
                        nbList[4] += 0.1;
                }

                D.Add(nbList);
            }
        }

        public void CalculateAveragedDifficulty(out List<double> avrD,matrix D)
        {
            avrD = new List<double>();
            foreach(var list in D)
            {
                double sum = list[2] + list[3] + list[4] - list[0] - list[1];
                avrD.Add(sum);
            }
        }

    }
}
