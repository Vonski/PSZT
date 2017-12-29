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
        }

        double[] GenerateChild(double[] parent)
        {
            double[] child = (double[])parent.Clone();
            for(int i=0; i<child.Length; ++i)
            {
                child[i] += randomGenerator.GenerateNormal(0, sigma);
            }
            return child;
        }

        double[] ChooseBetter(double[] parent, double[] child)
        {
            //testing parent
            problem.SetParameters(parent);
            bool check=true;
            while(check)
                check=problem.Iterate();
            double parentValue = problem.AdaptationFunction();
            Console.WriteLine("parent: "+parentValue);
            //testing child
            problem.SetParameters(child);
            check = true;
            while(check)
                check = problem.Iterate();
            double childValue = problem.AdaptationFunction();
            Console.WriteLine("child: "+childValue);

            //choosing better
            if (childValue>parentValue)
            {
                ++chosenChildren;

                return child;
            }
            else
            {
                return parent;
            }
        }
        void ChangeSigma()
        {
            fi = (double)(chosenChildren / m);
            chosenChildren = 0;

            if (fi < 0.2)
                sigma *= c1;
            else if (fi > 0.2)
                sigma *= c2;
        }

        public double[] Optimize()
        {
            int iterations = 0;
            double[] parent = problem.GetParameters();
            double[] child;
            while(sigma>min_sigma)
            {
                child = GenerateChild(parent);
                parent = ChooseBetter(parent, child);

                ++iterations;
                if(iterations==m)
                {
                    ChangeSigma();
                    iterations = 0;
                    Console.WriteLine("zmieniam sigme " + sigma);


                }
            }
            Console.WriteLine("juz");
            return parent;
        }

    }
}
