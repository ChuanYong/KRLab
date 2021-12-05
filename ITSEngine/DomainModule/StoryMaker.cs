using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;

namespace ITS.DomainModule
{
    public class StoryMaker
    {
        private SemanticNet _net;
        private string _knowledgePoint;
        private Story _story;
        private SNNode _storyNode;

        public SemanticNet Net
        {
            get { return _net; }
        } 
        public Story Story
        { get { return _story; } }

        public StoryMaker(SemanticNet net)
        {
            _net = net;
            _knowledgePoint = "";
            _storyNode = _net.GetNode("故事");
            _story = new Story(_storyNode,_net);

            if (_storyNode == null) throw new Exception("没有发现故事结点");

            AddStorylines(ref _story);
            if (_story.Storylines.Count == 0)
                throw new Exception("该故事中没有发现与" + _knowledgePoint + "相关的情节");

            //_storySubNets = SNetParser.CreateStoryNets(_currentSNet, _subNetName);
            //if (_storySubNets.Count == 0)
            //    throw new Exception("在" + _subNetName + "中没有找到相应的故事语义网");

        }

        public void AddStorylines(ref Story story)
        { 
            List<SNNode> nodes = _net.GetNodes("情节");
            if (nodes.Count == 0) throw new Exception("在故事结点中没有发现情节结点");
            foreach(var node in nodes)
            {
                Storyline tmp = new Storyline(_net, node);
                if(tmp.KnowledgePointNode.Name==_knowledgePoint)
                    story.Storylines.Add(tmp);
            } 
        }


    }
}
