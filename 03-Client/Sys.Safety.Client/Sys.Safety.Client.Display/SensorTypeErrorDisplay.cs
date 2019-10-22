using DevExpress.XtraEditors;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PointDefine;
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

namespace Sys.Safety.Client.Display
{
    public partial class SensorTypeErrorDisplay : XtraForm
    {
        IPointDefineService pointDefineService = ServiceFactory.Create<IPointDefineService>();
        string _point = "";
        public SensorTypeErrorDisplay(string point)
        {
            _point = point;
            InitializeComponent();
        }

        private void SensorTypeErrorDisplay_Load(object sender, EventArgs e)
        {
            try
            {
                PointDefineGetByPointRequest PointDefineRequest = new PointDefineGetByPointRequest();
                PointDefineRequest.Point = _point;
                Jc_DefInfo jc_defInfo = pointDefineService.GetPointDefineCacheByPoint(PointDefineRequest).Data;
                if (jc_defInfo != null)
                {
                    labelControl1.Text = "实际挂接类型：" + jc_defInfo.RealTypeInfo;
                    labelControl2.Text = "当前定义类型：" + jc_defInfo.DevName;
                }
            }
            catch(Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
    }
}
