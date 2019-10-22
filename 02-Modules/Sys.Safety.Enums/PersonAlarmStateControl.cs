using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Enums
{
    public class PersonAlarmStateControl
    {
        /// <summary>
        /// 添加状态位
        /// </summary>
        /// <param name="oldState">原状态</param>
        /// <param name="newState">待添加的状态</param>
        /// <returns></returns>
        public static int AddNewState(int oldState, PersonAlarmState newState)
        {
            int state = oldState;

            state |= (1 << ((int)newState - (int)PersonAlarmState.nomal));

            return state;
        }

        /// <summary>
        /// 删除状态位
        /// </summary>
        /// <param name="oldState">原状态</param>
        /// <param name="newState">待删除的状态</param>
        /// <returns></returns>
        public static int DeleteOldState(int oldState, PersonAlarmState newState)
        {
            int state = oldState;

            state &= (1 << ((int)newState - (int)PersonAlarmState.nomal));

            return state;
        }

        /// <summary>
        /// 获取当前人员状态的报警描述
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string GetPersonAlarmDescription(int state)
        {
            string description ="";

            if (state == 0)
            {
                description = "正常";
            }
            else
            {
                PersonAlarmState alarmState;
                for (int i = 0; i < 16; i++)
                {
                    if (((state >> i) & 0x01) == 0x01)
                    {
                        alarmState = (PersonAlarmState)(i + (int)PersonAlarmState.nomal);
                        description += EnumHelper.GetEnumDescription(alarmState) + ",";
                    }
                }
                if (description.Length > 0)
                {
                    description = description.Substring(0, description.Length - 1);
                }
            }

            return description;
        }
    }
}
