using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.IO;
using System.Drawing;


namespace Sys.Safety.Reports
{
    public class ResourceManager
    {
       

        private System.Resources.ResourceManager res;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">资源文件的类型</param>
        public ResourceManager(System.Type type)
        {
            if (type != null)
            {
                try
                {
                    string temp = type.Assembly.ManifestModule.Name;
                    if (temp.LastIndexOf(".") > 0)
                    {
                        temp = temp.Remove(temp.LastIndexOf("."));
                    }
                    res = new System.Resources.ResourceManager(temp + ".Properties.Resources", type.Assembly);
                }
                catch(Exception e)
                {
                   throw new Exception("未找到资源文件"+type.Namespace+",错误为"+e.ToString());
                }
            }
            else
            {
                throw new Exception("传入类型["+type+"]为空");
            }
        }
        /// <summary>
        /// 获取国际化后的字串
        /// </summary>
        /// <param name="name">需要国际化的字串</param>
        /// <returns>国际化后的结果,如果没找到节点,返回节点名</returns>
        public string GetString(string name)
        {
            return GetString(name,false);
        }
        /// <summary>
        /// 获取国际化后的字串
        /// </summary>
        /// <param name="name">需要国际化的字串</param>
        /// <param name="isThrowException">没找到节点时是否抛出异常</param>
        /// <returns>国际化后的结果</returns>
        public string GetString(string name,bool isThrowException)
        {
            bool isHave = true;
            string result = name;
            if (this.res != null)
            {
                try
                {
                    result = res.GetString(name);
                    if (result == null)
                    {
                        isHave = false;
                    }
                }
                catch(Exception e)
                {
                    throw new Exception("未找到资源节点["+name+"]:"+e.ToString());
                    isHave = false;
                }
                if (!isHave && isThrowException)
                {
                    throw new Exception(string.Format("未找到资源节点[{0}]!", name));
                }
            }
            return result;
        }
        /// <summary>
        /// 获取国际化后的字串
        /// </summary>
        /// <param name="name">需要国际化的字串</param>
        /// <param name="arg0">字串中的替换参数</param>
        /// <returns>国际化后的结果,如果没找到节点,返回节点名</returns>
        public string GetString(string name, string arg0)
        {
            return GetString(name,arg0,false);
        }
        /// <summary>
        /// 获取国际化后的字串
        /// </summary>
        /// <param name="name">需要国际化的字串</param>
        /// <param name="arg0">字串中的替换参数</param>
        /// <param name="isThrowException">没找到节点时是否抛出异常</param>
        /// <returns>国际化后的结果</returns>
        public string GetString(string name, string arg0, bool isThrowException)
        {
			string result = GetString(name);
			try
			{
				result = string.Format(result, arg0);
			}
            catch (Exception e)
            {
                if (isThrowException)
                {
                    throw new Exception(string.Format("未找到资源节点["+name+"]:"+e));
                }
            }
			return result;
        }
        /// <summary>
        /// 获取国际化后的字串
        /// </summary>
        /// <param name="name">需要国际化的字串</param>
        /// <param name="arg0">字串中的替换参数0</param>
        /// <param name="arg1">字串中的替换参数1</param>
        /// <returns>国际化后的结果,如果没找到节点,返回节点名</returns>
        public string GetString(string name, string arg0,string arg1)
        {
            return GetString(name, arg0, arg1, false);
        }
        /// <summary>
        /// 获取国际化后的字串
        /// </summary>
        /// <param name="name">需要国际化的字串</param>
        /// <param name="arg0">字串中的替换参数0</param>
        /// <param name="arg1">字串中的替换参数1</param>
        /// <param name="isThrowException">没找到节点时是否抛出异常</param>
        /// <returns>国际化后的结果</returns>
        public string GetString(string name, string arg0, string arg1, bool isThrowException)
        {
			string result = GetString(name);
			try
			{
				result = string.Format(result, arg0, arg1);
			}
            catch (Exception e)
            {
                if (isThrowException)
                {
                    throw new Exception(string.Format("未找到资源节点[{0}]:{1}", name, e));
                }
            }
			return result;
        }
        /// <summary>
        /// 获取国际化后的字串
        /// </summary>
        /// <param name="name">需要国际化的字串</param>
        /// <param name="arg0">字串中的替换参数0</param>
        /// <param name="arg1">字串中的替换参数1</param>
        /// <param name="arg2">字串中的替换参数2</param>
        /// <returns>国际化后的结果,如果没找到节点,返回节点名</returns>
        public string GetString(string name, string arg0, string arg1, string arg2)
        {
            return GetString(name, arg0, arg1, arg2, false);
        }
        /// <summary>
        /// 获取国际化后的字串
        /// </summary>
        /// <param name="name">需要国际化的字串</param>
        /// <param name="arg0">字串中的替换参数0</param>
        /// <param name="arg1">字串中的替换参数1</param>
        /// <param name="arg2">字串中的替换参数2</param>
        /// <param name="isThrowException">没找到节点时是否抛出异常</param>
        /// <returns>国际化后的结果</returns>
        public string GetString(string name, string arg0, string arg1, string arg2, bool isThrowException)
        {
			string result = GetString(name);
			try
			{
				result = string.Format(result, arg0, arg1, arg2);
			}
            catch (Exception e)
            {
                if (isThrowException)
                {
                    throw new Exception(string.Format("未找到资源节点[{0}]:{1}", name, e));
                }
            }
			return result;
        }
        /// <summary>
        /// 获取国际化后的字串
        /// </summary>
        /// <param name="name">需要国际化的字串</param>
        /// <param name="args">字串中的替换参数</param>
        /// <returns>国际化后的结果,如果没找到节点,返回节点名</returns>
        public string GetString(string name, object[] args)
        {
            return GetString(name, args, false);
        }
        /// <summary>
        /// 获取国际化后的字串
        /// </summary>
        /// <param name="name">需要国际化的字串</param>
        /// <param name="args">字串中的替换参数</param>
        /// <param name="isThrowException">没找到节点时是否抛出异常</param>
        /// <returns>国际化后的结果</returns>
        public string GetString(string name, object[] args, bool isThrowException)
        {
			string result = GetString(name);
			try
			{
				result = string.Format(result, args);
			}
            catch (Exception e)
            {
                if (isThrowException)
                {
                    throw new Exception(string.Format("未找到资源节点[{0}]:{1}", name, e));
                }
            }
            return result;
        }
        /// <summary>
        /// 获取图片资源
        /// </summary>
        /// <param name="name">资源名</param>
        /// <returns>资源,如果没找到,则返回空</returns>
        public Bitmap GetBitmap(string name)
        {
            return GetBitmap(name, false);
        }
        /// <summary>
        /// 获取图片资源
        /// </summary>
        /// <param name="name">资源名</param>
        /// <param name="isThrowException">没找到节点时是否抛出异常</param>
        /// <returns>资源</returns>
        public Bitmap GetBitmap(string name, bool isThrowException)
        {
            Bitmap result = null;
            try
            {
                result = (System.Drawing.Bitmap)res.GetObject(name);
            }
            catch (Exception e)
            {
                if (isThrowException)
                {
                    throw new Exception(string.Format("未找到资源节点[{0}]:{1}", name, e));
                }
            }
            return result;
        }
    }
}
