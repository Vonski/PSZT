using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rider
{
    class RandomGenerator
    {
        Random rand = new Random();

        public double GenerateNormal(double mean, double sigma)
        {            
            double u1 = 1.0 - rand.NextDouble(); 
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); 
            double randNormal = mean + sigma * randStdNormal;
            return randNormal;
        }
    }
   
}
