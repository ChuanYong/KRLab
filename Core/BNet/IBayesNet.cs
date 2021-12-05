using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KRLab.BNet
{
    public interface IBayesNet
    {
        string Name
        { get; set; }
        void Load(string file);
    }
}
