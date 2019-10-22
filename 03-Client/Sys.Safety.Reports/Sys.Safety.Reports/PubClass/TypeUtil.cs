using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;

namespace Sys.Safety.Reports
{
    public class TypeUtil
    {
        private TypeUtil()
        {

        }
        public static string ToString(object objValue)
        {
            try
            {
                return System.Convert.ToString(objValue);
            }
            catch
            {
                return "";
            }
        }
        public static double ToDouble(object objValue)
        {
            try
            {
                return System.Convert.ToDouble(objValue);
            }
            catch
            {
                return 0;
            }
        }
        public static decimal ToDecimal(object objValue)
        {
            try
            {
                return System.Convert.ToDecimal(objValue);
            }
            catch
            {
                return 0;
            }
        }
        public static decimal ToAmount(object objValue)
        {
            decimal result = ToDecimal(objValue);
            return Math.Round(result, 2);
        }
        public static int ToInt(object objValue)
        {
            try
            {
                return System.Convert.ToInt32(objValue);
            }
            catch
            {
                return 0;
            }
        }
        public static byte ToByte(object objValue)
        {
            try
            {
                return System.Convert.ToByte(objValue);
            }
            catch
            {
                return 0;
            }
        }
        public static DateTime ToDateTime(object objValue)
        {
            try
            {
                if (DateTime.Parse(objValue.ToString()) < DateTime.Parse("1900-1-1"))
                    //return DateTime.Now;
                    return DateTime.Parse("1900-1-1");
                else
                    return DateTime.Parse(objValue.ToString());
            }
            catch
            {
                //return DateTime.Now;
                return DateTime.Parse("1900-1-1");
            }
        }
        public static bool ToBool(object objValue)
        {
            try
            {
                return Convert.ToBoolean(objValue);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 转换金额为人民币大写
        /// </summary>
        /// <param name="n">Int64 金额</param>
        /// <returns>人民币大写</returns>        
        public static string ToRMB(double dn)
        {
            string[] strN = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string[] strC = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
            string[] strA = { "", "圆", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟", "万亿", "拾", "佰", "仟", "亿亿", };
            int[] nLoc = { 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };

            string strFrom = "";
            string strTo = "";
            string strChar;
            int m, mLast = -1, nCount = 0;

            if (strFrom.Length > strA.GetUpperBound(0) - 1) return "***拜托，这么多钱还需要数吗***";

            if (dn < 0)
            {
                dn *= -1;
                strTo = "负";
            }

            Int64 n1 = (Int64)dn;                   // 元
            strFrom = n1.ToString();

            for (int i = strFrom.Length; i > 0; i--)
            {
                strChar = strFrom.Substring(strFrom.Length - i, 1);
                m = Convert.ToInt32(strChar);
                if (m == 0)
                {
                    // 连续为０时需要补齐中间单位,且只补第一次
                    if (nLoc[i] > 0 && nCount == 0 && strFrom.Length > 1)
                    {
                        strTo = strTo + strA[i];
                        nCount++;
                    }
                }
                else
                {
                    // 补０
                    if (mLast == 0)
                    {
                        strTo = strTo + strC[0];
                    }

                    // 数字转换为大写
                    strTo = strTo + strC[m];
                    // 补足单位
                    strTo = strTo + strA[i];
                    nCount = 0;
                }
                mLast = m;
            }

            Int64 n2 = ((Int64)(dn * 100)) % 100;   // 角分
            Int64 n3 = n2 / 10;                     // 角
            Int64 n4 = n2 % 10;                     // 分
            string s2 = "";

            if (n4 > 0)
            {
                s2 = strC[n4] + "分";
                if (n3 > 0)
                {
                    s2 = strC[n3] + "角" + s2;
                }
            }
            else
            {
                if (n3 > 0)
                {
                    s2 = strC[n3] + "角";
                }
            }
            strTo = strTo + s2;

            if (strTo == "") strTo = strC[0];                   // 全0显示为零
            else if (s2 == "") strTo = strTo + "整";            // 无角分显示整
            return strTo;
        }

        /// <summary>
        /// 是否是数字类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool isNumericType(string type)
        {
            switch (type)
            {
                case "Decimal":
                case "Double":
                case "Int16":
                case "Int32":
                case "Int64":
                case "Single":
                case "UInt16":
                case "UInt32":
                case "UInt64":
                    return true;
                case "Boolean":
                case "Byte":
                case "Char":
                case "DateTime":
                case "SByte":
                case "String":
                case "TimeSpan":
                    return false;
            }
            return false;
        }
        /// <summary>
        /// 转换为指定类型的数据
        /// </summary>
        /// <param name="objValue"></param>
        /// <param name="toType"></param>
        /// <returns></returns>
        public static object ToRefType(object objValue, System.Type toType)
        {
            if (toType.Equals(typeof(bool)))
            {
                return ToBool(objValue);
            }
            else if (toType.Equals(typeof(DateTime)))
            {
                return ToDateTime(objValue);
            }
            else if (toType.Equals(typeof(int)))
            {
                return ToInt(objValue);
            }
            else if (toType.Equals(typeof(byte)))
            {
                return ToByte(objValue);
            }
            else if (toType.Equals(typeof(decimal)))
            {
                return ToDecimal(objValue);
            }
            else
            {
                return ToString(objValue);
            }
        }
        ///// <summary>
        ///// 将值对象列表转换为DataTable
        ///// 如果vos为空,则返回空
        ///// </summary>
        ///// <param name="voList"></param>
        ///// <returns></returns>
        //public static DataTable ToDataTable<T>(IList<T> vos)
        //{
        //    if (vos == null || vos.Count == 0)
        //    {
        //        return null;
        //    }
        //    Type voType = vos[0].GetType();
        //    DataTable dt = new DataTable(voType.Name);
        //    for (int i=0; i < vos.Count; i++)
        //    {
        //        voType = vos[i].GetType();          //构造数据表                
        //        PropertyInfo[] properties = voType.GetProperties();
        //        IDictionary<string, PropertyInfo> voProperties = new Dictionary<string, PropertyInfo>();
        //        //构造数据列
        //        foreach (PropertyInfo property in properties)
        //        {
        //            if (!IsDuplicateField(dt,property .Name ))
        //            {

        //                DataColumn col = new DataColumn(property.Name);
        //                col.DataType = property.PropertyType;
        //                col.Caption = property.Name;
        //                dt.Columns.Add(col);
        //                voProperties.Add(property.Name, property);
        //            }
        //        }
        //        //读取记录数据
        //        foreach (object obj in vos)
        //        {
        //            if (!IsDuplicateField(dt,obj.ToString ()))
        //            {
        //                DataRow dr = dt.NewRow();
        //                foreach (PropertyInfo pro in voProperties.Values)
        //                {
        //                    dr[pro.Name] = pro.GetValue(obj, null);
        //                }
        //                dt.Rows.Add(dr);
        //            }
        //        }
        //    }
        //    return dt;
        //}

        private static bool IsDuplicateField(DataTable dt, string fieldName)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ToString() == fieldName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 将值对象列表转换为DataTable
        /// 如果vos为空,则返回空
        /// </summary>
        /// <param name="voList"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(IList<T> vos)
        {
            Type voType = typeof(T);
            //构造数据表
            DataTable dt = new DataTable(voType.Name);
            PropertyInfo[] properties = voType.GetProperties();
            IDictionary<string, PropertyInfo> voProperties = new Dictionary<string, PropertyInfo>();
            //构造数据列
            foreach (PropertyInfo property in properties)
            {
                DataColumn col = new DataColumn(property.Name);
                col.DataType = property.PropertyType;
                col.Caption = property.Name;
                dt.Columns.Add(col);
                voProperties.Add(property.Name, property);
            }
            if (vos == null || vos.Count == 0)
            {
                return dt;
            }
            //读取记录数据
            foreach (object obj in vos)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo pro in voProperties.Values)
                {
                    dr[pro.Name] = pro.GetValue(obj, null);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// 将IList转换为DataTable
        /// </summary>
        /// <param name="vos"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(IList vos)
        {
            if (vos == null || vos.Count == 0)
            {
                return null;
            }
            IList[] vosMore = new IList[vos.Count]; ;
            int i = 0;
            try
            {
                foreach (IList v in vos)
                {
                    vosMore[i++] = v;
                }
                return ToDataTableFromMore(vosMore);

            }
            catch
            {
                return ToDataTableFromOne(vos);
            }

        }
        /// <summary>
        /// 将单实体值对象列表转换为DataTable
        /// 如果vos为空,则返回空
        /// </summary>
        /// <param name="vos"></param>
        /// <returns></returns>
        private static DataTable ToDataTableFromOne(IList vos)
        {
            if (vos == null || vos.Count == 0)
            {
                return null;
            }
            Type voType = vos[0].GetType();
            //构造数据表
            DataTable dt = new DataTable(voType.Name);
            PropertyInfo[] properties = voType.GetProperties();
            IDictionary<string, PropertyInfo> voProperties = new Dictionary<string, PropertyInfo>();
            //构造数据列
            foreach (PropertyInfo property in properties)
            {
                DataColumn col = new DataColumn(property.Name);
                col.DataType = property.PropertyType;
                col.Caption = property.Name;
                dt.Columns.Add(col);
                voProperties.Add(property.Name, property);
            }
            //读取记录数据
            foreach (object obj in vos)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo pro in voProperties.Values)
                {
                    dr[pro.Name] = pro.GetValue(obj, null);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// 将多实体值对象列表转换为DataTable
        /// 如果vos为空,则返回空
        /// </summary>
        /// <param name="vos"></param>
        /// <returns></returns>
        private static DataTable ToDataTableFromMore(IList[] vos)
        {
            if (vos == null)
            {
                return null;
            }
            Type voType = vos[0].GetType();
            DataTable dt = new DataTable(voType.Name);
            IDictionary<string, PropertyInfo> voProperties = new Dictionary<string, PropertyInfo>();

            for (int i = 0; i < vos[0].Count; i++)
            {
                voType = vos[0][i].GetType();

                //构造数据表                
                PropertyInfo[] properties = voType.GetProperties();
                //构造数据列
                foreach (PropertyInfo property in properties)
                {
                    if (!IsDuplicateField(dt, property.Name))
                    {
                        DataColumn col = new DataColumn(property.Name);
                        col.DataType = property.PropertyType;
                        col.Caption = property.Name;
                        dt.Columns.Add(col);
                        voProperties.Add(property.Name, property);
                    }
                }
            }
            //读取记录数据
            foreach (object[] vosRecord in vos)
            {
                DataRow dr = dt.NewRow();
                // 多表
                foreach (object voOne in vosRecord)
                {
                    voType = voOne.GetType();
                    PropertyInfo[] properties = voType.GetProperties();
                    // 表数据列
                    foreach (PropertyInfo property in properties)
                    {
                        dr[property.Name] = property.GetValue(voOne, null);
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static byte[] GetThumbnailBtyeArr(Image originalImage, int width, int height, string mode)
        {
            Image bitmap = null;
            Graphics g = null;
            byte[] buffer = new byte[0];
            try
            {
                int towidth = width;
                int toheight = height;
                int x = 0;
                int y = 0;
                int ow = originalImage.Width;
                int oh = originalImage.Height;
                switch (mode)
                {
                    case "HW "://指定高宽缩放（可能变形）   
                        break;
                    case "W "://指定宽，高按比例   
                        toheight = originalImage.Height * width / originalImage.Width;
                        break;
                    case "H "://指定高，宽按比例 
                        towidth = originalImage.Width * height / originalImage.Height;
                        break;
                    case "Cut "://指定高宽裁减（不变形）   
                        if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                        {
                            oh = originalImage.Height;
                            ow = originalImage.Height * towidth / toheight;
                            y = 0;
                            x = (originalImage.Width - ow) / 2;
                        }
                        else
                        {
                            ow = originalImage.Width;
                            oh = originalImage.Width * height / towidth;
                            x = 0;
                            y = (originalImage.Height - oh) / 2;
                        }
                        break;
                    default:
                        break;
                }

                bitmap = new System.Drawing.Bitmap(towidth, toheight);//新建一个bmp图片 
                g = System.Drawing.Graphics.FromImage(bitmap);//新建一个画板 

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;//设置高质量插值法 
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;   //设置高质量,低速度呈现平滑程度 
                g.Clear(Color.Transparent);   //清空画布并以透明背景色填充 
                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);//在指定位置并且按指定大小绘制原图片的指定部分  

                Stream stream = new MemoryStream();
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Gif);
                if (stream.Length > 0)
                {
                    buffer = new byte[stream.Length];
                    int len = (int)stream.Length;
                    stream.Seek(0, SeekOrigin.Begin);
                    int readLen = stream.Read(buffer, 0, len);
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }

            return buffer;
        }

        /// <summary>
        /// 检查级次编码合法性
        /// 编码名为A-Z,a-z,0-9和下画线( _ )，位置和长度不限
        /// 减号为级次编码的分隔线 
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static bool IsValidLevelCode(string strText)
        {
            Regex regex = new Regex(@"^([A-Za-z0-9_]+([-]{1}[A-Za-z0-9_]+)*)$");
            // Regex regex = new Regex(@"^([\w]+([-]{1}\w+)*)$");
            return regex.IsMatch(strText);
        }

        /// <summary>
        /// 检查非级次编码合法性
        /// 编码名为A-Z,a-z,0-9和下画线( _ )，位置和长度不限
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static bool IsValidCode(string strText)
        {
            Regex regex = new Regex(@"^([A-Za-z0-9_]+)$");
            // Regex regex = new Regex(@"^([\w]+)$");
            return regex.IsMatch(strText);
        }


        /// <summary>
        /// 移除泛型里相同的项
        /// </summary>
        /// <param name="list"></param>
        /// <returns>List</returns>
        public static List<int> RemoveSameItem(List<int> list)
        {
            int ItemValue = 0;
            for (int j = 0; j < list.Count; j++)
            {
                ItemValue = list[j];

                for (int m = j + 1; m < list.Count; m++)
                {
                    if (ItemValue == list[m])
                    {
                        list.Remove(list[m]);
                        j = -1; break;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="sPath"></param>
        public static void SerializeSys(object obj, string sPath)
        {
            System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();
            ns.Add("", "");
            System.Xml.Serialization.XmlSerializer xmlSerialization = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            using (Stream stream = new FileStream(sPath, FileMode.Create, FileAccess.Write))
            {
                xmlSerialization.Serialize(stream, obj);
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="sPath"></param>
        public static object DeSerializeSys(object obj, string sPath)
        {
            //System.Xml.Serialization.XmlSerializer xmlSerialization = new System.Xml.Serialization.XmlSerializer(typeof(Protocol));
            System.Xml.Serialization.XmlSerializer xmlSerialization = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            if (File.Exists(sPath))
            {
                //解密配置文件
                //DecryptConfigFile();

                Stream s = new FileStream(sPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                obj = xmlSerialization.Deserialize(s);
                //c = xmlSerialization.Deserialize(s) as Config;
                s.Close();

                //EncryptConfigFile();
            }
            return obj;
        }


        /// <summary>
        /// 将DataTable里面增加一列序号列,主要用于报表打印的时候需要显示序号列
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable AddNumberToDataTable(DataTable dt)
        {
            DataTable dtCopty = dt.Copy();
            if (dtCopty.Columns.Contains("colNumber"))
                dtCopty.Columns.Remove("colNumber");
            DataColumn dc = new DataColumn();
            dc.ColumnName = "colNumber";
            dc.DataType = typeof(int);
            dtCopty.Columns.Add(dc);

            for (int i = 0; i < dtCopty.Rows.Count; i++)
            {
                dtCopty.Rows[i]["colNumber"] = i + 1;
            }

            return dtCopty;
        }


        /// <summary>
        /// DataTable去掉重复行数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable DeleteRepeateRow(DataTable dt)
        {
            DataTable dtCopty = dt.Copy();
            ArrayList arraylist = new ArrayList();
            foreach (DataColumn column in dtCopty.Columns)
            {
                string strColumnName = TypeUtil.ToString(column.ColumnName);
                arraylist.Add(strColumnName);
            }
            string[] arrString = (string[])arraylist.ToArray(typeof(string));
            DataView dv = new DataView(dtCopty);//虚拟视图
            DataTable dtReturn = dv.ToTable(true, arrString);

            //以下代码只针对模拟量馈电异常日班报表有效(因为要排除一个测点有的数据有断电区域，有些数据没有断电区域的行，如果一个测点只有一条数据且是无断电区域，则不删除)
            DataRow[] rows = dtReturn.Select("ViewMLLDDDayReport1_wz = '无断电区域'");
            foreach (DataRow row in rows)
            {
                string strpoint = TypeUtil.ToString(row["ViewMLLDDDayReport1_point"]);
                DataRow[] rowa = dtReturn.Select("ViewMLLDDDayReport1_wz <> '无断电区域' and ViewMLLDDDayReport1_point='" + strpoint + "'");
                if (rowa.Length > 0)
                {
                    row.Delete();
                }
            }


            foreach (DataRow row in dtReturn.Rows)
            {
                string strwz = "ViewMLLDDDayReport1_wz";
            }


            return dtReturn;




        }



        /// <summary>
        /// 得到一个字符串在另一个字符合串出现的所有位置索引
        /// </summary>
        /// <param name="str"></param>
        /// <param name="substr"></param>
        /// <param name="StartPos"></param>
        /// <returns></returns>
        public  static int[] GetSubStrCountInStr(String str, String substr, int StartPos)
        {
            int foundPos = -1;
            int count = 0;
            List<int> foundItems = new List<int>();
            do
            {
                foundPos = str.IndexOf(substr, StartPos);
                if (foundPos > -1)
                {
                    StartPos = foundPos + 1;
                    count++;
                    foundItems.Add(foundPos);
                }
            } while (foundPos > -1 && StartPos < str.Length);

            return ((int[])foundItems.ToArray());
        }

    }
}
