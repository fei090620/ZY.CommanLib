using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

namespace ZY.Common.Datas
{
    /// <summary>
    /// 与地图交互统一数据接口
    /// </summary>
    public class FeatureObject : ICloneable
    {
        #region Construct
        public FeatureObject(IEnumerable<Point3D> coordinates = null,
                             DataRow row = null,
                             string layerName = null)
        {
            FeatureAttribute = row;
            LayerName = layerName;
            Coordinates = coordinates == null ? new List<Point3D>() : coordinates.ToList();
        }

        public FeatureObject(Point3D coordinate, DataRow row = null, string layerName = null)
        {
            FeatureAttribute = row;
            LayerName = layerName;
            Coordinates = coordinate == null ? new List<Point3D>() : new List<Point3D> { coordinate };
        }
        #endregion

        #region Properties
        /// <summary>
        /// 属性值
        /// </summary>
        public DataRow FeatureAttribute { get; set; }

        /// <summary>
        /// 点坐标
        /// </summary>
        public List<Point3D> Coordinates { get; set; }

        /// <summary>
        /// 图层名称
        /// </summary>
        public string LayerName { get; set; }
        #endregion

        #region Methods
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null || !GetType().IsEquivalentTo(obj.GetType()))
                return false;

            FeatureObject other = (FeatureObject)obj;

            if ((FeatureAttribute == null && other.FeatureAttribute != null) ||
                (FeatureAttribute != null && other.FeatureAttribute == null))
            {
                return false;
            }
            else if ((Coordinates == null && other.Coordinates != null) ||
                (Coordinates != null && other.Coordinates == null))
            {
                return false;
            }
            else if ((string.IsNullOrEmpty(LayerName) && !string.IsNullOrEmpty(other.LayerName)) ||
                (!string.IsNullOrEmpty(LayerName) && string.IsNullOrEmpty(other.LayerName)))
            {
                return false;
            }
            else
            {
                return (FeatureAttribute != null ? System.Data.DataRowComparer.Default.Equals(FeatureAttribute, other.FeatureAttribute) : true)
                    && (Coordinates != null ? Coordinates.SequenceEqual(other.Coordinates) : true)
                    && (!string.IsNullOrEmpty(LayerName) ? string.CompareOrdinal(LayerName, other.LayerName) == 0 : true);
            }
        }

        public override int GetHashCode()
        {
            return FeatureAttribute.GetHashCode()
                ^ Coordinates.GetHashCode()
                ^ LayerName.GetHashCode();
        }

        public object Clone()
        {
            FeatureObject feature = new FeatureObject();
            //20160810WMW添加
            if (LayerName != null)
                feature.LayerName = LayerName.Clone() as string;

            foreach (var item in Coordinates)
                feature.Coordinates.Add(item);

            feature.FeatureAttribute = FeatureAttribute.Table.Copy().NewRow();
            for (int i = 0; i < FeatureAttribute.ItemArray.Count(); i++)
            {
                feature.FeatureAttribute[feature.FeatureAttribute.Table.Columns[i].ColumnName] = FeatureAttribute.ItemArray[i];
            }

            return feature;
        }
        #endregion
    }
}
