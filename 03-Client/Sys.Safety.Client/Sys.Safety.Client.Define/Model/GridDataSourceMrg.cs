using Basic.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Sys.Safety.Client.Define.Model
{
    /// <summary>
    /// GRID 栏目管理
    /// </summary>
    public class GridClumnsMrg
    {
        /// <summary>
        /// 获取分站GRID 栏目
        /// </summary>
        /// <returns></returns>
        public static List<DevExpress.XtraGrid.Columns.GridColumn> StationGridColumn()
        {
            List<DevExpress.XtraGrid.Columns.GridColumn> ret = new List<DevExpress.XtraGrid.Columns.GridColumn>();
            DevExpress.XtraGrid.Columns.GridColumn col;
            try
            {
                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "Tag";
                col.Caption = "类型";
                col.VisibleIndex = -1;
                col.Width = 0;
                col.Visible = false;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "Code";
                col.Caption = "编码";
                col.VisibleIndex = -1;
                col.Width = 0;
                col.Visible = false;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "fzh";
                col.Caption = "地址号";
                col.VisibleIndex = 1;
                col.Width = 20;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "wz";
                col.Caption = "安装位置";
                col.VisibleIndex = 2;
                col.Width = 100;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "DevName";
                col.Caption = "设备类型";
                col.VisibleIndex = 3;
                col.Width = 70;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "RunState";
                col.Caption = "状态";
                col.VisibleIndex = 4;
                col.Width = 30;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "CommType";
                col.Caption = "通讯方式";
                col.VisibleIndex = 5;
                col.Width = 30;
                ret.Add(col);

                for (int i = 0; i < ret.Count; i++)
                {
                    //ret[i].Visible = true;
                    ret[i].OptionsColumn.ReadOnly = true;
                    ret[i].OptionsColumn.AllowFocus = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return ret;
        }

        /// <summary>
        /// 获取传感器GRID 栏目
        /// </summary>
        /// <returns></returns>
        public static List<DevExpress.XtraGrid.Columns.GridColumn> SensorGridColumn()
        {
            List<DevExpress.XtraGrid.Columns.GridColumn> ret = new List<DevExpress.XtraGrid.Columns.GridColumn>();
            DevExpress.XtraGrid.Columns.GridColumn col;
            try
            {
                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "Tag";
                col.Caption = "类型";
                col.Width = 0;
                col.VisibleIndex = -1;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "Code";
                col.Caption = "编码";
                col.Width = 0;
                col.VisibleIndex = -1;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "kh";
                col.Caption = "通道号";
                col.Width = 30;
                col.VisibleIndex = 1;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "Point";
                col.Caption = "测点编号";
                col.VisibleIndex = 2;
                col.Width = 50;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "wz";
                col.Caption = "测点名称";
                col.VisibleIndex = 3;
                col.Width = 100;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "RunState";
                col.Caption = "状态";
                col.VisibleIndex = 4;
                col.Width = 20;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "DevName";
                col.Caption = "设备类型";
                col.VisibleIndex = 5;
                col.Width = 50;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "z2";
                col.Caption = "报警值/0态";
                col.VisibleIndex = 6;
                col.Width = 50;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "z3";
                col.Caption = "断电值/1态";
                col.VisibleIndex = 7;
                col.Width = 50;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "z4";
                col.Caption = "复电值/2态";
                col.VisibleIndex = 8;
                col.Width = 50;
                ret.Add(col);

                for (int i = 0; i < ret.Count; i++)
                {
                    ret[i].OptionsColumn.ReadOnly = true;
                    ret[i].OptionsColumn.AllowFocus = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return ret;
        }

        /// <summary>
        /// 获取IP GRID 栏目
        /// </summary>
        /// <returns></returns>
        public static List<DevExpress.XtraGrid.Columns.GridColumn> IPGridColumn()
        {
            List<DevExpress.XtraGrid.Columns.GridColumn> ret = new List<DevExpress.XtraGrid.Columns.GridColumn>();
            DevExpress.XtraGrid.Columns.GridColumn col;
            try
            {
                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "Tag";
                col.Caption = "类型";
                col.VisibleIndex = -1;
                col.Width = 0;
                col.Visible = false;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "Code";
                col.Caption = "编码";
                col.VisibleIndex = -1;
                col.Width = 0;
                col.Visible = false;
                ret.Add(col);


                //col = new DevExpress.XtraGrid.Columns.GridColumn();
                //col.FieldName = "IP";
                //col.Caption = "IP";
                //col.VisibleIndex = 1;
                //col.Width = 60;
                //ret.Add(col);


                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "IPdz";
                col.Caption = "IP编号";
                col.VisibleIndex = 1;
                col.Visible = false;
                col.Width = 60;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "IP";
                col.Caption = "IP";
                col.VisibleIndex = 1;
                col.Width = 60;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "MAC";
                col.Caption = "MAC";
                col.VisibleIndex = 2;
                col.Width = 60;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "NetID";
                col.Caption = "连接号";
                col.VisibleIndex = 3;
                col.Visible = false;
                col.Width = 60;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "BingingStations";
                col.Caption = "绑定分站";
                col.VisibleIndex = 4;
                col.Width = 70;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "wz";
                col.Caption = "安装位置";               
                col.VisibleIndex = 5;
                col.Width = 100;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "TransportaCommFlag";
                col.Caption = "透传标记";
                col.Visible = false;
                col.VisibleIndex = -1;
                col.Width = 50;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "SwitchesMac";
                col.Caption = "所属交换机MAC";
                col.Visible = false;
                col.VisibleIndex = -1;
                col.Width = 80;
                ret.Add(col);

                for (int i = 0; i < ret.Count; i++)
                {
                    ret[i].OptionsColumn.ReadOnly = true;
                    ret[i].OptionsColumn.AllowFocus = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return ret;
        }

        /// <summary>
        /// 获取分站类型 GRID 栏目
        /// </summary>
        /// <returns></returns>
        public static List<DevExpress.XtraGrid.Columns.GridColumn> StationTyepGridColumn()
        {
            List<DevExpress.XtraGrid.Columns.GridColumn> ret = new List<DevExpress.XtraGrid.Columns.GridColumn>();
            DevExpress.XtraGrid.Columns.GridColumn col;
            try
            {
                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "Tag";
                col.Caption = "类型";
                col.VisibleIndex = -1;
                col.Width = 0;
                col.Visible = false;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "Code";
                col.Caption = "编码";
                col.VisibleIndex = -1;
                col.Width = 0;
                col.Visible = false;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "DevID";
                col.Caption = "设备编号";
                col.VisibleIndex = 1;
                col.Width = 30;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "DevName";
                col.Caption = "名称";
                col.VisibleIndex = 2;
                col.Width = 100;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "DriverID";
                col.Caption = "驱动编号";
                col.VisibleIndex = 3;
                col.Width = 50;
                ret.Add(col);

                for (int i = 0; i < ret.Count; i++)
                {
                    ret[i].OptionsColumn.ReadOnly = true;
                    ret[i].OptionsColumn.AllowFocus = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return ret;
        }

        /// <summary>
        /// 获取传感器类型 GRID 栏目
        /// </summary>
        /// <returns></returns>
        public static List<DevExpress.XtraGrid.Columns.GridColumn> SensorTyepGridColumn()
        {
            List<DevExpress.XtraGrid.Columns.GridColumn> ret = new List<DevExpress.XtraGrid.Columns.GridColumn>();
            DevExpress.XtraGrid.Columns.GridColumn col;
            try
            {
                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "Tag";
                col.Caption = "类型";
                col.VisibleIndex = -1;
                col.Width = 0;
                col.Visible = false;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "Code";
                col.Caption = "编码";
                col.VisibleIndex = -1;
                col.Width = 0;
                col.Visible = false;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "DevID";
                col.Caption = "设备编号";
                col.VisibleIndex = 1;
                col.Width = 30;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "DevName";
                col.Caption = "名称";
                col.VisibleIndex = 2;
                col.Width = 100;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "DevClassName";
                col.Caption = "设备种类";
                col.VisibleIndex = 3;
                col.Width = 50;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "DevPropertyName";
                col.Caption = "设备性质";
                col.VisibleIndex = 4;
                col.Width = 50;
                ret.Add(col);

                for (int i = 0; i < ret.Count; i++)
                {
                    ret[i].OptionsColumn.ReadOnly = true;
                    ret[i].OptionsColumn.AllowFocus = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return ret;
        }

        /// <summary>
        /// 获取安装位置 GRID 栏目
        /// </summary>
        /// <returns></returns>
        public static List<DevExpress.XtraGrid.Columns.GridColumn> WZGridColumn()
        {
            List<DevExpress.XtraGrid.Columns.GridColumn> ret = new List<DevExpress.XtraGrid.Columns.GridColumn>();
            DevExpress.XtraGrid.Columns.GridColumn col;
            try
            {
                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "Tag";
                col.Caption = "类型";
                col.VisibleIndex = -1;
                col.Width = 0;
                col.Visible = false;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "Code";
                col.Caption = "编码";
                col.VisibleIndex = -1;
                col.Width = 0;
                col.Visible = false;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "WZID";
                col.Caption = "位置编号";
                col.VisibleIndex = 1;
                col.Width = 30;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "WZ";
                col.Caption = "名称";
                col.VisibleIndex = 2;
                col.Width = 100;
                ret.Add(col);

                col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.FieldName = "UseState";
                col.Caption = "使用状态";
                col.VisibleIndex = 3;
                col.Width = 100;
                ret.Add(col);

                for (int i = 0; i < ret.Count; i++)
                {
                    ret[i].OptionsColumn.ReadOnly = true;
                    ret[i].OptionsColumn.AllowFocus = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return ret;
        }
    }

    /// <summary>
    /// TreeList数据源
    /// </summary>
    public class GridSource
    {
        /// <summary>
        /// GRID分站数据源
        /// </summary>
        public static List<GridStationItem> GridStationSource = new List<GridStationItem>();

        /// <summary>
        /// GRID传感器数据源
        /// </summary>
        public static List<GridSensorItem> GridSensorSource = new List<GridSensorItem>();

        /// <summary>
        /// GRID IP 数据源
        /// </summary>
        public static List<GridIPItem> GridIPSource = new List<GridIPItem>();

        /// <summary>
        /// GRID分站类型数据源
        /// </summary>
        public static List<GridStationTypeItem> GridStationTypeSource = new List<GridStationTypeItem>();

        /// <summary>
        /// GRID传感器类型数据源
        /// </summary>
        public static List<GridSensorTypeItem> GridSensorTypeSource = new List<GridSensorTypeItem>();

        /// <summary>
        /// GRID安装位置
        /// </summary>
        public static List<GridWZItem> GridWZSource = new List<GridWZItem>();
    }

    /// <summary>
    /// 分站对象
    /// </summary>
    [Serializable]
    public class GridStationItem
    {
        private string _RunState;
        private string _CommType;
        /// <summary>
        /// 地址号
        /// </summary>
        public string fzh { get; set; }

        /// <summary>
        /// 测点编号
        /// </summary>
        public string Point { get; set; }

        /// <summary>
        /// 安装位置
        /// </summary>
        public string wz { get; set; }

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string DevName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string RunState
        {
            get { return _RunState; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _RunState = value;
                    return;
                }
                if ((Convert.ToByte(value) & 0x2) == 0x2)
                {
                    _RunState = "休眠";
                }
                else if ((Convert.ToByte(value) & 0x4) == 0x4)
                {
                    _RunState = "检修";
                }
                else
                {
                    _RunState = "运行";
                }
            }
        }
        /// <summary>
        /// 安装位置
        /// </summary>
        public int ComNum { get; set; }

        /// <summary>
        /// 通讯方式
        /// </summary>
        public string CommType
        {
            get { return _CommType; }
            set
            {
                if (string.IsNullOrEmpty(value) && ComNum > 0)
                {
                    _CommType = "串口";
                }
                else if (!string.IsNullOrEmpty(value) && ComNum <= 0)
                {
                    _CommType = "网口";
                }
                else if (string.IsNullOrEmpty(value) && ComNum <= 0)
                {
                    _CommType = "未通讯";
                }
                else
                {
                    _CommType = "网口";
                }
            }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 列状态，1新增；2删除
        /// </summary>
        public string RowStatus { get; set; }
    }

    /// <summary>
    /// 传感器对象
    /// </summary>
    [Serializable]
    public class GridSensorItem
    {
        private string _RunState;
        /// <summary>
        /// 通道号
        /// </summary>
        public string kh { get; set; }

        /// <summary>
        /// 测点编号
        /// </summary>
        public string Point { get; set; }

        /// <summary>
        /// 测点名称
        /// </summary>
        public string wz { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string RunState
        {
            get { return _RunState; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _RunState = value;
                    return;
                }
                if ((Convert.ToByte(value) & 0x8) == 0x8)
                {
                    _RunState = "标校";
                }
                else if ((Convert.ToByte(value) & 0x4) == 0x4)
                {
                    _RunState = "检修";
                }
                else if ((Convert.ToByte(value) & 0x2) == 0x2)
                {
                    _RunState = "休眠";
                }
                else
                {
                    _RunState = "运行";
                }
            }
        }
        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string DevName { get; set; }

        /// <summary>
        /// 报警值/0态
        /// </summary>
        public string z2 { get; set; }

        /// <summary>
        /// 断电值/1态
        /// </summary>
        public string z3 { get; set; }

        /// <summary>
        /// 复电值/2态
        /// </summary>
        public string z4 { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 列状态，1新增；2删除
        /// </summary>
        public string RowStatus { get; set; }
    }

    /// <summary>
    /// IP模块对象
    /// </summary>
    [Serializable]
    public class GridIPItem
    {
        /// <summary>
        /// 交换名称   20170112
        /// </summary>
        public string jhjname { get; set; }

        /// <summary>
        /// ip地址  20170112
        /// </summary>
        public string IPdz { get; set; }

        /// <summary>
        /// 测点编号
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 测点名称
        /// </summary>
        public string MAC { get; set; }

        /// <summary>
        /// 报警类型
        /// </summary>
        public string BingingStations { get; set; }

        /// <summary>
        /// 报警类型名称
        /// </summary>
        public string wz { get; set; }

        /// <summary>
        /// 透明传输标记
        /// </summary>
        public string TransportaCommFlag { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 列状态，1新增；2删除
        /// </summary>
        public string RowStatus { get; set; }
        /// <summary>
        /// 连接号
        /// </summary>
        public string NetID { get; set; }
        /// <summary>
        /// 所属交换机MAC地址
        /// </summary>
        public string SwitchesMac { get; set; }

    }

    /// <summary>
    /// 分站类型对象
    /// </summary>
    [Serializable]
    public class GridStationTypeItem
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public string DevID { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DevName { get; set; }
        /// <summary>
        /// 驱动编号
        /// </summary>
        public string DriverID { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 列状态，1新增；2删除
        /// </summary>
        public string RowStatus { get; set; }
    }

    /// <summary>
    /// 传感器类型对象
    /// </summary>
    [Serializable]
    public class GridSensorTypeItem
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public string DevID { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DevName { get; set; }
        /// <summary>
        /// 设备种类名称
        /// </summary>
        public string DevClassName { get; set; }
        /// <summary>
        /// 设备性质名称
        /// </summary>
        public string DevPropertyName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 列状态，1新增；2删除
        /// </summary>
        public string RowStatus { get; set; }
    }

    /// <summary>
    /// 安装位置对象
    /// </summary>
    [Serializable]
    public class GridWZItem
    {
        /// <summary>
        /// 位置编号
        /// </summary>
        public string WZID { get; set; }
        /// <summary>
        /// 位置名称
        /// </summary>
        public string WZ { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 列状态，1新增；2删除
        /// </summary>
        public string RowStatus { get; set; }
        /// <summary>
        /// 使用状态
        /// </summary>
        public string UseState { get; set; }
    }
}
