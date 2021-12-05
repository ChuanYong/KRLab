using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    public class SymbolList:LinkedList<Symbol>
    {
        public static void print(SymbolList list)
        {
            if (list == null)
            {
                Console.WriteLine("this SymbolList is null!");
                return;
            }
            foreach(Symbol s in list)
            {
                Console.Write(s.name + '\t');
            }
            Console.Write('\n');
        }

    }
    public class ExpList:LinkedList<Expression>
    {
    }
    public class BindingList:LinkedList<Binding>
    {
        public BindingList(Binding b)
        {
            AddFirst(b);
        }
    }

    public class KaBodyElementList:LinkedList<KaBodyElement>
    {

    } 
    public class IntentionStackList:LinkedList<IntentionStack>
    {

    }

    public class ConditionList:LinkedList<Condition>
    {

    }

    public class ActionList:LinkedList<Action>
    {

    }
 
}
