using LuaInterface;
using Basic.Framework.Logging;
using DataAnalysis.Script;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sys.Safety.Processing.DataAnalysis
{
    public class FactorLuaService
    {
        #region 单例
        private volatile static FactorLuaService _instance = null;
        private static readonly object lockHelper = new object();
        private FactorLuaService() { }

        public static FactorLuaService CreateService()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new FactorLuaService();
                }
            }
            return _instance;
        }
        #endregion

        object callLockObject = new object();
        object registerFunctionLockObject = new object();
        private List<string> _registeredFunctionList = new List<string>();
        private List<string> RegisteredFunctionList
        {
            get
            {
                lock (registerFunctionLockObject)
                {
                    return _registeredFunctionList;
                }
            }
        }

        private LuaScriptContext luaScriptContext = new LuaScriptContext();
        public void RegisterFunction(FactorCalculateService factorCalculateService, string functionName)
        {
            lock (callLockObject)
            {
                if (RegisteredFunctionList.Any(q => q == functionName))
                    return;
                luaScriptContext.RegisterFunction(functionName, factorCalculateService, typeof(FactorCalculateService).GetMethod(functionName));
                RegisteredFunctionList.Add(functionName);
            }
        }

        public object[] CallFunction(string functionName, params object[] args)
        {
            lock (callLockObject)
            {
                string pointId = args[0].ToString();
                object[] results = null;
                try
                {
                    results = GetFunction(functionName).Call(args);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(string.Format("获取测点:{0},{1}值出错", pointId, functionName, ex.StackTrace));
                }

                return results;
            }
        }

        public object[] ExecuteLuaScript(string scriptContent)
        {
            lock (callLockObject)
            {
                return luaScriptContext.ExecuteLuaScript(string.Format("local result = {0}; return result;", scriptContent));
            }
        }

        private LuaFunction GetFunction(string functionName)
        {
            return luaScriptContext.GetLua().GetFunction(functionName);
        }
    }
}
