using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Threading;
using System.Runtime.InteropServices;
using Basic.Framework.Logging;

namespace Sys.Safety.Client.Alarm
{
    public partial class frmSound : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 是否正在播放中
        /// </summary>
        bool bIsPlaying = false;
        public int palyIndex = 0;

        public frmSound()
        {
            InitializeComponent();
        }

        private void frmSound_Load(object sender, EventArgs e)
        {
            try
            {
                this.Visible = false;
                //传入文件控件的句柄，当播放完声音后，会触发文件控件的text change事件
                SmartReader.Handler = this.textBox1.Handle.ToInt32();
                SmartReader.Handler2 = this.textBox2.Handle.ToInt32();

                SmartReader.InitialAuth();
                this.timerPlay.Enabled = false;
                this.timerPlay.Enabled = true;

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.Compare(this.textBox1.Text, "SMARTREAD_SDK_READ_END") == 0)
                {
                    bIsPlaying = false;
                    if (ClientAlarmConfigCache.showDataSound == null || ClientAlarmConfigCache.showDataSound.Count < 1) { return; }
                    //ClientAlarmConfigCache.showDataSound.RemoveAt(0);
                    if (palyIndex < ClientAlarmConfigCache.showDataSound.Count - 1)
                    {
                        palyIndex++;
                    }
                    else
                    {
                        palyIndex = 0;
                    }
                }
                else if (string.Compare(this.textBox1.Text, "SMARTREAD_SDK_READ_START") == 0)
                {
                    bIsPlaying = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void Play()
        {
            try
            {
                string sw = "";
                if (bIsPlaying) { return; }
                if (ClientAlarmConfigCache.showDataSound == null || ClientAlarmConfigCache.showDataSound.Count < 1) { return; }
                if (palyIndex >= ClientAlarmConfigCache.showDataSound.Count)
                {
                    palyIndex = 0;
                }
                if (ClientAlarmConfigCache.showDataSound[palyIndex].TypeDisplay == "逻辑分析报警")
                {
                    sw = string.Format("{0}{1}{2}", Number2Chn(ClientAlarmConfigCache.showDataSound[palyIndex].Wz),
                        ClientAlarmConfigCache.showDataSound[palyIndex].Ssz,
                        ClientAlarmConfigCache.showDataSound[palyIndex].TypeDisplay);
                }
                else if (ClientAlarmConfigCache.showDataSound[palyIndex].TypeDisplay == "定义异常报警")
                {
                    sw = string.Format("{0}{1}{2}", Number2Chn(ClientAlarmConfigCache.showDataSound[palyIndex].Wz),
                        ClientAlarmConfigCache.showDataSound[palyIndex].Ssz,
                        ClientAlarmConfigCache.showDataSound[palyIndex].TypeDisplay);
                }
                else if (ClientAlarmConfigCache.showDataSound[palyIndex].TypeDisplay == "倍数报警"
                   || ClientAlarmConfigCache.showDataSound[palyIndex].TypeDisplay == "风机局扇停报警"
                    || ClientAlarmConfigCache.showDataSound[palyIndex].TypeDisplay == "电源箱放电提醒")
                {
                    sw = string.Format("{0}{1}{2}{3}", Number2Chn(ClientAlarmConfigCache.showDataSound[palyIndex].Point)
                        , Number2Chn(ClientAlarmConfigCache.showDataSound[palyIndex].Wz),
                        ClientAlarmConfigCache.showDataSound[palyIndex].Ssz,
                        ClientAlarmConfigCache.showDataSound[palyIndex].TypeDisplay);
                }
                else
                {
                    float tempFloat = 0;
                    float.TryParse(ClientAlarmConfigCache.showDataSound[palyIndex].Ssz, out tempFloat);//是数值的时候才播报单位  20170715
                    if (tempFloat != 0)
                    {
                       // sw = string.Format("{0} {1} {2}{3} {4}"
                       //, Number2Chn(ClientAlarmConfigCache.showDataSound[palyIndex].Point)
                       //, Number2Chn(ClientAlarmConfigCache.showDataSound[palyIndex].Wz)
                       //, ClientAlarmConfigCache.showDataSound[palyIndex].Ssz
                       //, Unit2Chn(ClientAlarmConfigCache.showDataSound[palyIndex].Unit)
                       //, ClientAlarmConfigCache.showDataSound[palyIndex].TypeDisplay);
                        sw = string.Format("{0} {1} {2}{3}"                      
                       , Number2Chn(ClientAlarmConfigCache.showDataSound[palyIndex].Wz)
                       , ClientAlarmConfigCache.showDataSound[palyIndex].Ssz
                       , Unit2Chn(ClientAlarmConfigCache.showDataSound[palyIndex].Unit)
                       , ClientAlarmConfigCache.showDataSound[palyIndex].TypeDisplay);
                    }
                    else
                    {
                        //sw = string.Format("{0} {1} {2}{3} "
                        // , Number2Chn(ClientAlarmConfigCache.showDataSound[palyIndex].Point)
                        // , Number2Chn(ClientAlarmConfigCache.showDataSound[palyIndex].Wz)
                        // , ClientAlarmConfigCache.showDataSound[palyIndex].Ssz
                        // , ClientAlarmConfigCache.showDataSound[palyIndex].TypeDisplay);
                        sw = string.Format("{0} {1} {2}"                    
                         , Number2Chn(ClientAlarmConfigCache.showDataSound[palyIndex].Wz)
                         , ClientAlarmConfigCache.showDataSound[palyIndex].Ssz
                         , ClientAlarmConfigCache.showDataSound[palyIndex].TypeDisplay);
                    }
                }

                //加是否有声卡的判断  20170401
                if (Voice.SmartRead_GetVoiceDeviceNum() > 0)
                {
                    if (!SmartReader.isReading)
                    {
                        //Basic.Framework.Logging.LogHelper.Debug(sw);
                        SmartReader.Read(sw, 0);//智能语音报警(男声)
                        //SmartReader.Read(sw, 4);//智能语音报警(女声)
                        //SmartReader.Read(sw, 13);//智能语音报警(真人女声)
                        //SmartReader.Read(sw, 11);//智能语音报警(微软真人女声)
                        //ClientAlarmConfigCache.showDataSound.RemoveAt(0);
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error("报警语音播放发生异常 " + ex.Message);
            }
        }

        private void timerPlay_Tick(object sender, EventArgs e)
        {
            Play();
        }
        
        /// <summary>
        /// 停止所有语音报警
        /// </summary>
        public void CancelAlarm()
        {
            try
            {
                SmartReader.Stop();
                if (ClientAlarmConfigCache.showDataSound != null)
                {
                    ClientAlarmConfigCache.showDataSound.Clear();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 关闭所有语音报警
        /// </summary>
        public void CloseAlarm()
        {
            try
            {
                SmartReader.Close();
                if (ClientAlarmConfigCache.showDataSound != null)
                {
                    ClientAlarmConfigCache.showDataSound.Clear();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 关闭窗体之前，关闭SmartReader
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSound_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseAlarm();
        }
        /// <summary>
        /// 阿拉伯数组转中文数字
        /// </summary>
        /// <param name="wz">待处理的字符串</param>
        /// <returns>转换后的字符串</returns>
        private string Number2Chn(string wz)
        {
            try
            {
                if (string.IsNullOrEmpty(wz))
                {
                    return string.Empty;
                }
                string[] str = new string[10];
                str[0] = "零";
                str[1] = "一";
                str[2] = "二";
                str[3] = "三";
                str[4] = "四";
                str[5] = "五";
                str[6] = "六";
                str[7] = "七";
                str[8] = "八";
                str[9] = "九";
                for (int i = 0; i <= 9; i++)
                {
                    if (wz.Contains(i.ToString()))
                    {
                        wz = wz.Replace(i.ToString(), str[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return wz;
        }
        /// <summary>
        /// 符号单位转换为中文单位
        /// </summary>
        /// <param name="dw">待转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        private string Unit2Chn(string dw)
        {
            if (string.IsNullOrEmpty(dw))
            {
                return string.Empty;
            }
            string Str = dw;
            try
            {
                switch (dw)
                {
                    case "m/s":
                        Str = "米每秒";
                        break;
                    case "KPa":
                        Str = "千帕";
                        break;
                    case "℃":
                        Str = "度";
                        break;
                    case "M3/min":
                        Str = "立方每分";
                        break;
                    case "m":
                    case "M":
                        Str = "米";
                        break;
                    case "V":
                        Str = "伏";
                        break;
                    case "A":
                        Str = "安";
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Str;
        }
    }

    public class Voice
    {
        public static int SmartRead_iMultiRead = 0;
        public static int SmartRead_iMultiReadTimes = 0;

        [DllImport("smartread6.dll")]
        public static extern int SmartRead_Close();
        [DllImport("smartread6.dll")]
        public static extern int SmartRead_GetLocationInfo();
        [DllImport("smartread6.dll")]
        public static extern int SmartRead_GetSpeed();
        [DllImport("smartread6.dll")]
        public static extern int SmartRead_GetVoiceDeviceNum();
        [DllImport("smartread6.dll")]
        public static extern int SmartRead_GetVolume();
        [DllImport("smartread6.dll")]
        public static extern int SmartRead_InitialAuth(int hwndFrom, int hwndMessage, int iSpeech, int iRate, int iVolume, string lpMessage, string lpTip, string chMailBox, string chPassword);
        [DllImport("smartread6.dll")]
        public static extern int SmartRead_PauseORContinue();
        [DllImport("smartread6.dll")]
        public static extern int SmartRead_SetDialog(int hwndFather, string lpDownPage);
        [DllImport("smartread6.dll")]
        public static extern int SmartRead_SetSpeed(int iRate);
        [DllImport("smartread6.dll")]
        public static extern int SmartRead_SetVolume(int iVolume);
        [DllImport("smartread6.dll")]
        public static extern int SmartRead_Speak(string lpStr, int iStyle, int iSpeech, int iRate, int iVolume, int iPunctuation, int iSelVoiceDevice, string lpLink);
        [DllImport("smartread6.dll")]
        public static extern int SmartRead_SpeakToWave(string lpStr, string lpWaveFile, int iStyle, int iSpeech, int iRate, int iVolume, int iFormat, int iPunctuation, string lpLink);
        [DllImport("smartread6.dll")]
        public static extern int SmartRead_Stop();
    }
}