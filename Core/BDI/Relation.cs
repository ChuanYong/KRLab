using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    /// <summary>
    /// belief is a database of facts which are represented
    /// as relations. A relation has a name and a variable number of 
    /// fields.
    /// tree 20 "maple" "red"
    /// </summary>
    public class Relation
    {
        private static SymbolTable _relation_table=new SymbolTable(512,128,253);//symbol name and id;
        private int _id=-1;//the number
        private ExpList _args;
        private int _arity;//the length of _args

        private void _init(string s,ExpList el=null)
        {
            _id = _relation_table.get_id(s);
            if(_id<0)//if the string s is not in the table
            {
                _id = _relation_table.add(new Symbol(s)).id;
            }

            _args = el != null ? el : new ExpList();
            _arity = _args.Count;
        }

        public Relation(string s,ExpList el=null)
        {
            _init(s, el);
        }
        public Relation(string s,params Expression[] args)
        {
            ExpList el = new ExpList();
            foreach(Expression e in args)
            {
                el.AddLast(e);
            }
            _init(s, el);
        }

        public Relation(Relation r,Binding binding=null)
        {
            _id = r._id;
            _arity = r._arity;
            _args = utils.explist_eval_new(r._args, binding);
        }
        public int id
        {
            get { return _id; }
        }
        public string name
        {
            get { return _relation_table.lookup(id).name; }
        }
        public ExpList args
        { get { return _args; } }
        public int arity
        { get { return _arity; } }
        public Relation eval_args(Binding b)
        {
            Expression.explist_eval(_args, b);
            return this;           
        }  
        public void print(Binding b=null)
        {
            Console.Write(name+' ');
            utils.explist_print(args, b);
            Console.WriteLine("");
        }

    }
}
