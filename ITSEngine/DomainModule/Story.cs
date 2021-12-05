using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;


namespace ITS.DomainModule
{
    public class Story
    {
        private List<Storyline> _storylines;
        private SNNode _node;
        private SemanticNet _net;

        public List<Storyline> Storylines
        { get { return _storylines; } }
        public SNNode Node
        { get { return _node; } }

        public Story(SNNode node, SemanticNet net)
        {
            _node = node;
            _net = net;
            _storylines = new List<Storyline>();
        }

        public Storyline GetStorylineRandomly()
        {
            int i = new Random().Next(0, _storylines.Count);
            return _storylines[i];
        }
    }
}
