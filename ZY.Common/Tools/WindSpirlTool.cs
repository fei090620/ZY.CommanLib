using ZY.Common.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.Common.Tools
{
    public class WindSpirlTool
    {
        public static double Distance(Point3D pointA, Point3D pointB)
        {
            if (pointA == null
                || pointB == null)
                return -1;

            return Math.Sqrt(Math.Pow(pointA.X - pointB.X, 2) + Math.Pow(pointA.Y - pointB.Y, 2));
        }

        public static Point3D Polar(double radian, double sita)
        {
            return new Point3D() { X = (int)(radian * Math.Cos(sita)), Y = (int)(radian * Math.Sin(sita)) };
        }
    }
}
