using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS.DomainModule;

namespace ITS.MaterialModule
{
    public abstract class Answer
    { 
        public Answer( )
        { 
        }

        public abstract object Content
        {
            get;
        }
    }

}
