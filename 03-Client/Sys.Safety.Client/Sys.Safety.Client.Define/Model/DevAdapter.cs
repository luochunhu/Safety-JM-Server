using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Client.Define.Sensor;
using System.Data;
using System.Reflection;

namespace Sys.Safety.Client.Define.Model
{
    public class DevAdapter
    {
        /// <summary>
        ///根据参数返回 设备控件
        /// </summary>
        /// <returns></returns>
        public static CuBase DevSensorAdapter(string arrPonint, uint SourceNum, int devID, int devProperID)
        {
            CuBase ret = new CuBase();
            switch (devProperID)
            {
                case 1:
                    //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                    //stopwatch.Restart();
                    ret = new CuAnalog(arrPonint, devID, SourceNum);
                    //stopwatch.Stop();
                    //Basic.Framework.Logging.LogHelper.Debug(stopwatch.ElapsedMilliseconds);
                    break;
                case 2:
                    //  System.Diagnostics.Stopwatch stopwatch1 = new System.Diagnostics.Stopwatch();
                    //stopwatch1.Restart();
                    ret = new CuDerail(arrPonint, devID, SourceNum);
                    // stopwatch1.Stop();
                    //Basic.Framework.Logging.LogHelper.Debug(stopwatch1.ElapsedMilliseconds);
                    break;
                case 3:
                    ret = new CuControl(arrPonint, devID, SourceNum);
                    break;
                //case 4:
                //    ret = new CuCumulative(arrPonint, devID, SourceNum);
                //    break;
                case 7:
                    ret = new RecognizerExtendProperty(arrPonint, devID, SourceNum);
                    break;
                default:
                    break;
            }
            return ret;

        }
        /// <summary>
        /// 根据参数返回 设备类型控件
        /// </summary>
        /// <returns></returns>
        public static CuBaseType DevTypeAdapter(int devID, int devProperID)
        {
            CuBaseType ret = new CuBaseType();
            switch (devProperID)
            {
                case 1:
                    ret = new CuAnalogType(devID);
                    break;
                case 2:
                    ret = new CuDerailType(devID);
                    break;
                case 3:
                    ret = new CuControlType(devID);
                    break;
                default:
                    break;
            }
            return ret;
        }


        /// <summary>
        /// 将泛类型集合List类转换成DataTable
        /// </summary>
        /// <param name="list">泛类型集合</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(IList<T> entitys)
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                return null;
            }
            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    return null;
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }
    }
}
