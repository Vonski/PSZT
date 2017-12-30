using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rider
{
    interface IAlgorithm
    {
        double[] Optimize();
        double[] Iterate(int n);
        void SetProblem(IProblem problem);
    }
}
