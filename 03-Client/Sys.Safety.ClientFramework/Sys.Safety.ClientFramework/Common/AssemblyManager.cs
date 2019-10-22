using Basic.Framework.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Sys.Safety.ClientFramework.CBFCommon
{
    /// <summary>
    /// .net 反射管理类
    /// </summary>
    public class AssemblyManager
    {
        ///// <summary>
        ///// 缓存实例化的对象，解决内存泄漏问题  20171013
        ///// </summary>
        //static Dictionary<string, object> assemblyCreateInstanceList = new Dictionary<string, object>();
        ///// <summary>
        ///// 缓存实例化的对象，解决内存泄漏问题  20171013
        ///// </summary>
        //static Dictionary<string, Assembly> assemblyCreateList = new Dictionary<string, Assembly>();
        private AssemblyManager()
        {
        }
        /// <summary>
        /// 通过反射机制创建对象实例
        /// </summary>
        /// <param name="assemblyName">程序集名</param>
        /// <param name="typeName">类型</param>
        /// <param name="args">参数</param>
        /// <returns>对象实例</returns>
        public static object CreateInstance(string assemblyName, string typeName, object[] args, ref bool isReload)
        {
            object returnObject = null;
            System.Reflection.Assembly assemblyCreate = null;


            //if (assemblyCreateInstanceList.ContainsKey(typeName))
            //{
            //    returnObject = assemblyCreateInstanceList[typeName];

            //    assemblyCreate = assemblyCreateList[typeName];

            //    try
            //    {
            //        //如果已经反射了当前窗体，则直接调用窗体重新加载方法，进行重新加载  20171016
            //        var type = assemblyCreate.GetType(typeName);
            //        var method = type.GetMethod("Reload");
            //        if (args == null)
            //        {
            //            args = new object[1] { "" };
            //        }
            //        method.Invoke(returnObject, args);
            //        isReload = true;
            //    }
            //    catch (Exception ex)
            //    {
            //        Basic.Framework.Logging.LogHelper.Error(ex);
            //    }

            //    return returnObject;//如果已反射，则直接返回空
            //}
            //else
            //{
            //    isReload = false;
            //}


            try
            {
                if (assemblyCreate == null)
                {
                    assemblyCreate = Assembly.LoadFrom(assemblyName);
                    //assemblyCreateList.Add(typeName, assemblyCreate);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            if (returnObject == null)
            {
                if (args != null)
                    returnObject = assemblyCreate.CreateInstance(typeName, true, BindingFlags.CreateInstance, null, args, null, null);
                else
                    returnObject = assemblyCreate.CreateInstance(typeName, true);
                //assemblyCreateInstanceList.Add(typeName, returnObject);
            }
            return returnObject;
        }
        /// <summary>
        /// 通过反射机制创建对象实例并调用对象的方法
        /// </summary>
        /// <param name="assemblyName">程序集名</param>
        /// <param name="typeName">类型</param>
        /// <param name="args">参数</param>
        /// <param name="ClassName">方法名称</param>
        /// <returns>对象实例</returns>
        public static object CreateInstance(string assemblyName, string typeName, object[] args, string ClassName)
        {
            //AppDomainSetup info = new AppDomainSetup();
            //info.ApplicationBase = "file:///" + System.Environment.CurrentDirectory;
            //AppDomain dom = AppDomain.CreateDomain("KJ73NDomain", null, info);

            object returnValue = null;
            object instance = null;
            System.Reflection.Assembly assemblyCreate = null;

            //if (assemblyCreateInstanceList.ContainsKey(typeName))
            //{
            //    instance = assemblyCreateInstanceList[typeName];
            //    assemblyCreate = assemblyCreateList[typeName];
            //}

            try
            {
                //foreach (System.Reflection.Assembly tempAssembly in assemblyCreateList)
                //{
                //    if (tempAssembly.Location == assemblyName)
                //    {
                //        assemblyCreate = tempAssembly;
                //    }
                //}
                if (assemblyCreate == null)
                {
                    assemblyCreate = Assembly.LoadFrom(assemblyName);
                    //assemblyCreateList.Add(typeName, assemblyCreate);
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            var type = assemblyCreate.GetType(typeName);
            
            if (instance == null)
            {
                instance = assemblyCreate.CreateInstance(typeName, true);
                //assemblyCreateInstanceList.Add(typeName, instance);
            }

            var method = type.GetMethod(ClassName);

            method.Invoke(instance, args);

            //AppDomain.Unload(dom);

            return returnValue;
        }

    }
}
