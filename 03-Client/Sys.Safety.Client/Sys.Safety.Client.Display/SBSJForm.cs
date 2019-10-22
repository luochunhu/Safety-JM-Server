using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using DevExpress.XtraBars.Ribbon;
using Sys.Safety.DataContract;
using DevExpress.XtraEditors;

namespace Sys.Safety.Client.Display
{
    public partial class SBSJForm : XtraForm
    {


        public SBSJForm()
        {
            InitializeComponent();
        }
        public string[] sjzttxt = new string[] {"未知","请求中","等待文件下发","文件接收中","文件接收完成","重启升级中","升级完成","取消升级中",
            "请求成功","重启升级成功","取消升级成功","恢复备份成功","恢复备份中","获取版本信息","获取版本信息成功" ,"请求失败","取消升级失败","巡检接收情况","补发"};
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_sb.Text = openFileDialog1.FileName;
                StaticClass .UpdateFile = openFileDialog1.FileName;
                TxtRead(StaticClass.UpdateFile);
            }
        }
        public UInt32 get_crc32(byte[] data, UInt32 len)
        {
            UInt32 i;
            UInt32 crc = 0xFFFFFFFF;
            UInt32 temp,temp1,temp2,temp3,temp4;
            for (i = 0; i < len; i++)
            {
                //crc = (crc << 8) ^ Swap.crc32table[((crc >> 24) ^ data[i]) & 0xFF];
                temp = (crc >> 24);
                temp1= temp ^ data[i];
                temp2 = temp1 & 0xFF;
                temp3 = StaticClass.crc32table[temp2];
                temp4 = (crc << 8);
                crc = temp4 ^ temp3;
            }
            return crc;
        }
        /// <summary>
        /// 描述：读取TXT文件
        /// </summary>
        /// <param name="filename">文件路径名称</param>
        private void TxtRead(string filename)
        {
            try
            {
                byte[] buf;
                //版本名称 只用两位数字表示从01至99
                FileStream fs = new FileStream(filename, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                buf = new byte[(int)br.BaseStream.Length];
                int checksize = br.Read(buf, 0, (int)br.BaseStream.Length);
                if (checksize > 0)
                {
                    StaticClass.UpdateCount = buf.Length / 256;//下发总帧数
                    if ((buf.Length % 256) != 0)
                    {
                        StaticClass.UpdateCount++;//下发总帧数
                    }
                    label5.Text = "总帧数【" + StaticClass.UpdateCount + "】";
                }
                StaticClass.UpdateBuffer = new byte[StaticClass.UpdateCount * 256];
                for (int i = 0; i < StaticClass.UpdateBuffer.Length; i++)
                {
                    StaticClass.UpdateBuffer[i] = 0xff;
                }
                for (int i = 0; i < buf.Length; i++)
                {
                    StaticClass.UpdateBuffer[i] = buf[i];
                }
                StaticClass.UpdateCrc = get_crc32(StaticClass.UpdateBuffer, (uint)StaticClass.UpdateBuffer.Length);
                br.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string getsc(int m,int f)
        {
           int t,mc,h;
           string sc = "<"+f+"分钟";
           try
           {
               if (m > 1)
               {
                   mc = (m - 1) * f;
                   h = mc / 60;
                   mc = mc % 60;
                   if (h > 24)
                   {
                       t = h / 24;
                       h = h % 24;
                       sc = t +"天"+ h + "小时" + mc + "分";
                   }
                   else
                   {
                       sc = h + "小时" + mc + "分";
                   }
                   
               }
           }
           catch (Exception ex)
           {
               Basic.Framework.Logging.LogHelper.Error(ex);
           }
           return sc;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            lb_time.Text = "文件下发，所需时间【" + getsc(StaticClass.UpdateCount, int.Parse(cmb_pl.Text)) + "】";
        }

        private void SBSJForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 256; i++)
            {
                cmb_bbsx.Items.Add(i.ToString ());
                cmb_bbxx.Items.Add(i.ToString ());
                cmb_dqbb.Items.Add(i.ToString ());
                cmb_dqyjbb.Items.Add(i.ToString ());
            }
            ReadConfig();
            cmb_bbsx.Text = StaticClass .UpdateBbXS .ToString ();
            cmb_bbxx.Text = StaticClass .UpageBbXX.ToString ();
            cmb_dqbb.Text = StaticClass .UpdateBb.ToString ();
            cmb_dqyjbb.Text =StaticClass .UpdateYBb .ToString ();
            cmb_type.SelectedIndex = 1;
            for (int i = 0; i < cmb_type.Items.Count; i++)
            {
                if (cmb_type.Items[i].ToString().Substring (1,cmb_type.Items[i].ToString().IndexOf (']')) == StaticClass.UpdatetTypeid.ToString())
                {
                    cmb_type.SelectedIndex = i;
                    break;
                }
            }
            cmb_pl.SelectedIndex = 0;
            for (int i = 0; i < cmb_pl.Items.Count; i++)
            {
                if (cmb_pl.Items[i].ToString() == StaticClass.UpdatePL.ToString())
                {
                    cmb_pl.SelectedIndex = i;
                    break;
                }
            }
            if (StaticClass.UpdateFile != "")
            {
                txt_sb.Text = StaticClass.UpdateFile;
            }
        }

        private bool check()
        {
            bool flag = false;
            if (string.IsNullOrEmpty(StaticClass.UpdateFile))
            {
                flag = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(cmb_dqbb.Text))
                {
                    StaticClass.UpdateBb = byte.Parse(cmb_dqbb.Text);
                }
                else
                {
                    flag = true;
                }

                if (!string.IsNullOrEmpty(cmb_dqyjbb.Text))
                {
                    StaticClass.UpdateYBb = byte.Parse(cmb_dqyjbb.Text);
                }
                else
                {
                    flag = true;
                }

                if (!string.IsNullOrEmpty(cmb_bbsx.Text))
                {
                    StaticClass.UpdateBbXS = byte.Parse(cmb_bbsx.Text);
                }
                else
                {
                    flag = true;
                }

                if (!string.IsNullOrEmpty(cmb_bbxx.Text))
                {
                    StaticClass.UpageBbXX = byte.Parse(cmb_bbxx.Text);
                }
                else
                {
                    flag = true;
                }

                if (!string.IsNullOrEmpty(cmb_type.Text))
                {
                    StaticClass.UpdatetTypeid = byte.Parse(cmb_type.Text.Substring(1, cmb_type.Text.IndexOf(']') - 1));
                }
                else
                {
                    flag = true;
                }

                if (!string.IsNullOrEmpty(cmb_pl.Text))
                {
                    StaticClass.UpdatePL = byte.Parse(cmb_pl.Text);
                }
                else
                {
                    flag = true;
                }
            }
            return flag;
        }

        private void getfzh()
        {
            if (dataGridView1.Rows.Count > 0)
            {
                StaticClass.UpdateFzh.Clear();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (bool .Parse ( dataGridView1.Rows[i].Cells["sx"].Value.ToString()))
                    {
                        StaticClass.UpdateFzh.Add(byte.Parse(dataGridView1.Rows[i].Cells["dz"].Value.ToString()));
                    }
                }
            }
        }
        private Thread freshthread;

        private DataTable dt;
        private void fthread()
        {
            while (!StaticClass.SystemOut)
            {
                try
                {
                    getmsg();
                    MethodInvoker In = new MethodInvoker(showmsg);
                    this.BeginInvoke(In);
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex.ToString());
                }
                Thread.Sleep(4000);
            }
        }

        private void ReadConfig()
        {
            string tempstr = "";
            string[] temp;
            try
            {
                tempstr = Model.RealInterfaceFuction.ReadConfig("RemoteUpgradeConfig");
                if (!string.IsNullOrEmpty(tempstr))
                {
                    temp = tempstr.Split('|');
                    if (temp.Length >= 7)
                    {
                        StaticClass.UpdateFile = temp[0];
                        StaticClass.UpdateBb = byte.Parse(temp[1]);
                        StaticClass.UpdateYBb = byte.Parse(temp[2]);
                        StaticClass.UpdateBbXS = byte.Parse(temp[3]);
                        StaticClass.UpageBbXX = byte.Parse(temp[4]);
                        StaticClass.UpdatePL = int.Parse(temp[5]);
                        StaticClass.UpdatetTypeid = byte.Parse(temp[6]);
                        StaticClass.UpdateCount = byte.Parse(temp[7]);
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }

            try
            {
                tempstr = Model.RealInterfaceFuction.ReadConfig("UpdateBuff");
                if (!string.IsNullOrEmpty(tempstr))
                {
                    StaticClass.UpdateBuffer = Convert.FromBase64String(tempstr);
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }

            try
            {
                tempstr = Model.RealInterfaceFuction.ReadConfig("UpdateSending");
                StaticClass.IsUpdateFile = tempstr == "1" ? true : false;
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }

            try
            {
                tempstr = Model.RealInterfaceFuction.ReadConfig("UpdateCrc");
                if (!string.IsNullOrEmpty(tempstr))
                {
                    StaticClass.UpdateCrc = uint.Parse(tempstr);
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }

            try
            {
                tempstr = Model.RealInterfaceFuction.ReadConfig("UpdateIndexAndTime");
                if (!string.IsNullOrEmpty(tempstr))
                {
                    temp = tempstr.Split('|');
                    if (temp.Length >= 2)
                    {
                        StaticClass.CurrentIndex = uint.Parse(temp[0]);
                        StaticClass.UpdateTime = DateTime.Parse(temp[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }

            try
            {
                tempstr = Model.RealInterfaceFuction.ReadConfig("UpdateFzh");
                if (!string.IsNullOrEmpty(tempstr))
                {
                    temp = tempstr.Split('|');
                    StaticClass.UpdateFzh.Clear();
                    if (temp.Length > 0)
                    {
                        for (int i = 0; i < temp.Length; i++)
                        {
                            StaticClass.UpdateFzh.Add(byte.Parse(temp[i]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 存储升级配置
        /// </summary>
        private void SaveConfig()
        {
            try
            {
                #region 保存配置
                List<ConfigInfo> list = new List<ConfigInfo>();
                string RemoteUpgradeConfig = "";
                //格式 ：文件路径|软件版本|硬件版本|版本上限|版本下限|下发频率|升级的设备类型
                ConfigInfo c = new ConfigInfo();
                c.Name = "RemoteUpgradeConfig";
                RemoteUpgradeConfig += StaticClass.UpdateFile+"|";
                RemoteUpgradeConfig += StaticClass.UpdateBb + "|";
                RemoteUpgradeConfig += StaticClass.UpdateYBb + "|";
                RemoteUpgradeConfig += StaticClass.UpdateBbXS + "|";
                RemoteUpgradeConfig += StaticClass.UpageBbXX + "|";
                RemoteUpgradeConfig += StaticClass.UpdatePL + "|";
                RemoteUpgradeConfig += StaticClass.UpdatetTypeid+"|" ;
                RemoteUpgradeConfig += StaticClass.UpdateCount ;
                c.Text = RemoteUpgradeConfig;
                list.Add(c);                
                c = new ConfigInfo();
                c.Name = "UpdateBuff";
                c.Text =  Convert.ToBase64String( StaticClass .UpdateBuffer);
                list.Add(c);
                c = new ConfigInfo();
                c.Name = "UpdateCrc";
                c.Text = StaticClass.UpdateCrc.ToString ();
                list.Add(c);

                c = new ConfigInfo();
                c.Name = "UpdateFzh";
                for (int i = 0; i < StaticClass.UpdateFzh.Count; i++)
                {
                    c.Text = StaticClass.UpdateFzh[i] + "|";
                }
                c.Text = c.Text.Substring(0, c.Text.Length - 1);
                list.Add(c);

                Model.RealInterfaceFuction.SaveConfig(list);
                #endregion
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string fzhs = "";
            if (check())
            {
                MessageBox.Show("版本基础信息不完整", "提示");
                return;
            }
            else
            {
                getfzh();
                if (StaticClass.UpdateFzh.Count > 0)
                {
                    for (int i = 0; i < StaticClass.UpdateFzh.Count; i++)
                    {
                        fzhs += StaticClass.UpdateFzh[i] + "|";
                    }
                    fzhs = fzhs.Substring(0, fzhs.Length - 1);
                    if (DialogResult.OK == MessageBox.Show("确定对分站" + fzhs + "请求升级吗？", "提示", MessageBoxButtons.OKCancel))
                    {
                        Model.RealInterfaceFuction.RemoteUpgradeCommand (fzhs, 2, 1, 1);
                    }
                }
                else
                {
                    MessageBox.Show("未选择任何分站!","提示");
                }

            }
        }
        public string[] strfzzt = { "通讯中断", "通讯误码", "初始化中", "交流正常", "直流正常" };
        private void cmb_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            string msg = cmb_type.Text .Substring (cmb_type .Text .IndexOf (']')+1,cmb_type .Text .Length -cmb_type .Text .IndexOf (']')-1);
            byte type = byte.Parse(cmb_type.Text.Substring(1,cmb_type .Text .IndexOf (']')-1));
            dataGridView1.Rows.Clear();
            DataRow []rows = null;
            int xh=1;
            try
            {
                if (StaticClass.AllPointDt != null && StaticClass.AllPointDt.Rows.Count > 0)
                {
                    lock (StaticClass.allPointDtLockObj)
                    {
                        rows = StaticClass.AllPointDt.Select("lx='分站' and xhtype=" + type);
                        if (rows.Length > 0)
                        {
                            for (int i = 0; i < rows.Length; i++)
                            {
                                dataGridView1.Rows.Add(false, xh++, i + 1, rows[i]["wz"].ToString(), rows[i]["lb"].ToString(),
                                     OprFuction.StateChange(rows[i]["zt"].ToString()), "", "", "", "");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //if (UpdateClass.checkfz())
            //{
                string fzhs = "";
                if (StaticClass.UpdateFzh.Count > 0)
                {
                    for (int i = 0; i < StaticClass.UpdateFzh.Count; i++)
                    {
                        fzhs += StaticClass.UpdateFzh[i] + "|";
                    }
                    fzhs = fzhs.Substring(0, fzhs.Length - 1);
                    if (DialogResult.OK == MessageBox.Show("确定对分站" + fzhs + "进行下发数据吗？", "提示", MessageBoxButtons.OKCancel))
                    {
                        Model.RealInterfaceFuction.RemoteUpgradeCommand(fzhs, 2, 2, 3);
                    }
                }
                StaticClass.CurrentIndex = 0;
                StaticClass.IsUpdateFile = true;
            //}
            //else
            //{
            //    MessageBox.Show("设备未准备好接收文件,请先下发请求升级!","提示");
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<byte> fzh = new List<byte>();
            string fzhs = "";
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (bool.Parse(dataGridView1.Rows[i].Cells["sx"].Value.ToString()))
                    {
                        fzh.Add(byte.Parse(dataGridView1.Rows[i].Cells["dz"].Value.ToString()));
                    }
                }
            }
            if (fzh.Count > 0)
            {
                for (int i = 0; i < fzh.Count ; i++)
                {
                    fzhs += fzh[i] + "|";
                }
                fzhs = fzhs.Substring(0, fzhs.Length - 1);
                if (DialogResult.OK == MessageBox.Show("确定对分站"+fzhs+"取消升级吗？","提示",  MessageBoxButtons.OKCancel))
                {
                    Model.RealInterfaceFuction.RemoteUpgradeCommand(fzhs, 2, 3, 7);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<byte> fzh = new List<byte>();
            string fzhs = "";
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (bool.Parse(dataGridView1.Rows[i].Cells["sx"].Value.ToString()))
                    {
                        fzh.Add(byte.Parse(dataGridView1.Rows[i].Cells["dz"].Value.ToString()));
                    }
                }
            }
            if (fzh.Count > 0)
            {
                for (int i = 0; i < fzh.Count; i++)
                {
                    fzhs += fzh[i] + "|";
                }
                fzhs = fzhs.Substring(0, fzhs.Length - 1);
                if (DialogResult.OK == MessageBox.Show( "确定对分站" + fzhs + "重启升级吗？","提示", MessageBoxButtons.OKCancel))
                {
                    Model.RealInterfaceFuction.RemoteUpgradeCommand(fzhs, 2,4,5);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<byte> fzh = new List<byte>();
            string fzhs = "";
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (bool.Parse(dataGridView1.Rows[i].Cells["sx"].Value.ToString()))
                    {
                        fzh.Add(byte.Parse(dataGridView1.Rows[i].Cells["dz"].Value.ToString()));
                    }
                }
            }
            if (fzh.Count > 0)
            {
                for (int i = 0; i < fzh.Count; i++)
                {
                    fzhs += fzh[i] + "|";
                }
                fzhs = fzhs.Substring(0, fzhs.Length - 1);
                if (DialogResult.OK == MessageBox.Show( "确定对分站" + fzhs + "进行版本还原吗？","提示", MessageBoxButtons.OKCancel))
                {
                    Model.RealInterfaceFuction.RemoteUpgradeCommand(fzhs, 2, 5, 12);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<byte> fzh = new List<byte>();
            string fzhs = "";
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (bool.Parse(dataGridView1.Rows[i].Cells["sx"].Value.ToString()))
                    {
                        fzh.Add(byte.Parse(dataGridView1.Rows[i].Cells["dz"].Value.ToString()));
                    }
                }
            }
            if (fzh.Count > 0)
            {
                for (int i = 0; i < fzh.Count; i++)
                {
                    fzhs += fzh[i] + "|";
                }
                fzhs = fzhs.Substring(0, fzhs.Length - 1);
                if (DialogResult.OK == MessageBox.Show("确定获取分站" + fzhs + "的版本信息吗？", "提示", MessageBoxButtons.OKCancel))
                {
                    Model.RealInterfaceFuction.RemoteUpgradeCommand(fzhs, 2, 7, 13);
                }
            }
            else
            {
                MessageBox.Show("未选择分站,请在下面列表中选择分站","提示");
            }
        }

        private void getmsg()
        {
            int fzh = 0;
            string fzhs = "";
            try
            {
                if (dataGridView1.Rows.Count > 0&&dt!=null&&dt .Rows .Count >0)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        fzh = int.Parse(dataGridView1.Rows[i].Cells["dz"].Value.ToString());
                        fzhs += fzh + "|";
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void showmsg()
        {
            int fzh = 0;
            DataRow[] rows;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        fzh = int.Parse(dataGridView1.Rows[i].Cells["dz"].Value.ToString());
                        rows = dt.Select("c="+fzh);
                        if (rows.Length > 0)
                        {
                            dataGridView1.Rows[i].Cells["txzt"].Value =OprFuction .StateChange ( rows[0]["c1"].ToString ());
                            dataGridView1.Rows[i].Cells["dqbb"].Value = rows[0]["c2"].ToString();
                            dataGridView1.Rows[i].Cells["zs"].Value = rows[0]["c3"].ToString();
                            dataGridView1.Rows[i].Cells["sjzt"].Value = rows[0]["c4"].ToString();
                            dataGridView1.Rows[i].Cells["jd"].Value = rows[0]["c5"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 5)
                {
                    if (dataGridView1 .Rows [e.RowIndex ].Cells [e.ColumnIndex ].Value .ToString ().Contains ("交流正常"))
                    {
                        dataGridView1 .Rows [e.RowIndex ].Cells [e.ColumnIndex ].Style .ForeColor=Color .Blue ;
                    }
                    else 
                    {
                        dataGridView1 .Rows [e.RowIndex ].Cells [e.ColumnIndex ].Style .ForeColor=Color .Red  ;
                    }

                }
                else if (e.ColumnIndex == 8)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Contains("成功"))
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Blue;
                    }
                    else if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Contains("失败"))
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Purple;
                    }
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            List<byte> fzh = new List<byte>();
            string fzhs = "";
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (bool.Parse(dataGridView1.Rows[i].Cells["sx"].Value.ToString()))
                    {
                        fzh.Add(byte.Parse(dataGridView1.Rows[i].Cells["dz"].Value.ToString()));
                    }
                }
            }
            if (fzh.Count > 0)
            {
                for (int i = 0; i < fzh.Count; i++)
                {
                    fzhs += fzh[i] + "|";
                }
                fzhs = fzhs.Substring(0, fzhs.Length - 1);
                if (DialogResult.OK == MessageBox.Show("将对分站" + fzhs + "的发送巡检命令？", "提示", MessageBoxButtons.OKCancel))
                {
                    Model.RealInterfaceFuction.RemoteUpgradeCommand(fzhs, 2, 6, 17);
                }
            }
            else
            {
                MessageBox.Show("未选择分站,请在下面列表中选择分站", "提示");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string fzhs = "";
            if (check())
            {
                MessageBox.Show("版本基础信息不完整", "提示");
                return;
            }
            else
            {
                getfzh();
                if (StaticClass.UpdateFzh.Count > 0)
                {
                    for (int i = 0; i < StaticClass.UpdateFzh.Count; i++)
                    {
                        fzhs += StaticClass.UpdateFzh[i] + "|";
                    }
                    fzhs = fzhs.Substring(0, fzhs.Length - 1);
                    if (DialogResult.OK == MessageBox.Show("确定对分站" + fzhs + "请求升级吗？", "提示", MessageBoxButtons.OKCancel))
                    {
                        Model.RealInterfaceFuction.RemoteUpgradeCommand(fzhs, 2, 1, 1);
                        SaveConfig();
                    }
                }
                else
                {
                    MessageBox.Show("未选择任何分站!", "提示");
                }

            }
        }
    }
}
