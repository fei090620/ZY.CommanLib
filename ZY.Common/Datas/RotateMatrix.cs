using ZY.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("ZY.Common.Test")]
namespace ZY.Common.Datas
{
    /// <summary>
    /// 矩阵相关（2D）
    /// </summary>
    internal class RotateMatrix
    {
        internal int m_row;//行
        internal int m_col;//列
        internal double[,] m_data;//数据

        internal int Row
        { get { return m_row; } }

        internal int Column
        { get { return m_col; } }


        internal RotateMatrix()
        {
            this.m_row = 0;
            this.m_col = 0;
            this.m_data = new double[0, 0];
        }

        internal RotateMatrix(double[,] matrix)
        {
            this.m_row = matrix.GetLength(0);
            this.m_col = matrix.GetLength(1);
            this.m_data = matrix;
        }

        internal RotateMatrix(int row, int col)
        {
            if (row >= 0 && col >= 0)
            {
                m_row = row;
                m_col = col;
                m_data = new double[row, col];
            }
        }

        /// <summary>
        /// 加法
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        internal RotateMatrix Add(RotateMatrix matrix)
        {
            if (m_row != matrix.Row || m_col != matrix.Column)
            {
                return null;
            }

            RotateMatrix re = new RotateMatrix(m_row, m_col);
            for (int i = 0; i < matrix.Row; i++)
            {
                for (int j = 0; j < matrix.Column; j++)
                {
                    re.m_data[i, j] = m_data[i, j] + matrix.m_data[i, j];
                }
            }
            return re;
        }

        /// <summary>
        /// 减法
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        internal RotateMatrix Subtract(RotateMatrix matrix)
        {
            if (m_row != matrix.Row || m_col != matrix.Column)
            {
                return null;
            }

            RotateMatrix re = new RotateMatrix(m_row, m_col);
            for (int i = 0; i < matrix.Row; i++)
            {
                for (int j = 0; j < matrix.Column; j++)
                {
                    re.m_data[i, j] = m_data[i, j] - matrix.m_data[i, j];
                }
            }
            return re;
        }

        /// <summary>
        /// 乘法（乘以常数）
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        internal RotateMatrix Multiply(double count)
        {
            RotateMatrix re = new RotateMatrix(m_row, m_col);
            for (int i = 0; i < m_row; i++)
            {
                for (int j = 0; j < m_col; j++)
                {
                    re.m_data[i, j] = m_data[i, j] * count;
                }
            }
            return re;
        }

        /// <summary>
        /// 乘法（乘以矩阵）
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        internal RotateMatrix Multiply(RotateMatrix matrix)
        {
            if (this.Column != matrix.Row)
            {
                return null;
            }
            RotateMatrix re = new RotateMatrix(m_row, m_col);
            for (int i = 0; i < this.Row; i++)
            {
                for (int j = 0; j < matrix.Column; j++)
                {
                    for (int k = 0; k < this.Column; k++)
                    {
                        re.m_data[i, j] += m_data[i, k] * matrix.m_data[k, j];
                    }
                }
            }
            return re;
        }

        /// <summary>
        /// 旋转某角度
        /// </summary>
        /// <param name="angle"></param>
        ///  <param name="rotateDirection"></param>
        internal RotateMatrix Rotate2D(double angle, ArcDirctionType rotateDirection)
        {
            RotateMatrix result = RotateAt2D(angle, new Point3D() { X = 0, Y = 0, Z = 0 }, rotateDirection);
            return result;
        }

        /// <summary>
        /// 先平移后旋转
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="point"></param>
        /// <param name="rotateDirection"></param>
        internal RotateMatrix RotateAt2D(double angle, Point3D point, ArcDirctionType rotateDirection)
        {
            if (rotateDirection == ArcDirctionType.UNKONW)
            {
                throw new Exception("ArcDirctionType is wrong.");
            }
            else if (rotateDirection == ArcDirctionType.CLOCK_WISE)
            {
                angle = -angle;
            }

            double[,] doubleNew = new double[this.m_row, this.m_col + 1];
            for (int i = 0; i < this.m_row; i++)
            {
                for (int j = 0; j < this.m_col; j++)
                {

                    doubleNew[i, j] = this.m_data[i, j];
                }
                doubleNew[i, this.m_col] = 1;
            }

            angle %= 360.0;

            RotateMatrix result = new RotateMatrix(doubleNew).Multiply(CreateRotationRadians2D(angle * (Math.PI / 180.0), point.X, point.Y));
            return result;
        }

        /// <summary>
        /// Creates a rotation transformation about the given point(2D)
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <returns></returns>
        internal static RotateMatrix CreateRotationRadians2D(double angle, double centerX, double centerY)
        {
            double[,] data = new double[3, 2];
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);
            double dx = (centerX * (1.0 - cos)) + (centerY * sin);
            double dy = (centerY * (1.0 - cos)) - (centerX * sin);

            data[0, 0] = cos;
            data[0, 1] = sin;
            data[1, 0] = -sin;
            data[1, 1] = cos;
            data[2, 0] = dx;
            data[2, 1] = dy;

            RotateMatrix matrix = new RotateMatrix(data);
            return matrix;
        }
    }


}
