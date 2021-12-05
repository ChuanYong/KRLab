using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.BDI
{ 
    public class BDIEngine
    {
        private BDIGenerator _bdiGenerator;
        private BDI _bdi;

        public BDIEngine(BDIGenerator bdiGenerator)
        {
            _bdiGenerator = bdiGenerator;
            _bdi = bdiGenerator.BDI;
        } 

        public void config()
        {
            if (_bdi == null)
            {
                utils.print("please set bdi for this engine...");
                return;
            } 
        }

        public void Run()
        { 
            SoakElement selected_element; 
            while(true)
            {
                _bdi.ins.ExecuteCycle();

                Soak soak = new Soak(_bdi); 
                int size = soak.get_size(); 
                if (size == 0)
                {
                    if (_bdi.desires.all_goals_done())
                    {
                        utils.print("all top-level goals achieved!");
                        return;
                    }
                    if (_bdi.ins.get_top_level_goal() == null)
                    {
                        _bdi.desires.renew_leaf_goals();
                        continue;
                    }
                }
                else
                {
                    // intend one of the KAs selected randomly
                    // selected_element = soak->get_random();
                    // selected_element = soak->get_priority_first();
                    selected_element = soak.get_priority_random();
                    _bdi.ins.intend(selected_element);

                }
                if (_bdi.ins.activate() == StatusType.IS_FAILURE)
                {
                    _bdi.ins.clear_current_stack(); 
                } 

            }
        } 


    }
}
