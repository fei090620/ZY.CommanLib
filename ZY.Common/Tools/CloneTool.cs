using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace ZY.Common.Tools
{
    /// <summary>
    /// 深度克隆类型
    /// </summary>
    public enum DeepCloneType
    {
        Serialize,  //序列化
        Refactor,   //反射实例化
        DataRow,    //DataRow
    }

    /// <summary>
    /// Clone工具类
    /// </summary>
    public static class CloneTool
    {
        #region DeepClone
        /// <summary>
        /// 深度复制
        /// </summary>
        public static object DeepClone(object obj, DeepCloneType type)
        {
            if (obj == null)
                return null;

            switch (type)
            {
                case DeepCloneType.Serialize:
                    return DeepClonebySerialize(obj);
                case DeepCloneType.Refactor:
                    return DeepClonebyRefactor(obj);
                case DeepCloneType.DataRow:
                    return DeepClonebyDataRow(obj as DataRow);
                default:
                    break;
            }

            return null;
        }

        private static object DeepClonebySerialize(object obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, obj);
            memoryStream.Position = 0;
            return formatter.Deserialize(memoryStream);
        }

        private static object DeepClonebyRefactor(object obj)
        {
            Object targetDeepCopyObj;
            Type targetType = obj.GetType();
            //值类型  
            if (targetType.IsValueType == true)
            {
                targetDeepCopyObj = obj;
            }
            //引用类型   
            else
            {
                targetDeepCopyObj = System.Activator.CreateInstance(targetType);   //创建引用对象   
                System.Reflection.MemberInfo[] memberCollection = obj.GetType().GetMembers();

                foreach (System.Reflection.MemberInfo member in memberCollection)
                {
                    if (member.MemberType == System.Reflection.MemberTypes.Field)
                    {
                        System.Reflection.FieldInfo field = (System.Reflection.FieldInfo)member;
                        Object fieldValue = field.GetValue(obj);
                        if (fieldValue is ICloneable)
                        {
                            field.SetValue(targetDeepCopyObj, (fieldValue as ICloneable).Clone());
                        }
                        else
                        {
                            field.SetValue(targetDeepCopyObj, DeepClonebyRefactor(fieldValue));
                        }

                    }
                    else if (member.MemberType == System.Reflection.MemberTypes.Property)
                    {
                        System.Reflection.PropertyInfo myProperty = (System.Reflection.PropertyInfo)member;
                        MethodInfo info = myProperty.GetSetMethod(false);
                        if (info != null)
                        {
                            object propertyValue = myProperty.GetValue(obj, null);
                            if (propertyValue is ICloneable)
                            {
                                myProperty.SetValue(targetDeepCopyObj, (propertyValue as ICloneable).Clone(), null);
                            }
                            else
                            {
                                myProperty.SetValue(targetDeepCopyObj, DeepClonebyRefactor(propertyValue), null);
                            }
                        }
                    }
                }
            }

            return targetDeepCopyObj;
        }

        private static DataRow DeepClonebyDataRow(DataRow obj)
        {
            DataRow row = obj.Table.Clone().NewRow();
            foreach (DataColumn item in obj.Table.Columns)
                row[item.ColumnName] = obj[item.ColumnName];

            return row;
        }
        #endregion
    }
}
