using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rider
{
    class Program
    {
        static void Main(string[] args)
        {
            MotionEngine me = new MotionEngine();
            double[] p = { 0, 115, 0, -1.5, -1, 0, -0.5, -0.27 };
            me.SetParameters(p);
            //bool check = true;
            //while (check)
            //{
            //    check = me.Iterate();
            //    Console.WriteLine("Time: " + Math.Round(me.GetTime(),3) + "\tPosition: R=" + Math.Round(me.GetPoint().getR(),2) + "\tDegrees=" + Math.Round(me.GetPoint().getDegrees(),2));
            //}

            OnePlusOne opo = new OnePlusOne(10, 0.82, 1.2, 1.0);
            opo.SetProblem(me);
            opo.Optimize();

            //Console.WriteLine("Angle: " + me.GetPoint().getDegrees());
            Console.ReadKey();
        }
    }
}
