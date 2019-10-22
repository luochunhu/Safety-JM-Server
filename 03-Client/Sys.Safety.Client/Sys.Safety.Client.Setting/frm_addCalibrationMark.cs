using DevExpress.XtraEditors;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Position;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.Setting
{
    public partial class frm_addCalibrationMark : XtraForm
    {
        public frm_addCalibrationMark()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mark"></param>
        /// <param name="type">0 增加mark,1 移除mark</param>
        private void UpdateJC_WZ(string mark, int type)
        {
            try
            {
                IPositionService service_wz = ServiceFactory.Create<IPositionService>();
                var result_wz = service_wz.GetAllPositionCache();

                IPointDefineService service_def = ServiceFactory.Create<IPointDefineService>();
                var result_def = service_def.GetAllPointDefineCache();



                if (result_wz.IsSuccess && result_wz.Data != null
                    && result_def.IsSuccess && result_def.Data != null)
                {
                    List<Jc_WzInfo> items_wz = result_wz.Data;
                    Jc_WzInfo item_wz;
                    List<Jc_DefInfo> items_def = result_def.Data;

                    Dictionary<string, Jc_WzInfo> UpdateItems_wz = new Dictionary<string, Jc_WzInfo>();
                    Dictionary<string, Dictionary<string, object>> UpdateItems_def = new Dictionary<string, Dictionary<string, object>>();
                    Dictionary<string, object> UpdateItem_def;

                    foreach (Jc_DefInfo def in items_def)
                    {
                        if (def.DevPropertyID != 1)
                        {
                            continue;
                        }
                        if (def.DevClassID == 13)
                        {
                            //流量不修改
                            continue;
                        }
                        if (UpdateItems_wz.ContainsKey(def.Wzid))
                        {
                            //一个安装位置只修改一次
                            continue;
                        }
                        item_wz = items_wz.FirstOrDefault(a => a.WzID == def.Wzid);
                        if (item_wz != null)
                        {
                            if (type == 0)
                            {
                                //添加mark
                                if (!IsHaveMark(item_wz.Wz, mark))
                                {
                                    item_wz.Wz += mark;
                                    UpdateItems_wz.Add(item_wz.WzID, item_wz);

                                    UpdateItem_def = new Dictionary<string, object>();
                                    def.Wz = item_wz.Wz;
                                    UpdateItem_def.Add("Wz", def.Wz);
                                    UpdateItems_def.Add(def.PointID, UpdateItem_def);
                                }
                            }
                            else
                            {
                                //去掉mark
                                if (IsHaveMark(item_wz.Wz, mark))
                                {
                                    item_wz.Wz = item_wz.Wz.Substring(0, item_wz.Wz.Length - mark.Length);
                                    UpdateItems_wz.Add(item_wz.WzID, item_wz);

                                    UpdateItem_def = new Dictionary<string, object>();
                                    def.Wz = item_wz.Wz;
                                    UpdateItem_def.Add("Wz", def.Wz);
                                    UpdateItems_def.Add(def.PointID, UpdateItem_def);
                                }
                            }
                        }
                    }
                    if (UpdateItems_wz.Count > 0)
                    {
                        //更新安装位置到数据库和缓存
                        PositionsRequest request = new PositionsRequest();
                        request.PositionsInfo = UpdateItems_wz.Values.ToList();
                        service_wz.UpdatePositions(request);
                    }
                    if (UpdateItems_def.Count > 0)
                    {
                        //更新JC_DEF缓存
                        DefineCacheBatchUpdatePropertiesRequest request = new DefineCacheBatchUpdatePropertiesRequest();
                        request.PointItems = UpdateItems_def;
                        service_def.BatchUpdatePointDefineInfo(request);
                    }

                    IConfigService _ConfigService = ServiceFactory.Create<IConfigService>();
                    _ConfigService.SaveInspection();
                    XtraMessageBox.Show("操作成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
        }

        private bool IsHaveMark(string wz, string mark)
        {
            int Length = mark.Length;
            if (wz.Length > Length)
            {
                if (wz.Substring(wz.Length - Length, Length) == mark)
                {
                    //wz = wz.Substring(0, wz.Length - Length);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            UpdateJC_WZ(txt_model.Text.Trim(), 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateJC_WZ(txt_model.Text.Trim(), 1);
        }
    }
}
