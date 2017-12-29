using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rider
{
    class OnePlusOne : IAlgorithm
    {

        double m;
        double c1;
        double c2;
        double sigma;
        const double min_sigma=0.0001;
        double chosenChildren;
        double fi;
        IProblem problem;
        IProblem parentProblem;
        RandomGenerator randomGenerator;

        public OnePlusOne(int m, double c1, double c2, double sigma)
        {
            this.m = m;
            this.c1 = c1;
            this.c2 = c2;
            this.sigma = sigma;
            randomGenerator = new RandomGenerator();
        }

        public void SetProblem(IProblem problem)
        {
            this.problem = problem;
            parentProblem = problem.Clone();
            while (parentProblem.Iterate()) ;
        }

        double[] GenerateChild(double[] parent)
        {
            double[] child = (double[])parent.Clone();
            for(int i=0; i<child.Length; ++i)
                child[i] += randomGenerator.GenerateNormal(0, sigma);
            return child;
        }

        void ChooseBetter(double[] child)
        {
           
            //testing child
            problem.SetParameters(child);
            bool check = true;
            while(check)
                check = problem.Iterate();
            double childValue = problem.AdaptationFunction();

            //testing parent
            double parentValue = parentProblem.AdaptationFunction();

            //choosing better
            if (childValue>parentValue)
            {
                ++chosenChildren;
                parentProblem = problem.Clone();
                Console.WriteLine(childValue);
            }
        }
        void ChangeSigma()
        {
            fi = chosenChildren / m;
            chosenChildren = 0;

            if (fi < 0.2)
                sigma *= c1;
            else if (fi > 0.2)
                sigma *= c2;
        }

        public double[] Optimize()
        {
            int iterations = 0;
            double[] child;
            while(sigma>min_sigma)
            {
                child = GenerateChild(parentProblem.GetParameters());
                ChooseBetter(child);

                ++iterations;
                if(iterations%m==0)
                    ChangeSigma();
            }
            return parentProblem.GetParameters();
        }

    }
}
