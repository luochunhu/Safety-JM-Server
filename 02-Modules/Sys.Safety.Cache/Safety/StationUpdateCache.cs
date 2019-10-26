using Basic.Framework.Logging;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.DataContract;
using Sys.Safety.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Cache.Safety
{
    public class StationUpdateCache
    {
        #region ----变量定义----

        /// <summary>
        /// 分站内存
        /// </summary>
        private static StationUpdateItem[] stationItems = new StationUpdateItem[256];
        /// <summary>
        /// 预置状态
        /// </summary>
        //private static string[] stateItems = new string[] { "未知", "请求中", "等待文件下发", "文件接收中", "文件接收完成", "重启升级中", "升级完成", "取消升级中", "请求成功", "重启升级成功", "取消升级成功", "恢复备份成功", "恢复备份中", "获取版本信息", "获取版本信息成功", "请求失败", "取消升级失败", "巡检接收情况", "补发" };
        /// <summary>
        /// 待下发的数据文件Buffer
        /// </summary>
        private static byte[] updateBuffer = new byte[1];
        /// <summary>
        /// 设备类型编码
        /// </summary>
        private static int updatetTypeid;
        /// <summary>
        /// 硬件编码
        /// </summary>
        private static int updateHardVersion;
        /// <summary>
        /// 升级文件版本号
        /// </summary>
        private static int updateFileVersion;
        /// <summary>
        /// 版本上限
        /// </summary>
        private static int updateMaxVersion;
        /// <summary>
        /// 版本下限
        /// </summary>
        private static int updateMinVersion;
        /// <summary>
        /// 升级文件总片数
        /// </summary>
        private static int updateCount;
        /// <summary>
        /// CRC值
        /// </summary>
        private static long crcValue;

        #endregion
        public static void LoadUpdateBuffer(byte[] _updateBuffer, int _updatetTypeid, int _updateHardVersion, int _updateFileVersion, int _updateMaxVersion, int _updateMinVersion, int _updateCount, uint _crcValue)
        {
            updateBuffer = _updateBuffer;
            updatetTypeid = _updatetTypeid;
            updateHardVersion = _updateHardVersion;
            updateFileVersion = _updateFileVersion;
            updateMaxVersion = _updateMaxVersion;
            updateMinVersion = _updateMinVersion;
            updateCount = _updateCount;
            crcValue = _crcValue;

            IniStationItems();
        }

        private static void IniStationItems()
        {
            for (int i = 0; i < stationItems.Length; i++)
            {
                stationItems[i] = new StationUpdateItem();
                stationItems[i].fzh = i + 1;
                stationItems[i].updateState = StationUpdateState.unKnowm;
                stationItems[i].nowNeedSendBuffIndex = 1;
                stationItems[i].stationWorkState = new StationWorkState();
                stationItems[i].stationWorkState.softVersion = 0;
                stationItems[i].stationWorkState.hardVersion = 0;
                stationItems[i].stationWorkState.updateState= -1;
                stationItems[i].stationWorkState.devTypeID = 0;
                stationItems[i].isUpdate = false;
            }
        }

        #region ----生成命令下发Buffer----

        /// <summary>
        /// 表示广播的远程升级命令
        /// </summary>
        /// <param name="index">要获取的下发包下标</param>
        /// <returns></returns>
        public static byte[] GetBuffer_0(int index)
        {
            byte[] sendBuffer = new byte[1];

            

            return sendBuffer;
        }
        /// <summary>
        /// 中心站请求远程升级
        /// </summary>
        /// <returns></returns>
        public static byte[] GetBuffer_1(int fzh)
        {
            byte[] sendBuffer = new byte[19];

            IniSendBufferHeader(ref sendBuffer, fzh, 1);
            //升级版本控制	2 Byte  字节9：升级版本上限；字节10：升级版本下限；
            sendBuffer[9] = (byte)updateMaxVersion;
            sendBuffer[10] = (byte)updateMinVersion;
            //升级文件总片数	2 Byte 高在前，低在后
            sendBuffer[11] = (byte)((updateCount >> 8) & 0xFF);
            sendBuffer[12] = (byte)(updateCount & 0xFF);
            //文件校验	4 Byte 高在前，低在后
            sendBuffer[13] = (byte)((crcValue >> 24) & 0xFF);
            sendBuffer[14] = (byte)((crcValue >> 16) & 0xFF);
            sendBuffer[15] = (byte)((crcValue >> 8) & 0xFF);
            sendBuffer[16] = (byte)((crcValue >> 0) & 0xFF);
            //累加和 2 Byte  高在前，低在后
            int ljh = GetLJL(sendBuffer, 0);
            sendBuffer[sendBuffer.Length - 2] = (byte)((ljh >> 8) & 0xFF);
            sendBuffer[sendBuffer.Length - 1] = (byte)((ljh >> 0) & 0xFF);

            return sendBuffer;
        }
        /// <summary>
        /// 中心站巡检分站的文件接收情况
        /// </summary>
        /// <returns></returns>
        public static byte[] GetBuffer_4(int fzh)
        {
            byte[] sendBuffer = new byte[1];
            StationUpdateItem item = stationItems[fzh - 1];
            if (item.updateState == StationUpdateState.reiciveCheck)
            {
                #region ----巡檢----

                sendBuffer = new byte[13];

                IniSendBufferHeader(ref sendBuffer, fzh, 4);
                //缺失文件编号	2 Byte 若该字段=0，则表示查询分站的文件接收情况，后续无文件数据体字段。
                sendBuffer[9] = 0;
                sendBuffer[10] = 0;
                //文件数据体	256Byte
                //累加和	2 Byte
                int ljh = GetLJL(sendBuffer, 0);
                sendBuffer[sendBuffer.Length - 2] = (byte)((ljh >> 8) & 0xFF);
                sendBuffer[sendBuffer.Length - 1] = (byte)((ljh >> 0) & 0xFF);

                #endregion
            }
            else if (item.updateState == StationUpdateState.replacement)
            {
                #region ----补发----

                sendBuffer = new byte[269];

                IniSendBufferHeader(ref sendBuffer, fzh, 4);
                //缺失文件编号	2 Byte 若该字段=0，则表示查询分站的文件接收情况，后续无文件数据体字段。
                sendBuffer[9] = 0;
                sendBuffer[10] = 0;
                //文件数据体	256Byte
                byte[] fileBuffer = GetBuffer_0(item.nowNeedSendBuffIndex - 1);
                for (int i = 0; i < fileBuffer.Length; i++)
                {
                    sendBuffer[11 + i] = fileBuffer[i];
                }
                //累加和	2 Byte
                int ljh = GetLJL(sendBuffer, 0);
                sendBuffer[sendBuffer.Length - 2] = (byte)((ljh >> 8) & 0xFF);
                sendBuffer[sendBuffer.Length - 1] = (byte)((ljh >> 0) & 0xFF);

                #endregion
            }
            return sendBuffer;
        }
        /// <summary>
        /// 中心站告知分站重启并升级
        /// </summary>
        /// <returns></returns>
        public static byte[] GetBuffer_5(int fzh)
        {
            byte[] sendBuffer = new byte[11];
            IniSendBufferHeader(ref sendBuffer, fzh, 5);
            //累加和	2 Byte
            int ljh = GetLJL(sendBuffer, 0);
            sendBuffer[sendBuffer.Length - 2] = (byte)((ljh >> 8) & 0xFF);
            sendBuffer[sendBuffer.Length - 1] = (byte)((ljh >> 0) & 0xFF);
            return sendBuffer;
        }
        /// <summary>
        /// 停止升级
        /// </summary>
        /// <param name="fzh"></param>
        /// <returns></returns>
        public static byte[] GetBuffer_6(int fzh)
        {
            byte[] sendBuffer = new byte[11];
            IniSendBufferHeader(ref sendBuffer, fzh, 6);
            sendBuffer[6] = 0;//0：强制中止任何设备类型的升级流程
            sendBuffer[7] = 0;//0：强制任何类型的设备恢复备份
            sendBuffer[8] = 0;//0：强制中止任何升级文件版本号的升级流程
            //累加和	2 Byte
            int ljh = GetLJL(sendBuffer, 0);
            sendBuffer[sendBuffer.Length - 2] = (byte)((ljh >> 8) & 0xFF);
            sendBuffer[sendBuffer.Length - 1] = (byte)((ljh >> 0) & 0xFF);
            return sendBuffer;
        }
        /// <summary>
        /// 中心站告知分站恢复最近一次备份
        /// </summary>
        /// <returns></returns>
        public static byte[] GetBuffer_7(int fzh)
        {
            byte[] sendBuffer = new byte[11];
            IniSendBufferHeader(ref sendBuffer, fzh, 7);
            //累加和	2 Byte
            int ljh = GetLJL(sendBuffer, 0);
            sendBuffer[sendBuffer.Length - 2] = (byte)((ljh >> 8) & 0xFF);
            sendBuffer[sendBuffer.Length - 1] = (byte)((ljh >> 0) & 0xFF);
            return sendBuffer;
        }
        /// <summary>
        /// 中心站查询分站信息
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="dataMark">Bit0：=1表示获取软件版本号 - Bit1：=1表示获取远程升级状态 - Bit2：=1 表示获取设备类型 -  Bit3：=1表示获取设备硬件版本号</param>
        /// <returns></returns>
        public static byte[] GetBuffer_8(int fzh, int dataMark)
        {
            byte[] sendBuffer = new byte[8];
            //分站号	1 Byte
            sendBuffer[0] = (byte)fzh;
            //长度	2 Byte
            sendBuffer[1] = (byte)((sendBuffer.Length >> 8) & 0xFF);
            sendBuffer[2] = (byte)(sendBuffer.Length & 0xFF);
            //0x44	1 Byte
            sendBuffer[3] = 0x44;
            //标志位	1 Byte =3表示分站远程升级相关命令
            sendBuffer[4] = 3;
            //状态标志位	1 Byte	=8中心站查询分站信息；
            sendBuffer[5] = 8;
            //数据域标记字1	1Byte
            sendBuffer[6] = (byte)dataMark;
            //累加和	2 Byte
            int ljh = GetLJL(sendBuffer, 0);
            sendBuffer[sendBuffer.Length - 2] = (byte)((ljh >> 8) & 0xFF);
            sendBuffer[sendBuffer.Length - 1] = (byte)((ljh >> 0) & 0xFF);

            return sendBuffer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="fzh"></param>
        /// <param name="stateMark"> 1中心站请求远程升级,4中心站巡检分站的文件接收情况,5中心站告知分站重启并升级,6中心站告知分站该次升级过程中止,7中心站告知分站恢复最近一次备份,8中心站查询分站信息；</param>
        private static void IniSendBufferHeader(ref byte[] buffer, int fzh, int stateMark)
        {
            //分站号	1 Byte
            buffer[0] = (byte)fzh;
            //长度	2 Byte
            buffer[1] = (byte)((buffer.Length >> 8) & 0xFF);
            buffer[2] = (byte)(buffer.Length & 0xFF);
            //0x44	1 Byte
            buffer[3] = 0x44;
            //标志位	1 Byte =3表示分站远程升级相关命令
            buffer[4] = 3;
            //状态标志位	1 Byte  
            buffer[5] = (byte)stateMark;
            //设备编码	1 Byte
            buffer[6] = (byte)updatetTypeid;
            //硬件版本号	1 Byte
            buffer[7] = (byte)updateHardVersion;
            //升级文件版本号	1 Byte
            buffer[8] = (byte)updateFileVersion;
        }

        #endregion

        #region ----解析分站回发Buffer----
        public static void DataProc(byte[] buffer)
        {
            int fzh = buffer[0];
            int order = buffer[4];//0x44，D命令
            int mark = buffer[6];//3表示分站远程升级相关命令
            StationUpdateState state = StationUpdateState.unKnowm;
            string msg = "";
            if (fzh < stationItems.Length - 1 && order == 0x44 && mark == 3)
            {
                GetStateFromByte(buffer[7], ref state, ref msg);

                stationItems[fzh - 1].updateState = state;
                stationItems[fzh - 1].msg = msg;
                if (state == StationUpdateState.fileMissing)
                {
                    #region ----解析后续缺少序号----
                    
                    #endregion
                }
            }
            else
            {
                LogHelper.Error("回发数据包类型错误！");
            }

        }
        #endregion

        #region ----辅助函数----

        /// <summary>
        /// 累加和计算
        /// </summary>
        /// <param name="_buffer"></param>
        /// <param name="startIndex">从此位开始计算</param>
        /// <returns></returns>
        private static int GetLJL(byte[] _buffer, int startIndex)
        {
            int ljh = 0;
            for (int i = startIndex; i < (_buffer.Length - 2); i++)
            {
                ljh += _buffer[27 + i];//从分站号开始计算
            }
            return ljh;
        }
        /// <summary>
        /// 获取升级状态描述
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string GetStateStrByState(int state)
        {
            string str = "-";

            try
            {
                str = EnumHelper.GetEnumDescription((StationUpdateState)state);
            }
            catch
            {
                str = "-";
            }

            return str;
        }

        private static void GetStateFromByte(byte data, ref StationUpdateState state, ref string msg)
        {
            int bit0_3 = 0;
            int bit4_7 = 0;
            bit4_7 = (data >> 4) & 0xFF;
            bit0_3 = data & 0x0F;

            if (bit4_7 == 1)
            {
                #region ----=1中心站请求远程升级----
                if (bit0_3 == 0)
                {
                    state = StationUpdateState.requestSuccess;
                    msg = "请求升级成功";
                }
                else
                {
                    state = StationUpdateState.requestFailure;
                    if (bit0_3 == 1)
                    {
                        //=1：升级文件不属于该类设备（不能升级）
                        msg = "升级文件不属于该类设备";
                    }
                    else if (bit0_3 == 2)
                    {
                        //=2：升级条件不满足（不能升级）
                        msg = "升级条件不满足";
                    }
                    else if (bit0_3 == 3)
                    {
                        //=3：本地代码存储空间过小（不能升级）
                        msg = "本地代码存储空间过小";
                    }
                    else if (bit0_3 == 4)
                    {
                        //=4：分站已处于远程升级模式且升级软件版本号与本次请求一致（分站将不更新已接收数据）；
                        msg = "分站已处于远程升级模式且升级软件版本号与本次请求一致";
                    }
                    else if (bit0_3 == 5)
                    {
                        //=5：分站已处于升级模式且升级软件版本号与本次请求不匹配（中心站若想升级该分站，应下发强制中止当前升级流程再重新下发本次升级请求。）
                        msg = "分站已处于升级模式且升级软件版本号与本次请求不匹配";
                    }
                    else if (bit0_3 == 6)
                    {
                        //=6：本地存储器不可靠，最近一次升级失败（不能升级）
                        msg = "本地存储器不可靠，最近一次升级失败";
                    }
                    else if (bit0_3 == 7)
                    {
                        //=7：分站还未做好升级准备，稍后再试；（不能升级）
                        msg = "分站还未做好升级准备，稍后再试";
                    }
                    else if (bit0_3 == 8)
                    {
                        //=8：硬件版本号与本地不匹配（不能升级）
                        msg = "硬件版本号与本地不匹配";
                    }
                }
                #endregion
            }
            else if (bit4_7 == 4)
            {
                #region ----=4中心站巡检分站的文件接收情况----
                if (bit0_3 == 0)
                {
                    state = StationUpdateState.reciveComplete;
                    msg = "升级文件已接收完毕";
                }
                else
                {
                    state = StationUpdateState.error;
                    if (bit0_3 == 1)
                    {
                        //=1：升级文件不属于该类设备（不能升级）
                        msg = "升级文件不属于该类设备";
                    }
                    else if (bit0_3 == 2)
                    {
                        //=2：请求的升级文件版本号与本地的升级文件版本号不匹配；（不能升级）
                        msg = "请求的升级文件版本号与本地的升级文件版本号不匹配";
                    }
                    else if (bit0_3 == 3)
                    {
                        //=3：该设备不处于升级模式（不能升级）
                        msg = "该设备不处于升级模式";
                    }
                    else if (bit0_3 == 4)
                    {
                        //=4：升级文件缺失（不能升级，后续有缺失文件编号字段）
                        msg = "升级文件缺失";
                        state = StationUpdateState.fileMissing;
                    }
                    else if (bit0_3 == 5)
                    {
                        //=5：文件编号异常；
                        msg = "文件编号异常";
                    }
                    else if (bit0_3 == 6)
                    {
                        //=6：硬件版本号不匹配（不能升级）
                        msg = "硬件版本号不匹配";
                    }
                }
                #endregion
            }
            else if (bit4_7 == 5)
            {
                #region ----=5中心站告知分站重启并升级----
                if (bit0_3 == 0)
                {
                    state = StationUpdateState.restart;
                    msg = "分站准备重启并升级";
                }
                else
                {
                    state = StationUpdateState.error;
                    if (bit0_3 == 1)
                    {
                        //=1：升级文件不属于该类设备（不能升级）
                        msg = "升级文件不属于该类设备";
                    }
                    else if (bit0_3 == 2)
                    {
                        //=2：请求的升级文件版本号与本地的升级文件版本号不匹配；（不能升级）
                        msg = "请求的升级文件版本号与本地的升级文件版本号不匹配";
                    }
                    else if (bit0_3 == 3)
                    {
                        //=3：该分站不处于升级模式（不能升级）
                        msg = "该分站不处于升级模式";
                    }
                    else if (bit0_3 == 4)
                    {
                        //=4：该分站升级文件缺失；（不能升级）
                        msg = "该分站升级文件缺失";
                    }
                    else if (bit0_3 == 5)
                    {
                        //=5：文件校验失败（不能升级）
                        msg = "文件校验失败";
                    }
                    else if (bit0_3 == 6)
                    {
                        //=6：硬件版本号不匹配（不能升级）
                        msg = "硬件版本号不匹配";
                    }
                }
                #endregion
            }
            else if (bit4_7 == 6)
            {
                #region ----=6中心站告知分站该次升级过程中止----
                if (bit0_3 == 0)
                {
                    state = StationUpdateState.updateCancleSuccess;
                    msg = "分站已中止该次升级";
                }
                else
                {
                    state = StationUpdateState.error;
                    if (bit0_3 == 1)
                    {
                        //=1：设备类型不匹配（未能中止）
                        msg = "设备类型不匹配";
                    }
                    else if (bit0_3 == 2)
                    {
                        //=2：升级文件版本号不匹配（未能中止）
                        msg = "升级文件版本号不匹配";
                    }
                    else if (bit0_3 == 3)
                    {
                        //=3：硬件版本号不匹配（未能中止）
                        msg = "硬件版本号不匹配";
                    }
                }
                #endregion
            }
            else if (bit4_7 == 7)
            {
                #region ----=7中心站告知分站恢复最近一次备份----
                if (bit0_3 == 0)
                {
                    state = StationUpdateState.restore;
                    msg = "分站已准备恢复操作";
                }
                else
                {
                    state = StationUpdateState.error;
                    if (bit0_3 == 1)
                    {
                        //=1：设备类型不匹配（未能恢复）
                        msg = "设备类型不匹配";
                    }
                    else if (bit0_3 == 2)
                    {
                        //=2：版本号不匹配（未能恢复）
                        msg = "版本号不匹配";
                    }
                    else if (bit0_3 == 3)
                    {
                        //=3：该分站未进行备份，不能恢复；
                        msg = "该分站未进行备份";
                    }
                    else if (bit0_3 == 4)
                    {
                        //=4：分站最近因存储器不可靠而升级失败，不能恢复
                        msg = "分站最近因存储器不可靠而升级失败";
                    }
                    else if (bit0_3 == 5)
                    {
                        //=5：硬件版本号不匹配（未能恢复）
                        msg = "硬件版本号不匹配";
                    }
                }
                #endregion
            }
            else if (bit4_7 == 8)
            {
                #region ----=8中心站查询分站信息----
                if (bit0_3 == 0)
                {
                    state = StationUpdateState.normal;
                    msg = "分站状态正常";
                }
                else
                {
                    state = StationUpdateState.error;
                    msg = "未知故障码：" + bit4_7;
                }
                #endregion
            }
        }

        private static StationUpdateItem DeepCopy(StationUpdateItem _obj)
        {
            BinaryFormatter BF2 = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                BF2.Serialize(stream, _obj);
                stream.Position = 0;
                return (StationUpdateItem)BF2.Deserialize(stream);
            }
        }

        #endregion

        #region ----对外接口----
        /// <summary>
        /// 用户操作
        /// </summary>
        /// <param name="order">1 请求升级，2 下发文件，3 重启升级，4 取消升级，5 版本还原，6 获取版本</param>
        /// <param name="fzh"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static bool UpdateStationItemForUser(int order, int fzh, ref string errorMsg)
        {
            bool success = true;

            try
            {
                if (stationItems[fzh - 1] == null)
                {
                    errorMsg = "未请求初升级。";
                    stationItems[fzh - 1].Info = errorMsg;
                    return false;
                }
                #region ----命令切换----
                if (order == 1)
                {
                    //请求升级
                    stationItems[fzh - 1].updateState = StationUpdateState.requesting;
                    stationItems[fzh - 1].isUpdate = true;
                    stationItems[fzh - 1].Info = "";
                }
                else if (order == 2)
                {
                    if (stationItems[fzh - 1].updateState != StationUpdateState.requestSuccess)
                    {
                        errorMsg = "当前设备未准备就绪。";
                        stationItems[fzh - 1].Info = errorMsg;
                        return false;
                    }
                    else
                    {
                        //文件接收中
                        stationItems[fzh - 1].updateState = StationUpdateState.recivingFile;
                        stationItems[fzh - 1].isUpdate = true;
                        stationItems[fzh - 1].Info = "";
                    }
                }
                else if (order == 3)
                {
                    //重启升级
                    stationItems[fzh - 1].updateState = StationUpdateState.restart;
                    stationItems[fzh - 1].isUpdate = true;
                    stationItems[fzh - 1].Info = "";
                }
                else if (order == 4)
                {
                    //取消升级
                    stationItems[fzh - 1].updateState = StationUpdateState.updateCancle;
                    stationItems[fzh - 1].isUpdate = false;
                    stationItems[fzh - 1].Info = "";
                }
                else if (order == 5)
                {
                    //版本还原
                    stationItems[fzh - 1].updateState = StationUpdateState.restore;
                    stationItems[fzh - 1].isUpdate = true;
                    stationItems[fzh - 1].Info = "";
                }
                else if (order == 6)
                {
                    //获取版本
                    stationItems[fzh - 1].updateState = StationUpdateState.getVersion;
                    stationItems[fzh - 1].isUpdate = true;
                    stationItems[fzh - 1].Info = "";
                }
                #endregion
            }
            catch (Exception ex)
            {
                success = false;
                errorMsg = ex.Message;
            }

            return success;
        }

        /// <summary>
        /// 只更析 updateState、nowNeedSendBuffIndex、lastSendTime、stationWorkState
        /// </summary>
        /// <param name="item"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static bool UpdateStationItemForSys(StationUpdateItem item, ref string errorMsg)
        {
            bool success = true ;

            try
            {
                stationItems[item.fzh - 1].updateState = item.updateState;
                stationItems[item.fzh - 1].nowNeedSendBuffIndex = item.nowNeedSendBuffIndex;
                stationItems[item.fzh - 1].lastSendTime = item.lastSendTime;
                stationItems[item.fzh - 1].stationWorkState = item.stationWorkState;
                stationItems[item.fzh - 1].Info = item.Info;
            }
            catch (Exception ex)
            {
                LogHelper.Error("UpdateStationItemForSys Error:" + ex.Message);
                success = false;
                errorMsg = ex.Message;
            }

            return success;
        }

        public static StationUpdateItem GetStationItem(int fzh)
        {
            StationUpdateItem item = null;
            if (fzh >= 0 && fzh < stationItems.Length - 1)
            {
                if (stationItems[fzh - 1] == null)
                {
                    return item;
                }
                item = DeepCopy(stationItems[fzh - 1]);
                item.isSendBuffer = false;
                switch (item.updateState)
                {
                    case StationUpdateState.requesting://下发请求升级    下发请求升级
                        StationUpdateRequest stationUpdateRequest = new StationUpdateRequest();
                        stationUpdateRequest.DeviceCode = fzh.ToString().PadLeft(3, '0') + "0000";
                        stationUpdateRequest.DeviceId = (byte)updatetTypeid;
                        stationUpdateRequest.HardVersion = (byte)updateHardVersion;
                        stationUpdateRequest.FileVersion = (byte)updateFileVersion;
                        stationUpdateRequest.maxVersion = (byte)updateMaxVersion;
                        stationUpdateRequest.minVersion = (byte)updateMinVersion;
                        stationUpdateRequest.FileCount = updateCount;
                        stationUpdateRequest.Crc = crcValue;
                        item.isSendBuffer = true;
                        item.protocol = stationUpdateRequest;
                        item.protocolType = ProtocolType.StationUpdateRequest;
                        break;
                    case StationUpdateState.recivingFile://文件接收中    下发文件
                        if (item.nowNeedSendBuffIndex >= updateCount)
                        {
                            item.updateState = StationUpdateState.reiciveCheck;
                            return null;
                        }
                        SendUpdateBufferRequest sendUpdateBufferRequest = new SendUpdateBufferRequest();
                        sendUpdateBufferRequest.DeviceCode = fzh.ToString().PadLeft(3, '0') + "0000";
                        sendUpdateBufferRequest.DeviceId = (byte)updatetTypeid;
                        sendUpdateBufferRequest.HardVersion = (byte)updateHardVersion;
                        sendUpdateBufferRequest.FileVersion = (byte)updateFileVersion;
                        sendUpdateBufferRequest.NowBufferIndex = item.nowNeedSendBuffIndex;
                        sendUpdateBufferRequest.Buffer = new byte[256];
                        for (int i = 0; i < sendUpdateBufferRequest.Buffer.Length; i++)
                        {
                            sendUpdateBufferRequest.Buffer[i] = updateBuffer[(item.nowNeedSendBuffIndex - 1) * 256 + i];
                        }
                        item.isSendBuffer = true;
                        item.protocol = sendUpdateBufferRequest;
                        item.protocolType = ProtocolType.SendUpdateBufferRequest;
                        stationItems[fzh - 1].nowNeedSendBuffIndex += 1;
                        if (stationItems[fzh - 1].nowNeedSendBuffIndex >= updateCount)
                        {
                            stationItems[fzh - 1].updateState = StationUpdateState.reiciveCheck;//下发完成后自动切换到巡检
                        }
                        break;
                    case StationUpdateState.reiciveCheck://巡检文件接收情况     下发巡检
                        InspectionRequest inspectionRequest = new InspectionRequest();
                        inspectionRequest.DeviceCode = fzh.ToString().PadLeft(3, '0') + "0000";
                        inspectionRequest.DeviceId = (byte)updatetTypeid;
                        inspectionRequest.HardVersion = (byte)updateHardVersion;
                        inspectionRequest.FileVersion = (byte)updateFileVersion;
                        inspectionRequest.LostFileNum = 0;
                        item.isSendBuffer = true;
                        item.protocol = inspectionRequest;
                        item.protocolType = ProtocolType.InspectionRequest;
                        break;
                    case StationUpdateState.replacement://补发文件中       补发文件
                        InspectionRequest replacement = new InspectionRequest();
                        replacement.DeviceCode = fzh.ToString().PadLeft(3, '0') + "0000";
                        replacement.DeviceId = (byte)updatetTypeid;
                        replacement.HardVersion = (byte)updateHardVersion;
                        replacement.FileVersion = (byte)updateFileVersion;
                        replacement.LostFileNum = item.nowNeedSendBuffIndex;
                        if (replacement.LostFileNum > 0)
                        {
                            replacement.FileBuffer = new byte[256];
                            for (int i = 0; i < replacement.FileBuffer.Length; i++)
                            {
                                replacement.FileBuffer[i] = updateBuffer[(item.nowNeedSendBuffIndex - 1) * 256 + i];
                            }
                        }
                        item.isSendBuffer = true;
                        item.protocol = replacement;
                        item.protocolType = ProtocolType.InspectionRequest;
                        break;
                    case StationUpdateState.restart://重启升级      下发重启命令
                        RestartRequest restartRequest = new RestartRequest();
                        restartRequest.DeviceCode = fzh.ToString().PadLeft(3, '0') + "0000";
                        restartRequest.DeviceId = (byte)updatetTypeid;
                        restartRequest.HardVersion = (byte)updateHardVersion;
                        restartRequest.FileVersion = (byte)updateFileVersion;
                        item.isSendBuffer = true;
                        item.protocol = restartRequest;
                        item.protocolType = ProtocolType.RestartRequest;
                        break;
                    case StationUpdateState.restore://恢复备份      下发恢复备份命令
                        ReductionRequest reductionRequest = new ReductionRequest();
                        reductionRequest.DeviceCode = fzh.ToString().PadLeft(3, '0') + "0000";
                        reductionRequest.DeviceId = (byte)updatetTypeid;
                        reductionRequest.HardVersion = (byte)updateHardVersion;
                        reductionRequest.SoftVersion = (byte)updateFileVersion;
                        item.isSendBuffer = true;
                        item.protocol = reductionRequest;
                        item.protocolType = ProtocolType.ReductionRequest;
                        break;
                    case StationUpdateState.updateCancle: //取消升级    //下取消升级命令
                        UpdateCancleRequest updateCancleRequest = new UpdateCancleRequest();
                        updateCancleRequest.DeviceCode = fzh.ToString().PadLeft(3, '0') + "0000";
                        updateCancleRequest.DeviceId = (byte)updatetTypeid;
                        updateCancleRequest.HardVersion = (byte)updateHardVersion;
                        updateCancleRequest.FileVersion = (byte)updateFileVersion;
                        item.isSendBuffer = true;
                        item.protocol = updateCancleRequest;
                        item.protocolType = ProtocolType.UpdateCancleRequest;
                        break;
                    case StationUpdateState.getVersion://获取分站信息
                        GetStationUpdateStateRequest getStationUpdateStateRequest = new GetStationUpdateStateRequest();
                        getStationUpdateStateRequest.DeviceCode = fzh.ToString().PadLeft(3, '0') + "0000";
                        getStationUpdateStateRequest.GetSoftVersion = 1;
                        getStationUpdateStateRequest.GetUpdateState = 1;
                        getStationUpdateStateRequest.GetDevType = 1;
                        getStationUpdateStateRequest.GetHardVersion = 1;
                        item.isSendBuffer = true;
                        item.protocol = getStationUpdateStateRequest;
                        item.protocolType = ProtocolType.GetStationUpdateStateRequest;
                        break;
                }
            }
            return item;
        }

        public static List<StationUpdateItem> GetAllStationItems()
        {
            List<StationUpdateItem> items = new List<StationUpdateItem>();
            for (int i = 0; i < stationItems.Length; i++)
            {
                if (stationItems[i] != null)
                {
                    StationUpdateItem item = DeepCopy(stationItems[i]);
                    items.Add(item);
                }
            }
            return items;
        }
        #endregion
    }


  
}
