using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    public class Binding
    {
        private int _size;
        //true if the binding is done with at least one new 
        //WM(world model)entry
        private bool _new_wm_binding;
        //an array of values (variable-id is the index)
        private BindingValue [] _bvalues;
        private SymbolTable _symtab;

        public Binding(SymbolTable st)
        {
            _symtab = st;
            _new_wm_binding = false;
            _bvalues = new BindingValue[st.size()];

        }
        public Binding(Binding b)
        {
            if(b!=null)
            {
                _symtab = b._symtab;
                _new_wm_binding = b._new_wm_binding;
                _bvalues = new BindingValue[b._size];
                for (int i = 0; i < _size; i++)
                    _bvalues[i] = b._bvalues[i];
                    
            }
            else
            {
                _symtab = null;
                _new_wm_binding = false;
                _size = 0;
                _bvalues = null;
            }

        }

        //public static Binding operator = (Binding b)
        //{

        //}
        public void unbind_variable(Expression exp)
        {
            if (exp.is_variable())
                set_value(exp, Value.Undefined);

        }
        public void unbind_variables(ExpList exps)
        {
            foreach (Expression e in exps)
                unbind_variable(e);
        }
        public void link_variables(Expression var,Expression ext_var,
            Binding ext_binding)
        {
            int idx = var.get_variable().get_id();
            BindingValue bval = _bvalues[idx];
            bval._external_variable = (Variable)ext_var.get_variable();
            bval._external_binding = ext_binding;
            _bvalues[idx] = bval;
        }

        public bool is_local_binding(Expression var)
        {
            return !var.is_variable() || is_local_binding(var.get_variable().get_id());
        }
        public bool is_local_binding(int var_id)
        {
            return _bvalues[var_id]._external_variable == null;
        }
        public Value get_value(Expression var)
        {
            return get_value(var.get_variable().get_id());
        }
        public Value get_value(int var_id)
        {
            BindingValue bval = _bvalues[var_id];
            if (is_local_binding(var_id))
                return bval._value;
            else
                return bval._external_binding.get_value(bval._external_variable);

        }
        public void set_value(Expression var,Value val)
        {
            if (!var.is_variable())
                return;
            set_value(var.get_variable().get_id(), val);
        }
        public void set_value(int var_id,Value val)
        {
            if (is_local_binding(var_id))
                _bvalues[var_id]._value = val;
            else
                _bvalues[var_id]._external_binding.set_value(_bvalues[var_id]._external_variable, val);

        }
        public bool is_new_wm_binding()
        {
            return _new_wm_binding;
        }
        public bool check_new_wm_binding(bool new_wm)
        {
            return _new_wm_binding = _new_wm_binding || new_wm;
        }
        public void clear_new_wm_binding()
        {

        }
        public SymbolTable get_symtab()
        {
            return _symtab;
        }
        public bool is_empty()
        {
            return _size == 0;
        }
        public void print()
        {
            for(int i=0;i<_size;i++)
            {
                Console.Write(i + ":" + _symtab.lookup(i).name+"  ");
                if (get_value(i).is_defined())
                    Console.Write(get_value(i));
                else
                    Console.Write("NOT_BOUND");
            }
        }


    }
}
