using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{
    public class Agent
    {
        protected string _name;
        protected int _id;
        protected BDIEngine _engine;
        protected BDIGenerator _bdi;
        public Agent(string name, int id, BDIGenerator bdi)
        {
            if(!bdi.is_config)
            {
                Console.WriteLine("BDI:"+bdi.name+" is not configured!");
                return;
            }
            _name = name;
            _id = id;
            _bdi = bdi;
            if(_bdi==null)
            {

            }
            _engine = new BDIEngine(_bdi);
            _engine.config();
        }
 
        public void Go()
        {
            _engine.Run();
        }
    }
}
