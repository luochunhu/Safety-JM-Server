using Basic.Framework.Common;
using Basic.Framework.Rpc;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Position;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sys.Safety.Client.WindowHost
{
    public partial class frmRpcTest : Form
    {
        public frmRpcTest()
        {
            InitializeComponent();
        }

        private void btnRpcTest_Click(object sender, EventArgs e)
        {
            //初始化RPC客户端
            IRpcClient client = RpcFactory.CreateRpcClient(RpcModel.WebApiModel, "127.0.0.1", 10000);

            //构造调用入参数
            PositionRequest request = new PositionRequest(1);
            request.PositionInfo = new Jc_WzInfo()
            {
                ID = "6677",
                Wz = "测试位置",
                WzID = "1234",
                CreateTime = DateTime.Now,
                Upflag = "0"
            };

            //调用RPC远程服务端
            var response = client.Send<PositionRequest, PositionResponse>(request);

            //返回及结果判断
            if (response.IsSuccess)
            {
                MessageBox.Show("调用成功，结果为：\n" + JSONHelper.ToJSONString(response));

                PositionResponse data = response.Data;
            }
            else
            {
                MessageBox.Show("调用失败，结果为：\n" + JSONHelper.ToJSONString(response));
            }
        }

        private void btnWebApiTest_Click(object sender, EventArgs e)
        {
            ////创建业务服务对象（走webapi远程调用）
            //IUserService UserService = ServiceFactory.Create<IUserService>();

            //var ResultLuoch = UserService.GetUserList();

            //创建业务服务对象（走webapi远程调用）
            IPositionService wzService = ServiceFactory.Create<IPositionService>();

            //构建新增位置 的请求参数对象
            PositionAddRequest request = new PositionAddRequest();
            request.PositionInfo = new Jc_WzInfo()
            {
                ID = "5544",
                Wz = "测试位置201705181638",
                WzID = "52323",
                CreateTime = DateTime.Now,
                Upflag = "0"
            };
            //调用接口
            var result = wzService.AddPosition(request);

            MessageBox.Show("新增成功，结果为：\n" + JSONHelper.ToJSONString(result));

            //查询位置列表 
            var result1 = wzService.GetPositionList(new PositionGetListRequest() { PagerInfo = new Basic.Framework.Web.PagerInfo() { PageSize = 50 } });

            MessageBox.Show("查询列表成功，结果为：\n" + JSONHelper.ToJSONString(result1));

        }
    }
}
