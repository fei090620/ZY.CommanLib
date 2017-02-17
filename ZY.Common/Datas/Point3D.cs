using ZY.Common.Tools;
using ZY.Common.Types;
using System;

namespace ZY.Common.Datas
{
    [Serializable]
    /// <summary>
    /// 基础点类型
    /// </summary>
    public class Point3D
    {
        private double _x;
        private double _y;
        private double _z;

        #region Construct
        public Point3D()
        {
            _x = 0.0d;
            _y = 0.0d;
            _z = 0.0d;
        }

        public Point3D(Point3D other)
        {
            _x = other.X;
            _y = other.Y;
            _z = other.Z;
        }

        public Point3D(double x, double y)
        {
            _x = x;
            _y = y;
            _z = 0;
        }

        public Point3D(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
        public Point3D(double[] coordinates)
        {
            if (coordinates == null
                || coordinates.Length < 2)
                return;

            X = coordinates[0];
            Y = coordinates[1];
            Z = 0;
        }
        #endregion

        #region Properties
        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public double Z
        {
            get { return _z; }
            set { _z = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 已知距离和角度（弧度）计算下一个点
        /// </summary>
        /// <param name="radian"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public Point3D GetNextPoint(double radian, double precision)
        {
            radian = FormateTool.ToStandardRadian(radian);
            return new Point3D(precision * Math.Sin(radian) + this.X, precision * Math.Cos(radian) + this.Y, this.Z);
        }

        /// <summary>
        /// 计算当前点至另一个点的距离
        /// </summary>
        /// <param name="otherPoint"></param>
        /// <returns></returns>
        public double DisTo(Point3D otherPoint)
        {
            return Math.Sqrt(Math.Pow(this.X - otherPoint.X, 2)
                + Math.Pow(this.Y - otherPoint.Y, 2)
                + Math.Pow(this.Z - otherPoint.Z, 2));
        }

        /// <summary>
        /// 计算当前点至下一个点的方向与正北方向夹角（弧度表示）
        /// </summary>
        /// <param name="otherPoint"></param>
        /// <returns></returns>
        public double DirTo(Point3D otherPoint)
        {
            if (otherPoint == null)
                return 0;

            return Math.Atan2(otherPoint.X - this.X, otherPoint.Y - this.Y);
        }

        /// <summary>
        /// 以当前点为起点，以EndPoint为终点够着向量
        /// </summary>
        /// <param name="EndPoint"></param>
        /// <returns></returns>
        public Point3D Vector(Point3D EndPoint)
        {
            return new Point3D(EndPoint.X - this.X, EndPoint.Y - this.Y, EndPoint.Z - this.Z);
        }

        /// <summary>
        /// 根据当前点及距离构造向量
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public Point3D Vector(double radian)
        {
            return this.Vector(this.GetNextPoint(radian, 1000) as Point3D);
        }

        /// <summary>
        /// 向量叉积(默认维度为二维,暂时不支持三维向量的叉乘运算，因为三维叉乘返回三维向量)
        /// </summary>
        /// <param name="otherVector"></param>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public double VectorProduct(Point3D otherVector, DimensionType dimension = DimensionType.D2)
        {
            switch (dimension)
            {
                case DimensionType.D2:
                    return X * otherVector.Y - otherVector.X * Y;
                case DimensionType.D3:
                default:
                    break;
            }

            return double.NaN;
        }

        /// <summary>
        /// 获取向量的模或长度
        /// </summary>
        /// <returns></returns>
        public double GetModel()
        {
            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
        }

        /// <summary>
        /// 向量点乘
        /// </summary>
        /// <param name="otherVector"></param>
        /// <returns></returns>
        public double PointMulity(Point3D otherVector)
        {
            return X * otherVector.X + Y * otherVector.Y + Z * otherVector.Z;
        }

        /// <summary>
        /// 计算当前向量与otherVector的夹角
        /// </summary>
        /// <param name="otherVector"></param>
        /// <returns></returns>
        public double IncludedAngle(Point3D otherVector)
        {
            double cos = this.PointMulity(otherVector) / (this.GetModel() * (otherVector as Point3D).GetModel());
            cos = Math.Abs(cos + 1) <= OverrallVraTool.DoublePrecision ? -1
                : Math.Abs(cos - 1) <= OverrallVraTool.DoublePrecision ? 1
                : cos;

            return Math.Acos(cos);
        }

        /// <summary>
        /// 以当前点为基点，计算两个方向的向量夹角
        /// </summary>
        /// <param name="dir1"></param>
        /// <param name="dir2"></param>
        /// <returns></returns>
        public double IncludeAnglebyTwoDirs(double dir1, double dir2)
        {
            return (this.Vector(dir1) as Point3D).IncludedAngle(this.Vector(dir2));
        }

        public Point3D Add(Point3D other)
        {
            return new Point3D() { X = this.X + other.X, Y = this.Y + other.Y, Z = this.Z + other.Z };
        }
        #endregion

        #region Override Object Methods
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null || !GetType().IsEquivalentTo(obj.GetType()))
                return false;

            Point3D other = (Point3D)obj;
            return DisTo(other) <= OverrallVraTool.DoublePrecision;
        }

        public override int GetHashCode()
        {
            const int hashIndex = 307;
            var result = (!double.IsNaN(X)) ? X.GetHashCode() : 0;
            result = (result * hashIndex) ^ ((!double.IsNaN(Y)) ? Y.GetHashCode() : 0);
            result = (result * hashIndex) ^ ((!double.IsNaN(Z)) ? Z.GetHashCode() : 0);

            return result;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", X, Y, Z);
        }
        #endregion
    }
}
