using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rider
{
    interface IProblem
    {
        void SetParameters(double[] parameters);
        double[] GetParameters();
        double AdaptationFunction();
        bool Iterate();
        IProblem Clone();
    }
}
