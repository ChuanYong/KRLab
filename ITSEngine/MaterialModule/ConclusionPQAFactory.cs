using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITS.DomainModule;
using ITSText;
using KRLab.Core.SNet;
using Utilities;

namespace ITS.MaterialModule
{
    public class ConclusionPQAFactory:PQAFactory
    {
        public ConclusionKRModule KRModule
        {
            get { return (ConclusionKRModule)_krModule; }
        }
        public ConclusionPQAFactory(string course) :
            base(new ConclusionKRModule(course))
        { 
        }
        static void getRandSelection(ref List<string> options, ref Dictionary<char, string> dic)
        {
            char[] arr2 = { 'A', 'B', 'C', 'D' };
            for (int k = 0; k < options.Count; ++k)
            {
                dic[arr2[k]] = options[k];
            }
        }
        



        //static void getRandSelection(ref List<string> allOptions, ref Dictionary<char, string> dic)
        //{
        //    List<int> num = new List<int> { 0, 1, 2, 3 };//列表作为所有选项的索引值随机数
        //    char[] arr2 = { 'A', 'B', 'C', 'D' };
        //    //先把num的顺序打乱
        //    int randIdx = 0;
        //    Random rand = new Random();
        //    int numLength = num.Count;
        //    for (int i = 0; i < numLength; ++i)
        //    {
        //        randIdx = rand.Next(0, numLength);
        //        int temp = num[i];
        //        num[i] = num[randIdx];
        //        num[randIdx] = temp;
        //    }
        //    //然后对dic逐个赋值
        //    for (int i = 0; i < numLength; ++i)
        //    {
        //        dic[arr2[i]] = allOptions[num[i]];
        //    }
        //}
        public override PQA CreateSpecificPQA(string topic)
        {
            KRModuleSNet net = KRModule.GetKRModuleSNet(topic);
            if (net == null)
                return null;

            ConclusionTopicModule topicModule = new ConclusionTopicModule(KRModule.Course,net); 
            PQA pqa= new PQA(topic, new Problem());

            

            ///(2)测试结论的内容，选择题////////////////////////////////////////
            //List<string> interfers = topicModule.Interferences;//返回干扰项
            //string rightOption = topicModule.Options;//返回正确选项
            string rightOption = string.Empty;
            List<string> contKeyList = topicModule.ContKeyList;//返回定义节点的关联节点
            ConceptKRModule conceptKR = new ConceptKRModule(KRModule.Course);
            Dictionary<char, string> dic = new Dictionary<char, string>();
            char im = new char();
            List<string> nameString = conceptKR.NameString;
            Dictionary<int[], double> dic2 = conceptKR.Dic2;//拿过来相似性字典
            int flag = 0;
            for (int i = 0; i < contKeyList.Count; i++)//循环一个语义网中的所有contKey
            {
                flag++;
                if (nameString.Contains(contKeyList[i]))
                {
                    rightOption = contKeyList[i];
                    
                    
                    //int index=nameString.IndexOf(rightOption);//获取正确答案在列表中的位置
                    for (int j = 0; j < nameString.Count - 3; j++)
                    {
                        List<string> diagram = new List<string>() { nameString[j], nameString[j + 1], nameString[j + 2], nameString[j + 3] };
                        List<string> options = new List<string>();
                        if (diagram.Contains(rightOption))
                        {
                            options = diagram;
                            int a=nameString.IndexOf(rightOption);//找到正确答案的索引
                            int u = nameString.IndexOf(nameString[j]);
                            int v = nameString.IndexOf(nameString[j + 1]);
                            int w = nameString.IndexOf(nameString[j + 2]);
                            int x = nameString.IndexOf(nameString[j + 3]);
                            List<int> index = new List<int> { u, v, w, x };
                            List<int> distr = new List<int>();
                            List<int[]> riADistr = new List<int[]>();//定义一个数组集合存放正确答案和干扰项的索引对
                            //double f = 0;
                            double eves=0;
                            foreach(var ind in index)
                            {
                                if (ind !=a )
                                {
                                    distr.Add(ind);//存入三个干扰项的索引
                                }
                            }
                            for(int e = 0; e< distr.Count; e++)
                            {
                                
                                if (a < distr[i])//因为之前字典中的数组索引是有顺序的，小号在前,一共三对
                                {
                                    int[] vs = { a, distr[i] };
                                    riADistr.Add(vs);
                                }
                                else
                                {
                                    int[] vs = { distr[i], a };
                                    riADistr.Add(vs);
                                }
                            }
                            for(int k=0;k<riADistr.Count;k++)
                            {
                                double eve = dic2[riADistr[k]];
                                eves += eve;//获得相似性之和
                                //dic2.Values
                                //double eve = dic2.Values(riADistr[i]);
                                Console.WriteLine(eves);
                                
                            }
                           
                        }
                        
                        
                        getRandSelection(ref options, ref dic);
                        
                        foreach (var di in dic)
                        {
                            if (di.Value == rightOption)
                            {
                                im = di.Key;
                            }
                        }

                    }
                    // List<string> distractions =//返回contKey在名字串列表里相连三个，作为干扰项
                    string contain = TextProcessor.ReplaceWithUnderLine(topicModule.Content, rightOption);
                    AddQAs(ref pqa, new[] { 0.5, 0.1, 0.1 }, "给出下面空白处的选项：\n" + contain + "。\n"
                        + "A." + dic['A'] + "。\n" + "B." + dic['B'] + "。\n" + "C." + dic['C'] + "。\n" + "D." + dic['D'] + "。\n", im.ToString());
                    
                }
                else
                {
                    if (flag == 1)
                    {
                        ///(1) 测试结论的内容，填空题////////////////////////////////////////
                        List<string> charas = topicModule.ContentCharacts;//返回定义节点的关联节点
                        string content = TextProcessor.ReplaceWithUnderLine(topicModule.Content, charas); //结论节点的内容，conc
                        AddQAs(ref pqa, new[] { 0.5, 0.1, 0.1 }, "请按顺序填写下面的空缺：\n" + content + "。",
                            charas.ToArray());
                    }
                    else break;
                    
                    
                }
            }
            return pqa;


        }
    }
}
//List<string> ops = new List<string>();//列表存储随机选项
//ops.Add(AllOptions[i]);
//ops.Add(AllOptions[j]);
//ops.Add(AllOptions[k]);
//ops.Add(AllOptions[l]);
//foreach(var op in ops)
//{
//    if (op == RightOption)
//    {

//    }
//}
//int i = new Random().Next(num[0],num[3]);
//num.Remove(i);                    
//dic.Add('A', AllOptions[i]);

//int j = new Random().Next(num[0], num[2]);
//dic.Add('B', AllOptions[j]);
//num.Remove(j);

//int k = new Random().Next(num[0], num[1]);
//dic.Add('C', AllOptions[k]);
//num.Remove(k);

//int l = num[0];
//dic.Add('D', AllOptions[l]);
//num.Remove(l);

//List<string> allOptions = new List<string>();//用来存储所有的选项
//allOptions = interfers;//赋值干扰项
//allOptions.Add(rightOption);//添加正确选项到列表最后
//Dictionary<char, string> dic = new Dictionary<char, string>();//存储ABCD对应的选项
//getRandSelection(ref allOptions, ref dic);

//foreach(KeyValuePair<char,string> kvp in dic)
//{
//    Console.WriteLine("{0}={1}", kvp.Key, kvp.Value);
//}
//List<int> num = new List<int>{ 0, 1, 2, 3 };//列表作为所有选项的索引值随机数
//char[] arr2 = { 'A', 'B', 'C', 'D' };
//for(int i=3; i>=0; i--)
//{
//    int a = new Random().Next(num[0], num[i]);//产生列表元素中一个随机数
//    dic.Add(arr2[3 - i], AllOptions[a]);
//    num.Remove(a);
//}
