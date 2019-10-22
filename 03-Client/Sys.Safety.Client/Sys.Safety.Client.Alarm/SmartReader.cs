using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Logging;
using System.Windows.Forms;

namespace Sys.Safety.Client.Alarm
{
    public class SmartReader
    {
        public static int _readerReturnValue = 0;

        public static bool IsMultiRead { get; set; }
        /// <summary>
        /// 是否正在进行语音报警
        /// </summary>
        public static bool isReading { get; set; }
        /// <summary>
        /// 文件框的句柄
        /// </summary>
        public static int Handler { get; set; }
        public static int Handler2 { get; set; }

        public static int GetVoiceDeviceNum()
        {
            return SmartReadAPI.SmartRead_GetVoiceDeviceNum();
        }

        /// <summary>
        /// 初始化语音播放引擎
        /// 10 成功；
        /// 20 打开语音引擎失败；
        /// 30 未安装TTS语音引擎；
        /// 999 未安装声卡或未安装声卡驱动(无法获取设置) 
        /// </summary>
        /// <returns></returns>
        public static int InitialAuth()
        {
            _readerReturnValue = SmartReadAPI.SmartRead_GetVoiceDeviceNum();
            if (_readerReturnValue <= 0)//如果没有声音设备，则直接返回999
            {
                return 999;
            }
            _readerReturnValue = SmartReadAPI.SmartRead_InitialAuth(Handler2, Handler, -1, -1, -1, "", "", "zhangfeng@188.com", "84445591");
            if (_readerReturnValue == 20)
            {
                MessageBox.Show("打开语音引擎失败！");
            }
            if (_readerReturnValue == 30)
            {
                MessageBox.Show("本操作系统未安装TTS语音引擎！");  
            }
            return _readerReturnValue;
        }

        public static bool Read(string words, int speech)
        {
            isReading = true;
            bool result = false;
            try
            {
                if (_readerReturnValue == 50)//返回值为50时，如果后续再继续调用SmartRead_Speak方法，会导致整个程序挂掉并不能捕获它的异常
                {
                    return false;
                }
                if (_readerReturnValue != 10)//如果不等于10，则重新初始化
                {
                    _readerReturnValue = InitialAuth();
                }
                if (_readerReturnValue == 10)//如果初始化或者播放成功，则继续播放
                {
                    //iStyle:10-阻塞 11-非阻塞
                    _readerReturnValue = SmartReadAPI.SmartRead_Speak(words, 11, speech, 60, 100, 1, -1, "");
                }
                if (_readerReturnValue == 10)//如果返回值不等于10，则认为不成功
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogHelper.Error("读声音报警出错，Read（） 错误原因： ", ex);
            }
            isReading = false;
            return result;
        }

        public static void Stop()
        {
            try
            {
                SmartReadAPI.SmartRead_Stop();
            }
            catch (Exception ex)
            {
                LogHelper.Error("停止声音报警出错，Stop（） 错误原因： ", ex);
            }
        }

        public static void Close()
        {
            try
            {
                SmartReadAPI.SmartRead_Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error("关闭声音报警出错，Close（） 错误原因： ", ex);
            }
            _readerReturnValue = 0;
        }
    }
}
