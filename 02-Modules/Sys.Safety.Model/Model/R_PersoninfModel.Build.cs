using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("PE_PersonArchive")]
    public partial class R_PersoninfModel
    {
        /// <summary>
        /// 唯一编码
        /// </summary>

        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 局工号
        /// </summary>
        public string Jyid
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Tyid
        {
            get;
            set;
        }
        /// <summary>
        /// 内部编号
        /// </summary>
        [Key]
        public string Yid
        {
            get;
            set;
        }
        /// <summary>
        /// 主卡号
        /// </summary>
        public string Bh
        {
            get;
            set;
        }
        /// <summary>
        /// 副卡号
        /// </summary>
        public int Fbh
        {
            get;
            set;
        }
        /// <summary>
        /// 灯架号
        /// </summary>
        public string Djh
        {
            get;
            set;
        }
        /// <summary>
        /// 灯号
        /// </summary>
        public string Dh
        {
            get;
            set;
        }
        /// <summary>
        /// 自救灯号
        /// </summary>
        public string Zjqh
        {
            get;
            set;
        }
        /// <summary>
        /// 人员排序号
        /// </summary>
        public int Px
        {
            get;
            set;
        }
        /// <summary>
        /// 职务Id
        /// </summary>
        public string Zw
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Gz
        {
            get;
            set;
        }
        /// <summary>
        /// 职称Id
        /// </summary>
        public string Zc
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Rank
        {
            get;
            set;
        }
        /// <summary>
        /// 记录有效标识
        /// </summary>
        public string Sfyx
        {
            get;
            set;
        }
        /// <summary>
        /// 照片路径
        /// </summary>
        public string Photo
        {
            get;
            set;
        }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Bm
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Gh
        {
            get;
            set;
        }
        /// <summary>
        /// 考勤规则号
        /// </summary>
        public int Kqgz
        {
            get;
            set;
        }
        /// <summary>
        /// 班中餐规则号
        /// </summary>
        public int Bzcgz
        {
            get;
            set;
        }
        /// <summary>
        /// 岗位级别
        /// </summary>
        public string Jb
        {
            get;
            set;
        }
        /// <summary>
        /// 标志卡出厂日期
        /// </summary>
        public string Sysflat
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Bzkccrq
        {
            get;
            set;
        }
        /// <summary>
        /// 干部标志
        /// </summary>
        public string A0
        {
            get;
            set;
        }
        /// <summary>
        /// 员工归属
        /// </summary>
        public string A1
        {
            get;
            set;
        }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string A2
        {
            get;
            set;
        }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime A3
        {
            get;
            set;
        }
        /// <summary>
        /// 民族
        /// </summary>
        public string A4
        {
            get;
            set;
        }
        /// <summary>
        /// 政治面貌
        /// </summary>
        public string A5
        {
            get;
            set;
        }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string A6
        {
            get;
            set;
        }
        /// <summary>
        /// 籍贯
        /// </summary>
        public string A7
        {
            get;
            set;
        }
        /// <summary>
        /// 社保号
        /// </summary>
        public string A8
        {
            get;
            set;
        }
        /// <summary>
        /// 家庭住址
        /// </summary>
        public string A9
        {
            get;
            set;
        }
        /// <summary>
        /// 学历
        /// </summary>
        public string A10
        {
            get;
            set;
        }
        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime A11
        {
            get;
            set;
        }
        /// <summary>
        /// 员工类型
        /// </summary>
        public string A12
        {
            get;
            set;
        }
        /// <summary>
        /// 特长
        /// </summary>
        public string A13
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string A14
        {
            get;
            set;
        }
        /// <summary>
        /// 计算机水平
        /// </summary>
        public string A15
        {
            get;
            set;
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string A16
        {
            get;
            set;
        }
        /// <summary>
        /// 行政级别
        /// </summary>
        public string A17
        {
            get;
            set;
        }
        /// <summary>
        /// 外语语种
        /// </summary>
        public string A18
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string A19
        {
            get;
            set;
        }
        /// <summary>
        /// 专业
        /// </summary>
        public string A20
        {
            get;
            set;
        }
        /// <summary>
        /// 毕业学校
        /// </summary>
        public string A21
        {
            get;
            set;
        }
        /// <summary>
        /// 性别
        /// </summary>
        public string A22
        {
            get;
            set;
        }
        /// <summary>
        /// 户口
        /// </summary>
        public string A23
        {
            get;
            set;
        }
        /// <summary>
        /// 特种人员经过的点号及时间
        /// </summary>
        public string A24
        {
            get;
            set;
        }
        /// <summary>
        /// 员工入井规定时长
        /// </summary>
        public string A25
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string A26
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string A27
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string A28
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string A29
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string A30
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string A31
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By1
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By2
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By3
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By4
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By5
        {
            get;
            set;
        }
        /// <summary>
        /// OA用户名
        /// </summary>
        public string Username
        {
            get;
            set;
        }
        /// <summary>
        /// OA密码
        /// </summary>
        public string Password
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Upflag
        {
            get;
            set;
        }
    }
}

