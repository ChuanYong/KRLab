using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLabConsole
{
    using matrix = List<List<double>>;
    class Utilities
    {
        public static void Print(string name, matrix m)
        {
            Console.WriteLine("///////////////" + name + "/////////////////");
            foreach (var list in m)
            {
                foreach (var e in list)
                {
                    Console.Write(e + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("/////////////////////////////////////");
            Console.WriteLine();
        }
        public static void Print(string name, List<int> m)
        {
            Console.WriteLine("///////////////" + name + "/////////////////");
            foreach (var e in m)
            {
                Console.Write(e + " ");
            }
            Console.WriteLine();

            Console.WriteLine("/////////////////////////////////////");
            Console.WriteLine();
        }
        public static void Print(string name, List<double> v)
        {
            Console.WriteLine("////////////" + name + "///////////////");
            foreach (var e in v)
                Console.Write(e + " ");
            Console.WriteLine();
            Console.WriteLine("//////////////////////////////");
            Console.WriteLine();
        }

        /// <summary>
        /// 查找input中的排序序号，从大到小
        /// </summary>
        /// <param name="result"></param>
        /// <param name="sod"></param>
        public static void Sort(out List<int> result, List<double> input)
        {
            result = new List<int>();
            List<Tuple<int, double>> tuples = new List<Tuple<int, double>>();
            for (int i = 0; i < input.Count; i++)
            {
                tuples.Add(new Tuple<int, double>(i, input[i]));
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

            foreach (var e in tuples)
            {
                result.Add(e.Item1);
            }
        }
    }
}
