using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rider
{
    interface IAlgorithm
    {
        double[] Optimize();
        void SetProblem(IProblem problem);
    }
}
