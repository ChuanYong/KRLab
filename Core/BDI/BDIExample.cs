using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace KRLab.Core.BDI
{
    public class BDIExample: BDIGenerator
    {
        public BDIExample(string name)
      : base(name)
        {
        }

        public override void Config()
        {
            base.Config();
        }

        public override void ConfigBelief()
        {
            //define a belief 
            Relation r0 = new Relation("hao", new Value("grand_father"), new Value("unknown"));
            _bdi.beliefs.add(r0);

        }

        public override void ConfigDesire()
        {

            //set goal_list
            Achieve g0 = new Achieve("hao_grand_father", new Relation("hao_grand_father"));
            Achieve g1 = new Achieve("hao_mather", new Relation("hao_mother"));
            ///????????????????????????????如果有多个goal，前后链接要准备好
            _bdi.desires.add(g0);
            _bdi.desires.add(g1);
        }
        public override void ConfigIntention()
        {
            ///ka0////////////////////////////////////////////////////////////////////////////
            {
                Achieve ga = new Achieve("hao_grand_father1", new Relation("hao_grand_father1"));
                FactCondition fc = new FactCondition(new Relation("hao", new Value("grand_father"), new Value("unknown")));
                KaContext kc = new KaContext(fc);

                //Fact action = new Fact(data.beliefs.lookup(0).get_relation());
                Print p0 = new Print(new Value(0.5));
                Achieve ach = new Achieve("hao_grand_father2", new Relation("hao_grand_father2"));
                Ka ka = create_ka("first_ka", "", ach, kc, p0);
                _bdi.intentions.add_ka(ka);

                _bdi.current_ka = ka;
            }


            //ka1//////////////////////////////////////////////////////////////////////////////
            {
                Achieve ga = new Achieve("hao_grand_father2", new Relation("hao_grand_father2"));

                FactCondition fc = new FactCondition(new Relation("hao", new Value("grand_father"), new Value("unknown")));
                KaContext kc = new KaContext(fc);

                Fact action = new Fact(_bdi.beliefs.lookup("hao").get_relation());
                Print p0 = new Print(new Value(0.6));
                Achieve ach = new Achieve("hao_grand_father", new Relation("hao_grand_father"));
                Achieve ach1 = new Achieve("hao_father2", new Relation("hao_father2"));
                _bdi.intentions.add_ka(create_ka("second_ka", "", ach, kc, p0));
            }

            //ka2//////////////////////////////////////////////////////////////////////////////
            {
                Achieve ga = new Achieve("hao_father2", new Relation("hao_father2"));

                FactCondition fc = new FactCondition(new Relation("hao", new Value("grand_father"), new Value("unknown")));
                KaContext kc = new KaContext(fc);

                Fact action = new Fact(_bdi.beliefs.lookup("hao").get_relation());
                Print p0 = new Print(new Value(0.7));
                Achieve ach = new Achieve("hao_mother", new Relation("hao_mother"));
                _bdi.intentions.add_ka(create_ka("second_ka", "", ach, kc, p0));
            }

        } 
    }
}
