using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace NetSDKCS
{
    public static class NETClient
    {
        #region << constant >>

        /// <summary>
        /// zh-cn language
        /// </summary>
        private static Dictionary<EM_ErrorCode, string> en_us_String = new Dictionary<EM_ErrorCode, string>() 
        {
            {EM_ErrorCode.NET_NOERROR,"No error"},
            {EM_ErrorCode.NET_ERROR,"Unknown error"},
            {EM_ErrorCode.NET_SYSTEM_ERROR,"Windows system error"},
            {EM_ErrorCode.NET_NETWORK_ERROR,"Protocol error it may result from network timeout"},
            {EM_ErrorCode.NET_DEV_VER_NOMATCH,"Device protocol does not match"},
            {EM_ErrorCode.NET_INVALID_HANDLE,"Handle is invalid"},
            {EM_ErrorCode.NET_OPEN_CHANNEL_ERROR,"Failed to open channel"},
            {EM_ErrorCode.NET_CLOSE_CHANNEL_ERROR,"Failed to close channel"},
            {EM_ErrorCode.NET_ILLEGAL_PARAM,"User parameter is illegal"},
            {EM_ErrorCode.NET_SDK_INIT_ERROR,"SDK initialization error"}, 
            {EM_ErrorCode.NET_SDK_UNINIT_ERROR,"SDK clear error"},
            {EM_ErrorCode.NET_RENDER_OPEN_ERROR,"Error occurs when apply for render resources"},
            {EM_ErrorCode.NET_DEC_OPEN_ERROR,"Error occurs when opening the decoder library"},
            {EM_ErrorCode.NET_DEC_CLOSE_ERROR,"Error occurs when closing the decoder library"},
            {EM_ErrorCode.NET_MULTIPLAY_NOCHANNEL,"The detected channel number is 0 in multiple-channel preview"}, 
            {EM_ErrorCode.NET_TALK_INIT_ERROR,"Failed to initialize record library"},
            {EM_ErrorCode.NET_TALK_NOT_INIT,"The record library has not been initialized"},
            {EM_ErrorCode.NET_TALK_SENDDATA_ERROR,"Error occurs when sending out audio data"},
            {EM_ErrorCode.NET_REAL_ALREADY_SAVING,"The real-time has been protected"},
            {EM_ErrorCode.NET_NOT_SAVING,"The real-time data has not been save"},
            {EM_ErrorCode.NET_OPEN_FILE_ERROR,"Error occurs when opening the file"},
            {EM_ErrorCode.NET_PTZ_SET_TIMER_ERROR,"Failed to enable PTZ to control timer"},
            {EM_ErrorCode.NET_RETURN_DATA_ERROR,"Error occurs when verify returned data"},
            {EM_ErrorCode.NET_INSUFFICIENT_BUFFER,"There is no sufficient buffer"},
            {EM_ErrorCode.NET_NOT_SUPPORTED,"The current SDK does not support this function"},
            {EM_ErrorCode.NET_NO_RECORD_FOUND,"There is no searched result"},
            {EM_ErrorCode.NET_NOT_AUTHORIZED,"You have no operation right"},
            {EM_ErrorCode.NET_NOT_NOW,"Can not operate right now"},
            {EM_ErrorCode.NET_NO_TALK_CHANNEL,"There is no audio talk channel"},
            {EM_ErrorCode.NET_NO_AUDIO,"There is no audio"},
            {EM_ErrorCode.NET_NO_INIT,"The network SDK has not been initialized"},
            {EM_ErrorCode.NET_DOWNLOAD_END,"The download completed"},
            {EM_ErrorCode.NET_EMPTY_LIST,"There is no searched result"},
            {EM_ErrorCode.NET_ERROR_GETCFG_SYSATTR,"Failed to get system property setup"},
            {EM_ErrorCode.NET_ERROR_GETCFG_SERIAL,"Failed to get SN"},
            {EM_ErrorCode.NET_ERROR_GETCFG_GENERAL,"Failed to get general property"},
            {EM_ErrorCode.NET_ERROR_GETCFG_DSPCAP,"Failed to get DSP capacity description"},
            {EM_ErrorCode.NET_ERROR_GETCFG_NETCFG,"Failed to get network channel setup"},
            {EM_ErrorCode.NET_ERROR_GETCFG_CHANNAME,"Failed to get channel name"},
            {EM_ErrorCode.NET_ERROR_GETCFG_VIDEO,"Failed to get video property"},
            {EM_ErrorCode.NET_ERROR_GETCFG_RECORD,"Failed to get record setup"},
            {EM_ErrorCode.NET_ERROR_GETCFG_PRONAME,"Failed to get decoder protocol name"},
            {EM_ErrorCode.NET_ERROR_GETCFG_FUNCNAME,"Failed to get 232 COM function name"},
            {EM_ErrorCode.NET_ERROR_GETCFG_485DECODER,"Failed to get decoder property"},
            {EM_ErrorCode.NET_ERROR_GETCFG_232COM,"Failed to get 232 COM setup"},
            {EM_ErrorCode.NET_ERROR_GETCFG_ALARMIN,"Failed to get external alarm input setup"},
            {EM_ErrorCode.NET_ERROR_GETCFG_ALARMDET,"Failed to get motion detection alarm"},
            {EM_ErrorCode.NET_ERROR_GETCFG_SYSTIME,"Failed to get device time"},
            {EM_ErrorCode.NET_ERROR_GETCFG_PREVIEW,"Failed to get preview parameter"},
            {EM_ErrorCode.NET_ERROR_GETCFG_AUTOMT,"Failed to get audio maintenance setup"},
            {EM_ErrorCode.NET_ERROR_GETCFG_VIDEOMTRX,"Failed to get video matrix setup"},
            {EM_ErrorCode.NET_ERROR_GETCFG_COVER,"Failed to get privacy mask zone setup"},
            {EM_ErrorCode.NET_ERROR_GETCFG_WATERMAKE,"Failed to get video watermark setup"},
            {EM_ErrorCode.NET_ERROR_GETCFG_MULTICAST,"Failed to get config multicast port by channel"},
            {EM_ErrorCode.NET_ERROR_SETCFG_GENERAL,"Failed to modify general property"},
            {EM_ErrorCode.NET_ERROR_SETCFG_NETCFG,"Failed to modify channel setup"},
            {EM_ErrorCode.NET_ERROR_SETCFG_CHANNAME,"Failed to modify channel name"},
            {EM_ErrorCode.NET_ERROR_SETCFG_VIDEO,"Failed to modify video channel"},
            {EM_ErrorCode.NET_ERROR_SETCFG_RECORD,"Failed to modify record setup"},
            {EM_ErrorCode.NET_ERROR_SETCFG_485DECODER,"Failed to modify decoder property"},
            {EM_ErrorCode.NET_ERROR_SETCFG_232COM,"Failed to modify 232 COM setup"},
            {EM_ErrorCode.NET_ERROR_SETCFG_ALARMIN,"Failed to modify external input alarm setup"},
            {EM_ErrorCode.NET_ERROR_SETCFG_ALARMDET,"Failed to modify motion detection alarm setup"}, 
            {EM_ErrorCode.NET_ERROR_SETCFG_SYSTIME,"Failed to modify device time"}, 
            {EM_ErrorCode.NET_ERROR_SETCFG_PREVIEW,"Failed to modify preview parameter"},
            {EM_ErrorCode.NET_ERROR_SETCFG_AUTOMT,"Failed to modify auto maintenance setup"},
            {EM_ErrorCode.NET_ERROR_SETCFG_VIDEOMTRX,"Failed to modify video matrix setup"},
            {EM_ErrorCode.NET_ERROR_SETCFG_COVER,"Failed to modify privacy mask zone"},
            {EM_ErrorCode.NET_ERROR_SETCFG_WATERMAKE,"Failed to modify video watermark setup"},
            {EM_ErrorCode.NET_ERROR_SETCFG_WLAN,"Failed to modify wireless network information"},
            {EM_ErrorCode.NET_ERROR_SETCFG_WLANDEV,"Failed to select wireless network device"},
            {EM_ErrorCode.NET_ERROR_SETCFG_REGISTER,"Failed to modify the actively registration parameter setup"},
            {EM_ErrorCode.NET_ERROR_SETCFG_CAMERA,"Failed to modify camera property"},
            {EM_ErrorCode.NET_ERROR_SETCFG_INFRARED,"Failed to modify IR alarm setup"},
            {EM_ErrorCode.NET_ERROR_SETCFG_SOUNDALARM,"Failed to modify audio alarm setup"},
            {EM_ErrorCode.NET_ERROR_SETCFG_STORAGE,"Failed to modify storage position setup"},
            {EM_ErrorCode.NET_AUDIOENCODE_NOTINIT,"The audio encode port has not been successfully initialized"},
            {EM_ErrorCode.NET_DATA_TOOLONGH,"The data are too long"},
            {EM_ErrorCode.NET_UNSUPPORTED,"The device does not support current operation"}, 
            {EM_ErrorCode.NET_DEVICE_BUSY,"Device resources is not sufficient"},
            {EM_ErrorCode.NET_SERVER_STARTED,"The server has boot up"},
            {EM_ErrorCode.NET_SERVER_STOPPED,"The server has not fully boot up"},
            {EM_ErrorCode.NET_LISTER_INCORRECT_SERIAL,"Input serial number is not correct"},
            {EM_ErrorCode.NET_QUERY_DISKINFO_FAILED,"Failed to get HDD information"},
            {EM_ErrorCode.NET_ERROR_GETCFG_SESSION,"Failed to get connect session information"},
            {EM_ErrorCode.NET_USER_FLASEPWD_TRYTIME,"The password you typed is incorrect. You have exceeded the maximum number of retries"},
            {EM_ErrorCode.NET_LOGIN_ERROR_PASSWORD,"Password is not correct"},
            {EM_ErrorCode.NET_LOGIN_ERROR_USER,"The account does not exist"},
            {EM_ErrorCode.NET_LOGIN_ERROR_TIMEOUT,"Time out for log in returned value"},
            {EM_ErrorCode.NET_LOGIN_ERROR_RELOGGIN,"The account has logged in"},
            {EM_ErrorCode.NET_LOGIN_ERROR_LOCKED,"The account has been locked"},
            {EM_ErrorCode.NET_LOGIN_ERROR_BLACKLIST,"The account has been in the black list"},
            {EM_ErrorCode.NET_LOGIN_ERROR_BUSY,"Resources are not sufficient. System is busy now"},
            {EM_ErrorCode.NET_LOGIN_ERROR_CONNECT,"Time out. Please check network and try again"},
            {EM_ErrorCode.NET_LOGIN_ERROR_NETWORK,"Network connection failed"},
            {EM_ErrorCode.NET_LOGIN_ERROR_SUBCONNECT,"Successfully logged in the device but can not create video channel. Please check network connection"},
            {EM_ErrorCode.NET_LOGIN_ERROR_MAXCONNECT,"exceed the max connect number"},
            {EM_ErrorCode.NET_LOGIN_ERROR_PROTOCOL3_ONLY,"protocol 3 support"},
            {EM_ErrorCode.NET_LOGIN_ERROR_UKEY_LOST,"There is no USB or USB info error"},
            {EM_ErrorCode.NET_LOGIN_ERROR_NO_AUTHORIZED,"Client-end IP address has no right to login"},
            {EM_ErrorCode.NET_RENDER_SOUND_ON_ERROR,"Error occurs when Render library open audio"},
            {EM_ErrorCode.NET_RENDER_SOUND_OFF_ERROR,"Error occurs when Render library close audio"},
            {EM_ErrorCode.NET_RENDER_SET_VOLUME_ERROR,"Error occurs when Render library control volume"},
            {EM_ErrorCode.NET_RENDER_ADJUST_ERROR,"Error occurs when Render library set video parameter"},
            {EM_ErrorCode.NET_RENDER_PAUSE_ERROR,"Error occurs when Render library pause play"},
            {EM_ErrorCode.NET_RENDER_SNAP_ERROR,"Render library snapshot error"},
            {EM_ErrorCode.NET_RENDER_STEP_ERROR,"Render library stepper error"},
            {EM_ErrorCode.NET_RENDER_FRAMERATE_ERROR,"Error occurs when Render library set frame rate"},
            {EM_ErrorCode.NET_RENDER_DISPLAYREGION_ERROR,"Error occurs when Render lib setting show region"},
            {EM_ErrorCode.NET_RENDER_GETOSDTIME_ERROR,"An error occurred when Render library getting current play time"},
            {EM_ErrorCode.NET_GROUP_EXIST,"Group name has been existed"},
            {EM_ErrorCode.NET_GROUP_NOEXIST,"The group name does not exist"}, 
            {EM_ErrorCode.NET_GROUP_RIGHTOVER,"The group right exceeds the right list"},
            {EM_ErrorCode.NET_GROUP_HAVEUSER,"The group can not be removed since there is user in it"},
            {EM_ErrorCode.NET_GROUP_RIGHTUSE,"The user has used one of the group right. It can not be removed"}, 
            {EM_ErrorCode.NET_GROUP_SAMENAME,"New group name has been existed"},
            {EM_ErrorCode.NET_USER_EXIST,"The user name has been existed"},
            {EM_ErrorCode.NET_USER_NOEXIST,"The account does not exist"},
            {EM_ErrorCode.NET_USER_RIGHTOVER,"User right exceeds the group right"}, 
            {EM_ErrorCode.NET_USER_PWD,"Reserved account. It does not allow to be modified"},
            {EM_ErrorCode.NET_USER_FLASEPWD,"password is not correct"},
            {EM_ErrorCode.NET_USER_NOMATCHING,"Password is invalid"},
            {EM_ErrorCode.NET_USER_INUSE,"account in use"},
            {EM_ErrorCode.NET_ERROR_GETCFG_ETHERNET,"Failed to get network card setup"},
            {EM_ErrorCode.NET_ERROR_GETCFG_WLAN,"Failed to get wireless network information"},
            {EM_ErrorCode.NET_ERROR_GETCFG_WLANDEV,"Failed to get wireless network device"},
            {EM_ErrorCode.NET_ERROR_GETCFG_REGISTER,"Failed to get actively registration parameter"},
            {EM_ErrorCode.NET_ERROR_GETCFG_CAMERA,"Failed to get camera property"},
            {EM_ErrorCode.NET_ERROR_GETCFG_INFRARED,"Failed to get IR alarm setup"},
            {EM_ErrorCode.NET_ERROR_GETCFG_SOUNDALARM,"Failed to get audio alarm setup"},
            {EM_ErrorCode.NET_ERROR_GETCFG_STORAGE,"Failed to get storage position"},
            {EM_ErrorCode.NET_ERROR_GETCFG_MAIL,"Failed to get mail setup"},
            {EM_ErrorCode.NET_CONFIG_DEVBUSY,"Can not set right now"},
            {EM_ErrorCode.NET_CONFIG_DATAILLEGAL,"The configuration setup data are illegal"},
            {EM_ErrorCode.NET_ERROR_GETCFG_DST,"Failed to get DST setup"},
            {EM_ErrorCode.NET_ERROR_SETCFG_DST,"Failed to set DST"},
            {EM_ErrorCode.NET_ERROR_GETCFG_VIDEO_OSD,"Failed to get video osd setup"},
            {EM_ErrorCode.NET_ERROR_SETCFG_VIDEO_OSD,"Failed to set video osd"},
            {EM_ErrorCode.NET_ERROR_GETCFG_GPRSCDMA,"Failed to get CDMA\\GPRS configuration"},
            {EM_ErrorCode.NET_ERROR_SETCFG_GPRSCDMA,"Failed to set CDMA\\GPRS configuration"},
            {EM_ErrorCode.NET_ERROR_GETCFG_IPFILTER,"Failed to get IP Filter configuration"},
            {EM_ErrorCode.NET_ERROR_SETCFG_IPFILTER,"Failed to set IP Filter configuration"},
            {EM_ErrorCode.NET_ERROR_GETCFG_TALKENCODE,"Failed to get Talk Encode configuration"},
            {EM_ErrorCode.NET_ERROR_SETCFG_TALKENCODE,"Failed to set Talk Encode configuration"},
            {EM_ErrorCode.NET_ERROR_GETCFG_RECORDLEN,"Failed to get The length of the video package configuration"},
            {EM_ErrorCode.NET_ERROR_SETCFG_RECORDLEN,"Failed to set The length of the video package configuration"},
            {EM_ErrorCode.NET_DONT_SUPPORT_SUBAREA,"Not support Network hard disk partition"},
            {EM_ErrorCode.NET_ERROR_GET_AUTOREGSERVER,"Failed to get the register server information"},
            {EM_ErrorCode.NET_ERROR_CONTROL_AUTOREGISTER,"Failed to control actively registration"},
            {EM_ErrorCode.NET_ERROR_DISCONNECT_AUTOREGISTER,"Failed to disconnect actively registration"},
            {EM_ErrorCode.NET_ERROR_GETCFG_MMS,"Failed to get mms configuration"},
            {EM_ErrorCode.NET_ERROR_SETCFG_MMS,"Failed to set mms configuration"},
            {EM_ErrorCode.NET_ERROR_GETCFG_SMSACTIVATION,"Failed to get SMS configuration"},
            {EM_ErrorCode.NET_ERROR_SETCFG_SMSACTIVATION,"Failed to set SMS configuration"},
            {EM_ErrorCode.NET_ERROR_GETCFG_DIALINACTIVATION,"Failed to get activation of a wireless connection"},
            {EM_ErrorCode.NET_ERROR_SETCFG_DIALINACTIVATION,"Failed to set activation of a wireless connection"},
            {EM_ErrorCode.NET_ERROR_GETCFG_VIDEOOUT,"Failed to get the parameter of video output"},
            {EM_ErrorCode.NET_ERROR_SETCFG_VIDEOOUT,"Failed to set the configuration of video output"},
            {EM_ErrorCode.NET_ERROR_GETCFG_OSDENABLE,"Failed to get osd overlay enabling"},
            {EM_ErrorCode.NET_ERROR_SETCFG_OSDENABLE,"Failed to set OSD overlay enabling"},
            {EM_ErrorCode.NET_ERROR_SETCFG_ENCODERINFO,"Failed to set digital input configuration of front encoders"},
            {EM_ErrorCode.NET_ERROR_GETCFG_TVADJUST,"Failed to get TV adjust configuration"},
            {EM_ErrorCode.NET_ERROR_SETCFG_TVADJUST,"Failed to set TV adjust configuration"},
            {EM_ErrorCode.NET_ERROR_CONNECT_FAILED,"Failed to request to establish a connection"},
            {EM_ErrorCode.NET_ERROR_SETCFG_BURNFILE,"Failed to request to upload burn files"},
            {EM_ErrorCode.NET_ERROR_SNIFFER_GETCFG,"Failed to get capture configuration information"},
            {EM_ErrorCode.NET_ERROR_SNIFFER_SETCFG,"Failed to set capture configuration information"},
            {EM_ErrorCode.NET_ERROR_DOWNLOADRATE_GETCFG,"Failed to get download restrictions information"},
            {EM_ErrorCode.NET_ERROR_DOWNLOADRATE_SETCFG,"Failed to set download restrictions information"},
            {EM_ErrorCode.NET_ERROR_SEARCH_TRANSCOM,"Failed to query serial port parameters"},
            {EM_ErrorCode.NET_ERROR_GETCFG_POINT,"Failed to get the preset info"},
            {EM_ErrorCode.NET_ERROR_SETCFG_POINT,"Failed to set the preset info"},
            {EM_ErrorCode.NET_SDK_LOGOUT_ERROR,"SDK log out the device abnormally"},
            {EM_ErrorCode.NET_ERROR_GET_VEHICLE_CFG,"Failed to get vehicle configuration"},
            {EM_ErrorCode.NET_ERROR_SET_VEHICLE_CFG,"Failed to set vehicle configuration"},
            {EM_ErrorCode.NET_ERROR_GET_ATM_OVERLAY_CFG,"Failed to get ATM overlay configuration"},
            {EM_ErrorCode.NET_ERROR_SET_ATM_OVERLAY_CFG,"Failed to set ATM overlay configuration"},
            {EM_ErrorCode.NET_ERROR_GET_ATM_OVERLAY_ABILITY,"Failed to get ATM overlay ability"},
            {EM_ErrorCode.NET_ERROR_GET_DECODER_TOUR_CFG,"Failed to get decoder tour configuration"},
            {EM_ErrorCode.NET_ERROR_SET_DECODER_TOUR_CFG,"Failed to set decoder tour configuration"},
            {EM_ErrorCode.NET_ERROR_CTRL_DECODER_TOUR,"Failed to control decoder tour"},
            {EM_ErrorCode.NET_GROUP_OVERSUPPORTNUM,"Beyond the device supports for the largest number of user groups"},
            {EM_ErrorCode.NET_USER_OVERSUPPORTNUM,"Beyond the device supports for the largest number of users"},
            {EM_ErrorCode.NET_ERROR_GET_SIP_CFG,"Failed to get SIP configuration"},
            {EM_ErrorCode.NET_ERROR_SET_SIP_CFG,"Failed to set SIP configuration"},
            {EM_ErrorCode.NET_ERROR_GET_SIP_ABILITY,"Failed to get SIP capability"},
            {EM_ErrorCode.NET_ERROR_GET_WIFI_AP_CFG,"Failed to get 'WIFI ap' configuration"}, 
            {EM_ErrorCode.NET_ERROR_SET_WIFI_AP_CFG,"Failed to set 'WIFI ap' configuration"}, 
            {EM_ErrorCode.NET_ERROR_GET_DECODE_POLICY,"Failed to get decode policy"},
            {EM_ErrorCode.NET_ERROR_SET_DECODE_POLICY,"Failed to set decode policy"},
            {EM_ErrorCode.NET_ERROR_TALK_REJECT,"refuse talk"},
            {EM_ErrorCode.NET_ERROR_TALK_OPENED,"talk has opened by other client"},
            {EM_ErrorCode.NET_ERROR_TALK_RESOURCE_CONFLICIT,"resource conflict"},
            {EM_ErrorCode.NET_ERROR_TALK_UNSUPPORTED_ENCODE,"unsupported encode type"},
            {EM_ErrorCode.NET_ERROR_TALK_RIGHTLESS,"no right"},
            {EM_ErrorCode.NET_ERROR_TALK_FAILED,"request failed"},
            {EM_ErrorCode.NET_ERROR_GET_MACHINE_CFG,"Failed to get device relative config"},
            {EM_ErrorCode.NET_ERROR_SET_MACHINE_CFG,"Failed to set device relative config"},
            {EM_ErrorCode.NET_ERROR_GET_DATA_FAILED,"get data failed"},
            {EM_ErrorCode.NET_ERROR_MAC_VALIDATE_FAILED,"MAC validate failed"},
            {EM_ErrorCode.NET_ERROR_GET_INSTANCE,"Failed to get server instance"},
            {EM_ErrorCode.NET_ERROR_JSON_REQUEST,"Generated json string is error"},
            {EM_ErrorCode.NET_ERROR_JSON_RESPONSE,"The responding json string is error"},
            {EM_ErrorCode.NET_ERROR_VERSION_HIGHER,"The protocol version is lower than current version"},
            {EM_ErrorCode.NET_SPARE_NO_CAPACITY,"Hotspare disk operation failed. The capacity is low"},
            {EM_ErrorCode.NET_ERROR_SOURCE_IN_USE,"Display source is used by other output"},
            {EM_ErrorCode.NET_ERROR_REAVE,"advanced users grab low-level user resource"},
            {EM_ErrorCode.NET_ERROR_NETFORBID,"net forbid"},
            {EM_ErrorCode.NET_ERROR_GETCFG_MACFILTER,"get MAC filter configuration error"},
            {EM_ErrorCode.NET_ERROR_SETCFG_MACFILTER,"set MAC filter configuration error"},
            {EM_ErrorCode.NET_ERROR_GETCFG_IPMACFILTER,"get IP/MAC filter configuration error"},
            {EM_ErrorCode.NET_ERROR_SETCFG_IPMACFILTER,"set IP/MAC filter configuration error"},
            {EM_ErrorCode.NET_ERROR_OPERATION_OVERTIME,"operation over time"},
            {EM_ErrorCode.NET_ERROR_SENIOR_VALIDATE_FAILED,"senior validation failure"},
            {EM_ErrorCode.NET_ERROR_DEVICE_ID_NOT_EXIST,"device ID is not exist"},
            {EM_ErrorCode.NET_ERROR_UNSUPPORTED,"unsupport operation"},
            {EM_ErrorCode.NET_ERROR_PROXY_DLLLOAD,"proxy dll load error"},
            {EM_ErrorCode.NET_ERROR_PROXY_ILLEGAL_PARAM,"proxy user parameter is not legal"},
            {EM_ErrorCode.NET_ERROR_PROXY_INVALID_HANDLE,"handle invalid"},
            {EM_ErrorCode.NET_ERROR_PROXY_LOGIN_DEVICE_ERROR,"login device error"},
            {EM_ErrorCode.NET_ERROR_PROXY_START_SERVER_ERROR,"start proxy server error"},
            {EM_ErrorCode.NET_ERROR_SPEAK_FAILED,"request speak failed"},
            {EM_ErrorCode.NET_ERROR_NOT_SUPPORT_F6,"unsupport F6"},
            {EM_ErrorCode.NET_ERROR_CD_UNREADY,"CD is not ready"},
            {EM_ErrorCode.NET_ERROR_DIR_NOT_EXIST,"Directory does not exist"},
            {EM_ErrorCode.NET_ERROR_UNSUPPORTED_SPLIT_MODE,"The device does not support the segmentation model"},
            {EM_ErrorCode.NET_ERROR_OPEN_WND_PARAM,"Open the window parameter is illegal"},
            {EM_ErrorCode.NET_ERROR_LIMITED_WND_COUNT,"Open the window more than limit"},
            {EM_ErrorCode.NET_ERROR_UNMATCHED_REQUEST,"Request command with the current pattern don't match"},
            {EM_ErrorCode.NET_RENDER_ENABLELARGEPICADJUSTMENT_ERROR,"Render Library to enable high-definition image internal adjustment strategy error"},
            {EM_ErrorCode.NET_ERROR_UPGRADE_FAILED,"Upgrade equipment failure"},
            {EM_ErrorCode.NET_ERROR_NO_TARGET_DEVICE,"Can't find the target device"},
            {EM_ErrorCode.NET_ERROR_NO_VERIFY_DEVICE,"Can't find the verify device"}, 
            {EM_ErrorCode.NET_ERROR_CASCADE_RIGHTLESS,"No cascade permissions"},
            {EM_ErrorCode.NET_ERROR_LOW_PRIORITY,"low priority"},
            {EM_ErrorCode.NET_ERROR_REMOTE_REQUEST_TIMEOUT,"The remote device request timeout"},
            {EM_ErrorCode.NET_ERROR_LIMITED_INPUT_SOURCE,"Input source beyond maximum route restrictions"},
            {EM_ErrorCode.NET_ERROR_SET_LOG_PRINT_INFO,"Failed to set log print"},
            {EM_ErrorCode.NET_ERROR_PARAM_DWSIZE_ERROR,"'dwSize' is not initialized in input param"},
            {EM_ErrorCode.NET_ERROR_LIMITED_MONITORWALL_COUNT,"TV wall exceed limit"},
            {EM_ErrorCode.NET_ERROR_PART_PROCESS_FAILED,"Fail to execute part of the process"},
            {EM_ErrorCode.NET_ERROR_TARGET_NOT_SUPPORT,"Fail to transmit due to not supported by target"},
            {EM_ErrorCode.NET_ERROR_VISITE_FILE,"Access to the file failed"},
            {EM_ErrorCode.NET_ERROR_DEVICE_STATUS_BUSY,"Device busy"},
            {EM_ErrorCode.NET_USER_PWD_NOT_AUTHORIZED,"Fail to change the password"},
            {EM_ErrorCode.NET_USER_PWD_NOT_STRONG,"Password strength is not enough"},
            {EM_ErrorCode.NET_ERROR_NO_SUCH_CONFIG,"No corresponding setup"},
            {EM_ErrorCode.NET_ERROR_AUDIO_RECORD_FAILED,"Failed to record audio"},
            {EM_ErrorCode.NET_ERROR_SEND_DATA_FAILED,"Failed to send out data"},
            {EM_ErrorCode.NET_ERROR_OBSOLESCENT_INTERFACE,"Abandoned port"},
            {EM_ErrorCode.NET_ERROR_INSUFFICIENT_INTERAL_BUF,"Internal buffer is not sufficient"}, 
            {EM_ErrorCode.NET_ERROR_NEED_ENCRYPTION_PASSWORD,"verify password when changing device IP"},
            {EM_ErrorCode.NET_ERROR_SERIALIZE_ERROR,"Failed to serialize data"},
            {EM_ErrorCode.NET_ERROR_DESERIALIZE_ERROR,"Failed to deserialize data"},
            {EM_ErrorCode.NET_ERROR_LOWRATEWPAN_ID_EXISTED,"the wireless id is already existed"},
            {EM_ErrorCode.NET_ERROR_LOWRATEWPAN_ID_LIMIT,"the wireless id limited"},
            {EM_ErrorCode.NET_ERROR_LOWRATEWPAN_ID_ABNORMAL,"add the wireless id abnormaly"},
            
            {EM_ErrorCode.ERR_INTERNAL_INVALID_CHANNEL,"invaild channel"},
            {EM_ErrorCode.ERR_INTERNAL_REOPEN_CHANNEL,"reopen channel failed"},
            {EM_ErrorCode.ERR_INTERNAL_SEND_DATA,"send data failed"},
            {EM_ErrorCode.ERR_INTERNAL_CREATE_SOCKET,"create socket failed"},
        };

        /// <summary>
        /// zh-cn language
        /// </summary>
        private static Dictionary<EM_ErrorCode, string> zh_cn_String = new Dictionary<EM_ErrorCode, string>() 
        {
            {EM_ErrorCode.NET_NOERROR,"没有错误"},
            {EM_ErrorCode.NET_ERROR,"未知错误"},
            {EM_ErrorCode.NET_SYSTEM_ERROR,"Windows系统出错"},
            {EM_ErrorCode.NET_NETWORK_ERROR,"网络错误,可能是因为网络超时"},
            {EM_ErrorCode.NET_DEV_VER_NOMATCH,"设备协议不匹配"},
            {EM_ErrorCode.NET_INVALID_HANDLE,"句柄无效"},
            {EM_ErrorCode.NET_OPEN_CHANNEL_ERROR,"打开通道失败"},
            {EM_ErrorCode.NET_CLOSE_CHANNEL_ERROR,"关闭通道失败"},
            {EM_ErrorCode.NET_ILLEGAL_PARAM,"用户参数不合法"},
            {EM_ErrorCode.NET_SDK_INIT_ERROR,"SDK初始化出错"},
            {EM_ErrorCode.NET_SDK_UNINIT_ERROR,"SDK清理出错"},
            {EM_ErrorCode.NET_RENDER_OPEN_ERROR,"申请render资源出错"},
            {EM_ErrorCode.NET_DEC_OPEN_ERROR,"打开解码库出错"},
            {EM_ErrorCode.NET_DEC_CLOSE_ERROR,"关闭解码库出错"},
            {EM_ErrorCode.NET_MULTIPLAY_NOCHANNEL,"多画面预览中检测到通道数为0"},
            {EM_ErrorCode.NET_TALK_INIT_ERROR,"录音库初始化失败"},
            {EM_ErrorCode.NET_TALK_NOT_INIT,"录音库未经初始化"},
            {EM_ErrorCode.NET_TALK_SENDDATA_ERROR,"发送音频数据出错"},
            {EM_ErrorCode.NET_REAL_ALREADY_SAVING,"实时数据已经处于保存状态"},
            {EM_ErrorCode.NET_NOT_SAVING,"未保存实时数据"},
            {EM_ErrorCode.NET_OPEN_FILE_ERROR,"打开文件出错"},
            {EM_ErrorCode.NET_PTZ_SET_TIMER_ERROR,"启动云台控制定时器失败"},
            {EM_ErrorCode.NET_RETURN_DATA_ERROR,"对返回数据的校验出错"},
            {EM_ErrorCode.NET_INSUFFICIENT_BUFFER,"没有足够的缓存"},
            {EM_ErrorCode.NET_NOT_SUPPORTED,"当前SDK未支持该功能"},
            {EM_ErrorCode.NET_NO_RECORD_FOUND,"查询不到录象"},
            {EM_ErrorCode.NET_NOT_AUTHORIZED,"无操作权限"},
            {EM_ErrorCode.NET_NOT_NOW,"暂时无法执行"},
            {EM_ErrorCode.NET_NO_TALK_CHANNEL,"未发现对讲通道"},
            {EM_ErrorCode.NET_NO_AUDIO,"未发现音频"},
            {EM_ErrorCode.NET_NO_INIT,"网络SDK未经初始化"},
            {EM_ErrorCode.NET_DOWNLOAD_END,"下载已结束"},
            {EM_ErrorCode.NET_EMPTY_LIST,"查询结果为空"},
            {EM_ErrorCode.NET_ERROR_GETCFG_SYSATTR,"获取系统属性配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_SERIAL,"获取序列号失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_GENERAL,"获取常规属性失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_DSPCAP,"获取DSP能力描述失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_NETCFG,"获取网络配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_CHANNAME,"获取通道名称失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_VIDEO,"获取视频属性失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_RECORD,"获取录象配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_PRONAME,"获取解码器协议名称失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_FUNCNAME,"获取232串口功能名称失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_485DECODER,"获取解码器属性失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_232COM,"获取232串口配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_ALARMIN,"获取外部报警输入配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_ALARMDET,"获取动态检测报警失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_SYSTIME,"获取设备时间失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_PREVIEW,"获取预览参数失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_AUTOMT,"获取自动维护配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_VIDEOMTRX,"获取视频矩阵配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_COVER,"获取区域遮挡配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_WATERMAKE,"获取图象水印配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_MULTICAST,"获取配置失败位置：组播端口按通道配置"},
            {EM_ErrorCode.NET_ERROR_SETCFG_GENERAL,"修改常规属性失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_NETCFG,"修改网络配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_CHANNAME,"修改通道名称失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_VIDEO,"修改视频属性失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_RECORD,"修改录象配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_485DECODER,"修改解码器属性失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_232COM,"修改232串口配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_ALARMIN,"修改外部输入报警配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_ALARMDET,"修改动态检测报警配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_SYSTIME,"修改设备时间失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_PREVIEW,"修改预览参数失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_AUTOMT,"修改自动维护配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_VIDEOMTRX,"修改视频矩阵配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_COVER,"修改区域遮挡配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_WATERMAKE,"修改图象水印配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_WLAN,"修改无线网络信息失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_WLANDEV,"选择无线网络设备失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_REGISTER,"修改主动注册参数配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_CAMERA,"修改摄像头属性配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_INFRARED,"修改红外报警配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_SOUNDALARM,"修改音频报警配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_STORAGE,"修改存储位置配置失败"},
            {EM_ErrorCode.NET_AUDIOENCODE_NOTINIT,"音频编码接口没有成功初始化"},
            {EM_ErrorCode.NET_DATA_TOOLONGH,"数据过长"},
            {EM_ErrorCode.NET_UNSUPPORTED,"设备不支持该操作"},
            {EM_ErrorCode.NET_DEVICE_BUSY,"设备资源不足"},
            {EM_ErrorCode.NET_SERVER_STARTED,"服务器已经启动"},
            {EM_ErrorCode.NET_SERVER_STOPPED,"服务器尚未成功启动"},
            {EM_ErrorCode.NET_LISTER_INCORRECT_SERIAL,"输入序列号有误"},
            {EM_ErrorCode.NET_QUERY_DISKINFO_FAILED,"获取硬盘信息失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_SESSION,"获取连接Session信息"},
            {EM_ErrorCode.NET_USER_FLASEPWD_TRYTIME,"输入密码错误超过限制次数"},
            {EM_ErrorCode.NET_LOGIN_ERROR_PASSWORD,"密码不正确"},
            {EM_ErrorCode.NET_LOGIN_ERROR_USER,"帐户不存在"},
            {EM_ErrorCode.NET_LOGIN_ERROR_TIMEOUT,"等待登录返回超时"},
            {EM_ErrorCode.NET_LOGIN_ERROR_RELOGGIN,"帐号已登录"},
            {EM_ErrorCode.NET_LOGIN_ERROR_LOCKED,"帐号已被锁定"},
            {EM_ErrorCode.NET_LOGIN_ERROR_BLACKLIST,"帐号已被列为黑名单"},
            {EM_ErrorCode.NET_LOGIN_ERROR_BUSY,"资源不足,系统忙"},
            {EM_ErrorCode.NET_LOGIN_ERROR_CONNECT,"登录设备超时,请检查网络并重试"},
            {EM_ErrorCode.NET_LOGIN_ERROR_NETWORK,"网络连接失败"},
            {EM_ErrorCode.NET_LOGIN_ERROR_SUBCONNECT,"登录设备成功,但无法创建视频通道,请检查网络状况"},
            {EM_ErrorCode.NET_LOGIN_ERROR_MAXCONNECT,"超过最大连接数"},
            {EM_ErrorCode.NET_LOGIN_ERROR_PROTOCOL3_ONLY,"只支持3代协议"},
            {EM_ErrorCode.NET_LOGIN_ERROR_UKEY_LOST,"未插入U盾或U盾信息错误"},
            {EM_ErrorCode.NET_LOGIN_ERROR_NO_AUTHORIZED,"客户端IP地址没有登录权限"},  
            {EM_ErrorCode.NET_RENDER_SOUND_ON_ERROR,"Render库打开音频出错"},
            {EM_ErrorCode.NET_RENDER_SOUND_OFF_ERROR,"Render库关闭音频出错"},
            {EM_ErrorCode.NET_RENDER_SET_VOLUME_ERROR,"Render库控制音量出错"},
            {EM_ErrorCode.NET_RENDER_ADJUST_ERROR,"Render库设置画面参数出错"},
            {EM_ErrorCode.NET_RENDER_PAUSE_ERROR,"Render库暂停播放出错"},
            {EM_ErrorCode.NET_RENDER_SNAP_ERROR,"Render库抓图出错"},
            {EM_ErrorCode.NET_RENDER_STEP_ERROR,"Render库步进出错"},
            {EM_ErrorCode.NET_RENDER_FRAMERATE_ERROR,"Render库设置帧率出错"},
            {EM_ErrorCode.NET_RENDER_DISPLAYREGION_ERROR,"Render库设置显示区域出错"},
            {EM_ErrorCode.NET_RENDER_GETOSDTIME_ERROR,"Render库获取当前播放时间出错"},
            {EM_ErrorCode.NET_GROUP_EXIST,"组名已存在"},
            {EM_ErrorCode.NET_GROUP_NOEXIST,"组名不存在"},
            {EM_ErrorCode.NET_GROUP_RIGHTOVER,"组的权限超出权限列表范围"},
            {EM_ErrorCode.NET_GROUP_HAVEUSER,"组下有用户,不能删除"},
            {EM_ErrorCode.NET_GROUP_RIGHTUSE,"组的某个权限被用户使用,不能出除"},
            {EM_ErrorCode.NET_GROUP_SAMENAME,"新组名同已有组名重复"},
            {EM_ErrorCode.NET_USER_EXIST,"用户已存在"},
            {EM_ErrorCode.NET_USER_NOEXIST,"用户不存在"},
            {EM_ErrorCode.NET_USER_RIGHTOVER,"用户权限超出组权限"},
            {EM_ErrorCode.NET_USER_PWD,"保留帐号,不容许修改密码"},
            {EM_ErrorCode.NET_USER_FLASEPWD,"密码不正确"},
            {EM_ErrorCode.NET_USER_NOMATCHING,"密码不匹配"},
            {EM_ErrorCode.NET_USER_INUSE,"账号正在使用中"},
            {EM_ErrorCode.NET_ERROR_GETCFG_ETHERNET,"获取网卡配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_WLAN,"获取无线网络信息失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_WLANDEV,"获取无线网络设备失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_REGISTER,"获取主动注册参数失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_CAMERA,"获取摄像头属性失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_INFRARED,"获取红外报警配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_SOUNDALARM,"获取音频报警配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_STORAGE,"获取存储位置配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_MAIL,"获取邮件配置失败"},
            {EM_ErrorCode.NET_CONFIG_DEVBUSY,"暂时无法设置"},
            {EM_ErrorCode.NET_CONFIG_DATAILLEGAL,"配置数据不合法"},
            {EM_ErrorCode.NET_ERROR_GETCFG_DST,"获取夏令时配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_DST,"设置夏令时配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_VIDEO_OSD,"获取视频OSD叠加配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_VIDEO_OSD,"设置视频OSD叠加配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_GPRSCDMA,"获取CDMA\\GPRS网络配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_GPRSCDMA,"设置CDMA\\GPRS网络配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_IPFILTER,"获取IP过滤配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_IPFILTER,"设置IP过滤配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_TALKENCODE,"获取语音对讲编码配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_TALKENCODE,"设置语音对讲编码配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_RECORDLEN,"获取录像打包长度配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_RECORDLEN,"设置录像打包长度配置失败"},
            {EM_ErrorCode.NET_DONT_SUPPORT_SUBAREA,"不支持网络硬盘分区"},
            {EM_ErrorCode.NET_ERROR_GET_AUTOREGSERVER,"获取设备上主动注册服务器信息失败"},
            {EM_ErrorCode.NET_ERROR_CONTROL_AUTOREGISTER,"主动注册重定向注册错误"},
            {EM_ErrorCode.NET_ERROR_DISCONNECT_AUTOREGISTER,"断开主动注册服务器错误"},
            {EM_ErrorCode.NET_ERROR_GETCFG_MMS,"获取mms配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_MMS,"设置mms配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_SMSACTIVATION,"获取短信激活无线连接配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_SMSACTIVATION,"设置短信激活无线连接配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_DIALINACTIVATION,"获取拨号激活无线连接配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_DIALINACTIVATION,"设置拨号激活无线连接配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_VIDEOOUT,"查询视频输出参数配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_VIDEOOUT,"设置视频输出参数配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_OSDENABLE,"获取osd叠加使能配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_OSDENABLE,"设置osd叠加使能配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_ENCODERINFO,"设置数字通道前端编码接入配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_TVADJUST,"获取TV调节配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_TVADJUST,"设置TV调节配置失败"},
            {EM_ErrorCode.NET_ERROR_CONNECT_FAILED,"请求建立连接失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_BURNFILE,"请求刻录文件上传失败"},
            {EM_ErrorCode.NET_ERROR_SNIFFER_GETCFG,"获取抓包配置信息失败"},
            {EM_ErrorCode.NET_ERROR_SNIFFER_SETCFG,"设置抓包配置信息失败"},
            {EM_ErrorCode.NET_ERROR_DOWNLOADRATE_GETCFG,"查询下载限制信息失败"},
            {EM_ErrorCode.NET_ERROR_DOWNLOADRATE_SETCFG,"设置下载限制信息失败"},
            {EM_ErrorCode.NET_ERROR_SEARCH_TRANSCOM,"查询串口参数失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_POINT,"获取预制点信息错误"},
            {EM_ErrorCode.NET_ERROR_SETCFG_POINT,"设置预制点信息错误"},
            {EM_ErrorCode.NET_SDK_LOGOUT_ERROR,"SDK没有正常登出设备"},
            {EM_ErrorCode.NET_ERROR_GET_VEHICLE_CFG,"获取车载配置失败"},
            {EM_ErrorCode.NET_ERROR_SET_VEHICLE_CFG,"设置车载配置失败"},
            {EM_ErrorCode.NET_ERROR_GET_ATM_OVERLAY_CFG,"获取atm叠加配置失败"},
            {EM_ErrorCode.NET_ERROR_SET_ATM_OVERLAY_CFG,"设置atm叠加配置失败"},
            {EM_ErrorCode.NET_ERROR_GET_ATM_OVERLAY_ABILITY,"获取atm叠加能力失败"},
            {EM_ErrorCode.NET_ERROR_GET_DECODER_TOUR_CFG,"获取解码器解码轮巡配置失败"},
            {EM_ErrorCode.NET_ERROR_SET_DECODER_TOUR_CFG,"设置解码器解码轮巡配置失败"},
            {EM_ErrorCode.NET_ERROR_CTRL_DECODER_TOUR,"控制解码器解码轮巡失败"},
            {EM_ErrorCode.NET_GROUP_OVERSUPPORTNUM,"超出设备支持最大用户组数目"},
            {EM_ErrorCode.NET_USER_OVERSUPPORTNUM,"超出设备支持最大用户数目"},
            {EM_ErrorCode.NET_ERROR_GET_SIP_CFG,"获取SIP配置失败"},
            {EM_ErrorCode.NET_ERROR_SET_SIP_CFG,"设置SIP配置失败"},
            {EM_ErrorCode.NET_ERROR_GET_SIP_ABILITY,"获取SIP能力失败"},
            {EM_ErrorCode.NET_ERROR_GET_WIFI_AP_CFG,"获取WIFI ap配置失败"},
            {EM_ErrorCode.NET_ERROR_SET_WIFI_AP_CFG,"设置WIFI ap配置失败"},
            {EM_ErrorCode.NET_ERROR_GET_DECODE_POLICY,"获取解码策略配置失败"},
            {EM_ErrorCode.NET_ERROR_SET_DECODE_POLICY,"设置解码策略配置失败"},
            {EM_ErrorCode.NET_ERROR_TALK_REJECT,"拒绝对讲"},
            {EM_ErrorCode.NET_ERROR_TALK_OPENED,"对讲被其他客户端打开"},
            {EM_ErrorCode.NET_ERROR_TALK_RESOURCE_CONFLICIT,"资源冲突"},
            {EM_ErrorCode.NET_ERROR_TALK_UNSUPPORTED_ENCODE,"不支持的语音编码格式"},
            {EM_ErrorCode.NET_ERROR_TALK_RIGHTLESS,"无权限"},
            {EM_ErrorCode.NET_ERROR_TALK_FAILED,"请求对讲失败"},
            {EM_ErrorCode.NET_ERROR_GET_MACHINE_CFG,"获取机器相关配置失败"},
            {EM_ErrorCode.NET_ERROR_SET_MACHINE_CFG,"设置机器相关配置失败"},
            {EM_ErrorCode.NET_ERROR_GET_DATA_FAILED,"设备无法获取当前请求数据"},
            {EM_ErrorCode.NET_ERROR_MAC_VALIDATE_FAILED,"MAC地址验证失败 "},
            {EM_ErrorCode.NET_ERROR_GET_INSTANCE,"获取服务器实例失败"},
            {EM_ErrorCode.NET_ERROR_JSON_REQUEST,"生成的jason字符串错误"},
            {EM_ErrorCode.NET_ERROR_JSON_RESPONSE,"响应的jason字符串错误"},
            {EM_ErrorCode.NET_ERROR_VERSION_HIGHER,"协议版本低于当前使用的版本"},
            {EM_ErrorCode.NET_SPARE_NO_CAPACITY,"热备操作失败, 容量不足"},
            {EM_ErrorCode.NET_ERROR_SOURCE_IN_USE,"显示源被其他输出占用"},
            {EM_ErrorCode.NET_ERROR_REAVE,"高级用户抢占低级用户资源"},
            {EM_ErrorCode.NET_ERROR_NETFORBID,"禁止入网 "},
            {EM_ErrorCode.NET_ERROR_GETCFG_MACFILTER,"获取MAC过滤配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_MACFILTER,"设置MAC过滤配置失败"},
            {EM_ErrorCode.NET_ERROR_GETCFG_IPMACFILTER,"获取IP/MAC过滤配置失败"},
            {EM_ErrorCode.NET_ERROR_SETCFG_IPMACFILTER,"设置IP/MAC过滤配置失败"},
            {EM_ErrorCode.NET_ERROR_OPERATION_OVERTIME,"当前操作超时 "},
            {EM_ErrorCode.NET_ERROR_SENIOR_VALIDATE_FAILED,"高级校验失败"}, 
            {EM_ErrorCode.NET_ERROR_DEVICE_ID_NOT_EXIST,"设备ID不存在"},
            {EM_ErrorCode.NET_ERROR_UNSUPPORTED,"不支持当前操作"},
            {EM_ErrorCode.NET_ERROR_PROXY_DLLLOAD,"代理库加载失败"},
            {EM_ErrorCode.NET_ERROR_PROXY_ILLEGAL_PARAM,"代理用户参数不合法"},
            {EM_ErrorCode.NET_ERROR_PROXY_INVALID_HANDLE,"代理句柄无效"},
            {EM_ErrorCode.NET_ERROR_PROXY_LOGIN_DEVICE_ERROR,"代理登入前端设备失败"},
            {EM_ErrorCode.NET_ERROR_PROXY_START_SERVER_ERROR,"启动代理服务失败"},
            {EM_ErrorCode.NET_ERROR_SPEAK_FAILED,"请求喊话失败"},
            {EM_ErrorCode.NET_ERROR_NOT_SUPPORT_F6,"设备不支持此F6接口调用"},
            {EM_ErrorCode.NET_ERROR_CD_UNREADY,"光盘未就绪"},
            {EM_ErrorCode.NET_ERROR_DIR_NOT_EXIST,"目录不存在"},
            {EM_ErrorCode.NET_ERROR_UNSUPPORTED_SPLIT_MODE,"设备不支持的分割模式"},
            {EM_ErrorCode.NET_ERROR_OPEN_WND_PARAM,"开窗参数不合法"},
            {EM_ErrorCode.NET_ERROR_LIMITED_WND_COUNT,"开窗数量超过限制"},
            {EM_ErrorCode.NET_ERROR_UNMATCHED_REQUEST,"请求命令与当前模式不匹配"},
            {EM_ErrorCode.NET_RENDER_ENABLELARGEPICADJUSTMENT_ERROR,"Render库启用高清图像内部调整策略出错"},
            {EM_ErrorCode.NET_ERROR_UPGRADE_FAILED,"设备升级失败"},
            {EM_ErrorCode.NET_ERROR_NO_TARGET_DEVICE,"找不到目标设备"},
            {EM_ErrorCode.NET_ERROR_NO_VERIFY_DEVICE,"找不到验证设备"},
            {EM_ErrorCode.NET_ERROR_CASCADE_RIGHTLESS,"无级联权限"},
            {EM_ErrorCode.NET_ERROR_LOW_PRIORITY,"低优先级"},
            {EM_ErrorCode.NET_ERROR_REMOTE_REQUEST_TIMEOUT,"远程设备请求超时"},
            {EM_ErrorCode.NET_ERROR_LIMITED_INPUT_SOURCE,"输入源超出最大路数限制"},
            {EM_ErrorCode.NET_ERROR_SET_LOG_PRINT_INFO,"设置日志打印失败"},
            {EM_ErrorCode.NET_ERROR_PARAM_DWSIZE_ERROR,"入参的dwsize字段出错"},
            {EM_ErrorCode.NET_ERROR_LIMITED_MONITORWALL_COUNT,"电视墙数量超过上限"},
            {EM_ErrorCode.NET_ERROR_PART_PROCESS_FAILED,"部分过程执行失败"},
            {EM_ErrorCode.NET_ERROR_TARGET_NOT_SUPPORT,"该功能不支持转发"},
            {EM_ErrorCode.NET_ERROR_VISITE_FILE,"访问文件失败"},
            {EM_ErrorCode.NET_ERROR_DEVICE_STATUS_BUSY,"设备忙"},
            {EM_ErrorCode.NET_USER_PWD_NOT_AUTHORIZED,"修改密码无权限"},
            {EM_ErrorCode.NET_USER_PWD_NOT_STRONG,"密码强度不够"},
            {EM_ErrorCode.NET_ERROR_NO_SUCH_CONFIG,"没有对应的配置"},
            {EM_ErrorCode.NET_ERROR_AUDIO_RECORD_FAILED,"录音失败"},
            {EM_ErrorCode.NET_ERROR_SEND_DATA_FAILED,"数据发送失败"},
            {EM_ErrorCode.NET_ERROR_OBSOLESCENT_INTERFACE,"废弃接口"},
            {EM_ErrorCode.NET_ERROR_INSUFFICIENT_INTERAL_BUF,"内部缓冲不足"},
            {EM_ErrorCode.NET_ERROR_NEED_ENCRYPTION_PASSWORD,"修改设备ip时,需要校验密码"},
            {EM_ErrorCode.NET_ERROR_SERIALIZE_ERROR,"数据序列化错误"},
            {EM_ErrorCode.NET_ERROR_DESERIALIZE_ERROR,"数据反序列化错误"},
            {EM_ErrorCode.NET_ERROR_LOWRATEWPAN_ID_EXISTED,"该无线ID已存在"},
            {EM_ErrorCode.NET_ERROR_LOWRATEWPAN_ID_LIMIT,"无线ID数量已超限"},
            {EM_ErrorCode.NET_ERROR_LOWRATEWPAN_ID_ABNORMAL,"无线异常添加"},

            {EM_ErrorCode.ERR_INTERNAL_INVALID_CHANNEL,"错误的通道号"},
            {EM_ErrorCode.ERR_INTERNAL_REOPEN_CHANNEL,"打开重复通道"},
            {EM_ErrorCode.ERR_INTERNAL_SEND_DATA,"发送消息失败"},
            {EM_ErrorCode.ERR_INTERNAL_CREATE_SOCKET,"创建socket失败"},
        };
        #endregion //<< constant >>


        #region << C# SDK calls >>

        #region << init and login >>
        /// <summary>
        /// error code conversion error message described as standard equipment
        /// </summary>
        /// <param name="errorCode">SDK error code</param>
        /// <returns>Standard equipment error message description</returns>
        private static string GetLastErrorMessage(EM_ErrorCode errorCode)
        {
            string result = string.Empty;
            switch (System.Globalization.CultureInfo.CurrentCulture.LCID)
            {
                case 0x00804:
                    zh_cn_String.TryGetValue(errorCode, out result);
                    break;
                default:
                    en_us_String.TryGetValue(errorCode, out result);
                    break;
            }
            if (null == result)
            {
                result = errorCode.ToString("X");
            }
            return result;
		}

        /// <summary>
        /// judge the SDK function is failed or successful,if failed throw exception.
        /// </summary>
        /// <typeparam name="T">value type</typeparam>
        /// <param name="value">the value is SDK function returns value,the value must be value type</param>
        private static void NetGetLastError<T>(T value)
            where T:struct
        {
            object temp = value;
            bool isGetLastError = false;
            if (value is IntPtr)
            {
                IntPtr tempValue = (IntPtr)temp;
                if (IntPtr.Zero == tempValue)
                {
                    isGetLastError = true;
                }
            }
            else if (value is int)
            {
                int tempValue = (int)temp;
                if (0 > tempValue)
                {
                    isGetLastError = true;
                }
            }
            else if (value is bool)
            {
                bool tempValue = (bool)temp;
                if (false == tempValue)
                {
                    isGetLastError = true;
                }
            }
            else
            {
                return;
            }
            if (isGetLastError)
            {
                int error = OriginalSDK.CLIENT_GetLastError();
                if (0 != error)
                {
                    string errorMessage = GetLastErrorMessage((EM_ErrorCode)error);
                    throw new NETClientExcetion(error, errorMessage);
                }
            }
        }

        /// <summary>
        /// initialize SDK,can only be called once.Must be called before others SDK function,otherwise others SDK function will fail.
        /// </summary>
        /// <param name="cbDisConnect">disconnect the callback function, see the delegate</param>
        /// <param name="dwUser">user data, there is no data, please use IntPtr.Zero</param>
        /// <param name="initParam">initialization parameter,can input null</param>
        /// <returns>failed return false, successful return true</returns>
        public static bool Init(fDisConnectCallBack cbDisConnect, IntPtr dwUser, NETSDK_INIT_PARAM? stuInitParam)
        {
            bool result = false;
            IntPtr lpInitParam = IntPtr.Zero;
            try
            {
                if (null != stuInitParam)
                {
                    lpInitParam = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NETSDK_INIT_PARAM)));
                    Marshal.StructureToPtr(stuInitParam, lpInitParam, true);
                }
                result = OriginalSDK.CLIENT_InitEx(cbDisConnect, dwUser, lpInitParam);
                NetGetLastError(result);
            }
            finally
            {
                Marshal.FreeHGlobal(lpInitParam);
            }
            return result;
        }

        /// <summary>
        ///  empty SDK, release occupied resource,call after all SDK functions
        /// </summary>
        public static void Cleanup()
        {
            OriginalSDK.CLIENT_Cleanup();
        }

        /// <summary>
        /// registere users to the expansion interface device interface,Support a user to specify equipment support ability
        /// </summary>
        /// <param name="pchDVRIP">device IP</param>
        /// <param name="wDVRPort">device port</param>
        /// <param name="pchUserName">username</param>
        /// <param name="pchPassword">password</param>
        /// <param name="emSpecCap">device supported capacity,when the value is EM_LOGIN_SPAC_CAP_TYPE.SERVER_CONN means active listen mode user login(mobile dvr login)</param>
        /// <param name="pCapParam">nSpecCap compensation parameter，nSpecCap = EM_LOGIN_SPAC_CAP_TYPE.SERVER_CONN，pCapParam fill in device serial number string(mobile dvr login)</param>
        /// <param name="deviceInfo">device information，for output parmaeter</param>
        /// <returns>failed return 0,successful return LoginID,after successful login, device Operation may be via this this value(device handle)corresponding to corresponding device.</returns>
        public static IntPtr Login(string pchDVRIP, ushort wDVRPort, string pchUserName, string pchPassword, EM_LOGIN_SPAC_CAP_TYPE emSpecCap, IntPtr pCapParam, ref NET_DEVICEINFO_Ex deviceInfo)
        {
            IntPtr result = IntPtr.Zero;
            int error = 0;
            result = OriginalSDK.CLIENT_LoginEx2(pchDVRIP, wDVRPort, pchUserName, pchPassword, emSpecCap, pCapParam, ref deviceInfo, ref error);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// log off device user
        /// </summary>
        /// <param name="lLoginID">user LoginID:Login's returns value</param>
        /// <returns>failed return false, successful return true</returns>
        public static bool Logout(IntPtr lLoginID)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_Logout(lLoginID);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// set re-connection callback function after disconnection. Internal SDK  auto connect again after disconnection 
        /// </summary>
        /// <param name="cbAutoConnect">re-connection callback function</param>
        /// <param name="dwUser">user data, there is no data, please use IntPtr.Zero</param>
        public static void SetAutoReconnect(fHaveReConnectCallBack cbAutoConnect, IntPtr dwUser)
        {
            OriginalSDK.CLIENT_SetAutoReconnect(cbAutoConnect, dwUser);
        }

        /// <summary>
        /// set log in network environment
        /// </summary>
        /// <param name="netParam">network environment</param>
        public static void SetNetworkParam(NET_PARAM? netParam)
        {
            if (null == netParam)
            {
                return;
            }
            IntPtr lpNetParam = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_PARAM)));
            Marshal.StructureToPtr(netParam, lpNetParam, true);
            OriginalSDK.CLIENT_SetNetworkParam(lpNetParam);
            Marshal.FreeHGlobal(lpNetParam);
        }
        #endregion

        #region << real-time monitoring >>
        /// <summary>
        /// start real-time monitor
        /// </summary>
        /// <param name="lLoginID">user LoginID:Login's returns value</param>
        /// <param name="nChannelID"><para>real time monitor channel NO.(from 0).</para>
        ///                          <para>if rType is RType_Multiplay, nChannelID is reserved.</para>
        ///                          <para>when rType is RType_Multiplay_1 ~ RType_Multiplay_16, nChannelID determines the preview picture. </para>
        ///                          <para>ex. when RType_Multiplay_4, nChannelID is 4 or 5 or 6 or 7 will display fifth to seventh channels in the four picture preview</para> </param>
        /// <param name="hWnd">display window handle. When value is 0(NULL), data are not decoded or displayed </param>
        /// <param name="rType"></param>
        /// <param name="cbRealData"><seealso cref="SetRealDataCallBack"/></param>
        /// <param name="cbDisconnect">video monitor disconnect callback function</param>
        /// <param name="dwUser">user defined data, used in callback</param>
        /// <param name="dwWaitTime">waiting time</param>
        /// <returns>failed return 0, successful return the real time monitorID(real time monitor handle),as parameter of related function.</returns>
        public static IntPtr StartRealPlay(IntPtr lLoginID, int nChannelID, IntPtr hWnd, EM_RealPlayType rType, fRealDataCallBackEx cbRealData, fRealPlayDisConnectCallBack cbDisconnect, IntPtr dwUser, uint dwWaitTime)
        {
            IntPtr result = IntPtr.Zero;
            result = OriginalSDK.CLIENT_StartRealPlay( lLoginID,  nChannelID,  hWnd,  rType,  cbRealData,  cbDisconnect,  dwUser,  dwWaitTime);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// stop real time monitoring
        /// </summary>
        /// <param name="lRealHandle">monitor handle</param>
        /// <returns>failed return false, successful return true</returns>
        public static bool StopRealPlay(IntPtr lRealHandle)
        {
            bool result = false;
            result =OriginalSDK.CLIENT_StopRealPlayEx(lRealHandle);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// set real-time monitor data callback 
        /// <para>can set the data you need callback in dwFlag</para>
        /// </summary>
        /// <param name="lRealHandle">monitor handle</param>
        /// <param name="cbRealData">callback function</param>
        /// <param name="dwUser">user data, there is no data, please use IntPtr.Zero</param>
        /// <param name="dwFlag">by bit, can combine, when it is 0x1f, callback the five types,as:
        ///                      <para>0x00000001 is equivalent with original data</para>
        ///                      <para>0x00000002 is MPEG4/H264 standard data</para>
        ///                      <para>0x00000004 YUV data</para>
        ///                      <para>0x00000008 PCM data</para>
        ///                      <para>0x00000010 original audio data</para>
        ///                      <para>0x0000001f above five data type</para></param>
        /// <returns>failed return false, successful return true</returns>
        public static bool SetRealDataCallBack(IntPtr lRealHandle, fRealDataCallBackEx cbRealData, IntPtr dwUser, uint dwFlag)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_SetRealDataCallBackEx(lRealHandle, cbRealData, dwUser, dwFlag);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// save data as file
        /// </summary>
        /// <param name="lRealHandle">monitor handle</param>
        /// <param name="pchFileName">save path</param>
        /// <returns>failed return false, successful return true</returns>
        public static bool SaveRealData(IntPtr lRealHandle, string pchFileName)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_SaveRealData(lRealHandle, pchFileName);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// stop saving data as file
        /// </summary>
        /// <param name="lRealHandle">monitor handle</param>
        /// <returns>failed return false, successful return true</returns>
        public static bool StopSaveRealData(IntPtr lRealHandle)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_StopSaveRealData(lRealHandle);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// set snapshot callback function
        /// </summary>
        /// <param name="OnSnapRevMessage">snapshot data callback function</param>
        /// <param name="dwUser">user data, there is no data, please use IntPtr.Zero</param>
        public static void SetSnapRevCallBack(fSnapRevCallBack OnSnapRevMessage, IntPtr dwUser)
        {
            OriginalSDK.CLIENT_SetSnapRevCallBack(OnSnapRevMessage, dwUser);
        }

        /// <summary>
        /// snapshot request
        /// </summary>
        /// <param name="lLoginID">user LoginID:Login's return value</param>
        /// <param name="par">Snapshot parameter(structure)</param>
        /// <param name="reserved"></param>
        /// <returns>failed return false, successful return true</returns>
        public static bool SnapPictureEx(IntPtr lLoginID, NET_SNAP_PARAMS par, IntPtr reserved)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_SnapPictureEx(lLoginID, ref par, reserved);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// playback by time, support playback by direction
        /// </summary>
        /// <param name="lLoginID">user LoginID:Login's return value</param>
        /// <param name="nChannelID">channel number</param>
        /// <param name="pstNetIn">record play back parameter in</param>
        /// <param name="pstNetOut">record play back parameter out</param>
        /// <returns>failed return 0, successful return the playback ID(playback handle),as parameter of related function.</returns>
        public static IntPtr PlayBackByTime(IntPtr lLoginID, int nChannelID, NET_IN_PLAY_BACK_BY_TIME_INFO pstNetIn, ref NET_OUT_PLAY_BACK_BY_TIME_INFO pstNetOut)
        {
            IntPtr result = IntPtr.Zero;
            result = OriginalSDK.CLIENT_PlayBackByTimeEx2(lLoginID, nChannelID, ref pstNetIn, ref pstNetOut);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// Query video files
        /// </summary>
        /// <param name="lLoginID">Device handles user login</param>
        /// <param name="nChannelId">channelID</param>
        /// <param name="nRecordFileType">Video file types </param>
        /// <param name="tmStart">Recording start time</param>
        /// <param name="tmEnd">Recording end time</param>
        /// <param name="pchCardid">card number,Only for card number query effectively,In other cases you can fill NULL</param>
        /// <param name="nriFileinfo">Return to video file information,Is an array of structures NET_RECORDFILE_INFO[Video file information for the specified bar]</param>
        /// <param name="maxlen">nriFileinfoThe maximum length of the buffer;[Unit of byte,Dimensional structure of an array of size number*sizeof(NET_RECORDFILE_INFO),Victoria is the size of the array is equal to 1,Recommend less than 200]</param>
        /// <param name="filecount">The number of documents returned,Maximum output parameters are only found in video recording until the buffer is full</param>
        /// <param name="waittime">Waiting Time</param>
        /// <param name="bTime">Whether by time(Currently use false)</param>
        /// <returns>true:success;false:failure</returns>
        public static bool QueryRecordFile(IntPtr lLoginID, int nChannelId, EM_QUERY_RECORD_TYPE nRecordFileType, DateTime tmStart, DateTime tmEnd, string pchCardid, ref NET_RECORDFILE_INFO[] nriFileinfo, ref int filecount, int waittime, bool bTime)
        {
            bool returnValue = false;
            filecount = 0;
            IntPtr pBoxInfo = IntPtr.Zero;
            int maxlen = Marshal.SizeOf(typeof(NET_RECORDFILE_INFO)) * nriFileinfo.Length;
            try
            {
                NET_TIME timeStart = NET_TIME.FromDateTime(tmStart);
                NET_TIME timeEnd = NET_TIME.FromDateTime(tmEnd);
                pBoxInfo = Marshal.AllocHGlobal(maxlen);//Allocation of fixed specified the size of the memory space
                int fileCountMin = 0;
                if (pBoxInfo != IntPtr.Zero)
                {
                    returnValue = OriginalSDK.CLIENT_QueryRecordFile(lLoginID, nChannelId, (int)nRecordFileType, ref timeStart, ref timeEnd, pchCardid, pBoxInfo, maxlen, ref filecount, waittime, bTime);
                    NetGetLastError(returnValue);
                    fileCountMin = (filecount <= nriFileinfo.Length ? filecount : nriFileinfo.Length);
                    for (int dwLoop = 0; dwLoop < fileCountMin; dwLoop++)
                    {
                        // specify the memory space of the data is copied to the purpose in the array in the specified format
                        nriFileinfo[dwLoop] = (NET_RECORDFILE_INFO)Marshal.PtrToStructure(IntPtr.Add(pBoxInfo, Marshal.SizeOf(typeof(NET_RECORDFILE_INFO)) * dwLoop), typeof(NET_RECORDFILE_INFO));
                    }
                }
            }
            finally
            {
                Marshal.FreeHGlobal(pBoxInfo);//Release the fixed memory allocation
                pBoxInfo = IntPtr.Zero;
            }
            return returnValue;
        }

        public static bool GetPlayBackOsdTime(IntPtr lPlayHandle,ref  NET_TIME lpOsdTime, ref NET_TIME lpStartTime, ref NET_TIME lpEndTime)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_GetPlayBackOsdTime(lPlayHandle,ref  lpOsdTime, ref lpStartTime, ref lpEndTime);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// download video by time
        /// <para>sSavedFileName Is not null, video data written to the corresponding file path;</para>
        /// <para>fDownLoadDataCallBack Is not null, video data through the callback function returns</para>
        /// </summary>
        /// <param name="lLoginID">user LoginID:Login's return value</param>
        /// <param name="nChannelId">channel number</param>
        /// <param name="nRecordFileType"></param>
        /// <param name="tmStart">start time</param>
        /// <param name="tmEnd">end time</param>
        /// <param name="sSavedFileName"></param>
        /// <param name="cbTimeDownLoadPos">download by time's pos callback</param>
        /// <param name="dwUserData">cbTimeDownLoadPos's user data, there is no data, please use IntPtr.Zero</param>
        /// <param name="fDownLoadDataCallBack">video data's callback</param>
        /// <param name="dwDataUser">fDownLoadDataCallBack's user data, there is no data, please use IntPtr.Zero</param>
        /// <param name="pReserved"></param>
        /// <returns>failed return 0, successful return downloadsID,</returns>
        public static IntPtr DownloadByTime(IntPtr lLoginID, int nChannelId, EM_QUERY_RECORD_TYPE nRecordFileType, DateTime tmStart, DateTime tmEnd, string sSavedFileName,
                                                     fTimeDownLoadPosCallBack cbTimeDownLoadPos, IntPtr dwUserData,
                                                     fDataCallBack fDownLoadDataCallBack, IntPtr dwDataUser, IntPtr pReserved)
        {
            IntPtr result = IntPtr.Zero;
            NET_TIME startTime = NET_TIME.FromDateTime(tmStart);
            NET_TIME endTime = NET_TIME.FromDateTime(tmEnd);
            result = OriginalSDK.CLIENT_DownloadByTimeEx(lLoginID, nChannelId, (int)nRecordFileType, ref startTime, ref endTime, sSavedFileName,
                                                     cbTimeDownLoadPos, dwUserData, fDownLoadDataCallBack, dwDataUser, pReserved);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// stop downloads
        /// </summary>
        /// <param name="lFileHandle">downloadsID</param>
        /// <returns>failed return false, successful return true</returns>
        public static bool StopDownload(IntPtr lFileHandle)
        {
            bool result = false;
            result=OriginalSDK.CLIENT_StopDownload(lFileHandle);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// private PTZ control expansion port,support 3D quick positioning, Fish eye 
        /// </summary>
        /// <param name="lLoginID">user LoginID:Login's returns value</param>
        /// <param name="nChannelID">channel number</param>
        /// <param name="dwPTZCommand">PTZ control commands </param>
        /// <param name="lParam1">Parameter1 details refer to EM_EXTPTZ_ControlType</param>
        /// <param name="lParam2">Parameter2 details refer to EM_EXTPTZ_ControlType</param>
        /// <param name="lParam3">Parameter3 details refer to EM_EXTPTZ_ControlType</param>
        /// <param name="dwStop">stop or not, effective to PTZ eight-directions operation and lens operation. During other operation, this parameter should fill in false</param>
        /// <param name="param4"><para>support PTZ control extensive command，support these commands:</para> 
        ///                      <para>EM_EXTPTZ_ControlType.MOVE_ABSOLUTELY:Absolute motion control commands，param4 corresponding struct NET_PTZ_CONTROL_ABSOLUTELY</para>
        ///                      <para>EM_EXTPTZ_ControlType.MOVE_CONTINUOUSLY:Continuous motion control commands，param4 corresponding struct NET_PTZ_CONTROL_CONTINUOUSLY</para>
        ///                      <para>EM_EXTPTZ_ControlType.GOTOPRESET:PTZ control command, at a certain speed to preset locus，parm4 corresponding struct NET_PTZ_CONTROL_GOTOPRESET</para>
        ///                      <para>EM_EXTPTZ_ControlType.SET_VIEW_RANGE:Set to horizon(param4 corresponding struct NET_PTZ_VIEW_RANGE_INFO</para>
        ///                      <para>EM_EXTPTZ_ControlType.FOCUS_ABSOLUTELY:Absolute focus(param4 corresponding struct NET_PTZ_FOCUS_ABSOLUTELY</para>
        ///                      <para>EM_EXTPTZ_ControlType.HORSECTORSCAN:Level fan sweep(param4 corresponding NET_PTZ_CONTROL_SECTORSCAN,param1、param2、param3 is invalid</para>
        ///                      <para>EM_EXTPTZ_ControlType.VERSECTORSCAN:Vertical sweep fan(param4 corresponding NET_PTZ_CONTROL_SECTORSCAN,param1、param2、param3 is invalid</para>
        ///                      <para>EM_EXTPTZ_ControlType.SET_FISHEYE_EPTZ:Control fish eye PTZ，param4corresponding to structure NET_PTZ_CONTROL_SET_FISHEYE_EPTZ</para>
        ///                      <para>EM_EXTPTZ_ControlType.SET_TRACK_START/SET_TRACK_STOP:param4 corresponding to structure NET_PTZ_CONTROL_SET_TRACK_CONTROL,dwStop set as FALSE，param1、param2、param3  is invalid</para></param>
        /// <returns>failed return false, successful return true</returns>
		public static bool PTZControl(IntPtr lLoginID, int nChannelID, EM_EXTPTZ_ControlType dwPTZCommand, int lParam1, int lParam2, int lParam3, bool dwStop, IntPtr param4)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_DHPTZControlEx2(lLoginID, nChannelID, (uint)dwPTZCommand, lParam1, lParam2, lParam3, dwStop, param4);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// open audio
        /// </summary>
        /// <param name="lPlayHandle">real handle or palyback handle
        ///                            <para>StartRealPlay returns value</para>
        ///                            <para>PlayBackByTime returns value</para></param>
        /// <returns>failed return false, successful return true</returns>
        public static bool OpenSound(IntPtr lPlayHandle)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_OpenSound(lPlayHandle);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// stop audio
        /// </summary>
        /// <returns>failed return false, successful return true</returns>
        public static bool CloseSound()
        {
            bool result = false;
            result = OriginalSDK.CLIENT_CloseSound();
            NetGetLastError(result);
            return result;
        }
        #endregion

        /// <summary>
        /// control playback operation.
        /// </summary>
        /// <param name="lPlayHandle">lPlayHandle:palyback handle</param>
        /// <param name="type">control type</param>
        /// <returns>failed return false, successful return true</returns>
        public static bool PlayBackControl(IntPtr lPlayHandle, PlayBackType type)
        {
            bool result = false;
            switch (type)
            {
                case PlayBackType.Play:
                    result = OriginalSDK.CLIENT_PausePlayBack(lPlayHandle, false);
                    NetGetLastError(result);
                    break;
                case PlayBackType.Pause:
                    result = OriginalSDK.CLIENT_PausePlayBack(lPlayHandle, true);
                    NetGetLastError(result);
                    break;
                case PlayBackType.Stop:
                    result = OriginalSDK.CLIENT_StopPlayBack(lPlayHandle);
                    NetGetLastError(result);
                    break;
                case PlayBackType.Fast:
                    result = OriginalSDK.CLIENT_FastPlayBack(lPlayHandle);
                    NetGetLastError(result);
                    break;
                case PlayBackType.Slow:
                    result = OriginalSDK.CLIENT_SlowPlayBack(lPlayHandle);
                    NetGetLastError(result);
                    break;
                case PlayBackType.Normal:
                    result = OriginalSDK.CLIENT_NormalPlayBack(lPlayHandle);
                    NetGetLastError(result);
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// set user work mode
        /// </summary>
        /// <param name="lLoginID">loginID,login returns value</param>
        /// <param name="emType">user work mode</param>
        /// <param name="pValue">support these type:
        ///                     <para>EM_USEDEV_MODE.TALK_ENCODE_TYPE:corresponding structure NET_DEV_TALKDECODE_INFO</para>
        ///                     <para>EM_USEDEV_MODE.ALARM_LISTEN_MODE:value type int(0-16)</para>
        ///                     <para>EM_USEDEV_MODE.CONFIG_AUTHORITY_MODE:value type int(0/1)</para>
        ///                     <para>EM_USEDEV_MODE.TALK_TALK_CHANNEL:value type int(channel number)</para>
        ///                     <para>EM_USEDEV_MODE.TALK_SPEAK_PARAM:corresponding structure NET_SPEAK_PARAM</para>
        ///                     <para>EM_USEDEV_MODE.TALK_TRANSFER_MODE:corresponding structure NET_TALK_TRANSFER_PARAM</para>
        ///                     <para>EM_USEDEV_MODE.PLAYBACK_REALTIME_MODE:value type int(0/1)</para>
        ///                     <para>EM_USEDEV_MODE.RECORD_STREAM_TYPE:value type int(0/1/2)</para>
        ///                     <para>EM_USEDEV_MODE.RECORD_TYPE:see to EM_RECORD_TYPE</para>
        ///                     <para>EM_USEDEV_MODE.TALK_VT_PARAM:corresponding structure NET_VT_TALK_PARAM</para>
        ///                     <para>EM_USEDEV_MODE.TARGET_DEV_ID:value type int (0 or other)</para></param>
        /// <returns>failed return false, successful return true</returns>
        public static bool SetDeviceMode(IntPtr lLoginID, EM_USEDEV_MODE emType, IntPtr pValue)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_SetDeviceMode(lLoginID, emType, pValue);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// set alarm callback function 
        /// </summary>
        /// <param name="cbMessage">alarm callback</param>
        /// <param name="dwUser">user data</param>
        public static void SetDVRMessCallBack(fMessCallBackEx cbMessage, IntPtr dwUser)
        {
            OriginalSDK.CLIENT_SetDVRMessCallBackEx1(cbMessage, dwUser);
        }

        /// <summary>
        /// subscribe alarm
        /// </summary>
        /// <param name="lLoginID">loginID:login returns value</param>
        /// <returns>failed return false, successful return true</returns>
        public static bool StartListen(IntPtr lLoginID)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_StartListenEx(lLoginID);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// unsubscribe alarm
        /// </summary>
        /// <param name="lLoginID">loginID:login returns value</param>
        /// <returns>failed return false, successful return true</returns>
        public static bool StopListen(IntPtr lLoginID)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_StopListen(lLoginID);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lLoginID">loginID:login returns value</param>
        /// <param name="nChannelID">channel id</param>
        /// <param name="dwAlarmType">alarm type see EM_EVENT_IVS_TYPE</param>
        /// <param name="bNeedPicFile">subscribe image file or not,ture-yes,return intelligent image info during callback function,false not return intelligent image info during callback function</param>
        /// <param name="cbAnalyzerData">intelligent data analysis callback</param>
        /// <param name="dwUser">user data</param>
        /// <param name="reserved">reserved</param>
        /// <returns>failed return 0, successful return the analyzerHandle</returns>
        public static IntPtr RealLoadPicture(IntPtr lLoginID, int nChannelID, uint dwAlarmType, bool bNeedPicFile, fAnalyzerDataCallBack cbAnalyzerData, IntPtr dwUser, IntPtr reserved)
        {
            IntPtr result = IntPtr.Zero;
            result = OriginalSDK.CLIENT_RealLoadPictureEx(lLoginID, nChannelID, dwAlarmType, bNeedPicFile, cbAnalyzerData, dwUser, reserved);
            NetGetLastError(result);
            return result;
        }

        /// <summary>
        /// stop uploading intelligent analysis data－image
        /// </summary>
        /// <param name="lAnalyzerHandle">analyzerHandle:RealLoadPicture returns value</param>
        /// <returns>failed return false, successful return true</returns>
        public static bool StopLoadPic(IntPtr lAnalyzerHandle)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_StopLoadPic(lAnalyzerHandle);
            NetGetLastError(result);
            return result;
        }

        private static bool QuerySystemInfo(IntPtr lLoginID, EM_SYS_ABILITY nSystemType, IntPtr pSysInfoBuffer, int maxlen, ref int nSysInfolen, int waittime)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_QuerySystemInfo(lLoginID, (int)nSystemType, pSysInfoBuffer, maxlen, ref nSysInfolen, waittime);
            NetGetLastError(result);
            return result;
        }

        public static bool QuerySystemInfo(IntPtr lLoginID, EM_SYS_ABILITY nSystemType,ref object obj, int waittime)
        {
            bool result = false;
            IntPtr pBuf = IntPtr.Zero;
            Type type = obj.GetType();
            int len = Marshal.SizeOf(obj);
            int nSysInfolen = 0;
            try
            {
                pBuf = Marshal.AllocHGlobal(len);
                Marshal.StructureToPtr(obj, pBuf, true);
                result = QuerySystemInfo(lLoginID, nSystemType, pBuf, len, ref nSysInfolen, waittime);
                obj = Marshal.PtrToStructure(pBuf, type);
            }
            finally
            {
                Marshal.FreeHGlobal(pBuf);
            }
            return result;
        }


        public static bool QueryDeviceLog(IntPtr lLoginID, ref NET_QUERY_DEVICE_LOG_PARAM pQueryParam, IntPtr pLogBuffer, int nLogBufferLen, ref int pRecLogNum, int waittime)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_QueryDeviceLog(lLoginID, ref pQueryParam, pLogBuffer, nLogBufferLen, ref pRecLogNum, waittime);
            NetGetLastError(result);
            return result;
        }

        public static IntPtr StartTalk(IntPtr lLoginID, fAudioDataCallBack pfcb, IntPtr dwUser)
        {
            IntPtr result = IntPtr.Zero;
            result = OriginalSDK.CLIENT_StartTalkEx(lLoginID, pfcb, dwUser);
            NetGetLastError(result);
            return result;
        }

        public static bool StopTalk(IntPtr lTalkHandle)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_StopTalkEx(lTalkHandle);
            NetGetLastError(result);
            return result;
        }

        public static bool RecordStart(IntPtr lLoginID)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_RecordStartEx(lLoginID);
            NetGetLastError(result);
            return result;
        }

        public static bool RecordStop(IntPtr lLoginID)
        {
            bool result = false;
            result = OriginalSDK.CLIENT_RecordStopEx(lLoginID);
            NetGetLastError(result);
            return result;
        }

        public static int TalkSendData(IntPtr lTalkHandle, IntPtr pSendBuf, uint dwBufSize)
        {
            int result = 0;
            result = OriginalSDK.CLIENT_TalkSendData(lTalkHandle, pSendBuf, dwBufSize);
            NetGetLastError(result);
            return result;
        }

        public static void AudioDec(IntPtr pAudioDataBuf, uint dwBufSize)
        {
            OriginalSDK.CLIENT_AudioDec(pAudioDataBuf, dwBufSize);
        }

        #endregion //<< C# SDK calls >>







    }

    /// <summary>
    /// throw SDK exception Class
    /// </summary>
    public class NETClientExcetion : Exception
    {
        /// <summary>
        /// SDK error code property
        /// </summary>
        public int ErrorCode { get;private set; }

        /// <summary>
        /// SDK error message property
        /// </summary>
        new public string Message { get; private set; }

        /// <summary>
        /// construct function.
        /// </summary>
        /// <param name="errorCode">SDK error code number</param>
        /// <param name="message">SDK error message</param>
        public NETClientExcetion(int errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }

    #region << delegate >>

    /// <summary>
    /// network disconnection callback function original shape
    /// </summary>
    /// <param name="lLoginID">user LoginID:Login's returns value</param>
    /// <param name="pchDVRIP">device IP</param>
    /// <param name="nDVRPort">device prot</param>
    /// <param name="dwUser">user data from Init function</param>
    public delegate void fDisConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser);

    /// <summary>
    /// network re-connection callback function original shape
    /// </summary>
    /// <param name="lLoginID">user LoginID:Login's returns value</param>
    /// <param name="pchDVRIP">device IP,string type</param>
    /// <param name="nDVRPort">device prot</param>
    /// <param name="dwUser">user data from SetAutoReconnect function</param>
    public delegate void fHaveReConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser);
	
    /// <summary>
    /// real-time monitor data callback function original shape---extensive
    /// </summary>
    /// <param name="lRealHandle">monitor handle</param>
    /// <param name="dwDataType">callback data type ,only data set in dwFlag will be callback：
    ///                         <para>0 original data (identicla SaveRealData saveddata)</para>
    ///                         <para>1 frame data</para>
    ///                         <para>2 yuv data</para>
    ///                         <para>3 pcm audio data</para></param>
    /// <param name="pBuffer">byte array, length is dwBufSize
    ///                      <para>callback data, except type 0, other type is base on frame, one frame data per callback</para></param>
    /// <param name="dwBufSize">pBuffer's size</param>
    /// <param name="param">pointer to parameter structure,based on different type 
    ///                    <para>if type is 0(original) or 2(yuv), param is 0</para>
    ///                    <para>if callback data is frame data, pointer to NET_VideoFrameParam</para>
    ///                    <para>if callback data is PCM data, pointer to NET_CBPCMDataParam</para></param>
    /// <param name="dwUser">user data,which input above</param>
	public delegate void fRealDataCallBackEx(IntPtr lRealHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, int param, IntPtr dwUser);
	
    /// <summary>
    /// monitor disconnect callback function
    /// </summary>
    /// <param name="lOperateHandle">monitoring handle</param>
    /// <param name="dwEventType">event type</param>
    /// <param name="param">event parameter,currently not used</param>
    /// <param name="dwUser">user data,which input above</param>
    public delegate void fRealPlayDisConnectCallBack(IntPtr lRealHandle, EM_REALPLAY_DISCONNECT_EVENT_TYPE dwEventType, IntPtr param, IntPtr dwUser);

    /// <summary>
    /// snapshot callback function original shape,
    /// </summary>
    /// <param name="lLoginID">loginID,login returns value</param>
    /// <param name="pBuf">byte array, length is RevLen
    ///                    <para>pointer to data</para></param>
    /// <param name="RevLen">pBuf's size</param>
    /// <param name="EncodeType">image encode type：0：mpeg4 I frame;10：jpeg </param>
    /// <param name="CmdSerial">operation NO.,not used in Synchronous capture conditions</param>
    /// <param name="dwUser">user data,which input above</param>
    public delegate void fSnapRevCallBack(IntPtr lLoginID, IntPtr pBuf, uint RevLen, uint EncodeType, uint CmdSerial, IntPtr dwUser);

    /// <summary>
    /// vt event callback
    /// </summary>
    /// <param name="instId">vt instance id</param>
    /// <param name="ulRegisterId">register id</param>
    /// <param name="ulSessionId">session id</param>
    /// <param name="nEvent">evnet see EM_AUDIO_CB_FLAG</param>
    /// <param name="pDataBuf">date buffer,IntPtr.Zero now</param>
    /// <param name="dwBufSize">data size,0 now</param>
    /// <param name="dwUser">user data</param>
    /// <returns>reserved</returns>
    public delegate int fVtEventCallBack(IntPtr instId, IntPtr ulRegisterId, IntPtr ulSessionId, int nEvent, IntPtr pDataBuf, uint dwBufSize, IntPtr dwUser);

    /// <summary>
    /// play back progress's data callback function
    /// </summary>
    /// <param name="lPlayHandle">playback handle</param>
    /// <param name="dwTotalSize">total size of this play(kb)</param>
    /// <param name="dwDownLoadSize">played size(kb)
    ///                             <para>-1:playback has over</para>
    ///                             <para>-2:write file failed</para></param>
    /// <param name="dwUser">user data,which input above</param>
    public delegate void fDownLoadPosCallBack(IntPtr lPlayHandle, uint dwTotalSize, uint dwDownLoadSize, IntPtr dwUser);

    /// <summary>
    /// playback data callback function
    /// <para>pBuffer: data buffer, memory malloc are free was managed by SDK interior</para>
    /// </summary>
    /// <param name="lRealHandle">playback handle</param>
    /// <param name="dwDataType"></param>
    /// <param name="pBuffer">byte array, length is dwBufSize</param>
    /// <param name="dwBufSize">pBuffer's size</param>
    /// <param name="dwUser">user data,which input above</param>
    /// <returns>reserved</returns>
    public delegate int fDataCallBack(IntPtr lRealHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, IntPtr dwUser);

    /// <summary>
    /// download progress callback function
    /// </summary>
    /// <param name="lPlayHandle">playback handle</param>
    /// <param name="dwTotalSize">total size of this play(kb)</param>
    /// <param name="dwDownLoadSize">played size(kb)</param>
    /// <param name="index">file index</param>
    /// <param name="recordfileinfo">record file information</param>
    /// <param name="dwUser">user data,which input above</param>
    public delegate void fTimeDownLoadPosCallBack(IntPtr lPlayHandle, uint dwTotalSize, uint dwDownLoadSize, int index, NET_RECORDFILE_INFO recordfileinfo, IntPtr dwUser);

    /// <summary>
    /// alarm message callback function original shape
    /// </summary>
    /// <param name="lCommand">alarm type,see EM_ALARM_TYPE</param>
    /// <param name="lLoginID">loginID:login returns value</param>
    /// <param name="pBuf">alarm data</param>
    /// <param name="dwBufLen">alarm data length</param>
    /// <param name="pchDVRIP">device ip,string type</param>
    /// <param name="nDVRPort">device port</param>
    /// <param name="bAlarmAckFlag">true:the event is affirmable event;false:the event is not affirmable event</param>
    /// <param name="nEventID">used to AlarmAck function,when the bAlarmAckFlag is true,this paramter is valid</param>
    /// <param name="dwUser">user data from SetDVRMessCallBack function</param>
    /// <returns>reserved</returns>
    public delegate bool fMessCallBackEx(int lCommand, IntPtr lLoginID, IntPtr pBuf, uint dwBufLen, IntPtr pchDVRIP, int nDVRPort, bool bAlarmAckFlag, int nEventID, IntPtr dwUser);

    /// <summary>
    /// intelligent analysis data callback
    /// </summary>
    /// <param name="lAnalyzerHandle">analyzerHandle:RealLoadPicture returns value</param>
    /// <param name="dwEventType">event type,see EM_EVENT_IVS_TYPE</param>
    /// <param name="pEventInfo">event information</param>
    /// <param name="pBuffer">alarm data</param>
    /// <param name="dwBufSize">alarm data size</param>
    /// <param name="dwUser">user data from RealLoadPicture function</param>
    /// <param name="nSequence">means status of the same uploaded image, when it is 0, it appears first time.When it is 2, it appears last time or appears once.When it is 1, it will appear again.</param>
    /// <param name="reserved">int nState = (int) reserved means current callback data status;when it is 1, it means current data is real time and current callback data is offline;when it is 2,it means offline data send structure</param>
    /// <returns>reserved</returns>
    public delegate int fAnalyzerDataCallBack(IntPtr lAnalyzerHandle, uint dwEventType, IntPtr pEventInfo, IntPtr pBuffer, uint dwBufSize, IntPtr dwUser, int nSequence, IntPtr reserved);


    public delegate void fAudioDataCallBack(IntPtr lTalkHandle, IntPtr pDataBuf, uint dwBufSize, byte byAudioFlag, IntPtr dwUser);
    #endregion //<< delegate >>

    
}
