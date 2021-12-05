using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace Utilities
{
    public class AssemblyInstance
    { 
        protected Assembly _assembly = null;

        public AssemblyInstance(AssemblyName name)
        {
            _assembly = Assembly.Load(name);
        }

        public object CreateInstance(string typeName)
        {
            return _assembly.CreateInstance(typeName);
        }
    }
}
