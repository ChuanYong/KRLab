using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Rand
    {
        public static int Random(int max)
        {
            return new System.Random().Next(max);
        }

        public static int Random(int min,int max)
        {
            return new System.Random().Next(min, max);
        }

        /// <summary>
        /// 获取min到max直接的随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double Random(double min, double max)
        {
           return new System.Random().NextDouble()*(max - min) + min; 
        }
        /// <summary>
        ///产生一个与i不相同的随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int Random(int min,int max,int i)
        {
            int k;
            do
            {
                k = new System.Random().Next(min, max);
                if (k != i)
                    return k;

            } while (true);
        }

        /// <summary>
        /// 从一个列表中取出n对整数对
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static List<Tuple<int, int>> RandomPairs(List<int> numbers, int n)
        {
            int sum = 0;     //组合数个数
            List<int> tempList = new List<int>();//准备一个数组tempList存储索引号temp
            List<Tuple<int, int>> fullList = new List<Tuple<int, int>>();//存储所有的组合情况
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                for (int j = i + 1; j < numbers.Count; j++)
                {
                    fullList.Add(new Tuple<int, int>(numbers[i], numbers[j]));
                    sum++;
                }
            }
            if (n >= sum) return fullList;         //若n大于等于组合数返回全部组合
            Random random = new Random();
            for (; tempList.Count < n;)
            {
                int temp = random.Next(fullList.Count);//将产生的随机数作为被抽list的索引
                if (!tempList.Contains(temp))
                {
                    tempList.Add(temp);
                    list.Add(fullList[temp]);
                }
            }
            return list;
        }

        /// <summary>
        /// 从一个列表中取出一个与k不同的整数
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static int Random(List<int> numbers,int k)
        {
            if (numbers.Count == 0)
                return -1;

            List<int> tmp = numbers;
            if(tmp.Contains(k))
            {
                tmp.Remove(k);
            }

            if (tmp.Count == 0)
                return -1;
            int idx = new Random().Next(0, tmp.Count);

            return tmp[idx];

            //Random rd = new Random();
            //int index = 0;
            //int m = 0;
            //int w = 0;
            //int n = numbers.Count;
            //for (int i = 0; i < n; i++)
            //{
            //    index = rd.Next(0, n);
            //    if (k != numbers[index])
            //    {
            //        m = numbers[index];
            //        break;
            //    }
            //    numbers[index] = numbers[n - 1];
            //    n = n - 1;
            //}
            //if (m == 0) m = -1;
            //return m;
        }

        /// <summary>
        /// 从numbers中随机取出一个数，该数必须与diffs中的数不同。
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="diffs"></param>
        /// <returns></returns>
        public static int Random(List<int> numbers,List<int> diffs)
        {
            Random rd = new Random();
            int index = 0;
            int a = 0;
            int n = numbers.Count;
            for (int i = 0; i < n; i++)
            {
                index = rd.Next(0, n);
                foreach (int element in numbers)
                {
                    if (!diffs.Contains(numbers[index]))
                    {
                        a = numbers[index];
                        break;
                    }
                }
                numbers[index] = numbers[n - 1];
                n = n - 1;
            }
            if (a == 0) a = -1;
            return a;
        }

        /// <summary>
        /// 从numbers中随机取一个数
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static int Random(List<int>numbers)
        {
            if (numbers.Count == 0)
                return -1;

            Random rd = new Random();
            int n = rd.Next(0, numbers.Count);
            return numbers[n];
        }



    }
}
