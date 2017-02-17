using ZY.Common.Tools;
using ZY.Common.Types;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ZY.Common.Datas
{
    [XmlIncludeAttribute(typeof(List<ArcSegment>))]
    [XmlIncludeAttribute(typeof(List<LineSegment>))]
    [Serializable]
    /// <summary>
    /// 曲线段
    /// </summary>
    public abstract class CurveSegment
    {
        /// <summary>
        /// 根据指定精度获取点列
        /// </summary>
        /// <param name="precision">精度，默认0.1（弧度或距离）</param>
        /// <returns></returns>
        public abstract IReadOnlyCollection<Point3D> ToList(double precision = 0.1);

        /// <summary>
        /// 拷贝平移（原始曲线段保持不变，新增一个平移后的曲线段）
        /// </summary>
        /// <param name="radian">平移方向</param>
        /// <param name="distance">平移距离</param>
        /// <returns>平移后的曲线段</returns>
        public virtual CurveSegment CopyMove(double radian, double distance)
        {
            CurveSegment copyTemp = CloneTool.DeepClone(this, DeepCloneType.Serialize) as CurveSegment;
            if (copyTemp == null)
                throw new ArgumentNullException("拷贝曲线（CopyRotate）时，序列化拷贝曲线段时，出现了空引用");

            copyTemp.Move(radian, distance);
            return copyTemp;
        }

        /// <summary>
        /// 平移
        /// </summary>
        /// <param name="radian">平移方向</param>
        /// <param name="distance">平移距离</param>
        public abstract void Move(double radian, double distance);

        /// <summary>
        /// 拷贝旋转（原始曲线段保持不变，新增一个旋转后的曲线段）
        /// 指定基准点，按指定方向（顺/逆时针）旋转指定角度
        /// </summary>
        /// <param name="referencePos">旋转基准点</param>
        /// <param name="radian">旋转角度</param>
        /// <param name="rotateDirection">旋转方向</param>
        /// <returns>旋转后的曲线</returns>
        public virtual CurveSegment CopyRotate(Point3D referencePos, double radian, ArcDirctionType rotateDirection)
        {
            CurveSegment copyTemp = CloneTool.DeepClone(this, DeepCloneType.Serialize) as CurveSegment;
            if (copyTemp == null)
                throw new ArgumentNullException("拷贝曲线（CopyRotate）时，序列化拷贝曲线段时，出现了空引用");

            copyTemp.Rotate(referencePos, radian, rotateDirection);
            return copyTemp;
        }

        /// <summary>
        /// 指定基准点，按指定方向（顺/逆时针）旋转指定角度
        /// </summary>
        /// <param name="referencePos">旋转基准点</param>
        /// <param name="radian">旋转角度</param>
        /// <param name="rotateDirection">旋转方向</param>
        public abstract void Rotate(Point3D referencePos, double radian, ArcDirctionType rotateDirection);

        /// <summary>
        /// 根据曲线段上的一点截取点之前的曲线段
        /// </summary>
        /// <param name="point">曲线上用于截取的点</param>
        /// <returns>点之前的曲线段，点不在曲线上返回原始曲线的深度拷贝</returns>
        public virtual CurveSegment CutOutPreviousParagraphbyPoint(Point3D point)
        {
            List<CurveSegment> childCurveSegments = CuteOutbyPoint(point);
            if (childCurveSegments == null)
                throw new ArgumentNullException("根据曲线段上的一点截取点之前的曲线段（CutOutPreviousParagraphbyPoint）时，截取线段为空");

            if (childCurveSegments.Count <= 0)
                throw new ArgumentOutOfRangeException("根据曲线段上的一点截取点之前的曲线段（CutOutPreviousParagraphbyPoint）时，无截取子线段");

            return childCurveSegments[0];
        }

        /// <summary>
        /// 根据曲线段上的一点截取点之后的曲线段
        /// </summary>
        /// <param name="point">曲线上用于截取的点</param>
        /// <returns>点之后的曲线段，点不在曲线上则返回</returns>
        public virtual CurveSegment CuteOutAfterParagraphbyPoint(Point3D point)
        {
            List<CurveSegment> childCurveSegments = CuteOutbyPoint(point);
            if (childCurveSegments == null)
                throw new ArgumentNullException("根据曲线段上的一点截取点之前的曲线段（CutOutPreviousParagraphbyPoint）时，截取线段为空");

            if (childCurveSegments.Count <= 0)
                throw new ArgumentOutOfRangeException("根据曲线段上的一点截取点之前的曲线段（CutOutPreviousParagraphbyPoint）时，无截取子线段");

            return childCurveSegments.Count > 1 ? childCurveSegments[1] : childCurveSegments[0];
        }

        /// <summary>
        /// 根据曲线段上的一点把曲线段截为两个子曲线段
        /// </summary>
        /// <param name="point">曲线上用于截取的点</param>
        /// <returns>截取后的子曲线段，点不在曲线上返回原始曲线</returns>
        public abstract List<CurveSegment> CuteOutbyPoint(Point3D point);

        /// <summary>
        /// 判断射线与曲线段位置关系
        /// </summary>
        /// <param name="line">射线</param>
        /// <returns></returns>
        public abstract LineCurveRelationShipType GetCurveCurveRelationShipType(LineSegment line);

        /// <summary>
        /// 获取射线与曲线段的交点集合
        /// </summary>
        /// <param name="line">射线</param>
        /// <returns></returns>
        public abstract IReadOnlyCollection<Point3D> GetCrossPoints(LineSegment line);

        /// <summary>
        /// 获取曲线段的起始方向（弧线为沿曲线方向的切线方向），单位弧度
        /// </summary>
        /// <returns></returns>
        public abstract double GetStartCourse();

        /// <summary>
        /// 获取曲线段的末尾方向（弧线为沿曲线方向的切线方向），单位弧度
        /// </summary>
        /// <returns></returns>
        public abstract double GetEndCourse();

        /// <summary>
        /// 获取起始点
        /// </summary>
        /// <returns></returns>
        public abstract Point3D GetStartPoint();

        /// <summary>
        /// 反转曲线段（调转方向）
        /// </summary>
        /// <returns></returns>
        public abstract CurveSegment Reverse();

        /// <summary>
        /// 获取末尾点
        /// </summary>
        /// <returns></returns>
        public abstract Point3D GetEndPoint();

        /// <summary>
        /// 判断点是否在曲线段上
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public abstract bool PointIsOnCurveSegment(Point3D point);


        #region Override Object methods
        public abstract override bool Equals(object obj);
        public abstract override int GetHashCode();
        public abstract override string ToString();
        #endregion
    }
}
