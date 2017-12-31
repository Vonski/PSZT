using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Rider
{

    class OnePlusOneParallel : IAlgorithm
    {
        List<OnePlusOne> OPOinstances = new List<OnePlusOne>();
        double m;
        double c1;
        double c2;
        double sigma;
        IProblem bestSolution;


        public OnePlusOneParallel(double m, double c1, double c2, double sigma)
        {
            this.m = m;
            this.c1 = c1;
            this.c2 = c2;
            this.sigma = sigma;
        }

        public double[] Iterate(int n)
        {
            for (int i = 0; i < OPOinstances.Count; ++i)
            {
                OPOinstances[i].Iterate(n);
                if (OPOinstances[i].GetProblem().AdaptationFunction() > bestSolution.AdaptationFunction())
                    bestSolution = OPOinstances[i].GetProblem().Clone();
            }
            return bestSolution.GetParameters();
        }

        public double[] Optimize()
        {
            for(int i=0; i<OPOinstances.Count; ++i)
            {
                OPOinstances[i].Optimize();
                if (OPOinstances[i].GetProblem().AdaptationFunction() > bestSolution.AdaptationFunction())
                    bestSolution = OPOinstances[i].GetProblem().Clone();
            }
            return bestSolution.GetParameters();
        }

        public void SetProblem(IProblem problem)
        {
            OPOinstances.Add(new OnePlusOne(m, c1, c2, sigma));
            OPOinstances.Last<OnePlusOne>().SetProblem(problem.Clone());
            if(bestSolution==null)
                bestSolution = OPOinstances.First<OnePlusOne>().GetProblem().Clone();
        }

        public double[] GetParametersOfProblem(int n)
        {
            return OPOinstances[n].GetProblem().GetParameters();
        }

    }
}
