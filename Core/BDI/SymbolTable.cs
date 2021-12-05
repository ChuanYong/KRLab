using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    public class Symbol
    {
        private string _name;
        private int _id;

        protected void _init(string name, int id = -1)
        {
            _name = name;
            _id = id;
        }

        public Symbol(string name,int id=-1)
        {
            _init(name, id);
        }
        public Symbol(Symbol s)
        {
            _init(s._name, s._id);
        }
        public string name
        {
            get { return _name; }
        }

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual void print()
        {
            Console.WriteLine(_id + ":" + _name);
        }
    } 

    public class SymbolTable
    {
        private Symbol[] _symbtab;
        private SymbolList[] _hashtab;

        private const int _NullId=-1;

        protected int _symbtab_size;
        protected int _hashtab_size;
        protected int _nextid;
        protected int _symtab_inc_size;

        protected int hash(string name)
        {
            int key = 0;
            char[] char_name = name.ToCharArray();  
            int size = char_name.Length; 
            int idx = 0;
            while(idx<size && char_name[idx]!='\0')
            {
                key += char_name[idx];
                idx++;
            }

            int m=key % _hashtab_size;

            return m;
        }

        public SymbolTable(int symbtab_size=16,int symbtab_inc_sz=16,int hashtab_sz=7)
        {
            _symbtab_size = symbtab_size;
            _hashtab_size = hashtab_sz;
            _symtab_inc_size = symbtab_inc_sz;
            _nextid = 0;

            _symbtab = new Symbol[_symbtab_size];
            _hashtab = new SymbolList[_hashtab_size]; 
            for(int i=0;i<_hashtab.Count();i++)
            {
                _hashtab[i] = new SymbolList(); 
            }
        }
        public Symbol lookup(int id)
        {
            return (id < 0 || id > _symbtab_size) ? null : _symbtab[id];
        }
        public Symbol lookup(string name)
        {
            SymbolList sym_list = get_bucket(name);
            if (sym_list == null) return null;

            foreach(Symbol symb in sym_list)
            {
                if (symb.name == name)
                    return symb;
            }

            return null;

        }
        public Symbol add(Symbol symb)
        {
            if(_nextid>=_symbtab_size)
            {
                Symbol[] new_symbtab = new Symbol[_symbtab_size + _symtab_inc_size];
                for (int i = 0; i < _symbtab_size; i++)
                    new_symbtab[i] = _symbtab[i];
                _symbtab = new_symbtab;
                _symbtab_size += _symtab_inc_size;
            }
            symb.id = _nextid;
            _symbtab[_nextid++] = symb;

            _hashtab[hash(symb.name)].AddFirst(symb);

            return symb;
        }

        public SymbolList get_bucket(string name)
        {
            int idx = hash(name);
            return _hashtab[idx];
        }
        public SymbolList get_bucket(Symbol symb)
        {
            return get_bucket(symb.name);
        }
        public int get_id(string name)
        {
            Symbol s = lookup(name);
            return s != null ? s.id : _NullId;
        }
        public int size()
        {
            return _nextid;
        }

        public virtual void print()
        {
            for(int i=0;i<_nextid;i++)
            {
                lookup(i).print();
                Console.Write('\n');
            }

        }

    }


    public class SymbolTable1
    {
        protected Hashtable _id_name_table;
        protected Hashtable _name_id_table;
        protected const long _NullId = -1;  

        public SymbolTable1(int size=7)
        {
            _id_name_table = new Hashtable(size);
            _name_id_table = new Hashtable(size);
        }

        public Symbol lookup(int id)
        {
            Symbol symb = new Symbol((string)_id_name_table[id], id);
            return symb;

        }
        public Symbol lookup(string name)
        {
            Symbol symb = new Symbol(name,(int)_name_id_table[name]);
            return symb;
        }
        public Symbol add(Symbol symb)
        {
            _id_name_table.Add(symb.id, symb.name);
            _name_id_table.Add(symb.name, symb.id);

            return symb;
        }
        public void remove(Symbol symb)
        {
            _id_name_table.Remove(symb.id);
            _name_id_table.Remove(symb.name);

        } 

    }
}
