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
    /// 2020年3月22日的论文
    /// </summary>
    class Paper20200322
    {
        private LinguisticVariable _fuzzyVar;
        private LinguisticVariable _levelFuzzyVar;
        private List<IMembershipFunction> _mfs;
        private List<IMembershipFunction> _levelMFs;

        private IFuzzyEngine _fuzzyEngine;

        //data for accuracy rate matrix
        private matrix _arm;
        //data for difficulty rate matrix;
        private matrix _drm;
        //data for grades of question set
        private List<double> _g;
        //data for importance matrix
        private matrix _im;
        //data for complexity matrix
        private matrix _cm;
        //data for difficulty matrix
        private matrix _dm;

        private List<double> _iv;
        private List<double> _cv;
        private List<double> _dv;

        double x1 = 0.4;
        double x2 = 0.3;
        double x3 = 0.3;
       

        private string[] _fuzzySet = new[] {"low","medium","high"};
        private string[] _levelFuzzySet = new[] {"Very Low","Low","More or Less Medium",
            "Medium","More or Less high","High","Very High" };
        public Paper20200322()
        {
            InitARM();
            Utilities.Print("A", _arm);
            //(2)第二步，计算每个题目的平均准确率
            List<double> avrA;
            CalculateAverageAccuracy(out avrA);
            List<double> avrD;
            CalculateAverageDifficulty(out avrD);
            Utilities.Print("avrA", avrA);
            Utilities.Print("avrD", avrD);

            _g = new List<double>() { 10, 15, 20, 25, 30 };
            Utilities.Print("G", _g);
            _iv = new List<double>() { 0.2, 0.15, 0.5, 0.7, 0.25 };
            _cv = new List<double>() { 0.05, 0.15, 0.25, 0.7, 0.5 };
            _dv = avrD;

            InitFuzzyEngine();            

        }

        public void Run()
        {
            //(1)
            FuzzifyICDMatrix();
            Utilities.Print("IM", _im);
            Utilities.Print("CM", _cm);
            Utilities.Print("DM", _dm);
            //(2)，计算每个学生的总分
            List<double> scores;
            CalculateStudentTotalScore(out scores,_arm);
            Utilities.Print("Score", scores);
            List<List<int>> equScores;
            FindEqualScoreInfo(out equScores, scores);

            //(3)计算每个问题的level
            matrix qLevels = new matrix();
            for (int k = 0; k < 5; k++)
            {
                List<double> tmp = new List<double>()
                    {
                        Vj0(k,_im,_cm,_dm),
                        Vj1(k,_im,_cm,_dm),
                        Vj2(k,_im,_cm,_dm),
                        Vj3(k,_im,_cm,_dm),
                        Vj4(k,_im,_cm,_dm),
                        Vj5(k,_im,_cm,_dm),
                        Vj6(k,_im,_cm,_dm),

                    };

                qLevels.Add(tmp);
            }

            Utilities.Print("qLevels", qLevels);
            List<double> AL;
            CalculateAverageLevels(out AL, qLevels);
            Utilities.Print("AL", AL);

            matrix adjustedARM;
            AdjustAR(out adjustedARM, AL);
            Utilities.Print("adARM", adjustedARM); 

            List<int> sort;
            Utilities.Sort(out sort, scores);
            List<double> newScores;
            CalculateStudentTotalScore(out newScores, adjustedARM);
            Utilities.Print("new scores", newScores);
            List<int> newSort;
            Utilities.Sort(out newSort, newScores);

            foreach (var e in sort)
                Console.Write(e + " ");
            Console.WriteLine();
            foreach (var e in newSort)
                Console.Write(e + " ");
            Console.WriteLine();
             
        } 

        private void CalculateAverageLevels(out List<double>AL, matrix v)
        {
            AL = new List<double>()
            {
                (_g[0]/100)*(-v[0][0]-v[0][1]-v[0][2]+v[0][3]+v[0][4]+v[0][5]+v[0][6])/7,
                (_g[1]/100)*(-v[1][0]-v[1][1]-v[1][2]+v[1][3]+v[1][4]+v[1][5]+v[1][6])/7,
                (_g[2]/100)*(-v[2][0]-v[2][1]-v[2][2]+v[2][3]+v[2][4]+v[2][5]+v[2][6])/7,
                (_g[3]/100)*(-v[3][0]-v[3][1]-v[3][2]+v[3][3]+v[3][4]+v[3][5]+v[3][6])/7,
                (_g[4]/100)*(-v[4][0]-v[4][1]-v[4][2]+v[4][3]+v[4][4]+v[4][5]+v[4][6])/7
            };
        }

        private void AdjustAR(out matrix ad,List<double> al)
        { 
            //对每个学生进行计算
            List<double> l0 = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<double> l1 = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<double> l2 = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<double> l3 = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<double> l4 = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            ad = new matrix() { l0, l1, l2, l3, l4 }; 
            for (int i = 0; i < 10; i++)
            {
                //第i个学生的accuracy rate
                //ad[0][i] =  _arm[0][i] * (1+qLevls[0][6] + qLevls[0][5] + qLevls[0][4] + qLevls[0][3] - qLevls[0][2] - qLevls[0][1] - qLevls[0][0]);
                //ad[1][i] =  _arm[1][i] * (1+qLevls[1][6] + qLevls[1][5] + qLevls[1][4] + qLevls[1][3] - qLevls[1][2] - qLevls[1][1] - qLevls[1][0]);
                //ad[2][i] =  _arm[2][i] * (1+qLevls[2][6] + qLevls[2][5] + qLevls[2][4] + qLevls[2][3] - qLevls[2][2] - qLevls[2][1] - qLevls[2][0]);
                //ad[3][i] =  _arm[3][i] * (1+qLevls[3][6] + qLevls[3][5] + qLevls[3][4] + qLevls[3][3] - qLevls[3][2] - qLevls[3][1] - qLevls[3][0]);
                //ad[4][i] =  _arm[4][i] * (1+qLevls[4][6] + qLevls[4][5] + qLevls[4][4] + qLevls[4][3] - qLevls[4][2] - qLevls[4][1] - qLevls[4][0]);

                ad[0][i] = _arm[0][i] * (1 + al[0]);
                ad[1][i] = _arm[1][i] * (1 + al[1]);
                ad[2][i] = _arm[2][i] * (1 + al[2]);
                ad[3][i] = _arm[3][i] * (1 + al[3]);
                ad[4][i] = _arm[4][i] * (1 + al[4]);

                //ad[0][i] = _arm[0][i] ;
                //ad[1][i] = _arm[1][i] ;
                //ad[2][i] = _arm[2][i] ;
                //ad[3][i] = _arm[3][i] * (1 + 0.35);
                //ad[4][i] = _arm[4][i] ;
    

            }
        }

        private double Vj0(int i,matrix im,matrix cm,matrix dm)
        {
            return x1 * im[i][0] + x2 * cm[i][0] + x3 * dm[i][0];
        }
        private double Vj1(int i,matrix im,matrix cm,matrix dm)
        {
            List<double> tmp = new List<double>()
            {
                x1*im[i][1]+x2*cm[i][0]+x3*dm[i][0],
                x1*im[i][0]+x2*cm[i][1]+x3*dm[i][0],
                x1*im[i][0]+x2*cm[i][0]+x3*dm[i][1]
            };

            return tmp.Max();
        }
        private double Vj2(int i,matrix im,matrix cm,matrix dm)
        {
            List<double> tmp = new List<double>()
            {
                x1*im[i][1]+x2*cm[i][1]+x3*dm[i][0],
                x1*im[i][1]+x2*cm[i][0]+x3*dm[i][1],
                x1*im[i][0]+x2*cm[i][1]+x3*dm[i][1],
                x1*im[i][0]+x2*cm[i][0]+x3*dm[i][2],
                x1*im[i][0]+x2*cm[i][2]+x3*dm[i][0],
                x1*im[i][2]+x2*cm[i][0]+x3*dm[i][0],
            };
            return tmp.Max();
        }
        private double Vj3(int i, matrix im, matrix cm, matrix dm)
        {
            List<double> tmp = new List<double>()
            {
                x1*im[i][1]+x2*cm[i][1]+x3*dm[i][1],
                x1*im[i][0]+x2*cm[i][1]+x3*dm[i][2],
                x1*im[i][1]+x2*cm[i][2]+x3*dm[i][0],
                x1*im[i][2]+x2*cm[i][0]+x3*dm[i][1],
                x1*im[i][1]+x2*cm[i][0]+x3*dm[i][2],
                x1*im[i][0]+x2*cm[i][2]+x3*dm[i][1],
                x1*im[i][2]+x2*cm[1][1]+x3*dm[1][0]
            };
            return tmp.Max();
        }
        private double Vj4(int i, matrix im, matrix cm, matrix dm)
        {
            List<double> tmp = new List<double>()
            {
                x1*im[i][1]+x2*cm[i][1]+x3*dm[i][2],
                x1*im[i][1]+x2*cm[i][2]+x3*dm[i][1],
                x1*im[i][2]+x2*cm[i][1]+x3*dm[i][1],
                x1*im[i][2]+x2*cm[i][2]+x3*dm[i][0],
                x1*im[i][2]+x2*cm[i][0]+x3*dm[i][2],
                x1*im[i][0]+x2*cm[i][2]+x3*dm[i][2] 
            };
            return tmp.Max();
        }
        private double Vj5(int i, matrix im, matrix cm, matrix dm)
        {
            List<double> tmp = new List<double>()
            {
                x1*im[i][2]+x2*cm[i][2]+x3*dm[i][1],
                x1*im[i][2]+x2*cm[i][1]+x3*dm[i][2],
                x1*im[i][1]+x2*cm[i][2]+x3*dm[i][2],
            };
            return tmp.Max();
        }
        private double Vj6(int i, matrix im, matrix cm, matrix dm)
        {
            List<double> tmp = new List<double>()
            {
                x1*im[i][2]+x2*cm[i][2]+x3*dm[i][2]  
            };
            return tmp.Max();
        }

        private void InitFuzzyEngine()
        {
            _fuzzyEngine = new FuzzyEngineFactory().Default();

            //为学习成绩(learning performance)创建模糊集的成员关系函数(MF)
            _levelFuzzyVar = new LinguisticVariable("Level");
            _levelMFs = new List<IMembershipFunction>()
            {
                _levelFuzzyVar.MembershipFunctions.AddTriangle(_levelFuzzySet[0],0,0,0.15),
                _levelFuzzyVar.MembershipFunctions.AddTriangle(_levelFuzzySet[1],0,0.15,0.3),
                _levelFuzzyVar.MembershipFunctions.AddTriangle(_levelFuzzySet[2],0.175,0.325,0.475),
                _levelFuzzyVar.MembershipFunctions.AddTriangle(_levelFuzzySet[3],0.35,0.5,0.65),
                _levelFuzzyVar.MembershipFunctions.AddTriangle(_levelFuzzySet[4],0.525,0.675,0.825),
                _levelFuzzyVar.MembershipFunctions.AddTriangle(_levelFuzzySet[5],0.7,0.85,1),
                _levelFuzzyVar.MembershipFunctions.AddTriangle(_levelFuzzySet[6],0.85,1,1)
            }; 

            _fuzzyVar = new LinguisticVariable("FuzzyVariable");
            _mfs = new List<IMembershipFunction>()
            {
                _fuzzyVar.MembershipFunctions.AddTrapezoid(_fuzzySet[2], 0, 0, 0.2, 0.4),
                _fuzzyVar.MembershipFunctions.AddTriangle(_fuzzySet[1], 0.2, 0.5, 0.8),
                _fuzzyVar.MembershipFunctions.AddTrapezoid(_fuzzySet[0], 0.6, 0.8, 1,1)
            };

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
            for (int j = 0; j < _arm.Count; j++)
            {
                tmp.Add(_arm[j][i]);
            }

            if (tmp.Count != _g.Count)
                throw new Exception("Error for the score calculation");

            for (int j = 0; j < _g.Count; j++)
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
            for (int i = 0; i < nb; i++)
            {
                av.Add(_arm[i].Average());
            }
        }
        private void CalculateAverageDifficulty(out List<double> av)
        {
            int nb = _drm.Count;
            av = new List<double>();
            for (int i = 0; i < nb; i++)
            {
                av.Add(_drm[i].Average());
            }
        }
        public void CalculateStudentTotalScore(out List<double> data,matrix arm)
        {
            data = new List<double>();
            int nb = arm[0].Count;
            for (int i = 0; i < nb; i++)
            {
                double sum = 0.0;
                for (int j = 0; j < arm.Count; j++)
                {
                    sum += arm[j][i] * _g[j];
                }
                data.Add(sum);
            }

        }

        /// <summary>
        /// 查到scores中分数相同的元素，序号和值。
        /// </summary>
        /// <param name="scores"></param>
        public void FindEqualScoreInfo(out List<List<int>> index, List<double> scores)
        {
            List<double> tmp = new List<double>();
            for (int i = 0; i < scores.Count; i++)
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
            foreach (var e in tmp)
            {
                List<int> list = new List<int>();
                for (int i = 0; i < scores.Count; i++)
                {
                    if (e == scores[i])
                        list.Add(i);
                }
                index.Add(list);
            }

        }



        private void InitARM()
        {
            var row0 = new List<double>() { 0.59, 0.35, 1, 0.66, 0.11, 0.08, 0.84, 0.23, 0.4, 0.24 };
            var row1 = new List<double>() { 0.01, 0.27, 0.14, 0.04, 0.88, 0.16, 0.04, 0.22, 0.81, 0.53 };
            var row2 = new List<double>() { 0.77, 0.69, 0.97, 0.71, 0.17, 0.86, 0.87, 0.42, 0.91, 0.74 };
            var row3 = new List<double>() { 0.73, 0.72, 0.18, 0.16, 0.5, 0.02, 0.32, 0.92, 0.9, 0.25 };
            var row4 = new List<double>() { 0.93, 0.49, 0.08, 0.81, 0.65, 0.93, 0.39, 0.51, 0.97, 0.61 };

            _arm = new matrix() { row0, row1, row2, row3, row4 };
            _arm[4][0] = 0.93;
            _drm = new matrix();

            foreach(var list in _arm)
            {
                List<double> tmp = new List<double>();
                foreach(var x in list)
                {
                    tmp.Add(1 - x);
                }
                _drm.Add(tmp);
            }
        }
    
        private void FuzzifyICDMatrix()
        {
            //var row0 = new List<double>() { 0, 0,1 };
            //var row1 = new List<double>() { 0, 0.33, 0.67 };
            //var row2 = new List<double>() { 0.15, 0.85,0 };
            //var row3 = new List<double>() { 1, 0, 0};
            //var row4 = new List<double>() { 0.07, 0, 0.93 };

            //_im = new matrix() { row0, row1, row2, row3, row4 };

            //var row0 = new List<double>() { 0, 0.85, 0.15 };
            //var row1 = new List<double>() { 0.33, 0.67, 0 };
            //var row2 = new List<double>() { 0, 0.69, 0.31 };
            //var row3 = new List<double>() { 0.56, 0, 0.44 };
            //var row4 = new List<double>() { 0.7, 0.3, 0 };
            //_cm = new matrix() { row0, row1, row2, row3, row4 };

            _im = new matrix(); 
            foreach(var x in _iv)
            {
                List<double> tmp0 = new List<double>();
                foreach (var e in _mfs)
                {
                    tmp0.Add(e.Fuzzify(x));
                }
                _im.Add(tmp0);

            }
            _cm = new matrix();
            foreach(var x in _cv)
            {
                List<double> tmp0 = new List<double>();
                foreach (var e in _mfs)
                {
                    tmp0.Add(e.Fuzzify(x));
                }
                _cm.Add(tmp0);

            }
            _dm = new matrix();
            foreach(var x in _dv)
            {
                List<double> tmp0 = new List<double>();
                foreach (var e in _mfs)
                {
                    tmp0.Add(e.Fuzzify(x));
                }
                _dm.Add(tmp0);
            }

        }
       

        /// <summary>
        /// calculate the fuzzy grade matrix for difficulty.
        /// </summary>
        /// <param name="fas"></param>
        private void CalculateDifficultyMatrix(out matrix fa,List<double> accus)
        {
            fa = new matrix(); 
            int nb = accus.Count;//the number of questions
            for (int i = 0; i < nb; i++)
            {
                double aa = accus[i]; 
                List<double> tmp0 = new List<double>(); 
                foreach (var e in _mfs)
                {
                    tmp0.Add(e.Fuzzify(aa)); 
                }
                fa.Add(tmp0); 
            }
        }

         
    }
}
