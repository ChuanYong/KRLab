using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    public class utils
    {
        public static void print(string s)
        {
            Console.WriteLine(s);
        }

        public static int highest_stack_priority(IntentionStackList _top_level_goals,
            double priority)
        {
            utils.print("highest_stack_priority:TODO");
            System.Environment.Exit(0);
            return 1;
        }

        public static ExpList explist_eval_new(ExpList explist, Binding binding)
        {
            if (explist == null) return null;
            ExpList new_explist = new ExpList();
            foreach (Expression e in explist)
            {
                new_explist.AddLast(new Value(e.eval(binding)));
            }
            return new_explist;
        }

        public static void explist_print(ExpList explist, Binding b)
        {
            if (explist == null) return;
            foreach (Expression e in explist)
            {
                e.eval(b).print();
                Console.Write(" ");
            }

        }

        public static bool unify(Relation dst_rel, Binding dst_b,Relation src_rel, Binding src_b)
        { 
            //if the source and hte destination relations do not match,return false;
            //otherwise change the destination binding with linked variables
            //to the source relation binding, return true;
            if (src_rel.id != dst_rel.id)
                return false;
            if (src_rel.arity <= 0 && dst_rel.arity <= 0)
                return true;

            ExpList src_args = src_rel.args;
            ExpList dst_args = dst_rel.args;
            Binding dst_binding_copy = new Binding(dst_b);
            LinkedListNode<Expression> src_arg = src_args.First;
            LinkedListNode<Expression> dst_arg = dst_args.First;
            LinkedList<Expression>.Enumerator itor_src = src_args.GetEnumerator();
            LinkedList<Expression>.Enumerator itor_dst = dst_args.GetEnumerator();
            while (itor_src.MoveNext() && itor_dst.MoveNext())
            {
                Value src_val = itor_src.Current.eval(src_b);
                Value dst_val = itor_dst.Current.eval(dst_b);

                if (src_val.is_defined() && dst_val.is_defined())
                {
                    if (src_val.is_equal(dst_val))
                        continue;
                    else
                        return false;
                }
                if (itor_dst.Current.is_variable())
                {
                    if (itor_src.Current.is_variable())
                        dst_binding_copy.link_variables(itor_dst.Current, itor_src.Current, src_b);
                    else
                        dst_binding_copy.set_value(itor_dst.Current, src_val);
                }
                else
                {
                    if (!src_val.is_equal(dst_val))
                        return false;
                }
            }
            if (dst_b != null)
                dst_b = dst_binding_copy;
            return true;

            //for(;src_arg!=null && dst_arg!=null;src_arg=src_arg.Next,dst_arg=dst_arg.Next)
            //{
            //    Value src_val = src_arg.Value.eval(src_b);
            //    Value dst_val = dst_arg.Value.eval(dst_b);

            //    if (src_val.is_defined() && dst_val.is_defined())
            //    {
            //        if (src_val.is_equal(dst_val))
            //        { 
            //            continue;
            //        }
            //        else
            //            return false;
            //    }

            //    if (dst_arg.Value.is_variable())
            //    {
            //        if (src_arg.Value.is_variable())
            //            dst_binding_copy.link_variables(dst_arg.Value, src_arg.Value, src_b);
            //        else
            //            dst_binding_copy.set_value(dst_arg.Value, src_val);
            //    }
            //    else
            //    {
            //        if (!src_val.is_equal(dst_val))
            //            return false;
            //    }

            //}
            //if (dst_b != null)
            //{
            //    dst_b = dst_binding_copy;
            //}
            //return true;
        }
 

    }
}
