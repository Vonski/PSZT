using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rider
{
    class Point
    {
        public Point (double r, double angle)
        {
            this.R = r;
            this.Angle = angle;
        }
        double R;
        double Angle;

        public void SetR(double r)
        {
            this.R = r;
        }
        public void SetAngle(double angle)
        {
            this.Angle = angle;
        }
        public double getR()
        {
            return R;
        }
        public double GetAngle()
        {
            return Angle;
        }
        public double getDegrees()
        {
            return Angle * (180.0 / Math.PI);
        }
        public double getX()
        {
            return R * Math.Cos(Angle);
        }
        public double getY()
        {
            return R * Math.Sin(Angle);
        }
    }
}
