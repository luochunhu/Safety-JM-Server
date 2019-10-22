using Basic.Framework.Version;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Basic.Framework.Tools.SNMaker
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            txtMachineCode.Text = Clipboard.GetText();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMachineCode.Text))
            {
                Clipboard.SetText(txtMachineCode.Text);
            }

        }

        private void btnCopyRegist_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRegistCode.Text))
            {
                Clipboard.SetText(txtRegistCode.Text);
            }
        }

        /// <summary>
        /// 生成注册码（多个）
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="machineCodes"></param>
        /// <returns></returns>
        private string GetRegisterCode(string privateKey, string machineCodes)
        {
            //2017.08.01 注册码规则
            //以多个mac做为机器码，每个mac都生成一个加密的RAS字符串，解密验证时，只要其中一个验证成功，则认为是有效授权文件
            List<string> registerCodeList = new List<string>();

            machineCodes = DESHelper.Decrypt(machineCodes.Trim());
            var macList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(machineCodes);
            foreach (string machineCode in macList)
            {
                var registerCode = RSAHelper.CreateRegistCode(privateKey, machineCode);
                registerCodeList.Add(registerCode);
            }

            string listString = Newtonsoft.Json.JsonConvert.SerializeObject(registerCodeList);

            return DESHelper.Encrypt(listString);
        }

        private void btnCreateRegistCode_Click(object sender, EventArgs e)
        {
            string productCode = GetProductCode();
            if (productCode == "000" && this.rdoDevelopModel.Checked == false)
            {
                MessageBox.Show("只有在开发授权模式下才能选择授权产品为“所有产品”！");
                return;
            }

            try
            {
                string machineCode = txtMachineCode.Text.Trim();
                if (string.IsNullOrEmpty(machineCode))
                {
                    MessageBox.Show("请填写机器码！", "提示");
                    return;
                }
                string publicKey = string.Empty;
                string privateKey = string.Empty;
                RSAHelper.CreateKey(ref publicKey, ref privateKey);

                AuthorizationInfo authInfo = new AuthorizationInfo();
                authInfo.PublicKey = publicKey;
                //authInfo.RegisterCode = RSAHelper.CreateRegistCode(privateKey, machineCode);
                authInfo.RegisterCode = GetRegisterCode(privateKey, machineCode);//循环生成多个加密后的注册码字符串
                authInfo.ProductCode = GetProductCode();
                authInfo.CustomerInfo = this.txtCustomerInfo.Text.Trim();
                authInfo.EndTime = this.dtpEndTime.Value;
                //增加授权终端数量写入  20180228
                int terminalsInt = 0;
                if (int.TryParse(terminals.Text, out terminalsInt))
                {
                    authInfo.Terminals = terminalsInt;
                }
                else
                {
                    MessageBox.Show("授权终端数量输入不合法！", "提示");
                    return;
                }
                if (this.rdoTrialMode.Checked)
                {
                    authInfo.AuthorizeMode = 0;
                }
                else if (this.rdoDevelopModel.Checked)
                {
                    authInfo.AuthorizeMode = 1;
                }
                else if (this.rdoCommercialModel.Checked)
                {
                    authInfo.AuthorizeMode = 2;
                }

                string encryptContent = DESHelper.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(authInfo));

                txtRegistCode.Text = encryptContent;


            }
            catch (Exception ex)
            {
                MessageBox.Show("生成授权文件出错，请确认机器特征码是否正确。 错误原因：" + ex.Message);
            }

            return;
            //string productCode = GetProductCode();


            //string endTime = string.Empty;

            //if (rdoDevelopModel.Checked||rdoTrialMode.Checked)
            //{
            //    var endDate = dtpEndTime.Value;
            //    endTime += new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59).ToString();
            //}

            //if(!string.IsNullOrEmpty(endTime))
            //{
            //    txtRegistCode.Text = publicKey+"-"+Helper.CreateRegistCode(privateKey, machineCode) + "-" + Helper.Encrypt(endTime) + "-" + Helper.Encrypt(productCode);
            //}
            //else
            //{
            //    txtRegistCode.Text = publicKey + "-" + Helper.CreateRegistCode(privateKey, machineCode)+"-"+ Helper.Encrypt(new Random().NextDouble().ToString()) + "-" + Helper.Encrypt(productCode);
            //}
        }

        private string GetProductCode()
        {
            string productCode = "";
            if (this.cbxProducts.SelectedIndex >= 0)
            {
                productCode = cbxProducts.SelectedItem.ToString().Split('-')[0];
            }
            return productCode;
        }


        private void btnImportfile_Click(object sender, EventArgs e)
        {
            string registCode = txtRegistCode.Text.Trim();
            if (string.IsNullOrEmpty(registCode))
            {
                MessageBox.Show("请先生成注册码", "提示");
                return;
            }
            SaveFileDialog sfl = new SaveFileDialog();
            sfl.Filter = "授权文件(*.License)|*.License";
            sfl.FileName = "i";
            try
            {
                if (sfl.ShowDialog() == DialogResult.OK)
                {
                    string filename = sfl.FileName;
                    using (StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8))
                    {
                        sw.WriteLine(registCode);
                    }
                    MessageBox.Show("导出注册文件成功！", "提示");

                    StringCollection paths = new StringCollection();
                    paths.Add(filename);
                    Clipboard.SetFileDropList(paths);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex.Message, "提示");
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (this.cbxProducts.Items.Count > 0)
            {
                this.cbxProducts.SelectedIndex = 0;
            }

            SetEndTime();
        }

        private void rdoTrialMode_CheckedChanged(object sender, EventArgs e)
        {
            SetEndTime();
        }

        private void rdoCommercialModel_CheckedChanged(object sender, EventArgs e)
        {
            SetEndTime();
        }

        private void rdoDevelopModel_CheckedChanged(object sender, EventArgs e)
        {
            SetEndTime();
        }

        private void SetEndTime()
        {
            if (this.rdoTrialMode.Checked)
            {
                this.dtpEndTime.Value = DateTime.Now.AddMonths(3);
            }
            else if (this.rdoDevelopModel.Checked)
            {
                this.dtpEndTime.Value = DateTime.Now.AddYears(1);
            }
            else if (this.rdoCommercialModel.Checked)
            {
                this.dtpEndTime.Value = DateTime.Now.AddYears(10);
            }
        }

        private void btnCopyProductCode_Click(object sender, EventArgs e)
        {
            string productCode = GetProductCode();
            productCode = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(productCode));
            productCode = DESHelper.Encrypt(productCode);
            Clipboard.SetText(productCode);
            MessageBox.Show("复制成功");
        }


    }
}
