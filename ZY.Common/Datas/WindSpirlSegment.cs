using ZY.Common.Tools;
using ZY.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.Common.Datas
{
    public class WindSpirlSegment : CurveSegment
    {
        #region 参数
        private double startSita = 0;

        private double endSita = Math.PI;

        private double _esita;

        private double _r;

        //偏流角，是指飞机在空中飞行时，飞机相对空气运动的方向（航向）同相对地面运动的方向（航迹角）之间的夹角。
        private double _DA;

        private double _cosDA;

        private double _sinDA;
        #endregion

        //构造函数
        public WindSpirlSegment(TurnParameters tp, double startSita, double endSita)
        {
            _esita = tp.Esita;
            _r = tp.Radius;
            _DA = tp.DraftAngle;
            _cosDA = Math.Cos(_DA);
            _sinDA = Math.Sin(_DA);
            this.startSita = startSita;
            this.endSita = endSita;
        }

        //根据 alpha 角计算 sita 角
        public double GetSita(double alfa)
        {
            //输入alfa角（单位：度），迭代计算sita角度（单位：弧度）
            alfa = ((alfa % 360) + 360) % 360;//将负值转为正值，或将度数转到有效范围内。
            alfa = alfa * Math.PI / 180;
            double sita = alfa;//角度转弧度，然后将当前角度当做sita来用

            double rho = Math.Sqrt(_r * _r + sita * _esita * sita * _esita + 2 * _r * sita * _esita * _cosDA);
            double calcuAlpha = sita - Math.Asin(sita * _esita * _sinDA / rho);
            double tolerance = alfa - calcuAlpha;

            int times = 0;//统计迭代次数
            while ((tolerance > 0.000000001))
            {
                sita += tolerance;
                times += 1;
                rho = Math.Sqrt(_r * _r + sita * _esita * sita * _esita + 2 * _r * sita * _esita * _cosDA);
                calcuAlpha = sita - Math.Asin(sita * _esita * _sinDA / rho);
                tolerance = alfa - calcuAlpha;
            }
            //trace( times );
            return sita;
        }

        //根据线外点 pointB，求过该点的风螺旋线切线的切点所对应的 sita 角
        public double GetTangentSita(Point3D pointB)
        {
            //输入风螺旋线外点，求过该点的切线对应的sita角度（单位：弧度）
            //圆心点C，线外点B，圆弧上的切点A，螺旋线上的切点D
            Point3D pointC = new Point3D(0, 0);//圆心默认位置
            double dist = WindSpirlTool.Distance(pointB, pointC);

            //atan2 函数具有指向性，但常出现负值，需要转换处理。
            double alfa = Math.Atan2(pointB.Y - pointC.Y, pointB.X - pointC.X);
            alfa = (alfa + 2 * Math.PI) % (2 * Math.PI);//转换成正值
            double beta = Math.Acos((_r / dist));
            double sita = alfa - beta;

            //check distance of B is out of  the windspiral
            //用 getSpiralPoint 函数来检测
            Point3D tempPt = GetTangentPoint(GetSita(alfa));
            if (dist < WindSpirlTool.Distance(tempPt, pointC))
                throw new Exception("The distance of L is too short. Please enlarge the outbound time.");

            //the tangent point of Norminal Track
            Point3D p = WindSpirlTool.Polar(_r, sita);
            Point3D pointA = pointC.Add(p); //add

            //the initial tangent point of Wind Spiral
            p = WindSpirlTool.Polar((_esita * sita), sita - _DA);
            Point3D pointD = pointA.Add(p); //add

            double calcuSita = Math.Atan2(pointB.Y - pointD.Y, pointB.X - pointD.X);
            calcuSita = (calcuSita + 2 * Math.PI) % (2 * Math.PI);//转换成正值
            double tolerance = calcuSita - ((sita - _DA) + Math.PI / 2);
            double j = 0;
            while (tolerance > 0.0000001)
            {
                sita += tolerance;
                p = WindSpirlTool.Polar(_r, sita);
                pointA = pointC.Add(p); //add

                p = WindSpirlTool.Polar((_esita * sita), sita - _DA);
                pointD = pointA.Add(p); //add

                calcuSita = Math.Atan2(pointB.Y - pointD.Y, pointB.X - pointD.X);
                calcuSita = (calcuSita + 2 * Math.PI) % (2 * Math.PI);//转换成正值
                tolerance = calcuSita - sita + _DA - Math.PI / 2;
                //trace( newMinus );
                j += 1;
            }

            return sita;
        }

        //给定 sita 角度，得到该角度所对应的风螺旋线上点
        public Point3D GetTangentPoint(double sita)
        {
            //给定sita 角度，得到该角度所对应的风螺旋点。 单位：弧度
            double rho = Math.Sqrt(_r * _r + sita * _esita * sita * _esita + 2 * _r * sita * _esita * _cosDA);
            double alfa = sita - Math.Asin(sita * _esita * _sinDA / rho);
            Point3D spiralPoint = new Point3D((int)(rho * Math.Cos(alfa)), (int)(rho * Math.Sin(alfa)));
            return spiralPoint;
        }

        //非精确角度绘制整个螺旋
        /*public List<Point3D> DrawWindSprial(double startAlpha = 0, double endAlpha = 270, double step = 1)
        //{
        //    //绘制指定范围的风螺旋线，所有参数均为 度数，运算数据为弧度数。
        //    return DrawAccruateWindSprial(GetSita(startAlpha), GetSita(endAlpha), step);
        //}*/

        //精确角度绘制主区
        public List<Point3D> DrawAccruateWindSprial(double step = 1)
        {
            //绘制指定θ角范围的风螺旋线，所有参数均为 弧度数。
            double sita = startSita;
            double rho = Math.Sqrt(_r * _r + sita * _esita * sita * _esita + 2 * _r * sita * _esita * _cosDA);
            double alfa = sita - Math.Asin(sita * _esita * _sinDA / rho);

            //建立点串
            List<Point3D> points = new List<Point3D>();
            Point3D startPoint = new Point3D();
            startPoint.X = (int)(rho * Math.Cos(alfa));
            startPoint.Y = (int)(rho * Math.Sin(alfa));
            points.Add(startPoint);

            while ((sita < endSita))
            {
                sita += step * Math.PI / 180;
                rho = Math.Sqrt(_r * _r + sita * _esita * sita * _esita + 2 * _r * sita * _esita * _cosDA);
                alfa = sita - Math.Asin(sita * _esita * _sinDA / rho);

                Point3D endPoint1 = new Point3D();
                endPoint1.X = (int)(rho * Math.Cos(alfa));
                endPoint1.Y = (int)(rho * Math.Sin(alfa));
                points.Add(endPoint1);
            }
            rho = Math.Sqrt(_r * _r + endSita * _esita * endSita * _esita + 2 * _r * endSita * _esita * _cosDA);
            alfa = endSita - Math.Asin(endSita * _esita * _sinDA / rho);

            Point3D endPoint = new Point3D();
            endPoint.X = (int)(rho * Math.Cos(alfa));
            endPoint.Y = (int)(rho * Math.Sin(alfa));
            points.Add(endPoint);
            return points;
        }

        //精确角度绘制副区
        public List<Point3D> DrawAccruateOffset(double offSet = 0, double step = 1)
        {
            //绘制指定θ角范围的风螺旋线，所有参数均为 弧度数。
            double sita = startSita;
            Point3D p1 = GetTangentPoint(startSita);
            Point3D p2 = p1.Add(WindSpirlTool.Polar(offSet, startSita - _DA));

            //建立点串
            List<Point3D> points = new List<Point3D>();
            points.Add(p2);

            Point3D OffsetStartPoint = new Point3D();
            OffsetStartPoint.X = p2.X;
            OffsetStartPoint.Y = p2.Y;
            while (sita < endSita)
            {
                sita += step * Math.PI / 180;
                p1 = GetTangentPoint(sita);
                p2 = p1.Add(WindSpirlTool.Polar(offSet, sita - _DA));
                points.Add(p2);
            }

            p1 = GetTangentPoint(endSita);
            Point3D OffsetEndPoint = new Point3D();
            OffsetEndPoint = p1.Add(WindSpirlTool.Polar(offSet, endSita - _DA));
            points.Add(OffsetEndPoint);
            return points;
        }

        #region Override
        /// <summary>
        /// 获取点集合
        /// </summary>
        /// <param name="precision"></param>
        /// <returns></returns>
        public override IReadOnlyCollection<Point3D> ToList(double precision = 0.1)
        {
            List<Point3D> points = new List<Point3D>();
            points.Add(GetTangentPoint(this.startSita)); //第一个点
            precision = Math.Abs(precision);
            int pointCount = (int)((this.endSita - this.startSita) / precision);
            for (int i = 1; i <= pointCount; i++)
            {
                Point3D p = GetTangentPoint(this.startSita + Math.Abs(precision) * i); //其余点
                if (!points.Exists(x => x.Equals(p)))
                {
                    points.Add(p);
                }
            }
            return points;
        }

        public override void Move(double radian, double distance)
        {

        }

        public override void Rotate(Point3D referencePos, double radian, ArcDirctionType rotateDirection)
        {
        }

        public override List<CurveSegment> CuteOutbyPoint(Point3D point)
        {
            return null;
        }

        public override LineCurveRelationShipType GetCurveCurveRelationShipType(LineSegment line)
        {
            return LineCurveRelationShipType.Coincide;
        }

        public override IReadOnlyCollection<Point3D> GetCrossPoints(LineSegment line)
        {
            return null;
        }

        /// <summary>
        /// 起始弧度
        /// </summary>
        /// <returns></returns>
        public override double GetStartCourse()
        {
            return this.startSita;
        }

        /// <summary>
        /// 结束弧度
        /// </summary>
        /// <returns></returns>
        public override double GetEndCourse()
        {
            return this.endSita;
        }

        /// <summary>
        /// 起始点
        /// </summary>
        /// <returns></returns>
        public override Point3D GetStartPoint()
        {
            return GetTangentPoint(this.startSita);
        }

        /// <summary>
        /// 终止点
        /// </summary>
        /// <returns></returns>
        public override Point3D GetEndPoint()
        {
            return GetTangentPoint(this.endSita);
        }

        public override CurveSegment Reverse()
        {
            return null;
        }

        public override bool PointIsOnCurveSegment(Point3D point)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            return null;
        }
        #endregion
    }
}
