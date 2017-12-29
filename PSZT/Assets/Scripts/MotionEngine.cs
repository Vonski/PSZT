using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rider
{
    class MotionEngine : IProblem
    {
        Point point = new Point(35,0);
        double[] v = new double[2] { 0, 0 };//velocity
        double[] a0 = new double[2]; //const acceleration radial,transversal
        double[,] B = new double[2,2]; //speed proportional acceleration [0,x] - radial, [1,x] - transversal, [x,0] - from radial speed, [x,1] from transversal speed
        double[] c = new double[2]; //radius proportional acceleration radial, transversal
        static double dT = 0.001;
        double time = 0;
        //public void SetParameters(double constRadial, double constTransversal, double radialFromRadial,  double radialFromTransversal, double transversalFromRadial, double transversalFromTransversal, double radiusRadial, double radiusTransversal)
        //{
        //    a0[0] = constRadial;
        //    a0[1] = constTransversal;
        //    B[0, 0] = radialFromRadial;
        //    B[0, 1] = radialFromTransversal;
        //    B[1, 0] = transversalFromRadial;
        //    B[1, 1] = transversalFromTransversal;
        //    c[0] = radiusRadial;
        //    c[1] = radiusTransversal;
        //}
        public void SetParameters(double[] parameters)
        {
            Reset();
            a0[0] = parameters[0];
            a0[1] = parameters[1];
            B[0, 0] = parameters[2];
            B[0, 1] = parameters[3];
            B[1, 0] = parameters[4];
            B[1, 1] = parameters[5];
            c[0] = parameters[6];
            c[1] = parameters[7];
        }

        public double[] GetParameters()
        {
            double[] parameters = new double[8];
            parameters[0]=a0[0];
            parameters[1] = a0[1];
            parameters[2] = B[0, 0];
            parameters[3] = B[0, 1];
            parameters[4] = B[1, 0];
            parameters[5] = B[1, 1];
            parameters[6] = c[0];
            parameters[7] = c[1];
            return parameters;
        }

        public Point GetPoint()
        {
            return point;
        }
        public double GetTime()
        {
            return time;
        }
        public bool Iterate()
        {
            time += dT;
            v[0] += (a0[0] + (v[0] * B[0, 0]) + (v[1] * B[0, 1]) + (point.getR() * c[0]) + (v[0]*v[0]+v[1]*v[1])/point.getR()) * dT;
            v[1] += (a0[1] + (v[0] * B[1, 0]) + (v[1] * B[1, 1]) + (point.getR() * c[1])) * dT;
            point.SetAngle(point.GetAngle() + v[1] * dT / point.getR());
            point.SetR(point.getR() + v[0] * dT);
            if (point.getR() < 30 || point.getR() > 40 || point.getDegrees()>3600.0) //last condition means that paremeters are so good that we can ride 10 loops! (eventually to change)
                return false;
            return true;
        }

        public double AdaptationFunction()
        {
            return point.getDegrees();
        }

        public void Reset()
        {
            point = new Point(35, 0);
            time = 0;
            v = new double[2] { 0, 0 };
        }

        public IProblem Clone()
        {
            MotionEngine me = new MotionEngine();
            me.point = new Point(this.point.getR(), this.point.GetAngle());
            me.v = (double[])this.v.Clone();
            me.a0 = (double[])this.a0.Clone();
            me.B = (double[,])this.B.Clone();
            me.c = (double[])this.c.Clone();
            me.time = this.time;
            return me;
        }
    }
}
/*
    Zużlowiec porusza się po okrągłym torze o szerokości 10m i zewnętrznym promieniu
    40m. W każdej chwili jego przyspieszenie wynosi
    a = a0 + B v + c x
    w układzie współrzędnych, którego oś x pokrywa się z promieniem, na którym znajduje się
    żużlowiec, x to odległość między jego pozycją a środkiem toru, zaś v to prędkość w tym
    układzie współrzędnych; a0 ora c to wektory, zaś B jest macierzą. W sumie na a0, B, c składa
    się 8 parametrów
*/
