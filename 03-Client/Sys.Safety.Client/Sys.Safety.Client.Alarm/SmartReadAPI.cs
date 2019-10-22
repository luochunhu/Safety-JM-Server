using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Sys.Safety.Client.Alarm
{
    public class SmartReadAPI
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
